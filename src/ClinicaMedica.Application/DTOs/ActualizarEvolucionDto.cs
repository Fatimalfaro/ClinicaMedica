using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs;

public class ActualizarEvolucionDto
{
    [Range(1, int.MaxValue)]
    public int HistoriaClinicaId { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(2000, MinimumLength = 5)]
    public string Descripcion { get; set; } = string.Empty;
}