using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs;

public class ActualizarPedidoLaboratorioDto
{
    [Required(ErrorMessage = "Los estudios solicitados son obligatorios.")]
    [StringLength(2000, MinimumLength = 3)]
    public string EstudiosSolicitados { get; set; } = string.Empty;
}