using ItAccept.Teste.Domain.Models;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Models;
using ItAccept.Teste.Tests.ApiTests.Configuration;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ItAccept.Teste.Tests.ApiTests
{
    [Collection(nameof(AuthTestsCollection))]
    public class TokenAuthControllerTests
    {
        private AuthTestsFixture Fixture { get; set; }

        public TokenAuthControllerTests(AuthTestsFixture fixture)
        {
            Fixture = fixture;
        }

        // --
        [Fact(DisplayName = "POST v1/auth deve retornar 200 quando usuário for válido")]
        [Trait("Category", "Token Generation Api Tests")]
        public async Task Deve_retornar_200_quando_for_valido()
        {
            var request = new AuthRequest
            {
                Username = "admin",
                Password = "admin"
            };

            var httpResponse = await Fixture.HttpClient.PostAsJsonAsync("api/v1/auth", request);
            
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(await httpResponse.Content.ReadAsStringAsync());
            var content = JsonSerializer.Deserialize<AuthResponse>(apiResponse.Data.ToString());

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.Equal(ApiResponseState.Success, apiResponse.State);
            Assert.Empty(apiResponse.Message);
            Assert.NotNull(content.AccessToken);
        }

        // --
        [Fact(DisplayName = "POST v1/token deve retornar 400 quando request for nulo")]
        [Trait("Category", "Token Generation Api Tests")]
        public async Task Deve_retornar_400_quando_request_nulo()
        {
            var httpResponse = await Fixture.HttpClient.PostAsJsonAsync<AuthRequest>("api/v1/auth", null);
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(await httpResponse.Content.ReadAsStringAsync());

            JsonElement jsonElement = JsonSerializer.SerializeToElement(apiResponse.Erros);
            var errorMessage = jsonElement.GetProperty("")[0].ToString();

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
            Assert.Equal(ApiResponseState.Failed, apiResponse.State);
            Assert.Equal("A non-empty request body is required.", errorMessage);
        }

        // --
        [Fact(DisplayName = "POST v1/token deve retornar 400 Bad Request quando username for inválido")]
        [Trait("Category", "Token Generation Api Tests")]
        public async Task Deve_retornar_400_quando_username_invalido()
        {
            var request = new AuthRequest
            {
                Username = "ricardo@sinapse",
                Password = "ItAccept@1234!"
            };

            var httpResponse = await Fixture.HttpClient.PostAsJsonAsync("api/v1/auth", request);

            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(await httpResponse.Content.ReadAsStringAsync());
            
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
            Assert.Equal(ApiResponseState.Failed, apiResponse.State);
            Assert.Equal("Username ou Password são inválidos", apiResponse.Message);
            Assert.Null(apiResponse.Data);
        }

        // --
        [Fact(DisplayName = "POST v1/token deve retornar 400 quando username não for informado")]
        [Trait("Category", "Token Generation Api Tests")]
        public async Task Deve_retornar_400_quando_email_nao_informado()
        {
            var request = new AuthRequest
            {
                Password = "ItAccept@1234!"
            };

            var httpResponse = await Fixture.HttpClient.PostAsJsonAsync("api/v1/auth", request);

            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(await httpResponse.Content.ReadAsStringAsync());
            JsonElement jsonElement = JsonSerializer.SerializeToElement(apiResponse.Erros);
            var errorMessage = jsonElement.GetProperty("Username")[0].ToString();

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
            Assert.Equal(ApiResponseState.Failed, apiResponse.State);
            Assert.Equal("Username obrigatório", errorMessage);
        }

        // --
        [Fact(DisplayName = "POST v1/token deve retornar 400 quando senha for inválida")]
        [Trait("Category", "Token Generation Api Tests")]
        public async Task Deve_retornar_400_quando_senha_invalida()
        {
            var request = new AuthRequest
            {
                Username = "admin",
                Password = "123123"
            };

            var httpResponse = await Fixture.HttpClient.PostAsJsonAsync("api/v1/auth", request);

            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(await httpResponse.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
            Assert.Equal(ApiResponseState.Failed, apiResponse.State);
            Assert.Equal("Username ou Password são inválidos", apiResponse.Message);
            Assert.Null(apiResponse.Data);
        }

        // --
        [Fact(DisplayName = "POST v1/token deve retornar 400 quando senha não for informada")]
        [Trait("Category", "Token Generation Api Tests")]
        public async Task Deve_retornar_400_quando_senha_nao_informada()
        {
            var request = new AuthRequest
            {
                Username = "admin"
            };

            var httpResponse = await Fixture.HttpClient.PostAsJsonAsync("api/v1/auth", request);

            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(await httpResponse.Content.ReadAsStringAsync());
            JsonElement jsonElement = JsonSerializer.SerializeToElement(apiResponse.Erros);
            var errorMessage = jsonElement.GetProperty("Password")[0].ToString();

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
            Assert.Equal(ApiResponseState.Failed, apiResponse.State);
            Assert.Equal("Password obrigatório", errorMessage);
        }
    }
}