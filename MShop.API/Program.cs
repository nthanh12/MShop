using MShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MShop.Application.Interfaces;
using MShop.Infrastructure.Repository;
using MShop.Application.Services;
using MShop.Application.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using MShop.Domain.Entities;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Them dich vu JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])) // Secret key
        };
    });
// Them dich vu AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Cau hinh Identity
builder.Services.AddIdentity<SystemUser, IdentityRole>()
    .AddEntityFrameworkStores<ConnectDbContext>()
    .AddDefaultTokenProviders();

// Them DbContext
builder.Services.AddDbContext<MShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MShop"))
);
builder.Services.AddDbContext<ConnectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MShop")));

// Them Service
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddLogging(config =>
{
    config.ClearProviders();  
    config.AddConsole();      
    config.AddDebug();       
    config.AddEventSourceLogger(); 
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình Swagger để hỗ trợ Bearer Token
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
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
            new string[] {}
        }
    });
});


var app = builder.Build();


// Them Role Identity
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// Configure the HTTP request pipeline.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MShop API V1");
        options.OAuthClientId("swagger"); 
        options.OAuthAppName("Swagger API Client");
    });
}


app.UseHttpsRedirection();

// Them Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
