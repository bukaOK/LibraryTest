using AutoMapper;
using LibraryTest.BLL.Services.Interfaces;
using LibraryTest.DAL.Entities;
using LibraryTest.DAL.Repositories.Interfaces;
using LibraryTest.Models.Books;
using Manlike.BLL.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LibraryTest.BLL.Services
{
    public class BookMoveService : DataService, IBookMoveService
    {
        private readonly IMapper mapper;
        private readonly IBookMoveRepository moveRepository;

        public BookMoveService(ILogger<BookMoveService> logger, IMapper mapper, IBookMoveRepository moveRepository) : base(logger)
        {
            this.mapper = mapper;
            this.moveRepository = moveRepository;
        }

        public async Task<DataServiceResult> AddAsync(BookMoveModel moveModel)
        {
            try
            {
                var bookMove = mapper.Map<BookMove>(moveModel);
                bookMove.Date = DateTime.UtcNow;
                await moveRepository.AddAsync(bookMove);
                return DataServiceResult.Success(bookMove.Id);
            }
            catch(Exception e)
            {
                return CommonError("Ошибка при добавлении движения книги", e);
            }
        }

        public async Task<DataServiceResult> RemoveAsync(Guid id)
        {
            try
            {
                var bookMove = await moveRepository.GetAsync(id);
                if(bookMove == null)
                {
                    logger.LogWarning($"Не найдено движение книги при удалении: {id}");
                    return DataServiceResult.Failed("Не найдено движение книги при удалении");
                }
                await moveRepository.RemoveAsync(bookMove);
                return Success;
            }
            catch (Exception e)
            {
                return CommonError("Ошибка при добавлении движения книги", e);
            }
        }
    }
}
