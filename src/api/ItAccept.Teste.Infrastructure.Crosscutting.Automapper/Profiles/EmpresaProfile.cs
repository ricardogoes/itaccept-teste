using AutoMapper;
using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.ViewModels.Empresas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItAccept.Teste.Infrastructure.Crosscutting.Automapper.Profiles
{
    public class EmpresaProfile : Profile
    {
        public EmpresaProfile()
        {
            // Insert
            CreateMap<Empresa, EmpresaParaInserirVM>();
            CreateMap<EmpresaParaInserirVM, Empresa>();

            // Update
            CreateMap<Empresa, EmpresaParaAtualizarVM>();
            CreateMap<EmpresaParaAtualizarVM, Empresa>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
