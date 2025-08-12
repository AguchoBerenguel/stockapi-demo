using Microsoft.EntityFrameworkCore;
using StockApi.Data;
using StockApi;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde variable de entorno o appsettings.json
var connectionString = Environment.GetEnvironmentVariable("MYSQLCONN") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext
builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configuración de CORS para permitir tu frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("https://aguchoberenguel.github.io")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Middlewares
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// Solo comprobar conexión a la base de datos (no aplicar migraciones automáticas)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StockDbContext>();
    if (!context.Database.CanConnect())
    {
        Console.WriteLine("No se pudo conectar a la base de datos.");
    }
    else
    {
        Console.WriteLine("Conexión a la base de datos exitosa.");
    }
}

// Puerto de ejecución
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");
