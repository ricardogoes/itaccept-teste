using System.Data;

namespace ItAccept.Teste.Domain.Interfaces.AppSettings
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
