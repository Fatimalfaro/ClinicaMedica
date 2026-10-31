using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ClinicaMedica.Api.Controllers;

[ApiController]
[AllowAnonymous]

[Route("api/notificaciones/mercado-pago")]
public class NotificacionesPagoController : ControllerBase
{
    private readonly PagoService _pagoService;
    private readonly IMercadoPagoService _mercadoPagoService;
    private readonly VerificadorWebhook _verificador;
    private readonly IConfiguration _configuration;

    public NotificacionesPagoController(
        PagoService pagoService,
        IMercadoPagoService mercadoPagoService,
        VerificadorWebhook verificador,
        IConfiguration configuration)
    {
        _pagoService = pagoService;
        _mercadoPagoService = mercadoPagoService;
        _verificador = verificador;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> RecibirNotificacion()
    {
        var tipo = Request.Query["type"].ToString();
        var dataId = Request.Query["data.id"].ToString();

        if (string.IsNullOrWhiteSpace(tipo) ||
            !tipo.Equals("payment", StringComparison.OrdinalIgnoreCase) ||
            !long.TryParse(dataId, out var idExterno))
        {
            return BadRequest();
        }

        var firma = Request.Headers["x-signature"].ToString();
        var requestId = Request.Headers["x-request-id"].ToString();
        var secreto = _configuration["MercadoPago:WebhookSecret"];

        var firmaValida = _verificador.Validar(
            firma,
            requestId,
            dataId,
            secreto);

        if (!firmaValida)
        {
            return Unauthorized();
        }

        try
        {
            var pagoExterno = await _mercadoPagoService
                .ConsultarPagoAsync(idExterno);

            if (!int.TryParse(pagoExterno.Referencia, out var pagoLocalId))
            {
                return BadRequest();
            }

            var pagoLocal = await _pagoService
                .ObtenerPagoParaIntegracionAsync(pagoLocalId);

            if (pagoLocal == null)
            {
                return NotFound();
            }

            if (pagoLocal.Monto != pagoExterno.Monto)
            {
                return BadRequest(new
                {
                    mensaje = "El monto del pago no coincide."
                });
            }

            var estado = pagoExterno.Estado.ToLowerInvariant();

            switch (estado)
            {
                case "approved":
                    pagoLocal.Estado = "Aprobado";
                    break;

                case "rejected":
                    pagoLocal.Estado = "Rechazado";
                    break;

                case "cancelled":
                    pagoLocal.Estado = "Cancelado";
                    break;

                case "pending":
                case "in_process":
                case "authorized":
                    pagoLocal.Estado = "Pendiente";
                    break;

                default:
                    return Ok();
            }

            pagoLocal.PagoExternoId = idExterno.ToString();

            await _pagoService.GuardarActualizacionAsync(pagoLocal);

            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error procesando webhook: {ex}");

            return StatusCode(500);
        }
    }
}