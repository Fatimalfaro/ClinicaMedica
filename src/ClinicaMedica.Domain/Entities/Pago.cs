
namespace ClinicaMedica.Domain.Entities;

public class Pago
{
    public int Id { get; set; }

    public int TurnoId { get; set; }

    public Turno? Turno { get; set; }

    public decimal Monto { get; set; }

    public string Estado { get; set; } = "Pendiente";

    public string? PreferenciaPagoId { get; set; }

    public string? PagoExternoId { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}
