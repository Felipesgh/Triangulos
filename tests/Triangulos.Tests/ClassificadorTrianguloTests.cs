using Triangulos.Core;
using Xunit;

namespace Triangulos.Tests;

/// <summary>
/// Testes de ClassificadorTriangulo, derivados da especificacao (RN01, RN02, RN03).
/// </summary>
public class ClassificadorTrianguloTests
{
    // ---------- RN03: Equilatero ----------
    [Theory]
    [InlineData(1.0, 1.0, 1.0)]
    [InlineData(2.5, 2.5, 2.5)]
    [InlineData(0.5, 0.5, 0.5)]
    public void Classificar_TresLadosIguais_RetornaEquilatero(double a, double b, double c)
    {
        Assert.Equal(TipoTriangulo.Equilatero, ClassificadorTriangulo.Classificar(a, b, c));
    }

    // ---------- RN03: Isosceles (par igual em cada posicao) ----------
    [Theory]
    [InlineData(3.0, 3.0, 5.0)] // iguais: a e b
    [InlineData(3.0, 5.0, 3.0)] // iguais: a e c
    [InlineData(5.0, 3.0, 3.0)] // iguais: b e c
    [InlineData(5.0, 5.0, 3.0)]
    [InlineData(5.0, 3.0, 5.0)]
    [InlineData(3.0, 5.0, 5.0)]
    public void Classificar_ExatamenteDoisLadosIguais_RetornaIsosceles(double a, double b, double c)
    {
        Assert.Equal(TipoTriangulo.Isosceles, ClassificadorTriangulo.Classificar(a, b, c));
    }

    // ---------- RN03: Escaleno (todas as permutacoes) ----------
    [Theory]
    [InlineData(3.0, 4.0, 5.0)]
    [InlineData(3.0, 5.0, 4.0)]
    [InlineData(4.0, 3.0, 5.0)]
    [InlineData(4.0, 5.0, 3.0)]
    [InlineData(5.0, 3.0, 4.0)]
    [InlineData(5.0, 4.0, 3.0)]
    [InlineData(2.0, 3.0, 4.0)]
    public void Classificar_TresLadosDiferentes_RetornaEscaleno(double a, double b, double c)
    {
        Assert.Equal(TipoTriangulo.Escaleno, ClassificadorTriangulo.Classificar(a, b, c));
    }

    // ---------- RN02: soma de dois lados IGUAL ao terceiro (degenerado) ----------
    [Theory]
    [InlineData(1.0, 2.0, 3.0)]
    [InlineData(1.0, 3.0, 2.0)]
    [InlineData(2.0, 1.0, 3.0)]
    [InlineData(2.0, 3.0, 1.0)]
    [InlineData(3.0, 1.0, 2.0)]
    [InlineData(3.0, 2.0, 1.0)]
    [InlineData(1.0, 1.0, 2.0)]
    [InlineData(1.0, 2.0, 1.0)]
    [InlineData(2.0, 1.0, 1.0)]
    public void Classificar_SomaDeDoisLadosIgualAoTerceiro_RetornaNaoEhTriangulo(double a, double b, double c)
    {
        Assert.Equal(TipoTriangulo.NaoEhTriangulo, ClassificadorTriangulo.Classificar(a, b, c));
    }

    // ---------- RN02: soma de dois lados MENOR que o terceiro ----------
    [Theory]
    [InlineData(1.0, 2.0, 10.0)]
    [InlineData(1.0, 10.0, 2.0)]
    [InlineData(2.0, 1.0, 10.0)]
    [InlineData(2.0, 10.0, 1.0)]
    [InlineData(10.0, 1.0, 2.0)]
    [InlineData(10.0, 2.0, 1.0)]
    public void Classificar_SomaDeDoisLadosMenorQueOTerceiro_RetornaNaoEhTriangulo(double a, double b, double c)
    {
        Assert.Equal(TipoTriangulo.NaoEhTriangulo, ClassificadorTriangulo.Classificar(a, b, c));
    }

    // ---------- RN01: lados invalidos (zero, negativo, NaN, infinito) ----------
    [Theory]
    [InlineData(0.0, 4.0, 5.0)]
    [InlineData(4.0, 0.0, 5.0)]
    [InlineData(4.0, 5.0, 0.0)]
    [InlineData(0.0, 0.0, 0.0)]
    [InlineData(-3.0, 4.0, 5.0)]
    [InlineData(-1.0, 2.0, 2.0)]
    [InlineData(double.NaN, 1.0, 1.0)]
    [InlineData(1.0, double.NaN, 1.0)]
    [InlineData(1.0, 1.0, double.NaN)]
    [InlineData(double.PositiveInfinity, 1.0, 1.0)]
    [InlineData(1.0, double.PositiveInfinity, 1.0)]
    [InlineData(1.0, 1.0, double.PositiveInfinity)]
    public void Classificar_LadoInvalido_RetornaNaoEhTriangulo(double a, double b, double c)
    {
        Assert.Equal(TipoTriangulo.NaoEhTriangulo, ClassificadorTriangulo.Classificar(a, b, c));
    }

    // ---------- EhTriangulo (RN01 + RN02) ----------
    [Theory]
    [InlineData(3.0, 4.0, 5.0)]
    [InlineData(5.0, 3.0, 4.0)]
    [InlineData(4.0, 5.0, 3.0)]
    [InlineData(1.0, 1.0, 1.0)]
    [InlineData(1.0, 2.0, 2.99)] // limite: logo abaixo da igualdade
    public void EhTriangulo_LadosFormamTriangulo_RetornaTrue(double a, double b, double c)
    {
        Assert.True(ClassificadorTriangulo.EhTriangulo(a, b, c));
    }

    [Theory]
    [InlineData(1.0, 2.0, 3.0)]
    [InlineData(3.0, 1.0, 2.0)]
    [InlineData(2.0, 3.0, 1.0)]
    [InlineData(10.0, 1.0, 2.0)]
    [InlineData(0.0, 4.0, 5.0)]
    [InlineData(-3.0, 4.0, 5.0)]
    [InlineData(double.NaN, 1.0, 1.0)]
    [InlineData(double.PositiveInfinity, 1.0, 1.0)]
    public void EhTriangulo_LadosNaoFormamTriangulo_RetornaFalse(double a, double b, double c)
    {
        Assert.False(ClassificadorTriangulo.EhTriangulo(a, b, c));
    }
}