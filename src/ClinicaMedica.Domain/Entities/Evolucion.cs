namespace ClinicaMedica.Domain.Entities;

public class Evolucion
{
    public int Id { get; set; }

    public int HistoriaClinicaId { get; set; }

    public HistoriaClinica? HistoriaClinica { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;

    public string Descripcion { get; set; } = string.Empty;
}
