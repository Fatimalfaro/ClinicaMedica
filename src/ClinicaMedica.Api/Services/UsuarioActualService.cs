using System.Security.Claims;
using ClinicaMedica.Application.Interfaces;

namespace ClinicaMedica.Api.Services;

public class UsuarioActualService : IUsuarioActualService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioActualService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int ObtenerIdUsuario()
    {
        var valor = _httpContextAccessor.HttpContext?
            .User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(valor, out int usuarioId))
        {
            throw new UnauthorizedAccessException(
                "No se pudo identificar al usuario autenticado.");
        }

        return usuarioId;
    }

    public string ObtenerRol()
    {
        return _httpContextAccessor.HttpContext?
            .User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
    }
}