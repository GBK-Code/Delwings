using Delwings.Context;
using Delwings.Models.Basic;
using Delwings.Repositories;
using Delwings.Repositories.Interfaces;
using Delwings.Services;
using Delwings.Services.Dashboards;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Admin role
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
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

// Utils
builder.Services.AddScoped<TableSearchService>();
builder.Services.AddScoped<TableFilterService>();
builder.Services.AddScoped<DataGeneratorService>();
builder.Services.AddScoped<DataEraserService>();

// Specific
builder.Services.AddScoped<AccountRegistrationService>();
builder.Services.AddScoped<OrderTokensGenerator>();
builder.Services.AddScoped<CalculatorService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<DeliveryService>();

// Dashboards
builder.Services.AddScoped<HeadDashboardService>();
builder.Services.AddScoped<AdminDashboardService>();
builder.Services.AddScoped<OperatorVMService>();
builder.Services.AddScoped<CourierDashboardService>();
builder.Services.AddScoped<UserDashboardService>();
builder.Services.AddScoped<TrackPageService>();
builder.Services.AddScoped<DeveloperPageService>();

// Database
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
