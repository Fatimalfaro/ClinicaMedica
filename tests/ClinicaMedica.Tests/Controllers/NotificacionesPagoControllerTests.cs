using System.Security.Cryptography;
using System.Text;
using ClinicaMedica.Api.Controllers;
using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace ClinicaMedica.Tests.Controllers;

public class NotificacionesPagoControllerTests
{
    private const string Secreto = "secreto-de-prueba";
    private const string RequestId = "request-123";
    private const string DataId = "123456";

    private readonly Mock<IPagoRepository> _repositoryMock;
    private readonly Mock<IMercadoPagoService> _mercadoPagoMock;
    private readonly NotificacionesPagoController _controller;

    public NotificacionesPagoControllerTests()
    {
        _repositoryMock = new Mock<IPagoRepository>();
        _mercadoPagoMock = new Mock<IMercadoPagoService>();

        var pagoService = new PagoService(_repositoryMock.Object);

        var configuracion = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["MercadoPago:WebhookSecret"] = Secreto
            })
            .Build();

        _controller = new NotificacionesPagoController(
            pagoService,
            _mercadoPagoMock.Object,
            new VerificadorWebhook(),
            configuracion
        );

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        PrepararNotificacion();
    }

    private void PrepararNotificacion(
        string tipo = "payment",
        string dataId = DataId,
        bool firmaValida = true)
    {
        _controller.Request.QueryString = new QueryString(
            $"?type={tipo}&data.id={dataId}"
        );

        _controller.Request.Headers["x-request-id"] = RequestId;

        _controller.Request.Headers["x-signature"] =
            firmaValida
                ? GenerarFirma(dataId)
                : "ts=123456789,v1=firma-incorrecta";
    }

    private static string GenerarFirma(string dataId)
    {
        const string timestamp = "123456789";

        var manifest =
            $"id:{dataId.ToLowerInvariant()};request-id:{RequestId};ts:{timestamp};";

        var hash = HMACSHA256.HashData(
            Encoding.UTF8.GetBytes(Secreto),
            Encoding.UTF8.GetBytes(manifest)
        );

        return $"ts={timestamp},v1={Convert.ToHexString(hash).ToLowerInvariant()}";
    }

    private void ConfigurarPagoExterno(
        string estado = "approved",
        string referencia = "1",
        decimal monto = 15000m)
    {
        _mercadoPagoMock
            .Setup(m => m.ConsultarPagoAsync(long.Parse(DataId)))
            .ReturnsAsync((estado, referencia, monto));
    }

    private void ConfigurarPagoLocal(Pago? pago)
    {
        _repositoryMock
            .Setup(r => r.ObtenerPorIdAsync(1))
            .ReturnsAsync(pago);
    }

    [Fact]
    public async Task RecibirNotificacion_DebeRetornarBadRequest_CuandoElTipoNoEsPayment()
    {
        // Arrange
        PrepararNotificacion(tipo: "merchant_order");

        // Act
        var resultado = await _controller.RecibirNotificacion();

        // Assert
        Assert.IsType<BadRequestResult>(resultado);

        _mercadoPagoMock.Verify(
            m => m.ConsultarPagoAsync(It.IsAny<long>()),
            Times.Never
        );
    }

    [Fact]
    public async Task RecibirNotificacion_DebeRetornarBadRequest_CuandoElIdEsInvalido()
    {
        // Arrange
        PrepararNotificacion(dataId: "abc");

        // Act
        var resultado = await _controller.RecibirNotificacion();

        // Assert
        Assert.IsType<BadRequestResult>(resultado);
    }

    [Fact]
    public async Task RecibirNotificacion_DebeRetornarUnauthorized_CuandoLaFirmaEsIncorrecta()
    {
        // Arrange
        PrepararNotificacion(firmaValida: false);

        // Act
        var resultado = await _controller.RecibirNotificacion();

        // Assert
        Assert.IsType<UnauthorizedResult>(resultado);

        _mercadoPagoMock.Verify(
            m => m.ConsultarPagoAsync(It.IsAny<long>()),
            Times.Never
        );
    }

    [Fact]
    public async Task RecibirNotificacion_DebeRetornarBadRequest_CuandoLaReferenciaEsInvalida()
    {
        // Arrange
        ConfigurarPagoExterno(referencia: "referencia-invalida");

        // Act
        var resultado = await _controller.RecibirNotificacion();

        // Assert
        Assert.IsType<BadRequestResult>(resultado);
    }

    [Fact]
    public async Task RecibirNotificacion_DebeRetornarNotFound_CuandoElPagoLocalNoExiste()
    {
        // Arrange
        ConfigurarPagoExterno();
        ConfigurarPagoLocal(null);

        // Act
        var resultado = await _controller.RecibirNotificacion();

        // Assert
        Assert.IsType<NotFoundResult>(resultado);
    }

    [Fact]
    public async Task RecibirNotificacion_DebeRetornarBadRequest_CuandoLosMontosNoCoinciden()
    {
        // Arrange
        ConfigurarPagoExterno(monto: 20000m);

        ConfigurarPagoLocal(new Pago
        {
            Id = 1,
            Monto = 15000m
        });

        // Act
        var resultado = await _controller.RecibirNotificacion();

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);

        _repositoryMock.Verify(
            r => r.ActualizarAsync(It.IsAny<Pago>()),
            Times.Never
        );
    }

    [Theory]
    [InlineData("approved", "Aprobado")]
    [InlineData("rejected", "Rechazado")]
    [InlineData("cancelled", "Cancelado")]
    [InlineData("pending", "Pendiente")]
    [InlineData("in_process", "Pendiente")]
    [InlineData("authorized", "Pendiente")]
    public async Task RecibirNotificacion_DebeActualizarEstadoSegunRespuestaDeMercadoPago(
        string estadoExterno,
        string estadoEsperado)
    {
        // Arrange
        ConfigurarPagoExterno(estado: estadoExterno);

        var pago = new Pago
        {
            Id = 1,
            Monto = 15000m,
            Estado = "Pendiente"
        };

        ConfigurarPagoLocal(pago);

        // Act
        var resultado = await _controller.RecibirNotificacion();

        // Assert
        Assert.IsType<OkResult>(resultado);
        Assert.Equal(estadoEsperado, pago.Estado);
        Assert.Equal(DataId, pago.PagoExternoId);

        _repositoryMock.Verify(
            r => r.ActualizarAsync(pago),
            Times.Once
        );
    }

    [Fact]
    public async Task RecibirNotificacion_DebeIgnorarEstadoDesconocido()
    {
        // Arrange
        ConfigurarPagoExterno(estado: "unknown");

        var pago = new Pago
        {
            Id = 1,
            Monto = 15000m,
            Estado = "Pendiente"
        };

        ConfigurarPagoLocal(pago);

        // Act
        var resultado = await _controller.RecibirNotificacion();

        // Assert
        Assert.IsType<OkResult>(resultado);
        Assert.Equal("Pendiente", pago.Estado);

        _repositoryMock.Verify(
            r => r.ActualizarAsync(It.IsAny<Pago>()),
            Times.Never
        );
    }

    [Fact]
    public async Task RecibirNotificacion_DebeRetornarInternalServerError_CuandoMercadoPagoFalla()
    {
        // Arrange
        _mercadoPagoMock
            .Setup(m => m.ConsultarPagoAsync(long.Parse(DataId)))
            .ThrowsAsync(new Exception("Error de prueba"));

        // Act
        var resultado = await _controller.RecibirNotificacion();

        // Assert
        var respuesta = Assert.IsType<StatusCodeResult>(resultado);
        Assert.Equal(500, respuesta.StatusCode);
    }
}
