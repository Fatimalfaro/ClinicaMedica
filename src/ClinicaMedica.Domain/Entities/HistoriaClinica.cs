namespace ClinicaMedica.Domain.Entities;

public class HistoriaClinica
{
    public int Id { get; set; }

    public int PacienteId { get; set; }

    public Paciente? Paciente { get; set; }

    public int MedicoId { get; set; }

    public Medico? Medico { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public ICollection<Diagnostico> Diagnosticos { get; set; }
    = new List<Diagnostico>();
    public ICollection<Evolucion> Evoluciones { get; set; } = new List<Evolucion>();


}
