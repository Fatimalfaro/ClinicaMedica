using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.Validaciones;

public class FechaNacimientoValidaAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not DateTime fecha)
            return false;

        return fecha.Date < DateTime.Today;
    }

    public override string FormatErrorMessage(string name)
    {
        return "La fecha de nacimiento debe ser anterior al día de hoy.";
    }
}