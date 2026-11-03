using System.ComponentModel.DataAnnotations;
using ClinicaMedica.Application.Validaciones;
using ClinicaMedica.Domain.Entities;


namespace ClinicaMedica.Application.DTOs;

public class ActualizarTurnoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "El paciente indicado no es válido.")]
    public int PacienteId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El médico indicado no es válido.")]
    public int MedicoId { get; set; }

    [FechaTurnoValida]
    public DateTime FechaHora { get; set; }

     [EnumDataType(typeof(EstadoTurno),
        ErrorMessage = "El estado del turno no es válido.")]
    public EstadoTurno Estado { get; set; }
}