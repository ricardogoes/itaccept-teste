using AutoMapper;
using ItAccept.Teste.Infrastructure.Crosscutting.Automapper.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace ItAccept.Teste.Infrastructure.Crosscutting.Automapper
{
    public static class MappingConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            IMapper mapper = CreateMapper();
            services.AddSingleton(mapper);
        }

        public static IMapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(x =>
            {
                x.AddProfile<EmbarcadoraTransportadoraProfile>();
                x.AddProfile<EmpresaProfile>();
                x.AddProfile<LanceProfile>();
                x.AddProfile<OfertaProfile>();
                x.AddProfile<ProdutoProfile>();
                x.AddProfile<UsuarioProfile>();

            });
            return mapperConfig.CreateMapper();
        }
    }
}
