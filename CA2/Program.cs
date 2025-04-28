using Microsoft.EntityFrameworkCore;
using CA2.Data;
using CA2.Middleware;
using CA2.Services;
using CA2.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Threading.RateLimiting;

namespace CA2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(e => e.Value?.Errors.Count > 0)
                            .Select(e => new
                            {
                                Field = e.Key,
                                Message = e.Value?.Errors.First().ErrorMessage
                            });

                        return new BadRequestObjectResult(new
                        {
                            error = new
                            {
                                message = "Validation failed",
                                details = errors
                            }
                        });
                    };
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add JWT authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration.")))
                    };
                });

            // Add authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });

            // Add CORS with specific policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000") // Add your frontend URL
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    });
            });

            // Configure HTTPS port
            var port = builder.Configuration.GetValue<int>("HTTPS_PORT", 7185);
            builder.Services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = port;
            });

            // Add caching
            builder.Services.AddResponseCaching();

            // Add rate limiting
            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("fixed", options =>
                {
                    options.PermitLimit = 100;
                    options.Window = TimeSpan.FromMinutes(1);
                    options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 2;
                });
            });

            // Add health checks
            builder.Services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("database");

            // Register AuthService
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Add DbContext
            builder.Services.AddDbContext<FootballContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Use CORS
            app.UseCors("AllowSpecificOrigins");

            // Use authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Use exception handling middleware
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            // Use response caching
            app.UseResponseCaching();

            // Use rate limiting
            app.UseRateLimiter();

            // Add health check endpoints
            app.MapHealthChecks("/health");
            app.MapHealthChecks("/health/detailed", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(e => new
                        {
                            name = e.Key,
                            status = e.Value.Status.ToString(),
                            description = e.Value.Description,
                            exception = e.Value.Exception?.Message
                        })
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            });

            app.MapControllers();

            // Seed the database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<FootballContext>();
                DataSeeder.SeedData(context);
            }

            app.Run();
        }
    }
}
