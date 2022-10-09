using Microsoft.EntityFrameworkCore;
using UnitTestMVC.Models;

namespace UnitTestMVC.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbForUntTestContext _dbForUntTestContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbForUntTestContext context)
        {
            _dbForUntTestContext = context;
            _dbSet = _dbForUntTestContext.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> GeAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Create(TEntity entity)
        {
            await _dbForUntTestContext.AddAsync(entity);
            await _dbForUntTestContext.SaveChangesAsync();
        }

        public async void Update(TEntity entity)
        {
            _dbForUntTestContext.Entry(entity).State = EntityState.Modified;
            //_dbForUntTestContext.Update(entity);
            await _dbForUntTestContext.SaveChangesAsync();
        }

        public async Task Delete(TEntity entity)
        {
            _dbForUntTestContext.Remove(entity);
            await _dbForUntTestContext.SaveChangesAsync();
        }
    }
}
