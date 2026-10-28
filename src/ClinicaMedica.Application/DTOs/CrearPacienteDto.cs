using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs;

public class CrearPacienteDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [StringLength(20, MinimumLength = 7)]
    public string DNI { get; set; } = string.Empty;

    [Required]
    public DateTime FechaNacimiento { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Telefono { get; set; } = string.Empty;
}
