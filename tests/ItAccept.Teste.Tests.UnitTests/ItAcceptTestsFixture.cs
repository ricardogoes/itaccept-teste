using ItAccept.Teste.Domain.Interfaces.AppSettings;
using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Interfaces;
using ItAccept.Teste.Service.Controllers.v1;
using Moq;
using Moq.AutoMock;
using System.Data;
using Xunit;

namespace ItAccept.Teste.Tests.UnitTests
{
    [CollectionDefinition(nameof(ItAcceptTestsCollection))]
    public class ItAcceptTestsCollection : ICollectionFixture<ItAcceptTestsFixture>
    {
    }

    public class ItAcceptTestsFixture
    {
        public Mock<IDbConnection> DbConnectionMock { get; set; }
        public Mock<IDapperWrapper> DapperWrapperMock { get; set; }
        public Mock<IConnectionFactory> DatabaseConnectionFactory { get; set; }

        public Mock<ITokenAuthService> TokenAuthServiceMock { get; set; }
        public Mock<IUsuariosService> UsuariosServiceMock { get; set; }
        public Mock<IPasswordsService> PasswordsServiceMock { get; set; }

        // -- Clientes --

        public TokenAuthController GetTokenAuthController()
        {
            var mocker = new AutoMocker();

            var controller = mocker.CreateInstance<TokenAuthController>();

            UsuariosServiceMock = mocker.GetMock<IUsuariosService>();
            TokenAuthServiceMock = mocker.GetMock<ITokenAuthService>();
            PasswordsServiceMock = mocker.GetMock<IPasswordsService>();

            return controller;
        }

    }
}
