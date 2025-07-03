using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using todoTask.Data;
using todoTask.Models.Domain;

namespace todoTask.Repositories
{
    public class SQLTodotaskRepository : ITodotaskRepository
    {
        private readonly MKTodotaskDbContext dbContext;

        public SQLTodotaskRepository(MKTodotaskDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Todotask> CreateAsync(Todotask todotask)
        {
            await dbContext.Todotasks.AddAsync(todotask);
            await dbContext.SaveChangesAsync();
            return todotask;
        }

        public async Task<List<Todotask>> GetAllDataAsync()
        {
            //var getdata = await dbContext.Todotasks.ToArrayAsync();
            return await dbContext.Todotasks.ToListAsync(); ;

        }

        public async Task<Todotask> GetById(Guid id)
        {
            var value = await dbContext.Todotasks.FirstOrDefaultAsync(x => x.Id == id);
            return value;
        }

        public async Task<Todotask?> UpdateAsync(Guid id, Todotask todotask)
        {
            var existingTask = await dbContext.Todotasks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingTask == null)
            {
                return null;
            }

            existingTask.taskName = todotask.taskName;
            existingTask.taskStatus = todotask.taskStatus;

            dbContext.Todotasks.Update(existingTask);
            await dbContext.SaveChangesAsync();

            return existingTask;


        }

       public async Task<Todotask> DeleteAsync(Guid id)
        {
            var todotask = await dbContext.Todotasks.FirstOrDefaultAsync(x => x.Id == id);
            if (todotask == null)
            {
                return null;
            }
            dbContext.Todotasks.Remove(todotask);
            await dbContext.SaveChangesAsync();
            return todotask;
        }


    }
}
