namespace ClinicaMedica.Domain.Entities;

public class Receta
{
    public int Id { get; set; }

    public int HistoriaClinicaId { get; set; }

    public HistoriaClinica? HistoriaClinica { get; set; }

    public DateTime FechaEmision { get; set; } = DateTime.Now;

    public string Indicaciones { get; set; } = string.Empty;
}
