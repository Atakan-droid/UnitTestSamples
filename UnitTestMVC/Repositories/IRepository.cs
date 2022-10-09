namespace UnitTestMVC.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GeAll();
        Task<TEntity> GetById(int id);
        Task Create(TEntity entity);
        void Update(TEntity entity);
        Task Delete(TEntity entity);

    }
}
