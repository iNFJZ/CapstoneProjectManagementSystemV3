﻿using CapstoneProjectManagementSystem.Controllers.Common_Controller;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Infrastructure.Services.CommonServices.SessionExtensionService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ISessionExtensionService, SessionExtensionService>();

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});
ILogger<UserController> logger = loggerFactory.CreateLogger<UserController>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:7178/")
                                .AllowAnyHeader()
                                .AllowAnyOrigin()
                                .AllowAnyMethod();
        });
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
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
app.UseCors("AllowSpecificOrigin");
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
