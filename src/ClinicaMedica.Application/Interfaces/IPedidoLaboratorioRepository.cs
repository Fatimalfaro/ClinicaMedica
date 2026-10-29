using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface IPedidoLaboratorioRepository
{
    Task<List<PedidoLaboratorio>> ObtenerTodosAsync();
    Task<PedidoLaboratorio?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(PedidoLaboratorio pedido);
    Task ActualizarAsync(PedidoLaboratorio pedido);
    Task EliminarAsync(PedidoLaboratorio pedido);
    Task<bool> ExisteHistoriaClinicaAsync(int historiaClinicaId);
}