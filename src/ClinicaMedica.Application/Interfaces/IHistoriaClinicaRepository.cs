using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface IHistoriaClinicaRepository
{
    Task<List<HistoriaClinica>> ObtenerTodosAsync();
    Task<HistoriaClinica?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(HistoriaClinica historiaClinica);
    Task ActualizarAsync(HistoriaClinica historiaClinica);
    Task EliminarAsync(HistoriaClinica historiaClinica);

    Task<bool> ExistePacienteAsync(int pacienteId);
    Task<bool> ExisteMedicoAsync(int medicoId);
}