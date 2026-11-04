using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Application.Validaciones;

public class FechaTurnoValidaAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not DateTime fecha)
            return false;

        return fecha > DateTime.Now;
    }

    public override string FormatErrorMessage(string name)
    {
        return "La fecha del turno debe ser posterior a la fecha y hora actual.";
    }
}