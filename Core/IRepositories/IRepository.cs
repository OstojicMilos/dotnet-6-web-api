namespace CleanArchitecture.Core.IRepositories
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetAsync(TKey id);
        Task<TEntity> AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        TEntity Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        TEntity Delete(TEntity entity);
        Task<int> SaveChangesAsync();
    }
}
