using ItAccept.Teste.Domain.Interfaces.AppSettings;

namespace ItAccept.Teste.Domain.Models.AppSettings
{
    public sealed class ConnectionSettings : IConnectionSettings
    {
        public string DBConnection { get; set; }
    }
}
