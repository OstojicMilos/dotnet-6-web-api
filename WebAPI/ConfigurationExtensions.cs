using CleanArchitecture.Application.IServices;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Core.IRepositories;
using CleanArchitecture.DataAccess.Database;
using CleanArchitecture.DataAccess.Repositories;
using CleanArchitecture.WebAPI.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.WebAPI
{
    public static class ConfigurationExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("connection") ?? "Data Source=App.db";

            services.AddSqlite<ApplicationContext>(connectionString);
            services.AddTransient<JwtService>();
            services.AddTransient<IAuthorizationHandler, ValidTokenAuthorizationHandler>();

            services.AddScoped<IDevicesRepository, DevicesRepository>();
            services.AddScoped<IDeviceReadingsRepository, DeviceReadingsRepository>();
            services.AddScoped<IDeviceRegistrationsRepository, DeviceRegistrationsRepository>();
            services.AddScoped<IAlertsRepository, AlertsRepository>();

            services.AddScoped<IDevicesService, DevicesService>();
            services.AddScoped<IAlertsService, AlertsService>();
        }

        public static void AddOpenApiDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Web API",
                    Description = "Device Reporting API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "BearerAuth"
                        }
                    },
                    Array.Empty<string>()
                }
                });
            });
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var signingKey = configuration["Jwt:Key"];

            services.AddAuthorization();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidIssuer = issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeviceAdmin", policy =>
                    policy.RequireRole(JwtService.JwtScopeDeviceAdminService)
                );

                options.AddPolicy("DeviceIngestion", policy =>
                {
                    policy.RequireRole(JwtService.JwtScopeDeviceIngestionService);
                    policy.AddRequirements(new ValidTokenRequirement());
                });
            });

            services.AddHttpContextAccessor();
        }

        public static void UseOpenApiDocumentation(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API V1"); });
        }

        public static void MapControllers(this WebApplication app)
        {
            ControllerEndpointRouteBuilderExtensions.MapControllers(app);
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
        }

        public static void EnsureDatabaseSetup(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var db = services.GetRequiredService<ApplicationContext>();
            db.Database.EnsureCreated();
            DataSeeder.Seed(db);
        }
    }
}
