namespace ItAccept.Teste.Domain.Interfaces.Generic
{
    public interface ICrud<Entity, ViewModel> 
        where Entity : class
        where ViewModel : class
    {
        Task<int> InserirAsync(Entity entity);
        Task<int> AtualizarAsync(Entity entity);
        Task<ViewModel> ConsultarPeloIdAsync(int id);
    }
}
