namespace ItAccept.Teste.Domain.Interfaces.Generic
{
    public interface IApagar<T> where T : class
    {
        Task ApagarAsync(T entity);
    }
}
