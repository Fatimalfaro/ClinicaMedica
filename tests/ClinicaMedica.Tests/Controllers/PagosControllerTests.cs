using ClinicaMedica.Api.Controllers;
using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using ClinicaMedica.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClinicaMedica.Tests.Controllers;

public class PagosControllerTests
{
    private readonly Mock<IPagoRepository> _repositoryMock;
    private readonly Mock<IMercadoPagoService> _mercadoPagoMock;
    private readonly PagosController _controller;

    public PagosControllerTests()
    {
        _repositoryMock = new Mock<IPagoRepository>();
        _mercadoPagoMock = new Mock<IMercadoPagoService>();

        var pagoService = new PagoService(_repositoryMock.Object);

        _controller = new PagosController(
            pagoService,
            _mercadoPagoMock.Object
        );
    }

    [Fact]
    public async Task ObtenerPorId_DebeRetornarNotFound_CuandoNoExiste()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ObtenerPorIdAsync(99))
            .ReturnsAsync((Pago?)null);

        // Act
        var resultado = await _controller.ObtenerPorId(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(resultado);
    }

    [Fact]
    public async Task ObtenerPorId_DebeRetornarOk_CuandoExiste()
    {
        // Arrange
        var pago = new Pago
        {
            Id = 1,
            TurnoId = 10,
            Monto = 15000
        };

        _repositoryMock
            .Setup(r => r.ObtenerPorIdAsync(1))
            .ReturnsAsync(pago);

        // Act
        var resultado = await _controller.ObtenerPorId(1);

        // Assert
        var respuesta = Assert.IsType<OkObjectResult>(resultado);
        Assert.NotNull(respuesta.Value);
    }

    [Fact]
    public async Task Crear_DebeRetornarBadRequest_CuandoElTurnoNoExiste()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ExisteTurnoAsync(10))
            .ReturnsAsync(false);

        // Act
        var resultado = await _controller.Crear(
            new CrearPagoDto { TurnoId = 10 }
        );

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);

        _repositoryMock.Verify(
            r => r.AgregarAsync(It.IsAny<Pago>()),
            Times.Never
        );
    }

    [Fact]
    public async Task Crear_DebeRetornarBadRequest_CuandoNoSePuedeObtenerElMonto()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ExisteTurnoAsync(10))
            .ReturnsAsync(true);

        _repositoryMock
            .Setup(r => r.ObtenerMontoTurnoAsync(10))
            .ReturnsAsync((decimal?)null);

        // Act
        var resultado = await _controller.Crear(
            new CrearPagoDto { TurnoId = 10 }
        );

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);

        _repositoryMock.Verify(
            r => r.AgregarAsync(It.IsAny<Pago>()),
            Times.Never
        );
    }

    [Fact]
    public async Task Crear_DebeGuardarPagoYRetornarCreated()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ExisteTurnoAsync(10))
            .ReturnsAsync(true);

        _repositoryMock
            .Setup(r => r.ObtenerMontoTurnoAsync(10))
            .ReturnsAsync(15000m);

        _repositoryMock
            .Setup(r => r.AgregarAsync(It.IsAny<Pago>()))
            .Callback<Pago>(p => p.Id = 1)
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _controller.Crear(
            new CrearPagoDto { TurnoId = 10 }
        );

        // Assert
        var respuesta = Assert.IsType<CreatedAtActionResult>(resultado);

        Assert.Equal(nameof(PagosController.ObtenerPorId), respuesta.ActionName);

        _repositoryMock.Verify(
            r => r.AgregarAsync(
                It.Is<Pago>(p =>
                    p.TurnoId == 10 &&
                    p.Monto == 15000m &&
                    p.Estado == "Pendiente")
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task CrearPreferencia_DebeRetornarNotFound_CuandoNoExisteElPago()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ObtenerPorIdAsync(99))
            .ReturnsAsync((Pago?)null);

        // Act
        var resultado = await _controller.CrearPreferencia(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(resultado);
    }

    [Fact]
    public async Task CrearPreferencia_DebeRetornarBadRequest_CuandoElPagoNoEstaPendiente()
    {
        // Arrange
        var pago = new Pago
        {
            Id = 1,
            Estado = "Aprobado"
        };

        _repositoryMock
            .Setup(r => r.ObtenerPorIdAsync(1))
            .ReturnsAsync(pago);

        // Act
        var resultado = await _controller.CrearPreferencia(1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);

        _mercadoPagoMock.Verify(
            m => m.CrearPreferenciaAsync(It.IsAny<Pago>()),
            Times.Never
        );
    }

    [Fact]
    public async Task CrearPreferencia_DebeRetornarConflict_CuandoYaTienePreferencia()
    {
        // Arrange
        var pago = new Pago
        {
            Id = 1,
            Estado = "Pendiente",
            PreferenciaPagoId = "preferencia-existente"
        };

        _repositoryMock
            .Setup(r => r.ObtenerPorIdAsync(1))
            .ReturnsAsync(pago);

        // Act
        var resultado = await _controller.CrearPreferencia(1);

        // Assert
        Assert.IsType<ConflictObjectResult>(resultado);

        _mercadoPagoMock.Verify(
            m => m.CrearPreferenciaAsync(It.IsAny<Pago>()),
            Times.Never
        );
    }

    [Fact]
    public async Task CrearPreferencia_DebeCrearPreferenciaYRetornarOk()
    {
        // Arrange
        var pago = new Pago
        {
            Id = 1,
            TurnoId = 10,
            Monto = 15000,
            Estado = "Pendiente"
        };

        _repositoryMock
            .Setup(r => r.ObtenerPorIdAsync(1))
            .ReturnsAsync(pago);

        _mercadoPagoMock
            .Setup(m => m.CrearPreferenciaAsync(pago))
            .ReturnsAsync((
                "pref-123",
                "https://mercadopago.com/checkout",
                "https://sandbox.mercadopago.com/checkout"
            ));

        _repositoryMock
            .Setup(r => r.ActualizarAsync(pago))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _controller.CrearPreferencia(1);

        // Assert
        Assert.IsType<OkObjectResult>(resultado);
        Assert.Equal("pref-123", pago.PreferenciaPagoId);

        _repositoryMock.Verify(
            r => r.ActualizarAsync(pago),
            Times.Once
        );
    }

    [Fact]
    public async Task CrearPreferencia_DebeRetornar502_CuandoMercadoPagoFalla()
    {
        // Arrange
        var pago = new Pago
        {
            Id = 1,
            TurnoId = 10,
            Monto = 15000,
            Estado = "Pendiente"
        };

        _repositoryMock
            .Setup(r => r.ObtenerPorIdAsync(1))
            .ReturnsAsync(pago);

        _mercadoPagoMock
            .Setup(m => m.CrearPreferenciaAsync(pago))
            .ThrowsAsync(new Exception("Error de prueba"));

        // Act
        var resultado = await _controller.CrearPreferencia(1);

        // Assert
        var respuesta = Assert.IsType<ObjectResult>(resultado);
        Assert.Equal(502, respuesta.StatusCode);
    }
}