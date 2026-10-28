using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface IPacienteRepository
{
    Task<List<Paciente>> ObtenerTodosAsync();

    Task<Paciente?> ObtenerPorIdAsync(int id);

    Task AgregarAsync(Paciente paciente);

    Task ActualizarAsync(Paciente paciente);

    Task EliminarAsync(Paciente paciente);
}
