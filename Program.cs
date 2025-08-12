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

// Configuración de CORS para permitir tu frontend de GitHub Pages
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("https://aguchoberenguel.github.io") // Solo el dominio
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Middlewares
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

// Activar CORS antes de Authorization
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// Comprobar conexión a la base de datos
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

// Ejecutar (Railway manejará el puerto y HTTPS)
app.Run();
