using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.AppSettings;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Domain.ViewModels.Usuarios;
using MySqlConnector;
using System.Data;

namespace ItAccept.Teste.Infrastructure.Data.Repositories
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly IDapperWrapper _dapperWrapper;
        
        public UsuariosRepository(IDapperWrapper dapperWrapper)
        {
            _dapperWrapper = dapperWrapper ?? throw new ArgumentNullException(nameof(dapperWrapper));
        }

        public async Task<int> AtualizarAsync(Usuario usuario)
        {
            if (usuario is null)
                throw new ArgumentNullException(nameof(usuario));

            var sqlCommand = $@"UPDATE usuarios
			                        SET nome_usuario = '{MySqlHelper.EscapeString(usuario.NomeUsuario)}',
                                        username = '{MySqlHelper.EscapeString(usuario.Username)}',
                                        password = '{MySqlHelper.EscapeString(usuario.Password)}',
                                        password_hash = {usuario.PasswordHash},
                                        empresa_id = {usuario.EmpresaId},
                                        tipo_usuario = '{MySqlHelper.EscapeString(usuario.TipoUsuario.ToString())}'
		                        WHERE usuario_id = {usuario.UsuarioId};";

            await _dapperWrapper.ExecuteAsync(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return usuario.UsuarioId;
        }

        public async Task<IEnumerable<UsuarioParaConsultarVM>> ConsultarPelaEmpresaAsync(int empresaId)
        {
            if (empresaId <= 0)
                throw new ArgumentException("Inválido", nameof(empresaId));

            var sqlCommand = $@"SELECT 	U.usuario_id AS UsuarioId,
				                        U.nome_usuario AS NomeUsuario,
                                        U.username AS Username,
                                        U.empresa_id AS EmpresaId,
                                        E.nome_empresa AS NomeEmpresa,
                                        U.status AS Status,
                                        U.tipo_usuario AS TipoUsuario,
                                        E.tipo_empresa AS TipoEmpresa
		                        FROM usuarios U
                                INNER JOIN empresas E
			                        ON U.empresa_id = E.empresa_id
                                WHERE U.empresa_id = {empresaId};";

            var usuarios = await _dapperWrapper.QueryAsync<UsuarioParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return usuarios;
        }

        public async Task<IEnumerable<UsuarioParaConsultarVM>> ConsultarAtivosPelaEmpresaAsync(int empresaId)
        {
            if (empresaId <= 0)
                throw new ArgumentException("Inválido", nameof(empresaId));

            var sqlCommand = $@"SELECT 	U.usuario_id AS UsuarioId,
				                        U.nome_usuario AS NomeUsuario,
                                        U.username AS Username,
                                        U.empresa_id AS EmpresaId,
                                        E.nome_empresa AS NomeEmpresa,
                                        U.status AS Status,
                                        U.tipo_usuario AS TipoUsuario,
                                        E.tipo_empresa AS TipoEmpresa
		                        FROM usuarios U
                                INNER JOIN empresas E
			                        ON U.empresa_id = E.empresa_id
                                WHERE U.empresa_id = {empresaId}
                                  AND U.status = 1;";

            var usuarios = await _dapperWrapper.QueryAsync<UsuarioParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return usuarios;
        }

        public async Task<UsuarioParaConsultarVM> ConsultarPeloUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Inválido", nameof(username));

            var sqlCommand = $@"SELECT 	U.usuario_id AS UsuarioId,
				                        U.nome_usuario AS NomeUsuario,
                                        U.username AS Username,
                                        U.empresa_id AS EmpresaId,
                                        E.nome_empresa AS NomeEmpresa,
                                        U.status AS Status,
                                        U.tipo_usuario AS TipoUsuario,
                                        E.tipo_empresa AS TipoEmpresa
		                        FROM usuarios U
                                LEFT JOIN empresas E
			                        ON U.empresa_id = E.empresa_id
                                WHERE U.username = '{MySqlHelper.EscapeString(username)}';";

            var usuario = await _dapperWrapper.QueryFirstOrDefaultAsync<UsuarioParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return usuario;
        }

        public async Task<UsuarioParaConsultarVM> ConsultarPeloUsernameEPasswordAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Inválido", nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Inválido", nameof(password));

            var sqlCommand = $@"SELECT 	U.usuario_id AS UsuarioId,
				                        U.nome_usuario AS NomeUsuario,
                                        U.username AS Username,
                                        U.empresa_id AS EmpresaId,
                                        E.nome_empresa AS NomeEmpresa,
                                        U.status AS Status,
                                        U.tipo_usuario AS TipoUsuario,
                                        E.tipo_empresa AS TipoEmpresa
		                        FROM usuarios U
                                LEFT JOIN empresas E
			                        ON U.empresa_id = E.empresa_id
                                WHERE U.username = '{MySqlHelper.EscapeString(username)}'
                                  AND U.password = '{MySqlHelper.EscapeString(password)}';";

            var usuario = await _dapperWrapper.QueryFirstOrDefaultAsync<UsuarioParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return usuario;
        }

        public async Task<UsuarioParaConsultarVM> ConsultarPeloIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Inválido", nameof(id));

            var sqlCommand = $@"SELECT 	U.usuario_id AS UsuarioId,
				                        U.nome_usuario AS NomeUsuario,
                                        U.username AS Username,
                                        U.empresa_id AS EmpresaId,
                                        E.nome_empresa AS NomeEmpresa,
                                        U.status AS Status,
                                        U.tipo_usuario AS TipoUsuario,
                                        E.tipo_empresa AS TipoEmpresa
		                        FROM usuarios U
                                LEFT JOIN empresas E
			                        ON U.empresa_id = E.empresa_id
                                WHERE U.usuario_id = {id};";

            var usuario = await _dapperWrapper.QueryFirstOrDefaultAsync<UsuarioParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return usuario;
        }

        public async Task<int> InativarAsync(Usuario usuario)
        {
            if (usuario is null)
                throw new ArgumentNullException(nameof(usuario));

            var sqlCommand = $@"UPDATE usuarios
			                        SET status = {usuario.Status}
		                        WHERE usuario_id = {usuario.UsuarioId};";

            await _dapperWrapper.ExecuteAsync(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return usuario.UsuarioId;
        }

        public async Task<int> InserirAsync(Usuario usuario)
        {
            if (usuario is null)
                throw new ArgumentNullException(nameof(usuario));

            var idInserido = await _dapperWrapper.QueryFirstOrDefaultAsync<int>(
                sql: $@"INSERT INTO usuarios(nome_usuario, username, password, password_hash, empresa_id, status, tipo_usuario)
                            VALUES(@nome_usuario, @username, @password, @password_hash, @empresa_id, @status, @tipo_usuario);

                        SELECT LAST_INSERT_ID();", 
                param: new 
                { 
                    nome_usuario = MySqlHelper.EscapeString(usuario.NomeUsuario),
                    username = MySqlHelper.EscapeString(usuario.Username),
                    password = MySqlHelper.EscapeString(usuario.Password),
                    password_hash = usuario.PasswordHash,
                    empresa_id = usuario.EmpresaId,
                    status = usuario.Status,
                    tipo_usuario = MySqlHelper.EscapeString(usuario.TipoUsuario),
                },
                commandType: CommandType.Text);

            return idInserido;
        }
    }
}
