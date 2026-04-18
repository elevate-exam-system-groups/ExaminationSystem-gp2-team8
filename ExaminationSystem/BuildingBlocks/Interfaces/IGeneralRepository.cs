namespace ExaminationSystem.BuildingBlocks.Interfaces
{
    public interface IGeneralRepository<TEntity> where TEntity : class, Domain.Entities.IBaseEntity
    {
            Task<TEnity?> GetByIdAsync(int id);
            IQueryable<TEnity> GetAll();
            void Add(TEnity entity);
            void Update(TEnity entity);
            void Delete(TEnity entity);
    }
}
