﻿using RazorApp1.Domain.Services;
using RazorApp1.Domain.Services.MailService;
using RazorApp1.Implementation.BackgroundServices;
using RazorApp1.Implementation.Services;
using MediatR;
using System.Reflection;
using Microsoft.AspNetCore.HttpLogging;

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
            services.AddSingleton<Data>()

            // Майл сервис содается один раз в рамказ одного запроса, потому что его метод SendEmailAsync включает полный цикл
            // подключение, авторизация, отключение
                .AddScoped<IMailService, MailService>()
            // Scoped - т.к. обращается к MailService. А так можно было сделать singlton
                .AddScoped<ICatalogService, CatalogService>()
                .AddScoped<IFileReaderService, FileReaderService>()
                .AddHostedService<ServerWorkReport>()
                .AddHttpContextAccessor()
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddScoped<ContextData>()
                .AddHttpLogging(opt =>
                {
                    opt.LoggingFields = HttpLoggingFields.RequestHeaders
                        | HttpLoggingFields.ResponseHeaders
                        | HttpLoggingFields.RequestBody
                        | HttpLoggingFields.ResponseBody;
                });

            return services;
        }
    }
}
