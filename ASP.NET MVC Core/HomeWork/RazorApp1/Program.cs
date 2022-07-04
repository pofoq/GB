using RazorApp1;
using RazorApp1.Middlewares;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    var config = Config.Build();

    builder.Services.RegisterServices(config);

    builder.Host.UseSerilog((ctx, conf) => conf
        .ReadFrom.Configuration(ctx.Configuration)
    );

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.Use(async (HttpContext context, Func<Task> next) =>
    {
        var userAgent = context.Request.Headers.UserAgent.ToString();
        if (!userAgent.Contains("edg", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.ContentType = "text/plain;charset=utf-8";
            await context.Response.WriteAsync("Ваш браузер не поддерживается");
            return;
        }
        await next();
    });

    app.UseHttpLogging();

    app.UseMiddleware<MetricsMiddleware>();

    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Server Stopped");
}