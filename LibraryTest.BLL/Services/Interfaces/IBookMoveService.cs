using LibraryTest.Models.Books;
using Manlike.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.BLL.Services.Interfaces
{
    public interface IBookMoveService : IDataService
    {
        Task<DataServiceResult> AddAsync(BookMoveModel moveModel);
        Task<DataServiceResult> RemoveAsync(Guid id);
    }
}
