using RazorApp1.Domain.Services;
using RazorApp1.Domain.Services.MailService;
using RazorApp1.Implementation.BackgroundServices;
using RazorApp1.Implementation.Services;
using MediatR;
using System.Reflection;

namespace RazorApp1
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration config)
        {
            // Add services to the container.
            services.AddControllersWithViews();
            services.Configure<MailOption>(config.GetSection(nameof(MailOption)));


            // Имитация БД. Один раз создаем и обращаемся всегда к одному объекту
            services.AddSingleton<Data>();

            // Майл сервис содается один раз в рамказ одного запроса, потому что его метод SendEmailAsync включает полный цикл
            // подключение, авторизация, отключение
            services.AddScoped<IMailService, MailService>();
            // Scoped - т.к. обращается к MailService. А так можно было сделать singlton
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<IFileReaderService, FileReaderService>();
            
            services.AddHostedService<ServerWorkReport>();
            
            services.AddHttpContextAccessor();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<ContextData>();

            return services;
        }
    }
}
