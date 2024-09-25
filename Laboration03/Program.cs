using Laboration03.Application.Common.Interfaces;
using Laboration03.Infrastructure;
using Laboration03.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the DatabaseTest class
builder.Services.AddSingleton<DatabaseTest>();

// Register the UnitOfWork with the connection string
builder.Services.AddScoped<IUnitOfWork>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new UnitOfWork(connectionString); // Pass the connection string to UnitOfWork
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
