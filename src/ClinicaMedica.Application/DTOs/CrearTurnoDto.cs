using System.ComponentModel.DataAnnotations;
using ClinicaMedica.Application.Validaciones;

namespace ClinicaMedica.Application.DTOs;

public class CrearTurnoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "El paciente indicado no es válido.")]
    public int PacienteId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El médico indicado no es válido.")]
    public int MedicoId { get; set; }

    [FechaTurnoValida]
    public DateTime FechaHora { get; set; }
}