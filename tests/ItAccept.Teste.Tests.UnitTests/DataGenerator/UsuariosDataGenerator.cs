using Bogus;
using ItAccept.Teste.Domain.ViewModels.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItAccept.Teste.Tests.UnitTests.DataGenerator
{
    public class UsuariosDataGenerator
    {
        public static UsuarioParaConsultarVM GerarUsuarioParaConsultarValido()
        {
            var usuario = new Faker<UsuarioParaConsultarVM>("pt_BR")
                .CustomInstantiator(f => new UsuarioParaConsultarVM
                {
                    UsuarioId = 1,
                    NomeUsuario = f.Name.FullName(),
                    Status = true,
                    TipoUsuario = "Funcionario",
                    Username = "admin",
                    EmpresaId = 1,
                    NomeEmpresa = f.Company.CompanyName(),
                    TipoEmpresa = "Embarcadora",
                    
                });

            return usuario.Generate(1).FirstOrDefault();
        }
    }
}
