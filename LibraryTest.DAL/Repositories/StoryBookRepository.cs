using LibraryTest.DAL.Core;
using LibraryTest.DAL.Entities;
using LibraryTest.DAL.Infrastructure;
using LibraryTest.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.DAL.Repositories
{
    public class StoryBookRepository : Repository<StoryBook>, IStoryBookRepository
    {
        public StoryBookRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<StoryBook> GetWithStoriesAsync(Guid id)
        {
            return await Table.AsNoTracking()
                .Include(x => x.Stories)
                .FirstOrDefaultAsync(x => x.BookId == id);
        }

        public async override Task UpdateAsync(StoryBook entity)
        {
            var newStories = entity.Stories.ToList();
            entity.Stories = null;

            var existingStories = await DbContext.Stories.Where(x => x.StoryBookId == entity.BookId).ToListAsync();

            var storiesToAdd = newStories.Where(x => !existingStories.Any(y => y.Id == x.Id));
            DbContext.Stories.AddRange(storiesToAdd);

            var storiesToRemove = existingStories.Where(x => !newStories.Any(y => y.Id == x.Id));
            DbContext.Stories.RemoveRange(storiesToRemove);

            var storiesToUpdate = existingStories.Where(x => newStories.Any(y => y.Id == x.Id));
            DbContext.Stories.UpdateRange(storiesToUpdate);

            await base.UpdateAsync(entity);
        }
    }
}
