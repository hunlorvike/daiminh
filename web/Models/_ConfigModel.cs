namespace web.Models;

public class _ConfigModel
{
    public SiteSettings SiteSettings { get; set; }
}

public class SiteSettings
{
    public GeneralSettings General { get; set; }
}

public class GeneralSettings
{
    public string SiteName { get; set; }
    public string SiteDescription { get; set; }
    public string ContactEmail { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public SocialMedia SocialMedia { get; set; }
}

public class SocialMedia
{
    public string Facebook { get; set; }
}