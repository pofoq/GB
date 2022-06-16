using RazorApp1;
using RazorApp1.Domain.Services;
using RazorApp1.Domain.Services.MailService;
using RazorApp1.Implementation.Services;

var builder = WebApplication.CreateBuilder(args);

var config = Config.Build();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<MailOption>(config.GetSection(nameof(MailOption)));
builder.Services.AddHttpContextAccessor();
// ���� ������ �������� ���� ��� � ������ ������ �������, ������ ��� ��� ����� SendEmailAsync �������� ������ ����
// �����������, �����������, ����������
builder.Services.AddScoped<IMailService, MailService>();
// Scoped - �.�. ���������� � MailService. � ��� ����� ���� ������� singlton
builder.Services.AddScoped<ICatalogService, CatalogService>();
// �������� ��. ���� ��� ������� � ���������� ������ � ������ �������
builder.Services.AddSingleton<Data>();

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
