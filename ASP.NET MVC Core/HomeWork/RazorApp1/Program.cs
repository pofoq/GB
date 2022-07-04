using RazorApp1;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = Config.Build();

builder.Host.UseSerilog((ctx, conf) => conf
        .ReadFrom.Configuration(ctx.Configuration)
//.Enrich.FromLogContext()
//.WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
//.WriteTo.File("log/log_.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
//.WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
);

builder.Services.RegisterServices(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
