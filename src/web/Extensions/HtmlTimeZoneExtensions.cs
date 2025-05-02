using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Extensions;

public static class HtmlTimeZoneExtensions
{
    public static IHtmlContent ToLocalTime(this IHtmlHelper htmlHelper, DateTime? utcTime, string timeZoneId = "Asia/Ho_Chi_Minh", string format = "dd/MM/yyyy HH:mm")
    {
        if (utcTime == null) return HtmlString.Empty;
        var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utcTime.Value, DateTimeKind.Utc), tz);
        return new HtmlString(localTime.ToString(format));
    }
}
