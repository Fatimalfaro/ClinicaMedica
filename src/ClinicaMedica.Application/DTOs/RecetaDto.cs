namespace ClinicaMedica.Application.DTOs;

public class RecetaDto
{
    public int Id { get; set; }
    public int HistoriaClinicaId { get; set; }
    public DateTime FechaEmision { get; set; }
    public string Indicaciones { get; set; } = string.Empty;
}