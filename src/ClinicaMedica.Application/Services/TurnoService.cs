using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Services;

public class TurnoService
{
    private readonly ITurnoRepository _turnoRepository;

    public TurnoService(ITurnoRepository turnoRepository)
    {
        _turnoRepository = turnoRepository;
    }

    public Task<List<Turno>> ObtenerTodosAsync()
    {
        return _turnoRepository.ObtenerTodosAsync();
    }

    public Task<Turno?> ObtenerPorIdAsync(int id)
    {
        return _turnoRepository.ObtenerPorIdAsync(id);
    }

    public Task AgregarAsync(Turno turno)
    {
        return _turnoRepository.AgregarAsync(turno);
    }

    public Task ActualizarAsync(Turno turno)
    {
        return _turnoRepository.ActualizarAsync(turno);
    }

    public Task EliminarAsync(Turno turno)
    {
        return _turnoRepository.EliminarAsync(turno);
    }

    public Task<bool> ExistePacienteAsync(int pacienteId)
    {
        return _turnoRepository.ExistePacienteAsync(pacienteId);
    }

    public Task<bool> ExisteMedicoAsync(int medicoId)
    {
        return _turnoRepository.ExisteMedicoAsync(medicoId);
    }


    public Task<List<Turno>> ObtenerPorPacienteUsuarioAsync(int usuarioId)
    {
        return _turnoRepository.ObtenerPorPacienteUsuarioAsync(usuarioId);
    }

    public Task<List<Turno>> ObtenerPorMedicoUsuarioAsync(int usuarioId)
    {
        return _turnoRepository.ObtenerPorMedicoUsuarioAsync(usuarioId);
    }

    public Task<Turno?> ObtenerPorIdYPacienteUsuarioAsync(
        int turnoId, int usuarioId)
    {
        return _turnoRepository.ObtenerPorIdYPacienteUsuarioAsync(
            turnoId, usuarioId);
    }

    public Task<Turno?> ObtenerPorIdYMedicoUsuarioAsync(
        int turnoId, int usuarioId)
    {
        return _turnoRepository.ObtenerPorIdYMedicoUsuarioAsync(
            turnoId, usuarioId);
    }

    public Task<Paciente?> ObtenerPacientePorUsuarioIdAsync(int usuarioId)
    {
        return _turnoRepository.ObtenerPacientePorUsuarioIdAsync(usuarioId);
    }

}