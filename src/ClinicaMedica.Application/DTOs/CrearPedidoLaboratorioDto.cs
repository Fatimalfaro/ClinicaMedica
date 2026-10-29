using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.DTOs;

public class CrearPedidoLaboratorioDto
{
    [Range(1, int.MaxValue)]
    public int HistoriaClinicaId { get; set; }

    [Required]
    [StringLength(2000, MinimumLength = 3)]
    public string EstudiosSolicitados { get; set; } = string.Empty;
}