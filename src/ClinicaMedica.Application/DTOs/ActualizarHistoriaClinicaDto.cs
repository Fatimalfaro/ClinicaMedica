using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs;

public class ActualizarHistoriaClinicaDto
{
    [Range(1, int.MaxValue)]
    public int PacienteId { get; set; }

    [Range(1, int.MaxValue)]
    public int MedicoId { get; set; }
}