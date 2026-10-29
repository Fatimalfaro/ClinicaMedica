using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Services;

public class DiagnosticoService
{
    private readonly IDiagnosticoRepository _repository;

    public DiagnosticoService(IDiagnosticoRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Diagnostico>> ObtenerTodosAsync()
    {
        return _repository.ObtenerTodosAsync();
    }

    public Task<Diagnostico?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task<List<Diagnostico>> ObtenerPorHistoriaClinicaAsync(
        int historiaClinicaId)
    {
        return _repository.ObtenerPorHistoriaClinicaAsync(
            historiaClinicaId);
    }

    public Task<bool> ExisteHistoriaClinicaAsync(int historiaClinicaId)
    {
        return _repository.ExisteHistoriaClinicaAsync(historiaClinicaId);
    }

    public Task AgregarAsync(Diagnostico diagnostico)
    {
        return _repository.AgregarAsync(diagnostico);
    }
}