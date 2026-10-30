using System.Security.Cryptography;
using System.Text;
using ClinicaMedica.Infrastructure.Services;
using Xunit;

namespace ClinicaMedica.Tests.Services;

public class VerificadorWebhookTests
{
    private readonly VerificadorWebhook _verificador = new();

    [Fact]
    public void Validar_DebeRetornarTrue_CuandoLaFirmaEsCorrecta()
    {
        // Arrange
        var secreto = "secreto-de-prueba";
        var requestId = "request-123";
        var dataId = "123456";
        var timestamp = "123456789";

        var manifest =
            $"id:{dataId};request-id:{requestId};ts:{timestamp};";

        var clave = Encoding.UTF8.GetBytes(secreto);
        var contenido = Encoding.UTF8.GetBytes(manifest);

        var hash = HMACSHA256.HashData(clave, contenido);
        var firmaGenerada = Convert.ToHexString(hash).ToLowerInvariant();

        var firma = $"ts={timestamp},v1={firmaGenerada}";

        // Act
        var resultado = _verificador.Validar(
            firma,
            requestId,
            dataId,
            secreto
        );

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public void Validar_DebeRetornarFalse_CuandoLaFirmaEsIncorrecta()
    {
        // Arrange
        var firma = "ts=123456789,v1=firma-incorrecta";

        // Act
        var resultado = _verificador.Validar(
            firma,
            "request-123",
            "123456",
            "secreto-de-prueba"
        );

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public void Validar_DebeRetornarFalse_CuandoFaltanDatos()
    {
        // Act
        var resultado = _verificador.Validar(
            null,
            "request-123",
            "123456",
            "secreto-de-prueba"
        );

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public void Validar_DebeRetornarFalse_CuandoElSecretoEsIncorrecto()
    {
        // Arrange
        var secretoCorrecto = "secreto-correcto";
        var requestId = "request-123";
        var dataId = "123456";
        var timestamp = "123456789";

        var manifest =
            $"id:{dataId};request-id:{requestId};ts:{timestamp};";

        var hash = HMACSHA256.HashData(
            Encoding.UTF8.GetBytes(secretoCorrecto),
            Encoding.UTF8.GetBytes(manifest)
        );

        var firma = $"ts={timestamp},v1={Convert.ToHexString(hash).ToLowerInvariant()}";

        // Act
        var resultado = _verificador.Validar(
            firma,
            requestId,
            dataId,
            "otro-secreto"
        );

        // Assert
        Assert.False(resultado);
    }
}
