using AutoMapper;
using LibraryTest.BLL.Services.Interfaces;
using LibraryTest.DAL.Entities;
using LibraryTest.DAL.Repositories.Interfaces;
using LibraryTest.Models.Genres;
using Manlike.BLL.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryTest.BLL.Services
{
    public class GenreService : DataService, IGenreService
    {
        private readonly IMapper mapper;
        private readonly IGenreRepository genreRepository;

        public GenreService(ILogger<GenreService> logger, IMapper mapper, IGenreRepository genreRepository) : base(logger)
        {
            this.mapper = mapper;
            this.genreRepository = genreRepository;
        }

        public async Task<DataServiceResult> AddAsync(GenreModel model)
        {
            try
            {
                var entity = mapper.Map<Genre>(model);
                await genreRepository.AddAsync(entity);
                return DataServiceResult.Success(entity.Id);
            }
            catch(Exception e)
            {
                return CommonError("Ошибка при добавлении жанра", e);
            }
        }

        public async Task<IList<GenreModel>> GetAllAsync()
        {
            var entities = await genreRepository.GetAllAsync();
            var models = mapper.Map<IList<GenreModel>>(entities);
            return models;
        }

        public async Task<GenreModel> GetAsync(Guid id)
        {
            var genre = await genreRepository.GetAsync(id);
            if (genre == null)
                return null;
            return mapper.Map<GenreModel>(genre);
        }

        public async Task<DataServiceResult> RemoveAsync(Guid id)
        {
            try
            {
                var entity = await genreRepository.GetAsync(id);
                if(entity == null)
                {
                    logger.LogWarning("Не найден жанр при удалении");
                    return DataServiceResult.Failed();
                }
                await genreRepository.RemoveAsync(entity);

                return Success;
            }
            catch(Exception e)
            {
                return CommonError("Ошибка при удалении жанра", e);
            }
        }

        public async Task<DataServiceResult> UpdateAsync(GenreModel model)
        {
            try
            {
                var entity = await genreRepository.GetAsync(model.Id);
                if(entity == null)
                {
                    logger.LogWarning("Не найден жанр при обновлении");
                    return DataServiceResult.Failed();
                }
                mapper.Map(model, entity);
                await genreRepository.UpdateAsync(entity);
                return Success;
            }
            catch (Exception e)
            {
                return CommonError("Ошибка при добавлении жанра", e);
            }
        }
    }
}
