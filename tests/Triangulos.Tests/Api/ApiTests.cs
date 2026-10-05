using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Triangulos.Tests.Api;

/// <summary>
/// Testes de integracao da Minimal API (RN10): status 200, 400 e 422.
/// </summary>
public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    private async Task<(HttpStatusCode Status, JsonElement Json)> Classificar(string query)
    {
        var resposta = await _client.GetAsync($"/api/triangulos/classificar?{query}");
        var json = await resposta.Content.ReadFromJsonAsync<JsonElement>();
        return (resposta.StatusCode, json);
    }

    // ---------- GET classificar: 200 ----------
    [Theory]
    [InlineData("a=3&b=3&c=3", "Equilatero")]
    [InlineData("a=3&b=3&c=5", "Isosceles")]
    [InlineData("a=5&b=3&c=5", "Isosceles")]
    [InlineData("a=3&b=4&c=5", "Escaleno")]
    [InlineData("a=1&b=2&c=3", "NaoEhTriangulo")]
    [InlineData("a=10&b=1&c=2", "NaoEhTriangulo")]
    [InlineData("a=0&b=4&c=5", "NaoEhTriangulo")]
    [InlineData("a=-3&b=4&c=5", "NaoEhTriangulo")]
    [InlineData("a=NaN&b=1&c=1", "NaoEhTriangulo")]
    [InlineData("a=Infinity&b=1&c=1", "NaoEhTriangulo")]
    public async Task Classificar_LadosValidosNumericos_Retorna200ComTipoEsperado(string query, string tipoEsperado)
    {
        var (status, json) = await Classificar(query);

        Assert.Equal(HttpStatusCode.OK, status);
        Assert.Equal(tipoEsperado, json.GetProperty("tipo").GetString());
    }

    // ---------- GET classificar: 400 ----------
    [Theory]
    [InlineData("a=3&b=4")]          // c ausente
    [InlineData("a=3&c=5")]          // b ausente
    [InlineData("b=4&c=5")]          // a ausente
    [InlineData("")]                 // nenhum parametro
    [InlineData("a=abc&b=4&c=5")]    // nao numerico
    [InlineData("a=3&b=&c=5")]       // vazio
    public async Task Classificar_ParametroAusenteOuNaoNumerico_Retorna400(string query)
    {
        var resposta = await _client.GetAsync($"/api/triangulos/classificar?{query}");

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);
    }

    // ---------- POST analisar: 200 ----------
    [Fact]
    public async Task Analisar_Triangulo345_Retorna200ComAnaliseCompleta()
    {
        var resposta = await _client.PostAsJsonAsync("/api/triangulos/analisar", new { a = 3, b = 4, c = 5 });
        var json = await resposta.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        Assert.Equal("Escaleno", json.GetProperty("tipo").GetString());
        Assert.Equal("Retangulo", json.GetProperty("tipoPorAngulo").GetString());
        Assert.Equal(12.0, json.GetProperty("perimetro").GetDouble(), precision: 10);
        Assert.Equal(6.0, json.GetProperty("area").GetDouble(), precision: 10);
        Assert.Equal(36.87, json.GetProperty("angulos").GetProperty("a").GetDouble(), precision: 10);
        Assert.Equal(53.13, json.GetProperty("angulos").GetProperty("b").GetDouble(), precision: 10);
        Assert.Equal(90.0, json.GetProperty("angulos").GetProperty("c").GetDouble(), precision: 10);
    }

    [Fact]
    public async Task Analisar_Triangulo234_RetornaAnguloObtusoCorreto()
    {
        var resposta = await _client.PostAsJsonAsync("/api/triangulos/analisar", new { a = 2, b = 3, c = 4 });
        var json = await resposta.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        Assert.Equal("Obtusangulo", json.GetProperty("tipoPorAngulo").GetString());
        Assert.Equal(104.48, json.GetProperty("angulos").GetProperty("c").GetDouble(), precision: 10);
    }

    [Theory]
    [InlineData(5, 3, 4)]
    [InlineData(3, 5, 4)]
    [InlineData(4, 5, 3)]
    public async Task Analisar_HipotenusaEmQualquerPosicao_RetornaRetangulo(int a, int b, int c)
    {
        var resposta = await _client.PostAsJsonAsync("/api/triangulos/analisar", new { a, b, c });
        var json = await resposta.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        Assert.Equal("Retangulo", json.GetProperty("tipoPorAngulo").GetString());
    }

    // ---------- POST analisar: 422 ----------
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(3, 1, 2)]
    [InlineData(10, 1, 2)]
    [InlineData(0, 4, 5)]
    [InlineData(-3, 4, 5)]
    public async Task Analisar_LadosQueNaoFormamTriangulo_Retorna422ComErro(int a, int b, int c)
    {
        var resposta = await _client.PostAsJsonAsync("/api/triangulos/analisar", new { a, b, c });
        var json = await resposta.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.UnprocessableEntity, resposta.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(json.GetProperty("erro").GetString()));
    }

    // ---------- POST analisar: 400 ----------
    [Fact]
    public async Task Analisar_JsonMalformado_Retorna400()
    {
        var corpo = new StringContent("{ \"a\": 3, \"b\": ", Encoding.UTF8, "application/json");

        var resposta = await _client.PostAsync("/api/triangulos/analisar", corpo);

        Assert.Equal(HttpStatusCode.BadRequest, resposta.StatusCode);
    }

    // ---------- GET /health ----------
    [Fact]
    public async Task Health_Retorna200ComStatusOk()
    {
        var resposta = await _client.GetAsync("/health");
        var json = await resposta.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(HttpStatusCode.OK, resposta.StatusCode);
        Assert.Equal("ok", json.GetProperty("status").GetString());
    }
}