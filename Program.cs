using Microsoft.EntityFrameworkCore;
using RestaurationAPI.Entities;
using RestaurationAPI;
using RestaurationAPI.Service;
using NLog.Web;
using RestaurationAPI.Middleware;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using RestaurationAPI.Models;
using RestaurationAPI.Models.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestaurationAPI.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// 1. Rejestracja serwisów w DI
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>(); // Rejestracja wszystkich walidatorów w assembly
builder.Services.AddValidatorsFromAssemblyContaining<RestaurantQueryValidator>();

// autentykacja i wczytanie z appsetting.json
var authenticationsSetting = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationsSetting);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false; // Możesz zmienić na true w zależności od wymagań
    cfg.SaveToken = true; // Zapisujemy token

    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationsSetting.JwtIssuer,
        ValidAudience = authenticationsSetting.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationsSetting.JwtKey)),
        
        // Ustawienie typu claim dla roli
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HasNationality",builder => builder.RequireClaim("Nationality","Polish","German"));
    options.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
    options.AddPolicy("AtleastCreatedTwoRestaurant", builder => builder.AddRequirements(new MinimumRestaurantCreated(2)));
});


builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, MinimumRestaurantCreatedHandler>();
// Dodanie FluentValidation automatycznie do walidacji modeli
builder.Services.AddFluentValidationAutoValidation(); // Dodanie FluentValidation do ASP.NET Core
builder.Services.AddSingleton(authenticationsSetting);
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();
builder.Services.AddScoped<RestaurantSeeder>(); 
builder.Services.AddScoped<IRestaurantService, RestaurantService>(); 
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IDishService, DishService>(); 
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor(); //  WSZYSTKIEKNIE HTTPCONTEXACCESOR DO NASZEGO USERCONTXSERVICE

builder.Services.AddSwaggerGen();
builder.Logging.ClearProviders(); // Usunięcie domyślnych loggerów
builder.Logging.SetMinimumLevel(LogLevel.Trace); // Ustawienie minimalnego poziomu logowania
builder.Host.UseNLog(); // Używamy NLog

builder.Services.AddDbContext<RestaurantDBContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantDb")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddAutoMapper(typeof(RestaurantMappingProfile).Assembly); // Dodanie serwisów AutoMapper do kontenera zależności

var allowedOrigins = builder.Configuration
    .GetSection("AllowedOrigins")
    .Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseCors("FronFrontEndClient");

// 2. Seedowanie danych
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
    seeder.Seed();
}

// 3. Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API V1");
    });
}

// Middleware na samym początku
// Middleware na samym początku
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();

// 1. Ustawienie autentykacji (sprawdzanie tokenu)
app.UseAuthentication();

// 2. Ustawienie autoryzacji (sprawdzanie ról i uprawnień)
app.UseAuthorization();

// Middleware dla HTTPS
app.UseHttpsRedirection();

app.MapControllers();
app.Run();
