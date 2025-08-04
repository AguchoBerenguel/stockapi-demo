using Microsoft.EntityFrameworkCore;
using StockApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:80");

// Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseSqlite("Data Source=database.db"));

builder.Services.AddCors();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("https://aguchoberenguel.github.io/StockFrontend-demo/") 
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
