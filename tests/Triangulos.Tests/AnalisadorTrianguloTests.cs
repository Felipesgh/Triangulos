using Triangulos.Core;
using Xunit;

namespace Triangulos.Tests;

/// <summary>
/// Testes de AnalisadorTriangulo.Analisar (RN09 + integracao das demais regras).
/// </summary>
public class AnalisadorTrianguloTests
{
    [Theory]
    [InlineData(3.0, 4.0, 5.0, TipoTriangulo.Escaleno, TipoPorAngulo.Retangulo, 12.0, 6.0, 36.87, 53.13, 90.0)]
    [InlineData(5.0, 3.0, 4.0, TipoTriangulo.Escaleno, TipoPorAngulo.Retangulo, 12.0, 6.0, 90.0, 36.87, 53.13)]
    [InlineData(2.0, 3.0, 4.0, TipoTriangulo.Escaleno, TipoPorAngulo.Obtusangulo, 9.0, 2.90, 28.96, 46.57, 104.48)]
    [InlineData(5.0, 5.0, 6.0, TipoTriangulo.Isosceles, TipoPorAngulo.Acutangulo, 16.0, 12.0, 53.13, 53.13, 73.74)]
    [InlineData(5.0, 6.0, 5.0, TipoTriangulo.Isosceles, TipoPorAngulo.Acutangulo, 16.0, 12.0, 53.13, 73.74, 53.13)]
    [InlineData(2.0, 2.0, 2.0, TipoTriangulo.Equilatero, TipoPorAngulo.Acutangulo, 6.0, 1.73, 60.0, 60.0, 60.0)]
    public void Analisar_TrianguloValido_RetornaAnaliseCompleta(
        double a, double b, double c,
        TipoTriangulo tipo, TipoPorAngulo tipoPorAngulo,
        double perimetro, double area,
        double alfa, double beta, double gama)
    {
        var analise = AnalisadorTriangulo.Analisar(a, b, c);

        Assert.Equal(tipo, analise.Tipo);
        Assert.Equal(tipoPorAngulo, analise.TipoPorAngulo);
        Assert.Equal(perimetro, analise.Perimetro, precision: 10);
        Assert.Equal(area, analise.Area, precision: 10);
        Assert.Equal(alfa, analise.Angulos.A, precision: 10);
        Assert.Equal(beta, analise.Angulos.B, precision: 10);
        Assert.Equal(gama, analise.Angulos.C, precision: 10);
    }

    [Theory]
    [InlineData(1.0, 2.0, 3.0)]
    [InlineData(3.0, 1.0, 2.0)]
    [InlineData(2.0, 3.0, 1.0)]
    [InlineData(1.0, 2.0, 10.0)]
    [InlineData(10.0, 1.0, 2.0)]
    [InlineData(0.0, 4.0, 5.0)]
    [InlineData(-3.0, 4.0, 5.0)]
    [InlineData(double.NaN, 1.0, 1.0)]
    [InlineData(double.PositiveInfinity, 1.0, 1.0)]
    public void Analisar_LadosInvalidos_LancaTrianguloInvalidoException(double a, double b, double c)
    {
        Assert.Throws<TrianguloInvalidoException>(() => AnalisadorTriangulo.Analisar(a, b, c));
    }
}