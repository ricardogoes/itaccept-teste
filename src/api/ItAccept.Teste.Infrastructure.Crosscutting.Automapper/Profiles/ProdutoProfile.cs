using AutoMapper;
using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.ViewModels.Produtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItAccept.Teste.Infrastructure.Crosscutting.Automapper.Profiles
{
    public class ProdutoProfile : Profile
    {
        public ProdutoProfile()
        {
            // Insert
            CreateMap<Produto, ProdutoParaInserirVM>();
            CreateMap<ProdutoParaInserirVM, Produto>();

            // Update
            CreateMap<Produto, ProdutoParaAtualizarVM>();
            CreateMap<ProdutoParaAtualizarVM, Produto>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));

            // Grid
            CreateMap<Produto, ProdutoParaConsultarVM>();
            CreateMap<ProdutoParaConsultarVM, Produto>();
        }
    }
}
