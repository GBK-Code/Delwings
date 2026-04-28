using Delwings.Context;
using Delwings.Models;
using Delwings.Repositories;
using Delwings.Repositories.Interfaces;
using Delwings.Services;
using Delwings.Services.Dashboards;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Admin role
builder.Services.AddAuthentication("Cookies") // схема по умолчанию
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/Login";        // если не авторизован — редирект на форму
        options.AccessDeniedPath = "/Home/AccessDenied"; // если нет прав
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

// Add database context
builder.Services.AddDbContext<AppDbContext>
    (
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

builder.Services.Configure<ApiSettings>
    (
        builder.Configuration.GetSection("ApiSettings")
    );

builder.Services.AddHttpClient<ApiService>();

builder.Services.AddScoped<AccountRegistrationService>();

// Dashboards
builder.Services.AddScoped<HeadDashboardService>();
builder.Services.AddScoped<AdminDashboardService>();

builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<OrdersService>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<AccountService>();

builder.Services.AddScoped<IPlaceRepository, PlaceRepository>();
builder.Services.AddScoped<PlaceService>();

builder.Services.AddScoped<ICourierApplicationRepository, CourierApplicationRepository>();
builder.Services.AddScoped<CourierApplicationService>();

builder.Services.AddScoped<IOperatorPlacesRepository, OperatorPlacesRepository>();
builder.Services.AddScoped<OperatorPlacesService>();

builder.Services.AddScoped<IOrdersHistoryRepository, OrdersHistoryRepository>();
builder.Services.AddScoped<OrdersHistoryService>();

builder.Services.AddScoped<ICourierOrdersRepository, CourierOrdersRepository>();
builder.Services.AddScoped<CourierOrdersService>();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
