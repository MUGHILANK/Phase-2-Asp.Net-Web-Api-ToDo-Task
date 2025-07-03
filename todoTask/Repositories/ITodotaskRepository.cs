using todoTask.Models.Domain;

namespace todoTask.Repositories
{
    public interface ITodotaskRepository
    {
        //from Domain models
        Task<Todotask> CreateAsync(Todotask todotask);
        Task<List<Todotask>>GetAllDataAsync();
        Task<Todotask> GetById(Guid id);
        Task<Todotask> UpdateAsync(Guid id,Todotask todotask);

        Task<Todotask> DeleteAsync(Guid id);
    }
}
