using System.Text;
using CamQuizz.Application;
using CamQuizz.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Claims;
using CamQuizz.Application.Interfaces;
using CamQuizz.Infrastructure.Cloudinary;
using CamQuizz.Infrastructure.SignalR;
using CamQuizz.Presentation.Hubs;

var builder = WebApplication.CreateBuilder(args);

//builder.AddServiceDefaults();

// ----------------------------
// 1. Configure Serilog
// ----------------------------
builder.Host.UseSerilog(
    (context, config) =>
    {
        config.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext();
    }
);

// ----------------------------
// 2. Add Services
// ----------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Optional: Add Swagger for API docs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mapp
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Optional: Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder
            .WithOrigins("http://localhost:3001") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
var jwtKey = builder.Configuration["Jwt:Key"] ?? "super_secret_key";
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role
        };
        opt.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddScoped<IAuthorizationHandler, BanCheckHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("NotBanned", policy =>
        policy.Requirements.Add(new NotBannedRequirement()));
});

builder.Services.AddSwaggerGen(opt =>
{
    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Reference = new()
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme,
        },
    };
    opt.AddSecurityDefinition(scheme.Reference.Id, scheme);
    opt.AddSecurityRequirement(
        new OpenApiSecurityRequirement { { scheme, Array.Empty<string>() } }
    );
});

builder.Services.AddApplicationServices()
    .AddPersistence(builder.Configuration)
    .AddCloudinaryInfrastructure(builder.Configuration)
    .AddSignalRInfrastructure()
    .AddSignalR();
// ----------------------------
// Build application
// ----------------------------
var app = builder.Build();


// ----------------------------
// Configure Middleware
// ----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chat");

app.Run();
