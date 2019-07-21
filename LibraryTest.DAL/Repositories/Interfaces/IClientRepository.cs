using LibraryTest.CodeData.Enums;
using LibraryTest.DAL.Core;
using LibraryTest.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.DAL.Repositories.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> GetByPhoneAsync(string phone);
        Task<(int clientsCount, IList<Client> clients)> FilterAsync(string name, ClientTypes clientTypes, int startIndex, int limit);
        Task<bool> IsExistAsync(Guid id);
    }
}
