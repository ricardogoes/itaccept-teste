using AutoMapper;
using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.ViewModels.Lances;

namespace ItAccept.Teste.Infrastructure.Crosscutting.Automapper.Profiles
{
    public class LanceProfile : Profile
    {
        public LanceProfile()
        {
            // Insert
            CreateMap<Lance, LanceParaInserirVM>();
            CreateMap<LanceParaInserirVM, Lance>();

            // Update
            CreateMap<Lance, LanceParaAtualizarVM>();
            CreateMap<LanceParaAtualizarVM, Lance>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));

            // Grid
            CreateMap<Lance, LanceParaConsultarVM>();
            CreateMap<LanceParaConsultarVM, Lance>();
        }
    }
}
