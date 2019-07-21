using LibraryTest.Models.Genres;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryTest.Tests.Services.GenreServiceTests
{
    public class AddTest : GenreServiceTest
    {
        [Fact]
        public async Task Add()
        {
            var model = new GenreModel
            {
                Id = null,
                Name = "Mine"
            };
            var result = await genreService.AddAsync(model);

            Assert.True(result.Succeeded);
            Assert.IsType<Guid>(result.ResultData);
        }
    }
}
