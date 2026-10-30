using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface IMercadoPagoService
{
    Task<(
        string PreferenciaId,
        string InitPoint,
        string SandboxInitPoint)> CrearPreferenciaAsync(Pago pago);

    Task<(string Estado, string Referencia, decimal Monto)>
        ConsultarPagoAsync(long pagoExternoId);
}