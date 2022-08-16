using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Interfaces;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Services;
using ItAccept.Teste.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ItAccept.Teste.Infrastructure.Crosscutting.IoC
{
    public class ServiceServices : ServiceBase
    {
        protected override void HttpClient(IServiceCollection services)
        {
            base.HttpClient(services);
        }

        protected override void Scoped(IServiceCollection services)
        {
            base.Scoped(services);
        }

        protected override void Singleton(IServiceCollection services)
        {
            base.Singleton(services);
        }

        protected override void Transient(IServiceCollection services)
        {
            services.AddTransient<IEmbarcadorasTransportadorasService, EmbarcadorasTransportadorasService>();
            services.AddTransient<IEmpresasService, EmpresasService>();
            services.AddTransient<ILancesService, LancesService>();
            services.AddTransient<IOfertasService, OfertasService>();
            services.AddTransient<IProdutosService, ProdutosService>(); 
            services.AddTransient<IPasswordsService, PasswordsService>();
            services.AddTransient<ITokenAuthService, TokenAuthService>();
            services.AddTransient<IUsuariosService, UsuariosService>();

            base.Transient(services);
        }
    }
}
