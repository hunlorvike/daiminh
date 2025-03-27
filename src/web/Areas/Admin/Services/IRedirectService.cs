namespace web.Areas.Admin.Services;

public interface IRedirectService
{
    Task<(string TargetUrl, int StatusCode)?> CheckForRedirectAsync(string path);
    Task IncrementHitCountAsync(int redirectId);
}
