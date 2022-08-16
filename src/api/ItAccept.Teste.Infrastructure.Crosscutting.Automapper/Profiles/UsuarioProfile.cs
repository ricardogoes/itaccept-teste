using AutoMapper;
using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.ViewModels.Usuarios;

namespace ItAccept.Teste.Infrastructure.Crosscutting.Automapper.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            // Insert
            CreateMap<Usuario, UsuarioParaInserirVM>();
            CreateMap<UsuarioParaInserirVM, Usuario>();

            // Update
            CreateMap<Usuario, UsuarioParaAtualizarVM>();
            CreateMap<UsuarioParaAtualizarVM, Usuario>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));

            // Grid
            CreateMap<Usuario, UsuarioParaConsultarVM>();
            CreateMap<UsuarioParaConsultarVM, Usuario>();
        }
    }
}
