
using ClinicaMedica.Application.Interfaces;
using ClinicaMedica.Application.Services;
using ClinicaMedica.Domain.Entities;
using Moq;
using Xunit;

namespace ClinicaMedica.Tests.Services;

public class PagoServiceTests
{
    private readonly Mock<IPagoRepository> _repositoryMock;
    private readonly PagoService _service;

    public PagoServiceTests()
    {
        _repositoryMock = new Mock<IPagoRepository>();
        _service = new PagoService(_repositoryMock.Object);
    }

    [Fact]
    public async Task ObtenerTodosAsync_DebeRetornarTodosLosPagos()
    {
        // Arrange
        var pagos = new List<Pago>
        {
            new() { Id = 1, TurnoId = 1, Monto = 15000 },
            new() { Id = 2, TurnoId = 2, Monto = 20000 }
        };

        _repositoryMock
            .Setup(r => r.ObtenerTodosAsync())
            .ReturnsAsync(pagos);

        // Act
        var resultado = await _service.ObtenerTodosAsync();

        // Assert
        Assert.Equal(2, resultado.Count);
        _repositoryMock.Verify(r => r.ObtenerTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_DebeRetornarPagoExistente()
    {
        // Arrange
        var pago = new Pago
        {
            Id = 1,
            TurnoId = 1,
            Monto = 15000
        };

        _repositoryMock
            .Setup(r => r.ObtenerPorIdAsync(1))
            .ReturnsAsync(pago);

        // Act
        var resultado = await _service.ObtenerPorIdAsync(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_DebeRetornarNull_CuandoNoExiste()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ObtenerPorIdAsync(99))
            .ReturnsAsync((Pago?)null);

        // Act
        var resultado = await _service.ObtenerPorIdAsync(99);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task AgregarAsync_DebeGuardarElPago()
    {
        // Arrange
        var pago = new Pago
        {
            TurnoId = 1,
            Monto = 15000
        };

        _repositoryMock
            .Setup(r => r.AgregarAsync(pago))
            .Returns(Task.CompletedTask);

        // Act
        await _service.AgregarAsync(pago);

        // Assert
        _repositoryMock.Verify(
            r => r.AgregarAsync(pago),
            Times.Once
        );
    }

    [Fact]
    public async Task ExisteTurnoAsync_DebeRetornarTrue_CuandoExiste()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.ExisteTurnoAsync(1))
            .ReturnsAsync(true);

        // Act
        var resultado = await _service.ExisteTurnoAsync(1);

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public async Task ObtenerPagoParaIntegracionAsync_DebeBuscarElPago()
    {
        // Arrange
        var pago = new Pago
        {
            Id = 1,
            TurnoId = 1,
            Monto = 15000
        };

        _repositoryMock
            .Setup(r => r.ObtenerPorIdAsync(1))
            .ReturnsAsync(pago);

        // Act
        var resultado = await _service.ObtenerPagoParaIntegracionAsync(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);
    }

    [Fact]
    public async Task GuardarActualizacionAsync_DebeActualizarElPago()
    {
        // Arrange
        var pago = new Pago
        {
            Id = 1,
            Estado = "Aprobado"
        };

        _repositoryMock
            .Setup(r => r.ActualizarAsync(pago))
            .Returns(Task.CompletedTask);

        // Act
        await _service.GuardarActualizacionAsync(pago);

        // Assert
        _repositoryMock.Verify(
            r => r.ActualizarAsync(pago),
            Times.Once
        );
    }
}
