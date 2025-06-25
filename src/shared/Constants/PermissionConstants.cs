using System.Reflection;

namespace shared.Constants;

public static class PermissionConstants
{
    // GENERAL ACCESS
    public const string AdminAccess = "Admin.Access";
    public const string SuperAdminAccess = "SuperAdmin.Access";
    public const string DashboardView = "Dashboard.View";

    // ARTICLE MANAGEMENT
    public const string ArticleView = "Article.View";
    public const string ArticleCreate = "Article.Create";
    public const string ArticleEdit = "Article.Edit";
    public const string ArticleDelete = "Article.Delete";

    // BANNER MANAGEMENT
    public const string BannerView = "Banner.View";
    public const string BannerCreate = "Banner.Create";
    public const string BannerEdit = "Banner.Edit";
    public const string BannerDelete = "Banner.Delete";

    // CATEGORY MANAGEMENT
    public const string CategoryView = "Category.View";
    public const string CategoryCreate = "Category.Create";
    public const string CategoryEdit = "Category.Edit";
    public const string CategoryDelete = "Category.Delete";

    // CLAIM DEFINITION MANAGEMENT
    public const string ClaimDefinitionManage = "ClaimDefinition.Manage";

    // CONTACT MANAGEMENT
    public const string ContactView = "Contact.View";
    public const string ContactCreate = "Contact.Create";
    public const string ContactEdit = "Contact.Edit";
    public const string ContactDelete = "Contact.Delete";

    // FAQ MANAGEMENT
    public const string FAQView = "FAQ.View";
    public const string FAQCreate = "FAQ.Create";
    public const string FAQEdit = "FAQ.Edit";
    public const string FAQDelete = "FAQ.Delete";

    // NEWSLETTER MANAGEMENT
    public const string NewsletterView = "Newsletter.View";
    public const string NewsletterCreate = "Newsletter.Create";
    public const string NewsletterEdit = "Newsletter.Edit";
    public const string NewsletterDelete = "Newsletter.Delete";

    // PAGE MANAGEMENT
    public const string PageView = "Page.View";
    public const string PageCreate = "Page.Create";
    public const string PageEdit = "Page.Edit";
    public const string PageDelete = "Page.Delete";

    // PRODUCT MANAGEMENT
    public const string ProductView = "Product.View";
    public const string ProductCreate = "Product.Create";
    public const string ProductEdit = "Product.Edit";
    public const string ProductDelete = "Product.Delete";

    // PRODUCT VARIATION MANAGEMENT
    public const string ProductVariationView = "ProductVariation.View";
    public const string ProductVariationCreate = "ProductVariation.Create";
    public const string ProductVariationEdit = "ProductVariation.Edit";
    public const string ProductVariationDelete = "ProductVariation.Delete";

    // ROLE MANAGEMENT
    public const string RoleManage = "Role.Manage";

    // TAG MANAGEMENT
    public const string TagView = "Tag.View";
    public const string TagCreate = "Tag.Create";
    public const string TagEdit = "Tag.Edit";
    public const string TagDelete = "Tag.Delete";

    // TESTIMONIAL MANAGEMENT
    public const string TestimonialView = "Testimonial.View";
    public const string TestimonialCreate = "Testimonial.Create";
    public const string TestimonialEdit = "Testimonial.Edit";
    public const string TestimonialDelete = "Testimonial.Delete";

    // USER MANAGEMENT
    public const string UserManage = "User.Manage";

    // MEDIA MANAGEMENT
    public const string MediaView = "Media.View";
    public const string MediaUpload = "Media.Upload";
    public const string MediaDelete = "Media.Delete";
    public const string MediaEdit = "Media.Edit";


    public static IEnumerable<string> GetAllPermissions()
    {
        return typeof(PermissionConstants)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
            .Select(f => (string)f.GetValue(null)!);
    }
}
