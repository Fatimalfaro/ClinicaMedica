namespace ClinicaMedica.Application.DTOs;

public class HistoriaClinicaDto
{
    public int Id { get; set; }
    public int PacienteId { get; set; }
    public int MedicoId { get; set; }
    public DateTime FechaCreacion { get; set; }
}