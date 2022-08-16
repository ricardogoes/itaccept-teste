using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Domain.ViewModels.Usuarios;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Interfaces;

namespace ItAccept.Teste.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IUsuariosRepository _repository;
        private readonly IPasswordsService _passwordsService;

        public UsuariosService(IUsuariosRepository repository, IPasswordsService passwordsService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _passwordsService = passwordsService ?? throw new ArgumentNullException(nameof(passwordsService));
        }

        public async Task<int> AtualizarAsync(Usuario entity)
            => await _repository.AtualizarAsync(entity);

        public async Task<IEnumerable<UsuarioParaConsultarVM>> ConsultarPelaEmpresaAsync(int empresaId)
            => await _repository.ConsultarPelaEmpresaAsync(empresaId);

        public async Task<IEnumerable<UsuarioParaConsultarVM>> ConsultarAtivosPelaEmpresaAsync(int empresaId)
            => await _repository.ConsultarAtivosPelaEmpresaAsync(empresaId);

        public async Task<UsuarioParaConsultarVM> ConsultarPeloUsernameAsync(string username)
            => await _repository.ConsultarPeloUsernameAsync(username);

        public async Task<UsuarioParaConsultarVM> ConsultarPeloUsernameEPasswordAsync(string username, string password)
            => await _repository.ConsultarPeloUsernameEPasswordAsync(username, password);

        public async Task<UsuarioParaConsultarVM> ConsultarPeloIdAsync(int id)
            => await _repository.ConsultarPeloIdAsync(id);

        public async Task<int> InativarAsync(Usuario entity)
            => await _repository.InativarAsync(entity);

        public async Task<int> InserirAsync(Usuario usuario)
        {
            var hash = _passwordsService.Encriptar(usuario.Password);

            usuario.Password = hash.EncryptedPassword;
            usuario.PasswordHash = hash.Salt;
            usuario.Status = true;

            //if (await UsernameEstaDuplicadoAsync(usuario.Username))
            //    throw new EntityValidationException(UtilsService.CriaEntityValidationException("E-mail duplicado"));

            return await _repository.InserirAsync(usuario);
        }

        public async Task<bool> UsernameEstaDuplicadoAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Inválido", nameof(username));

            var usuario = await ConsultarPeloUsernameAsync(username);
            return usuario != null;
        }
    }
}
