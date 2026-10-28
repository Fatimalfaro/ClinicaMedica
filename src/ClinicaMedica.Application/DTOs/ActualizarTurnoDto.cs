using System.ComponentModel.DataAnnotations;
using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.DTOs;

public class ActualizarTurnoDto
{
    [Range(1, int.MaxValue)]
    public int PacienteId { get; set; }

    [Range(1, int.MaxValue)]
    public int MedicoId { get; set; }

    [Required]
    public DateTime FechaHora { get; set; }

    public EstadoTurno Estado { get; set; }
}