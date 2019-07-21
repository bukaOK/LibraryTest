using LibraryTest.DAL.Core;
using LibraryTest.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.DAL.Repositories.Interfaces
{
    public interface IStoryBookRepository : IRepository<StoryBook>
    {
        Task<StoryBook> GetWithStoriesAsync(Guid id);
    }
}
