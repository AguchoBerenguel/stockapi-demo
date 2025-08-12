using Microsoft.EntityFrameworkCore;
using StockApi.Data; 
using StockApi;

var builder = WebApplication.CreateBuilder(args);

// Usa MYSQLCONN si está definido (producción), sino usa DefaultConnection (local)
var connectionString = Environment.GetEnvironmentVariable("MYSQLCONN") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de EF Core con MySQL
builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configuración de CORS para permitir frontend de GitHub Pages
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

// Middlewares para servir frontend estático
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowFrontend");

app.UseAuthorization();

// Swagger solo para debug
app.UseSwagger();
app.UseSwaggerUI();

// Rutas de la API
app.MapControllers();

// Ejecuta migraciones al iniciar
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StockDbContext>();
    context.Database.Migrate(); 
}

// Usa el puerto de Railway o 5000 por defecto
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");