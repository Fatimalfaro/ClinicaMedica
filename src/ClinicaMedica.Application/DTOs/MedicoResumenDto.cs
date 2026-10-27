namespace ClinicaMedica.Application.DTOs;

public class MedicoResumenDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Matricula { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public decimal PrecioConsulta { get; set; }
}