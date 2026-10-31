using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface ITurnoRepository
{
    Task<List<Turno>> ObtenerTodosAsync();

    Task<Turno?> ObtenerPorIdAsync(int id);

    Task<List<Turno>> ObtenerPorPacienteUsuarioAsync(int usuarioId);

    Task<List<Turno>> ObtenerPorMedicoUsuarioAsync(int usuarioId);

    Task<Turno?> ObtenerPorIdYPacienteUsuarioAsync(int turnoId, int usuarioId);

    Task<Turno?> ObtenerPorIdYMedicoUsuarioAsync(int turnoId, int usuarioId);

    Task AgregarAsync(Turno turno);

    Task ActualizarAsync(Turno turno);

    Task EliminarAsync(Turno turno);

    Task<bool> ExistePacienteAsync(int pacienteId);

    Task<bool> ExisteMedicoAsync(int medicoId);

    Task<Paciente?> ObtenerPacientePorUsuarioIdAsync(int usuarioId);
}
