using ItAccept.Teste.Domain.Interfaces.AppSettings;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Infrastructure.Data.Configuration;
using ItAccept.Teste.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ItAccept.Teste.Infrastructure.Crosscutting.IoC
{
    public class ServiceRepositories : ServiceBase
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
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            base.Singleton(services);
        }

        protected override void Transient(IServiceCollection services)
        {

            services.AddTransient<IEmbarcadorasTransportadorasRepository, EmbarcadorasTransportadorasRepository>();
            services.AddTransient<IEmpresasRepository, EmpresasRepository>();
            services.AddTransient<ILancesRepository, LancesRepository>();
            services.AddTransient<IOfertasRepository, OfertasRepository>();
            services.AddTransient<IPasswordsRepository, PasswordsRepository>();
            services.AddTransient<IProdutosRepository, ProdutosRepository>();
            services.AddTransient<IUsuariosRepository, UsuariosRepository>();

            base.Transient(services);
        }
    }
}
