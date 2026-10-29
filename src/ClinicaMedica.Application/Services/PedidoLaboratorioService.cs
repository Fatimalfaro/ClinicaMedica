using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Services;

public class PedidoLaboratorioService
{
    private readonly IPedidoLaboratorioRepository _repository;

    public PedidoLaboratorioService(
        IPedidoLaboratorioRepository repository)
    {
        _repository = repository;
    }

    public Task<List<PedidoLaboratorio>> ObtenerTodosAsync()
    {
        return _repository.ObtenerTodosAsync();
    }

    public Task<PedidoLaboratorio?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task AgregarAsync(PedidoLaboratorio pedido)
    {
        return _repository.AgregarAsync(pedido);
    }

    public Task ActualizarAsync(PedidoLaboratorio pedido)
    {
        return _repository.ActualizarAsync(pedido);
    }

    public Task EliminarAsync(PedidoLaboratorio pedido)
    {
        return _repository.EliminarAsync(pedido);
    }

    public Task<bool> ExisteHistoriaClinicaAsync(int historiaClinicaId)
    {
        return _repository.ExisteHistoriaClinicaAsync(historiaClinicaId);
    }
}