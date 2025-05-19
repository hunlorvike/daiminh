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
        // 1. Route cho các area, hỗ trợ Admin/Client đều có AccountController riêng biệt
        endpoints.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

        // 2. Định nghĩa rõ route cho Account từng area (ưu tiên, không nhầm lẫn)
        endpoints.MapControllerRoute(
            name: "admin_account",
            pattern: "admin/tai-khoan/{action=Login}/{id?}",
            defaults: new { area = "Admin", controller = "Account" });

        endpoints.MapControllerRoute(
            name: "client_account",
            pattern: "tai-khoan/{action=Login}/{id?}",
            defaults: new { area = "Client", controller = "Account" });

        // 3. Các route ưu tiên cố định
        endpoints.MapControllerRoute(
            name: "contact",
            pattern: "lien-he",
            defaults: new { area = "Client", controller = "Contact", action = "Index" });

        endpoints.MapControllerRoute(
            name: "faq",
            pattern: "cau-hoi-thuong-gap",
            defaults: new { area = "Client", controller = "FAQ", action = "Index" });

        // 4. Route mặc định cho Client
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}",
            defaults: new { area = "Client" });

        // 5. Route slug động cho Page/Detail
        endpoints.MapControllerRoute(
            name: "slug",
            pattern: "{slug}",
            defaults: new { area = "Client", controller = "Page", action = "Detail" });
    }
}
