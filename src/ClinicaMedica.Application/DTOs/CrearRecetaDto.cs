using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs;

public class CrearRecetaDto
{
    [Range(1, int.MaxValue)]
    public int HistoriaClinicaId { get; set; }

    [Required]
    [StringLength(2000, MinimumLength = 5)]
    public string Indicaciones { get; set; } = string.Empty;
}