using CleanArchitecture.Core.IRepositories;
using CleanArchitecture.DataAccess.Database;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.DataAccess.Repositories
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected ApplicationContext _context;
        private DbSet<TEntity> _set;

        public Repository(ApplicationContext context)
        {
            _context = context;
            _set = context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            return (await _set.AddAsync(entity)).Entity;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _set.AddRangeAsync(entities);
        }

        public TEntity Delete(TEntity entity)
        {
            return (_set.Remove(entity)).Entity;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public async Task<TEntity?> GetAsync(TKey id)
        {
            return await _set.FindAsync(id);
        }

        public TEntity Update(TEntity entity)
        {
            return _set.Update(entity).Entity;
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _set.UpdateRange(entities);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
