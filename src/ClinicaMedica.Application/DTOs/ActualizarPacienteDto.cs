using System.ComponentModel.DataAnnotations;
using ClinicaMedica.Application.Validaciones;

namespace ClinicaMedica.Application.DTOs;

public class ActualizarPacienteDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(100, MinimumLength = 2)]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [RegularExpression(@"^\d{7,8}$",
            ErrorMessage = "El DNI debe tener 7 u 8 números.")]
    public string DNI { get; set; } = string.Empty;

    [FechaNacimientoValida]
    public DateTime FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no es válido.")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [StringLength(30, MinimumLength = 7)]
    [RegularExpression(@"^[0-9+\s()\-]+$",
        ErrorMessage = "El teléfono contiene caracteres no válidos.")]    
    public string Telefono { get; set; } = string.Empty;
}
