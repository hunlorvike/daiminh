using System.Text.RegularExpressions;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;

namespace web.Areas.Admin.Services;

public class RedirectService : IRedirectService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RedirectService> _logger;

    // Cache for redirects to avoid database queries on every request
    private static Dictionary<string, (int Id, string TargetUrl, RedirectType Type)> _exactRedirectsCache = new();
    private static List<(int Id, Regex Regex, string TargetUrl, RedirectType Type)> _regexRedirectsCache = new();
    private static DateTime _lastCacheRefresh = DateTime.MinValue;
    private static readonly TimeSpan _cacheRefreshInterval = TimeSpan.FromMinutes(5);
    private static readonly object _cacheLock = new();

    public RedirectService(ApplicationDbContext context, ILogger<RedirectService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<(string TargetUrl, int StatusCode)?> CheckForRedirectAsync(string path)
    {
        // Normalize the path
        path = path.TrimEnd('/');
        if (string.IsNullOrEmpty(path))
        {
            path = "/";
        }

        // Refresh cache if needed
        await RefreshCacheIfNeededAsync();

        // Check for exact match first (faster)
        if (_exactRedirectsCache.TryGetValue(path, out var exactMatch))
        {
            await IncrementHitCountAsync(exactMatch.Id);
            return (exactMatch.TargetUrl, exactMatch.Type == RedirectType.Permanent ? 301 : 302);
        }

        // Check for regex match
        foreach (var regexRedirect in _regexRedirectsCache)
        {
            if (regexRedirect.Regex.IsMatch(path))
            {
                // Handle capture groups in the target URL
                string targetUrl = regexRedirect.Regex.Replace(path, regexRedirect.TargetUrl);
                await IncrementHitCountAsync(regexRedirect.Id);
                return (targetUrl, regexRedirect.Type == RedirectType.Permanent ? 301 : 302);
            }
        }

        // No redirect found
        return null;
    }

    public async Task IncrementHitCountAsync(int redirectId)
    {
        try
        {
            // Use raw SQL for better performance
            await _context.Database.ExecuteSqlRawAsync(
                "UPDATE redirects SET hit_count = hit_count + 1 WHERE id = {0}", redirectId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error incrementing hit count for redirect {RedirectId}", redirectId);
        }
    }

    private async Task RefreshCacheIfNeededAsync()
    {
        // Check if cache refresh is needed
        if (DateTime.UtcNow - _lastCacheRefresh < _cacheRefreshInterval)
        {
            return;
        }

        // Use lock to prevent multiple refreshes
        if (Monitor.TryEnter(_cacheLock))
        {
            try
            {
                // Double-check after acquiring lock
                if (DateTime.UtcNow - _lastCacheRefresh < _cacheRefreshInterval)
                {
                    return;
                }

                // Get all active redirects
                var redirects = await _context.Set<Redirect>()
                    .Where(r => r.IsActive)
                    .ToListAsync();

                // Create new cache instances
                var exactRedirects = new Dictionary<string, (int Id, string TargetUrl, RedirectType Type)>();
                var regexRedirects = new List<(int Id, Regex Regex, string TargetUrl, RedirectType Type)>();

                foreach (var redirect in redirects)
                {
                    if (redirect.IsRegex)
                    {
                        try
                        {
                            var regex = new Regex(redirect.SourceUrl, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            regexRedirects.Add((redirect.Id, regex, redirect.TargetUrl, redirect.Type));
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Invalid regex pattern in redirect {RedirectId}: {Pattern}",
                                redirect.Id, redirect.SourceUrl);
                        }
                    }
                    else
                    {
                        // Normalize the source URL
                        var sourceUrl = redirect.SourceUrl.TrimEnd('/');
                        if (string.IsNullOrEmpty(sourceUrl))
                        {
                            sourceUrl = "/";
                        }

                        exactRedirects[sourceUrl] = (redirect.Id, redirect.TargetUrl, redirect.Type);
                    }
                }

                // Update cache
                _exactRedirectsCache = exactRedirects;
                _regexRedirectsCache = regexRedirects;
                _lastCacheRefresh = DateTime.UtcNow;

                _logger.LogInformation("Redirect cache refreshed. {ExactCount} exact redirects, {RegexCount} regex redirects",
                    exactRedirects.Count, regexRedirects.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing redirect cache");
            }
            finally
            {
                Monitor.Exit(_cacheLock);
            }
        }
    }
}