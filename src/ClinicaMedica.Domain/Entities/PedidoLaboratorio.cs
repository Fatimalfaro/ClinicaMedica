
namespace ClinicaMedica.Domain.Entities;

public class PedidoLaboratorio
{
    public int Id { get; set; }

    public int HistoriaClinicaId { get; set; }

    public HistoriaClinica? HistoriaClinica { get; set; }

    public DateTime FechaSolicitud { get; set; } = DateTime.Now;

    public string EstudiosSolicitados { get; set; } = string.Empty;

    public string Estado { get; set; } = "Pendiente";
}
