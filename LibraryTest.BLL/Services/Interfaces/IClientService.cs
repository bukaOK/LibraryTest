using LibraryTest.CodeData.Enums;
using LibraryTest.Models.Clients;
using Manlike.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.BLL.Services.Interfaces
{
    public interface IClientService : IDataService
    {
        Task<ClientModel> GetAsync(Guid id);
        Task<ClientListModel> GetClientsAsync(int page, string name, ClientTypes clientType);
        Task<ClientModel> GetByPhoneAsync(string phone);
        Task<ClientModel> GetClientAsync(Guid id);
        Task<DataServiceResult> AddAsync(ClientModel model);
        Task<DataServiceResult> UpdateAsync(ClientModel model);
        Task<DataServiceResult> RemoveAsync(Guid id);
    }
}
