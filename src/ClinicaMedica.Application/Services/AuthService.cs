
using ClinicaMedica.Application.DTOs.Autenticacion;
using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Domain.Entities;

namespace ClinicaMedica.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<RespuestaAutenticacionDto> RegistrarAsync(
        RegistroUsuarioDto dto)
    {
        string email = dto.Email.Trim().ToLowerInvariant();
        string dni = dto.DNI.Trim();

        if (dto.FechaNacimiento.Date >= DateTime.Today)
        {
            throw new ArgumentException(
                "La fecha de nacimiento debe ser anterior a la fecha actual.");
        }

        if (await _usuarioRepository.ExisteEmailAsync(email))
        {
            throw new ArgumentException(
                "Ya existe una cuenta registrada con ese email.");
        }

        if (await _usuarioRepository.ExisteDniPacienteAsync(dni))
        {
            throw new ArgumentException(
                "Ya existe un paciente registrado con ese DNI.");
        }

        var usuario = new Usuario
        {
            Nombre = dto.Nombre.Trim(),
            Apellido = dto.Apellido.Trim(),
            Email = email,
            PasswordHash = _passwordHasher.HashPassword(dto.Password),
            Rol = RolUsuario.Paciente
        };

        var paciente = new Paciente
        {
            Nombre = dto.Nombre.Trim(),
            Apellido = dto.Apellido.Trim(),
            DNI = dni,
            FechaNacimiento = dto.FechaNacimiento,
            Email = email,
            Telefono = dto.Telefono.Trim(),
            Usuario = usuario
        };

        await _usuarioRepository.RegistrarPacienteAsync(
            usuario,
            paciente);

        return CrearRespuesta(usuario);
    }

    public async Task<RespuestaAutenticacionDto> LoginAsync(
        LoginUsuarioDto dto)
    {
        string email = dto.Email.Trim().ToLowerInvariant();

        var usuario = await _usuarioRepository.ObtenerPorEmailAsync(email);

        if (usuario == null ||
            !_passwordHasher.VerifyPassword(
                dto.Password,
                usuario.PasswordHash))
        {
            throw new UnauthorizedAccessException(
                "Email o contraseña incorrectos.");
        }

        return CrearRespuesta(usuario);
    }

    private RespuestaAutenticacionDto CrearRespuesta(Usuario usuario)
    {
        
        string rol = usuario.Rol.ToString();

        string token = _tokenService.GenerarToken(
            usuario.Id,
            usuario.Email,
            rol);

        return new RespuestaAutenticacionDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Email = usuario.Email,
            Rol = rol,
            Token = token,
            FechaExpiracion = DateTime.UtcNow.AddMinutes(60)
        };
    }
}
