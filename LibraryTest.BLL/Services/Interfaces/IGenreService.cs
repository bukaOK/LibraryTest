using LibraryTest.Models.Genres;
using Manlike.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.BLL.Services.Interfaces
{
    public interface IGenreService : IDataService
    {
        Task<GenreModel> GetAsync(Guid id);
        Task<IList<GenreModel>> GetAllAsync();
        Task<DataServiceResult> AddAsync(GenreModel model);
        Task<DataServiceResult> UpdateAsync(GenreModel model);
        Task<DataServiceResult> RemoveAsync(Guid id);
    }
}
