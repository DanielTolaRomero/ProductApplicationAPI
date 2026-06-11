using WebApplicationPractica.Data;
using Microsoft.EntityFrameworkCore;
using WebApplicationPractica.Services;
using WebApplicationPractica.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// obtencion de cadena de conexion con la base de datos desde el archivo appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// registro 

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString)
);

// Regstro del servicio de ProductService para inyeccion de dependencias

builder.Services.AddScoped<IProductService, ProductService>();

// Registro del manejador de excepciones personalizado para productos

builder.Services.AddExceptionHandler<ProductExceptionHandler>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Este comando crea el archivo .db y las tablas si no existen
        await context.Database.EnsureCreatedAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al crear la base de datos.");
    }
}

app.Run();
