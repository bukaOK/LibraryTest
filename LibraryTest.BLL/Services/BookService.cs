using AutoMapper;
using LibraryTest.BLL.Services.Interfaces;
using LibraryTest.CodeData.Enums;
using LibraryTest.DAL.Entities;
using LibraryTest.DAL.Filters;
using LibraryTest.DAL.Repositories.Interfaces;
using LibraryTest.Models.Books;
using Manlike.BLL.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryTest.BLL.Services
{
    public class BookService : DataService, IBookService
    {
        private readonly IMapper mapper;
        private readonly IBookRepository bookRepository;
        private readonly IClientRepository clientRepository;
        private readonly IClassicBookRepository classicBookRepository;
        private readonly IStoryBookRepository storyBookRepository;
        private readonly IBookMoveService bookMoveService;
        private readonly IGenreRepository genreRepository;
        private readonly IConfiguration configuration;

        public BookService(ILogger<BookService> logger, IMapper mapper, IBookRepository bookRepository,
            IClientRepository clientRepository, IClassicBookRepository classicBookRepository, 
            IStoryBookRepository storyBookRepository, IBookMoveService bookMoveService, 
            IGenreRepository genreRepository, IConfiguration configuration) : base(logger)
        {
            this.mapper = mapper;
            this.bookRepository = bookRepository;
            this.clientRepository = clientRepository;
            this.classicBookRepository = classicBookRepository;
            this.storyBookRepository = storyBookRepository;
            this.bookMoveService = bookMoveService;
            this.genreRepository = genreRepository;
            this.configuration = configuration;
        }

        public async Task<DataServiceResult> AddAsync(BookModel model)
        {
            using (var trans = await bookRepository.BeginTransactionAsync())
            {
                try
                {
                    var isGenreExist = await genreRepository.IsExistAsync((Guid)model.GenreId);
                    if (!isGenreExist)
                    {
                        logger.LogWarning("Жанр не найден при добавлении книги");
                        return DataServiceResult.Failed("Жанр не найден");
                    }
                    var entity = mapper.Map<Book>(model);
                    entity.RegisterDate = DateTime.UtcNow;

                    await bookRepository.AddAsync(entity);

                    if (model.Category == BookCategories.Classic)
                    {
                        if (model.VolCount == null)
                        {
                            trans.Rollback();
                            return DataServiceResult.Failed("Заполните количество глав");
                        }
                        var classicBook = new ClassicBook
                        {
                            BookId = entity.Id,
                            VolCount = (int)model.VolCount
                        };
                        await classicBookRepository.AddAsync(classicBook);
                        trans.Commit();
                    }
                    else if (model.Category == BookCategories.Story)
                    {
                        var storyBook = new StoryBook
                        {
                            BookId = entity.Id,
                            Stories = mapper.Map<IEnumerable<Story>>(model.Stories).Select(st =>
                            {
                                st.StoryBookId = entity.Id;
                                return st;
                            }).ToList()
                        };
                        await storyBookRepository.AddAsync(storyBook);
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback();
                        return DataServiceResult.Failed("Заполните категорию");
                    }
                    return DataServiceResult.Success(entity.Id);
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    return CommonError("Ошибка при добавлении книги", e);
                }
            }
        }

        public async Task<IList<CatalogBookModel>> GetByClientAsync(Guid clientId)
        {
            var books = await bookRepository.GetByClientAsync(clientId);
            return mapper.Map<IList<CatalogBookModel>>(books);
        }

        public async Task<BookListModel> GetCatalogAsync(BookFilterModel filterModel)
        {
            var rowsPerPage = int.Parse(configuration["filter:rowsPerPage"]);

            var filter = mapper.Map<BookListFilter>(filterModel);
            filter.StartIndex = (filterModel.Page - 1) * rowsPerPage;
            filter.Limit = rowsPerPage;

            var (booksCount, books) = await bookRepository.FilterAsync(filter);
            return new BookListModel
            {
                Books = mapper.Map<IList<CatalogBookModel>>(books),
                PagesCount = (int)Math.Ceiling((double)booksCount / rowsPerPage)
            };
        }

        public async Task<BookModel> GetForEditAsync(Guid id)
        {
            var book = await bookRepository.GetAsync(id);
            var model = mapper.Map<BookModel>(book);
            model.Category = await bookRepository.GetBookCategoryAsync(id);
            if(model.Category == BookCategories.Classic)
            {
                var classicBook = await classicBookRepository.GetAsync(id);
                model.VolCount = classicBook.VolCount;
            }
            else if(model.Category == BookCategories.Story)
            {
                var storyBook = await storyBookRepository.GetWithStoriesAsync(id);
                model.Stories = mapper.Map<IList<StoryModel>>(storyBook.Stories);
            }
            return model;
        }

        public async Task<DataServiceResult> RemoveAsync(Guid bookId)
        {
            try
            {
                var book = await bookRepository.GetAsync(bookId);
                if(book == null)
                {
                    logger.LogWarning($"Не найдена книга при удалении: {bookId}");
                    return DataServiceResult.Failed("Книга не найдена");
                }
                await bookRepository.RemoveAsync(book);
                return Success;
            }
            catch(Exception e)
            {
                return CommonError("Ошибка при удалении книги", e);
            }
        }

        public async Task<DataServiceResult> RemoveClientAsync(Guid bookId)
        {
            using(var trans = await bookRepository.BeginTransactionAsync())
            {
                try
                {
                    var book = await bookRepository.GetAsync(bookId);

                    await bookMoveService.AddAsync(new BookMoveModel
                    {
                        ClientId = book.ClientId,
                        BookId = book.Id,
                        Id = Guid.NewGuid(),
                        NewStatus = BookStatuses.InPlace
                    });

                    book.ClientId = null;
                    await bookRepository.UpdateAsync(book);

                    trans.Commit();
                    return Success;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    return CommonError("Ошибка при удалении клиента у книги", e);
                }
            }
        }

        public async Task<DataServiceResult> SetClientAsync(BookMoveModel model)
        {
            using(var trans = await bookRepository.BeginTransactionAsync())
            {
                try
                {
                    var book = await bookRepository.GetAsync(model.BookId);
                    if (book == null)
                    {
                        trans.Rollback();

                        logger.LogWarning($"Не найдена книга при установке клиента: {model.BookId}");
                        return DataServiceResult.Failed("Книга не найдена");
                    }
                    var client = await clientRepository.GetAsync((Guid)model.ClientId);
                    if (client == null)
                    {
                        trans.Rollback();

                        logger.LogWarning($"Не найден клиент при установке клиента: {(Guid)model.ClientId}");
                        return DataServiceResult.Failed("Клиент не найден");
                    }
                    book.ClientId = model.ClientId;
                    await bookRepository.UpdateAsync(book);

                    model.NewStatus = BookStatuses.Taked;
                    var bookMoveResult = await bookMoveService.AddAsync(model);
                    if (bookMoveResult.Succeeded)
                    {
                        trans.Commit();
                        return DataServiceResult.Success(client);
                    }
                    else
                    {
                        trans.Rollback();
                        return bookMoveResult;
                    }
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    return CommonError("Ошибка при установке клиента", e);
                }
            }
        }

        public async Task<DataServiceResult> UpdateAsync(BookModel model)
        {
            try
            {
                var book = await bookRepository.GetAsync(model.Id);
                if (book == null)
                {
                    logger.LogWarning($"Не найдена книга при обновлении: {model.Id}");
                    return DataServiceResult.Failed("Книга не найдена");
                }
                mapper.Map(model, book);
                await bookRepository.UpdateAsync(book);
                if(model.Category == BookCategories.Classic && model.VolCount != null)
                {
                    var classicBook = await classicBookRepository.GetAsync(model.Id);
                    classicBook.VolCount = (int)model.VolCount;
                    await classicBookRepository.UpdateAsync(classicBook);
                }
                else if(model.Category == BookCategories.Story)
                {
                    if (model.Stories == null)
                        model.Stories = new List<StoryModel>();
                    var storyBook = await storyBookRepository.GetAsync(model.Id);
                    storyBook.Stories = mapper.Map<IEnumerable<Story>>(model.Stories).Select(st =>
                    {
                        st.StoryBookId = (Guid)model.Id;
                        return st;
                    }).ToList();

                    await storyBookRepository.UpdateAsync(storyBook);
                }
                return Success;
            }
            catch (Exception e)
            {
                return CommonError("Ошибка при добавлении книги", e);
            }
        }
    }
}
