using AutoMapper;
using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.ViewModels.Ofertas;

namespace ItAccept.Teste.Infrastructure.Crosscutting.Automapper.Profiles
{
    public class OfertaProfile : Profile
    {
        public OfertaProfile()
        {
            // Insert
            CreateMap<Oferta, OfertaParaInserirVM>();
            CreateMap<OfertaParaInserirVM, Oferta>();

            // Update
            CreateMap<Oferta, OfertaParaAtualizarVM>();
            CreateMap<OfertaParaAtualizarVM, Oferta>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));

            // Grid
            CreateMap<Oferta, OfertaParaConsultarVM>();
            CreateMap<OfertaParaConsultarVM, Oferta>();
        }
    }
}
