using HotelApp.Business.Services;
using HotelApp.Data.Contexts;
using HotelApp.Data.Models;
using HotelApp.Data.Repositories;
using HotelApp.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<HotelAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("HotelAppConnection"));
});

// Add Identity service
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddEntityFrameworkStores<HotelAppDbContext>()
    .AddDefaultTokenProviders();

// Set up Serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Day);
});

// Add Generic Repository to the container
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Add Unit of Work to the container
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Room Services to the container
builder.Services.AddScoped<IRoomService, RoomService>();

// Add Amenity Services to the container
builder.Services.AddScoped<IAmenityService, AmenityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Seed Data
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<HotelAppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    DbInitializer.SeedData(dbContext, userManager, roleManager);
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

await app.RunAsync();
