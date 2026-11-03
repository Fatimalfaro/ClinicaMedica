using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs;

public class CrearHistoriaClinicaDto
{
    [Range(1, int.MaxValue, ErrorMessage = "El paciente indicado no es válido.")]
    public int PacienteId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El médico indicado no es válido.")]
    public int MedicoId { get; set; }
}