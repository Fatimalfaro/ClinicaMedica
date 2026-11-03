using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs;

public class ActualizarMedicoDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(100, MinimumLength = 2)]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "La matrícula es obligatoria.")]
    [StringLength(50, MinimumLength = 3)]
    public string Matricula { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no es válido.")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [StringLength(30, MinimumLength = 7)]
    [RegularExpression(@"^[0-9+\s()\-]+$")]
    public string Telefono { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "99999999.99",
        ErrorMessage = "El precio de consulta debe ser mayor que cero.")]
    public decimal PrecioConsulta { get; set; }

    [Range(1, int.MaxValue)]
    public int EspecialidadId { get; set; }
}