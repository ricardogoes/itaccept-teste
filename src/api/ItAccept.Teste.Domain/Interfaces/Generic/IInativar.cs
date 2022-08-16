namespace ItAccept.Teste.Domain.Interfaces.Generic
{
    public interface IInativar<T> where T : class
    {
        Task<int> InativarAsync(T entity);
    }
}
