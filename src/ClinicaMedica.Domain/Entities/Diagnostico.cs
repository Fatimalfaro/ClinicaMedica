using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Domain.Entities;

public class Diagnostico
{
    public int Id { get; set; }

    [Required]
    public int HistoriaClinicaId { get; set; }

    public HistoriaClinica? HistoriaClinica { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Descripcion { get; set; } = string.Empty;

    public DateTime FechaDiagnostico { get; set; } = DateTime.Now;
}