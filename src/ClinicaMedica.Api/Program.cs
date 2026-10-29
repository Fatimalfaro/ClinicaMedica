using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Registrar los controladores
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Configurar la conexion con MySQL
var connectionString = builder.Configuration
    .GetConnectionString("ClinicaMedica");

builder.Services.AddDbContext<ClinicaMedicaContext>(options =>
    options.UseMySQL(connectionString!));

builder.Services.AddScoped<IEspecialidadRepository, EspecialidadRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();

builder.Services.AddScoped<EspecialidadService>();
builder.Services.AddScoped<MedicoService>();

builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<PacienteService>();

builder.Services.AddScoped<ITurnoRepository, TurnoRepository>();
builder.Services.AddScoped<TurnoService>();

builder.Services.AddScoped<IHistoriaClinicaRepository,HistoriaClinicaRepository>();
builder.Services.AddScoped<HistoriaClinicaService>();

builder.Services.AddScoped<IEvolucionRepository, EvolucionRepository>();
builder.Services.AddScoped<EvolucionService>();

builder.Services.AddScoped<IRecetaRepository, RecetaRepository>();
builder.Services.AddScoped<RecetaService>();

builder.Services.AddScoped<IPedidoLaboratorioRepository, PedidoLaboratorioRepository>();
builder.Services.AddScoped<PedidoLaboratorioService>();

builder.Services.AddScoped<IPagoRepository, PagoRepository>();
builder.Services.AddScoped<PagoService>();

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
