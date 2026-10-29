using System.Security.Cryptography;
using System.Text;

namespace ClinicaMedica.Infrastructure.Services;

public class VerificadorWebhook
{
    public bool Validar(
        string? firma,
        string? requestId,
        string? dataId,
        string? secreto)
    {
        if (string.IsNullOrWhiteSpace(firma) ||
            string.IsNullOrWhiteSpace(requestId) ||
            string.IsNullOrWhiteSpace(dataId) ||
            string.IsNullOrWhiteSpace(secreto))
        {
            return false;
        }

        string? timestamp = null;
        string? hashRecibido = null;

        foreach (var parte in firma.Split(','))
        {
            var valores = parte.Split('=', 2);

            if (valores.Length != 2)
                continue;

            if (valores[0].Trim() == "ts")
                timestamp = valores[1].Trim();

            if (valores[0].Trim() == "v1")
                hashRecibido = valores[1].Trim();
        }

        if (string.IsNullOrWhiteSpace(timestamp) ||
            string.IsNullOrWhiteSpace(hashRecibido))
        {
            return false;
        }

        var manifest =
            $"id:{dataId.ToLowerInvariant()};request-id:{requestId};ts:{timestamp};";

        var clave = Encoding.UTF8.GetBytes(secreto);
        var contenido = Encoding.UTF8.GetBytes(manifest);

        var hashCalculado = HMACSHA256.HashData(clave, contenido);
        var hashEsperado = Convert.ToHexString(hashCalculado).ToLowerInvariant();

        try
        {
            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(hashEsperado),
                Encoding.UTF8.GetBytes(hashRecibido.ToLowerInvariant()));
        }
        catch
        {
            return false;
        }
    }
}