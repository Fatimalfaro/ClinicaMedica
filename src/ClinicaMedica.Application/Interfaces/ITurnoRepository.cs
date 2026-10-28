using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface ITurnoRepository
{
    Task<List<Turno>> ObtenerTodosAsync();
    Task<Turno?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(Turno turno);
    Task ActualizarAsync(Turno turno);
    Task EliminarAsync(Turno turno);

    Task<bool> ExistePacienteAsync(int pacienteId);
    Task<bool> ExisteMedicoAsync(int medicoId);
}