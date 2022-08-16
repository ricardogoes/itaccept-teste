﻿using System.ComponentModel.DataAnnotations;

namespace ItAccept.Teste.Domain.ViewModels.Usuarios
{
    public class UsuarioParaInserirVM
    {
        [Required(ErrorMessage = "NomeUsuario obrigatório")]
        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "Username obrigatório")] 
        public string Username { get; set; }

        [Required(ErrorMessage = "Password obrigatório")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "EmpresaId obrigatório")] 
        public int? EmpresaId { get; set; }
        
        [Required(ErrorMessage = "TipoUsuario obrigatório")] 
        public string TipoUsuario { get; set; }
    }
}
