namespace UaiGranja.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
