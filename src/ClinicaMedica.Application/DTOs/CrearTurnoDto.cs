using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs;

public class CrearTurnoDto
{
    [Range(1, int.MaxValue)]
    public int PacienteId { get; set; }

    [Range(1, int.MaxValue)]
    public int MedicoId { get; set; }

    [Required]
    public DateTime FechaHora { get; set; }
}