using ClinicaMedica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Infrastructure.Repositories;
using ClinicaMedica.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

builder.Services.AddScoped<IDiagnosticoRepository, DiagnosticoRepository>();
builder.Services.AddScoped<DiagnosticoService>();

builder.Services.AddScoped<MercadoPagoService>();
builder.Services.AddScoped<IMercadoPagoService>(
    provider => provider.GetRequiredService<MercadoPagoService>()
);

builder.Services.AddScoped<VerificadorWebhook>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey) ||
    string.IsNullOrWhiteSpace(jwtIssuer) ||
    string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException(
        "Falta configurar Jwt:Key, Jwt:Issuer o Jwt:Audience.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configurar los endpoints de OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
