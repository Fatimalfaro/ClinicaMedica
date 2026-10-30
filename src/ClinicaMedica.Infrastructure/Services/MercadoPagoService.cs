using ClinicaMedica.Domain.Entities;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using Microsoft.Extensions.Configuration;
using MercadoPago.Client.Payment;
using ClinicaMedica.Application.Interfaces;
namespace ClinicaMedica.Infrastructure.Services;

public class MercadoPagoService : IMercadoPagoService
{
    private readonly IConfiguration _configuration;

    public MercadoPagoService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<(
    string PreferenciaId,
    string InitPoint,
    string SandboxInitPoint)> CrearPreferenciaAsync(Pago pago)
    {
        var accessToken = _configuration["MercadoPago:AccessToken"];

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new InvalidOperationException(
                "No está configurado el Access Token de Mercado Pago.");
        }

        MercadoPagoConfig.AccessToken = accessToken;

        var cliente = new PreferenceClient();

        var solicitud = new PreferenceRequest
        {
            ExternalReference = pago.Id.ToString(),
            Items = new List<PreferenceItemRequest>
            {
                new()
                {
                    Title = $"Consulta médica - Turno {pago.TurnoId}",
                    Quantity = 1,
                    UnitPrice = pago.Monto,
                    CurrencyId = "ARS"
                }
            }
        };

        var preferencia = await cliente.CreateAsync(solicitud);

        if (string.IsNullOrWhiteSpace(preferencia.Id) ||
            string.IsNullOrWhiteSpace(preferencia.InitPoint) ||
            string.IsNullOrWhiteSpace(preferencia.SandboxInitPoint))
        {
            throw new InvalidOperationException(
                "Mercado Pago no devolvió los datos de la preferencia");
        }

        return (preferencia.Id,
                preferencia.InitPoint,
                preferencia.SandboxInitPoint);
    }

    public async Task<(string Estado, string Referencia, decimal Monto)>
    ConsultarPagoAsync(long pagoExternoId)
{
    var accessToken = _configuration["MercadoPago:AccessToken"];

    if (string.IsNullOrWhiteSpace(accessToken))
    {
        throw new InvalidOperationException(
            "No está configurado el Access Token de Mercado Pago.");
    }

    MercadoPagoConfig.AccessToken = accessToken;

    var cliente = new PaymentClient();
    var pago = await cliente.GetAsync(pagoExternoId);

    return (
        pago.Status ?? string.Empty,
        pago.ExternalReference ?? string.Empty,
        pago.TransactionAmount ?? 0m
    );
}

    
}