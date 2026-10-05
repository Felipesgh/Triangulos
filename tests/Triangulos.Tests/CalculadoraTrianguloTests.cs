using Triangulos.Core;
using Xunit;

namespace Triangulos.Tests;

/// <summary>
/// Testes de CalculadoraTriangulo, derivados da especificacao (RN04 a RN09).
/// </summary>
public class CalculadoraTrianguloTests
{
    // Lados que NAO formam triangulo (RN01/RN02), usados nos testes de excecao (RN09).
    public static IEnumerable<object[]> LadosInvalidos => new[]
    {
        new object[] { 1.0, 2.0, 3.0 },   // soma igual
        new object[] { 3.0, 1.0, 2.0 },   // soma igual, outra posicao
        new object[] { 10.0, 1.0, 2.0 },  // soma menor
        new object[] { 0.0, 4.0, 5.0 },   // zero
        new object[] { -3.0, 4.0, 5.0 },  // negativo
        new object[] { double.NaN, 1.0, 1.0 },
        new object[] { double.PositiveInfinity, 1.0, 1.0 },
    };

    // ---------- RN04: Perimetro ----------
    [Theory]
    [InlineData(3.0, 4.0, 5.0, 12.0)]
    [InlineData(5.0, 3.0, 4.0, 12.0)]
    [InlineData(2.5, 2.5, 2.5, 7.5)]
    [InlineData(0.5, 0.5, 0.5, 1.5)]
    [InlineData(5.0, 5.0, 6.0, 16.0)]
    public void Perimetro_TrianguloValido_RetornaSomaDosLados(double a, double b, double c, double esperado)
    {
        Assert.Equal(esperado, CalculadoraTriangulo.Perimetro(a, b, c), precision: 10);
    }

    // ---------- RN05: Area (Heron, 2 casas, AwayFromZero) ----------
    [Theory]
    [InlineData(3.0, 4.0, 5.0, 6.0)]
    [InlineData(5.0, 3.0, 4.0, 6.0)]
    [InlineData(4.0, 5.0, 3.0, 6.0)]
    [InlineData(2.0, 3.0, 4.0, 2.90)]    // 2,9047... arredonda para 2,90
    [InlineData(1.0, 1.0, 1.0, 0.43)]    // 0,4330... arredonda para 0,43
    [InlineData(2.0, 2.0, 2.0, 1.73)]    // 1,7320... arredonda para 1,73
    [InlineData(5.0, 5.0, 6.0, 12.0)]
    [InlineData(5.0, 12.0, 13.0, 30.0)]
    public void Area_TrianguloValido_RetornaAreaDeHeronArredondada(double a, double b, double c, double esperado)
    {
        Assert.Equal(esperado, CalculadoraTriangulo.Area(a, b, c), precision: 10);
    }

    // ---------- RN06: Angulos (lei dos cossenos, 2 casas) ----------
    [Theory]
    [InlineData(3.0, 4.0, 5.0, 36.87, 53.13, 90.0)]
    [InlineData(5.0, 3.0, 4.0, 90.0, 36.87, 53.13)]
    [InlineData(3.0, 5.0, 4.0, 36.87, 90.0, 53.13)]
    [InlineData(4.0, 5.0, 3.0, 53.13, 90.0, 36.87)]
    [InlineData(2.0, 3.0, 4.0, 28.96, 46.57, 104.48)]
    [InlineData(3.0, 3.0, 3.0, 60.0, 60.0, 60.0)]
    [InlineData(5.0, 5.0, 6.0, 53.13, 53.13, 73.74)]
    public void Angulos_TrianguloValido_RetornaAngulosOpostosAosLados(
        double a, double b, double c, double alfa, double beta, double gama)
    {
        var angulos = CalculadoraTriangulo.Angulos(a, b, c);

        Assert.Equal(alfa, angulos.A, precision: 10);
        Assert.Equal(beta, angulos.B, precision: 10);
        Assert.Equal(gama, angulos.C, precision: 10);
    }

    // ---------- RN07: EhRetangulo (hipotenusa em qualquer posicao) ----------
    [Theory]
    [InlineData(3.0, 4.0, 5.0)]
    [InlineData(5.0, 3.0, 4.0)]
    [InlineData(4.0, 5.0, 3.0)]
    [InlineData(5.0, 12.0, 13.0)]
    [InlineData(6.0, 8.0, 10.0)]
    [InlineData(0.3, 0.4, 0.5)]                  // tolerancia e relativa, vale em escala pequena
    [InlineData(3000000.0, 4000000.0, 5000000.0)] // e em escala grande
    public void EhRetangulo_LadosDeTrianguloRetangulo_RetornaTrue(double a, double b, double c)
    {
        Assert.True(CalculadoraTriangulo.EhRetangulo(a, b, c));
    }

    [Theory]
    [InlineData(2.0, 3.0, 4.0)]      // obtusangulo
    [InlineData(4.0, 5.0, 6.0)]      // acutangulo
    [InlineData(1.0, 1.0, 1.0)]
    [InlineData(3.0, 4.0, 5.001)]    // quase retangulo, fora da tolerancia
    [InlineData(5.001, 3.0, 4.0)]
    public void EhRetangulo_TrianguloNaoRetangulo_RetornaFalse(double a, double b, double c)
    {
        Assert.False(CalculadoraTriangulo.EhRetangulo(a, b, c));
    }

    // ---------- RN08: ClassificarPorAngulo ----------
    [Theory]
    [InlineData(3.0, 4.0, 5.0)]
    [InlineData(5.0, 3.0, 4.0)]
    [InlineData(4.0, 5.0, 3.0)]
    public void ClassificarPorAngulo_LadosPitagoricos_RetornaRetangulo(double a, double b, double c)
    {
        Assert.Equal(TipoPorAngulo.Retangulo, CalculadoraTriangulo.ClassificarPorAngulo(a, b, c));
    }

    [Theory]
    [InlineData(2.0, 3.0, 4.0)]
    [InlineData(4.0, 2.0, 3.0)]
    [InlineData(3.0, 4.0, 2.0)]
    [InlineData(2.0, 2.0, 3.0)]
    public void ClassificarPorAngulo_MaiorLadoAoQuadradoMaiorQueSomaDosOutros_RetornaObtusangulo(
        double a, double b, double c)
    {
        Assert.Equal(TipoPorAngulo.Obtusangulo, CalculadoraTriangulo.ClassificarPorAngulo(a, b, c));
    }

    [Theory]
    [InlineData(4.0, 5.0, 6.0)]
    [InlineData(6.0, 5.0, 4.0)]
    [InlineData(5.0, 6.0, 4.0)]
    [InlineData(1.0, 1.0, 1.0)]
    [InlineData(5.0, 5.0, 6.0)]
    public void ClassificarPorAngulo_MaiorLadoAoQuadradoMenorQueSomaDosOutros_RetornaAcutangulo(
        double a, double b, double c)
    {
        Assert.Equal(TipoPorAngulo.Acutangulo, CalculadoraTriangulo.ClassificarPorAngulo(a, b, c));
    }

    // ---------- RN09: triangulo invalido lanca TrianguloInvalidoException ----------
    [Theory]
    [MemberData(nameof(LadosInvalidos))]
    public void Perimetro_LadosInvalidos_LancaTrianguloInvalidoException(double a, double b, double c)
    {
        Assert.Throws<TrianguloInvalidoException>(() => CalculadoraTriangulo.Perimetro(a, b, c));
    }

    [Theory]
    [MemberData(nameof(LadosInvalidos))]
    public void Area_LadosInvalidos_LancaTrianguloInvalidoException(double a, double b, double c)
    {
        Assert.Throws<TrianguloInvalidoException>(() => CalculadoraTriangulo.Area(a, b, c));
    }

    [Theory]
    [MemberData(nameof(LadosInvalidos))]
    public void Angulos_LadosInvalidos_LancaTrianguloInvalidoException(double a, double b, double c)
    {
        Assert.Throws<TrianguloInvalidoException>(() => CalculadoraTriangulo.Angulos(a, b, c));
    }

    [Theory]
    [MemberData(nameof(LadosInvalidos))]
    public void EhRetangulo_LadosInvalidos_LancaTrianguloInvalidoException(double a, double b, double c)
    {
        Assert.Throws<TrianguloInvalidoException>(() => CalculadoraTriangulo.EhRetangulo(a, b, c));
    }

    [Theory]
    [MemberData(nameof(LadosInvalidos))]
    public void ClassificarPorAngulo_LadosInvalidos_LancaTrianguloInvalidoException(double a, double b, double c)
    {
        Assert.Throws<TrianguloInvalidoException>(() => CalculadoraTriangulo.ClassificarPorAngulo(a, b, c));
    }

    // ---------- Propriedades (invariantes) ----------
    // A soma dos tres angulos e 180 (a menos do arredondamento: 3 x 0,005 = 0,015).
    [Theory]
    [InlineData(3.0, 4.0, 5.0)]
    [InlineData(2.0, 3.0, 4.0)]
    [InlineData(5.0, 5.0, 6.0)]
    [InlineData(1.0, 1.0, 1.0)]
    [InlineData(7.0, 8.0, 9.0)]
    [InlineData(2.0, 9.0, 10.0)]
    public void Angulos_QualquerTrianguloValido_SomaCentoEOitentaGraus(double a, double b, double c)
    {
        var angulos = CalculadoraTriangulo.Angulos(a, b, c);

        Assert.InRange(angulos.A + angulos.B + angulos.C, 179.98, 180.02);
    }

    // O resultado nao depende da ordem em que os lados sao informados (nada privilegia a posicao).
    [Theory]
    [InlineData(3.0, 4.0, 5.0)]
    [InlineData(2.0, 3.0, 4.0)]
    [InlineData(5.0, 5.0, 6.0)]
    [InlineData(7.0, 8.0, 9.0)]
    public void PerimetroEAreaEClassificacao_PermutacaoDosLados_NaoMudaOResultado(double a, double b, double c)
    {
        var permutacoes = new[]
        {
            (a, b, c), (a, c, b), (b, a, c), (b, c, a), (c, a, b), (c, b, a),
        };

        foreach (var (x, y, z) in permutacoes)
        {
            Assert.Equal(CalculadoraTriangulo.Perimetro(a, b, c), CalculadoraTriangulo.Perimetro(x, y, z), precision: 10);
            Assert.Equal(CalculadoraTriangulo.Area(a, b, c), CalculadoraTriangulo.Area(x, y, z), precision: 10);
            Assert.Equal(CalculadoraTriangulo.ClassificarPorAngulo(a, b, c), CalculadoraTriangulo.ClassificarPorAngulo(x, y, z));
            Assert.Equal(ClassificadorTriangulo.Classificar(a, b, c), ClassificadorTriangulo.Classificar(x, y, z));
        }
    }
}