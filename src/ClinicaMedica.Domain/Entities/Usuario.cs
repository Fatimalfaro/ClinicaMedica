namespace ClinicaMedica.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public Paciente? Paciente { get; set; }

    public Medico? Medico { get; set; }

    public RolUsuario Rol { get; set; } = RolUsuario.Paciente;
}

public enum RolUsuario
{
    Paciente,
    Medico,
    Administrador
}
