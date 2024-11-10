using MShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MShop.Application.Interfaces;
using MShop.Infrastructure.Repository;
using MShop.Application.Services;
using MShop.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<MShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MShop"))
);

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
