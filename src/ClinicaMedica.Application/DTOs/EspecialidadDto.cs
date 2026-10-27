namespace ClinicaMedica.Application.DTOs;

public class EspecialidadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public List<MedicoResumenDto> Medicos { get; set; } = new();
}

public class EspecialidadSimpleDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}