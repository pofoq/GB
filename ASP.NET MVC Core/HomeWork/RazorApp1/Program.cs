using RazorApp1;
using RazorApp1.Domain.Services;
using RazorApp1.Domain.Services.MailService;
using RazorApp1.Implementation.BackgroundServices;
using RazorApp1.Implementation.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = Config.Build();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<MailOption>(config.GetSection(nameof(MailOption)));

builder.Host.UseSerilog((ctx, conf) => conf
        .ReadFrom.Configuration(ctx.Configuration)
//.Enrich.FromLogContext()
//.WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
//.WriteTo.File("log/log_.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
//.WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
);

// Имитация БД. Один раз создаем и обращаемся всегда к одному объекту
builder.Services.AddSingleton<Data>();

// Майл сервис содается один раз в рамказ одного запроса, потому что его метод SendEmailAsync включает полный цикл
// подключение, авторизация, отключение
builder.Services.AddScoped<IMailService, MailService>();
// Scoped - т.к. обращается к MailService. А так можно было сделать singlton
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IFileReaderService, FileReaderService>();

builder.Services.AddHostedService<ServerWorkReport>();

builder.Services.AddHttpContextAccessor();

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
