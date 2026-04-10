namespace ExaminationSystem.BuildingBlocks.Interfaces
{
    public interface IGeneralRepository<TEnity> where TEnity : class
    {
            Task<TEnity?> GetByIdAsync(int id);
            IQueryable<TEnity> GetAll();
            void Add(TEnity entity);
            void Update(TEnity entity);
            void Delete(TEnity entity);
    }
}
