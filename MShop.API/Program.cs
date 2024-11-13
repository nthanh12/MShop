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
            ValidIssuer = builder.Configuration["Jwt:Issuer"],   // ??a ch? Issuer
            ValidAudience = builder.Configuration["Jwt:Audience"], // ??a ch? Audience
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
    config.ClearProviders();  // X�a c�c provider m?c ??nh, n?u kh�ng c?n thi?t
    config.AddConsole();      // Th�m Console logger
    config.AddDebug();        // Th�m Debug logger
    config.AddEventSourceLogger(); // Th�m EventSource logger (c� th? d�ng cho c�c h? th?ng logging ph?c t?p h?n)
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Them Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
