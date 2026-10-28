using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.DTOs;

public class TurnoDto
{
    public int Id { get; set; }
    public int PacienteId { get; set; }
    public int MedicoId { get; set; }
    public DateTime FechaHora { get; set; }
    public EstadoTurno Estado { get; set; }
}