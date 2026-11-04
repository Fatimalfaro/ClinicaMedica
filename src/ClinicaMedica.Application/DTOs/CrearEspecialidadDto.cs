using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs;

public class CrearEspecialidadDto
{
    [Required(ErrorMessage = "El nombre de la especialidad es obligatorio.")]
    [StringLength(100, MinimumLength = 3)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    public string Descripcion { get; set; } = string.Empty;
}