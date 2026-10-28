using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs;

public class CrearEvolucionDto
{
    [Range(1, int.MaxValue)]
    public int HistoriaClinicaId { get; set; }

    [Required]
    [StringLength(2000, MinimumLength = 5)]
    public string Descripcion { get; set; } = string.Empty;
}