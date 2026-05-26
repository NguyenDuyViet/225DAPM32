using Backend.Data;
using Backend.Extensions;
using Backend.Hubs;
using Backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Repositories
builder.Services.AddRepositories();
// Add Services
builder.Services.AddApplicationServices();

// SignalR
builder.Services.AddSignalR();



// ==============================
// Swagger + JWT
// ==============================

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Backend API",
            Version = "v1"
        }
    );

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter JWT Token"
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
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
                Array.Empty<string>()
            }
        }
    );
});


// ==============================
// Database
// ==============================

// // builder.Services.AddDbContext<AppDbContext>(options =>
// //     options.UseMySql(
// //         builder.Configuration.GetConnectionString("MySqlConnection"),
// //         new MySqlServerVersion(new Version(8, 0, 21)),
// //         mySqlOptions => mySqlOptions.EnableRetryOnFailure(
// //             maxRetryCount: 5,
// //             maxRetryDelay: TimeSpan.FromSeconds(10),
// //             errorNumbersToAdd: null
// //         )
// //     )
// // );

// Change to SQL Server if needed
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// ==============================
// JWT Authentication
// ==============================

var jwtKey = builder.Configuration["Jwt:Key"]
             ?? throw new Exception("JWT Key is missing");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],

                ValidAudience = builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    )
            };
    });


// ==============================
// Dependency Injection
// ==============================


// ==============================
// CORS
// ==============================

var frontendOrigins = (builder.Configuration["Cors:AllowedOrigins"]
                       ?? "http://localhost:5241;https://localhost:7297")
    .Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins(frontendOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); // Required for SignalR WebSocket
        }
    );
});


// ==============================
// Build App
// ==============================

var app = builder.Build();
var forceSeed = args.Contains("--force-seed", StringComparer.OrdinalIgnoreCase);
var seedOnly = args.Contains("--seed-only", StringComparer.OrdinalIgnoreCase);


// ==============================
// Middleware
// ==============================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


// DEV MODE
// Nếu React bị lỗi CORS/HTTPS thì comment dòng này
// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");


// ==============================
// Database Migration + Seed
// ==============================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        // Auto migrate database
        context.Database.EnsureCreated();

        // Seed sample data
        SeedData.Initialize(context, forceSeed);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();

        logger.LogError(
            ex,
            "An error occurred while migrating/seeding the database."
        );
    }
}


// ==============================
// Run App
// ==============================

if (seedOnly)
{
    return;
}

app.Run();
