using AutoMapper;
using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.ViewModels.EmbarcadorasTransportadoras;

namespace ItAccept.Teste.Infrastructure.Crosscutting.Automapper.Profiles
{
    public class EmbarcadoraTransportadoraProfile : Profile
    {
        public EmbarcadoraTransportadoraProfile()
        {
            // Insert
            CreateMap<EmbarcadoraTransportadora, EmbarcadoraTransportadoraParaInserirVM>();
            CreateMap<EmbarcadoraTransportadoraParaInserirVM, EmbarcadoraTransportadora>();

            // Update
            CreateMap<EmbarcadoraTransportadora, EmbarcadoraTransportadoraParaAtualizarVM>();
            CreateMap<EmbarcadoraTransportadoraParaAtualizarVM, EmbarcadoraTransportadora>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));

            // Grid
            CreateMap<EmbarcadoraTransportadora, EmbarcadoraTransportadoraParaConsultarVM>();
            CreateMap<EmbarcadoraTransportadoraParaConsultarVM, EmbarcadoraTransportadora>();
        }
    }
}
