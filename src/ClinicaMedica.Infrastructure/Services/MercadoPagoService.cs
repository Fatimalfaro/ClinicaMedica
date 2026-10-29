using ClinicaMedica.Domain.Entities;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using Microsoft.Extensions.Configuration;

namespace ClinicaMedica.Infrastructure.Services;

public class MercadoPagoService
{
    private readonly IConfiguration _configuration;

    public MercadoPagoService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<(string PreferenciaId, string Url)> CrearPreferenciaAsync(
        Pago pago)
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
            string.IsNullOrWhiteSpace(preferencia.InitPoint))
        {
            throw new InvalidOperationException(
                "Mercado Pago no devolvió el ID y la URL de checkout.");
        }

        return (preferencia.Id, preferencia.InitPoint);
    }
}