using ItAccept.Teste.Domain.Models.AppSettings;
using Microsoft.Extensions.DependencyInjection;

namespace ItAccept.Teste.Infrastructure.Crosscutting.IoC
{
    public abstract class ServiceBase
    {
        public static T GetInstance<T>()
            => (T)Activator.CreateInstance(typeof(T));

        public static T GetInstance<T>(AppSettings appSettings)
            => (T)Activator.CreateInstance(typeof(T), new object[] { appSettings });

        public void Add(IServiceCollection services)
        {
            HttpClient(services);
            Scoped(services);
            Singleton(services);
            Transient(services);
        }

        protected virtual void HttpClient(IServiceCollection services)
        {
        }

        protected virtual void Scoped(IServiceCollection services)
        {
        }

        protected virtual void Singleton(IServiceCollection services)
        {
        }

        protected virtual void Transient(IServiceCollection services)
        {
        }
    }
}
