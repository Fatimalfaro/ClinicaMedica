namespace ClinicaMedica.Application.DTOs;

public class DiagnosticoDto
{
    public int Id { get; set; }

    public int HistoriaClinicaId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public DateTime FechaDiagnostico { get; set; }
}