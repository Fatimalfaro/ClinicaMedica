
using ClinicaMedica.Application.DTOs.Autenticacion;

namespace ClinicaMedica.Application.Interfaces;

public interface IAuthService
{
    Task<RespuestaAutenticacionDto> RegistrarAsync(
        RegistroUsuarioDto dto);

    Task<RespuestaAutenticacionDto> LoginAsync(
        LoginUsuarioDto dto);
}
