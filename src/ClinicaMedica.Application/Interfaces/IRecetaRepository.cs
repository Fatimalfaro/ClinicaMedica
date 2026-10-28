using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Interfaces;

public interface IRecetaRepository
{
    Task<List<Receta>> ObtenerTodosAsync();
    Task<Receta?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(Receta receta);
    Task ActualizarAsync(Receta receta);
    Task EliminarAsync(Receta receta);
    Task<bool> ExisteHistoriaClinicaAsync(int historiaClinicaId);
}