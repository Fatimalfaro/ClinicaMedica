
using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs.Autenticacion;

public class RegistroUsuarioDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100, MinimumLength = 2)]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [StringLength(100, MinimumLength = 8,
        ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "El DNI es obligatorio")]
    [RegularExpression(@"^\d{7,8}$",
        ErrorMessage = "El DNI debe tener 7 u 8 números")]
    public string DNI { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public DateTime FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [StringLength(30)]
    public string Telefono { get; set; } = string.Empty;
}
