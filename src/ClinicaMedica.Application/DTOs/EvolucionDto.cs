namespace ClinicaMedica.Application.DTOs;

public class EvolucionDto
{
    public int Id { get; set; }
    public int HistoriaClinicaId { get; set; }
    public DateTime Fecha { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}