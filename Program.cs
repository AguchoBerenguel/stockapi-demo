using Microsoft.EntityFrameworkCore;
using StockApi.Data;
using StockApi;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:80");

// Cargar cadena de conexión de appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Usar MySQL en lugar de SQLite
builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configurar CORS
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

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var mysqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
    var migrador = new MigrationService(mysqlConnection);
    await migrador.MigrarAsync();
}

app.Run();
