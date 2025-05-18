namespace web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
    {
        var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
        if (env != null && !env.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/error?statusCode={0}");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    public static void MapDefaultRoutes(this IEndpointRouteBuilder endpoints)
    {
        // 1. Route cho các area (Admin, Client,...)
        endpoints.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

        // 2. Các route ưu tiên cố định
        endpoints.MapControllerRoute(
            name: "contact",
            pattern: "lien-he",
            defaults: new { area = "Client", controller = "Contact", action = "Index" });

        endpoints.MapControllerRoute(
            name: "faq",
            pattern: "cau-hoi-thuong-gap",
            defaults: new { area = "Client", controller = "FAQ", action = "Index" });

        endpoints.MapControllerRoute(
            name: "account_login",
            pattern: "tai-khoan/dang-nhap",
            defaults: new { area = "Client", controller = "Account", action = "Login" });

        endpoints.MapControllerRoute(
            name: "account_register",
            pattern: "tai-khoan/dang-ky",
            defaults: new { area = "Client", controller = "Account", action = "Register" });

        endpoints.MapControllerRoute(
            name: "account_forgot",
            pattern: "tai-khoan/quen-mat-khau",
            defaults: new { area = "Client", controller = "Account", action = "ForgotPassword" });

        endpoints.MapControllerRoute(
            name: "account_reset",
            pattern: "tai-khoan/khoi-phuc-mat-khau",
            defaults: new { area = "Client", controller = "Account", action = "ResetPassword" });

        endpoints.MapControllerRoute(
            name: "account_forgot_confirm",
            pattern: "tai-khoan/xac-nhan-quen-mat-khau",
            defaults: new { area = "Client", controller = "Account", action = "ForgotPasswordConfirmation" });

        // 3. Route mặc định
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}",
            defaults: new { area = "Client" });

        // 4. Route slug động
        endpoints.MapControllerRoute(
            name: "slug",
            pattern: "{slug}",
            defaults: new { area = "Client", controller = "Page", action = "Detail" });
    }
}
