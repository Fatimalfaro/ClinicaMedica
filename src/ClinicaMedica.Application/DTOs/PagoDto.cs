namespace ClinicaMedica.Application.DTOs;

public class PagoDto
{
    public int Id { get; set; }
    public int TurnoId { get; set; }
    public decimal Monto { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? PreferenciaPagoId { get; set; }
    public string? PagoExternoId { get; set; }
    public DateTime FechaCreacion { get; set; }
}