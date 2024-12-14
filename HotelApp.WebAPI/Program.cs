using System.Reflection;
using System.Text;
using HotelApp.Business.Services;
using HotelApp.Data.Contexts;
using HotelApp.Data.Models;
using HotelApp.Data.Repositories;
using HotelApp.Data.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Hotel App API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' following by space and JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"] ?? "congdinh2021@gmail.com"))
    };
});


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

// Add Booking Services to the container
builder.Services.AddScoped<IBookingService, BookingService>();

// Add User Services to the container
builder.Services.AddScoped<IUserService, UserService>();

// Add Role Services to the container
builder.Services.AddScoped<IRoleService, RoleService>();

// Add Auth Services to the container
builder.Services.AddScoped<IAuthService, AuthService>();

// Add CORS policy with allowed origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        options.AddPolicy("AllowedOrigins", builder => builder
            .WithOrigins("http://localhost:4200", "https://localhost:4200")
            .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization, HeaderNames.Accept, HeaderNames.XRequestedWith)
            .WithMethods("GET", "POST", "PUT", "DELETE"));
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel App API v1");
        options.RoutePrefix = string.Empty;
    });

    // Seed Data
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<HotelAppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    DbInitializer.SeedData(dbContext, userManager, roleManager);
}

// Error Handling:
// Implement global error handling middleware to catch and log exceptions thrown during request processing.
// Use status codes (e.g., BadRequest, NotFound, InternalServerError) to return appropriate HTTP responses for different error scenarios.
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        Log.Error(exception, "An unhandled exception has occurred.");

        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain";
        var errorResponse = new {
             message = exception?.Message,
             statusCode = context.Response.StatusCode
        };
        var errorResponseJson = System.Text.Json.JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(errorResponseJson);
    });
});

app.UseHttpsRedirection();

app.UseRouting();

// Enable CORS using AllowedOrigins policy
app.UseCors("AllowedOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
