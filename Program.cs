using System.Text;
using CamQuizz.Application;
using CamQuizz.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSignalR();

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

builder.Services.AddApplicationServices().AddPersistence(builder.Configuration);

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
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
