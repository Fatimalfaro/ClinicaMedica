using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs;

public class CrearDiagnosticoDto
{
    [Range(1, int.MaxValue)]
    public int HistoriaClinicaId { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 5)]
    public string Descripcion { get; set; } = string.Empty;
}