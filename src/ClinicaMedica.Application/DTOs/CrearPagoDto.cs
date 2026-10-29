using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs;

public class CrearPagoDto
{
    [Range(1, int.MaxValue)]
    public int TurnoId { get; set; }
}