using AutoMapper;
using LibraryTest.BLL.Services.Interfaces;
using LibraryTest.CodeData.Enums;
using LibraryTest.DAL.Entities;
using LibraryTest.DAL.Repositories.Interfaces;
using LibraryTest.Models.Clients;
using Manlike.BLL.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryTest.BLL.Services
{
    public class ClientService : DataService, IClientService
    {
        private readonly IMapper mapper;
        private readonly IClientRepository clientRepository;
        private readonly IConfiguration configuration;

        public ClientService(ILogger<ClientService> logger, IMapper mapper, IClientRepository clientRepository,
            IConfiguration configuration) : base(logger)
        {
            this.mapper = mapper;
            this.clientRepository = clientRepository;
            this.configuration = configuration;
        }

        public async Task<DataServiceResult> AddAsync(ClientModel model)
        {
            try
            {
                var entity = mapper.Map<Client>(model);
                entity.RegisterDate = DateTime.UtcNow;

                await clientRepository.AddAsync(entity);
                return DataServiceResult.Success(entity.Id);
            }
            catch(Exception e)
            {
                return CommonError("Ошибка при добавлении клиента", e);
            }
        }

        public async Task<ClientModel> GetByPhoneAsync(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return null;
            var client = await clientRepository.GetByPhoneAsync(phone);
            if (client == null)
                return null;

            return mapper.Map<ClientModel>(client);
        }

        public async Task<ClientModel> GetClientAsync(Guid id)
        {
            var entity = await clientRepository.GetAsync(id);
            var model = mapper.Map<ClientModel>(entity);
            return model;
        }

        public async Task<ClientListModel> GetClientsAsync(int page, string name, ClientTypes clientType)
        {
            var rowsPerPage = int.Parse(configuration["Filter:rowsPerPage"]);

            var startIndex = (page - 1) * rowsPerPage;
            var (clientsCount, entities) = await clientRepository.FilterAsync(name, clientType, startIndex, rowsPerPage);
            var clients = mapper.Map<IList<ClientModel>>(entities);

            return new ClientListModel
            {
                Clients = clients,
                PagesCount = (int)Math.Ceiling((double)clientsCount / rowsPerPage)
            };
        }
        public async Task<ClientModel> GetAsync(Guid id)
        {
            var client = await clientRepository.GetAsync(id);
            return mapper.Map<ClientModel>(client);
        }

        public async Task<DataServiceResult> RemoveAsync(Guid id)
        {
            try
            {
                var client = await clientRepository.GetAsync(id);
                if(client == null)
                {
                    logger.LogWarning("Не найден клиент при удалении");
                    return DataServiceResult.Failed("Клиента не существует");
                }
                await clientRepository.RemoveAsync(client);
                return Success;
            }
            catch(Exception e)
            {
                return CommonError("Ошибка при удалении клиента", e);
            }
        }

        public async Task<DataServiceResult> UpdateAsync(ClientModel model)
        {
            try
            {
                var entity = await clientRepository.GetAsync(model.Id);
                if(entity == null)
                {
                    logger.LogWarning("Не найден клиент при обновлении");
                    return DataServiceResult.Failed("Клиент не найден");
                }
                mapper.Map(model, entity);
                await clientRepository.UpdateAsync(entity);
                return DataServiceResult.Success(entity.Id);
            }
            catch (Exception e)
            {
                return CommonError("Ошибка при добавлении клиента", e);
            }
        }
    }
}
