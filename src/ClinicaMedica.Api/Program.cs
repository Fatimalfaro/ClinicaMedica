
using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar los controladores
builder.Services.AddControllers();

// Configurar la conexion con MySQL
var connectionString = builder.Configuration
    .GetConnectionString("ClinicaMedica");

builder.Services.AddDbContext<ClinicaMedicaContext>(options =>
    options.UseMySQL(connectionString!));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configurar los endpoints de OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
