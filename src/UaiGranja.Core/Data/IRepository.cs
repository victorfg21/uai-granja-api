using UaiGranja.Core.DomainObjects;

namespace UaiGranja.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        Task<T> ObterPorId(Guid id);

        Task<T> ObterPorIdNoTracking(Guid id);

        Task<IEnumerable<T>> ObterTodosNoTracking();
    }
}
