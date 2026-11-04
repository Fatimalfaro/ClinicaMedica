using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs.Autenticacion;

public class LoginUsuarioDto
{
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no es válido.")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
}