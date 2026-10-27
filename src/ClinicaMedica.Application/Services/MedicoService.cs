
using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Services;

public class MedicoService
{
    private readonly IMedicoRepository _repository;

    public MedicoService(IMedicoRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Medico>> ObtenerTodosAsync()
    {
        return _repository.ObtenerTodosAsync();
    }

    public Task<Medico?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task AgregarAsync(Medico medico)
    {
        return _repository.AgregarAsync(medico);
    }

    public Task ActualizarAsync(Medico medico)
    {
        return _repository.ActualizarAsync(medico);
    }

    public Task EliminarAsync(Medico medico)
    {
        return _repository.EliminarAsync(medico);
    }
}
