using FluentAssertions;
using ItAccept.Teste.Domain.Models;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Models;
using ItAccept.Teste.Tests.UnitTests.DataGenerator;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ItAccept.Teste.Tests.UnitTests.TokenAuth
{
    [Collection(nameof(ItAcceptTestsCollection))]
    public class ClientesControllerTests
    {
        public ItAcceptTestsFixture Fixture { get; set; }

        public ClientesControllerTests(ItAcceptTestsFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact(DisplayName = "GetAuthToken() deve retornar token válido quando dados de acesso forem válidos")]
        [Trait("Category", "TokenAuth Controller Tests")]
        public async Task GetAuthToken_deve_retornar_200_quando_dados_forem_validos()
        {
            // Arrange
            var controller = Fixture.GetTokenAuthController();

            Random rnd = new Random();
            Byte[] b = new Byte[10];
            rnd.NextBytes(b);

            var usuario = UsuariosDataGenerator.GerarUsuarioParaConsultarValido();

            Fixture.PasswordsServiceMock.Setup(c => c.ConsultarHashPasswordPorUsernameAsync("admin")).Returns(Task.FromResult<byte[]>(b));
            Fixture.PasswordsServiceMock.Setup(c => c.GerarHashPassword("admin", b)).Returns("djadjaodaodahdas9d8ahd");

            Fixture.UsuariosServiceMock.Setup(c => c.ConsultarPeloUsernameAsync("admin")).Returns(Task.FromResult(usuario));
            Fixture.UsuariosServiceMock.Setup(c => c.ConsultarPeloUsernameEPasswordAsync("admin", "djadjaodaodahdas9d8ahd")).Returns(Task.FromResult(usuario));
            Fixture.TokenAuthServiceMock.Setup(c => c.GenerateToken(usuario)).Returns("ey_d91he91h39123gh1923g1293g12931g931g");

            // Act
            var response = (await controller.GetAuthToken(new AuthRequest { Username = "admin", Password = "admin" })) as ObjectResult;
            var data = response.Value as ApiResponse;

            // Assert Fluent Assertions
            response.StatusCode.Should().Be(200);
            data.State.Should().Be(ApiResponseState.Success);
            var authResponse = data.Data as AuthResponse;
            authResponse.TokenType.Should().Be("Bearer");
            authResponse.AccessToken.Should().Be("ey_d91he91h39123gh1923g1293g12931g931g");
        }

        [Fact(DisplayName = "GetAuthToken() deve retornar 400 quando username não existir")]
        [Trait("Category", "TokenAuth Controller Tests")]
        public async Task GetAuthToken_deve_retornar_400_quando_username_nao_existir()
        {
            // Arrange
            var controller = Fixture.GetTokenAuthController();

            Random rnd = new Random();
            Byte[] b = new Byte[10];
            rnd.NextBytes(b);

            var usuario = UsuariosDataGenerator.GerarUsuarioParaConsultarValido();

            Fixture.PasswordsServiceMock.Setup(c => c.ConsultarHashPasswordPorUsernameAsync("admin")).Returns(Task.FromResult<byte[]>(b));
            Fixture.PasswordsServiceMock.Setup(c => c.GerarHashPassword("admin", b)).Returns("djadjaodaodahdas9d8ahd");

            Fixture.UsuariosServiceMock.Setup(c => c.ConsultarPeloUsernameAsync("admin")).Returns(Task.FromResult(usuario));
            Fixture.UsuariosServiceMock.Setup(c => c.ConsultarPeloUsernameEPasswordAsync("admin", "djadjaodaodahdas9d8ahd")).Returns(Task.FromResult(usuario));
            Fixture.TokenAuthServiceMock.Setup(c => c.GenerateToken(usuario)).Returns("ey_d91he91h39123gh1923g1293g12931g931g");

            // Act
            var response = (await controller.GetAuthToken(new AuthRequest { Username = "admin.invalido", Password = "admin" })) as ObjectResult;
            var data = response.Value as ApiResponse;

            // Assert Fluent Assertions
            response.StatusCode.Should().Be(400);
            data.State.Should().Be(ApiResponseState.Failed);
            data.Message.Should().Be("Username ou Password são inválidos");
        }

        [Fact(DisplayName = "GetAuthToken() deve retornar 400 quando senha for inválida")]
        [Trait("Category", "TokenAuth Controller Tests")]
        public async Task GetAuthToken_deve_retornar_400_quando_senha_for_invalida()
        {
            // Arrange
            var controller = Fixture.GetTokenAuthController();

            Random rnd = new Random();
            Byte[] b = new Byte[10];
            rnd.NextBytes(b);

            var usuario = UsuariosDataGenerator.GerarUsuarioParaConsultarValido();

            Fixture.PasswordsServiceMock.Setup(c => c.ConsultarHashPasswordPorUsernameAsync("admin")).Returns(Task.FromResult<byte[]>(b));
            Fixture.PasswordsServiceMock.Setup(c => c.GerarHashPassword("admin", b)).Returns("djadjaodaodahdas9d8ahd");

            Fixture.UsuariosServiceMock.Setup(c => c.ConsultarPeloUsernameAsync("admin")).Returns(Task.FromResult(usuario));
            Fixture.UsuariosServiceMock.Setup(c => c.ConsultarPeloUsernameEPasswordAsync("admin", "djadjaodaodahdas9d8ahd")).Returns(Task.FromResult(usuario));
            Fixture.TokenAuthServiceMock.Setup(c => c.GenerateToken(usuario)).Returns("ey_d91he91h39123gh1923g1293g12931g931g");

            // Act
            var response = (await controller.GetAuthToken(new AuthRequest { Username = "admin", Password = "admin.invalido" })) as ObjectResult;
            var data = response.Value as ApiResponse;

            // Assert Fluent Assertions
            response.StatusCode.Should().Be(400);
            data.State.Should().Be(ApiResponseState.Failed);
            data.Message.Should().Be("Username ou Password são inválidos");
        }
    }
}
