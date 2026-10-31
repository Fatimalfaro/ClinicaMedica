
namespace ClinicaMedica.Application.Interfaces;

public interface ITokenService
{
    string GenerarToken(int usuarioId, string email, string rol);
}
