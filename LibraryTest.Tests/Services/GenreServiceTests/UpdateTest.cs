using LibraryTest.Models.Genres;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryTest.Tests.Services.GenreServiceTests
{
    public class UpdateTest : GenreServiceTest
    {
        [Fact]
        public async Task Update_IdNull()
        {
            var model = new GenreModel
            {
                Id = null,
                Name = "Mine"
            };
            var result = await genreService.UpdateAsync(model);

            Assert.False(result.Succeeded);
        }
        [Fact]
        public async Task Update_IdNotNull_NonExistingId()
        {
            var model = new GenreModel
            {
                Id = NonExistingId,
                Name = "Mine"
            };
            var result = await genreService.UpdateAsync(model);

            Assert.False(result.Succeeded);
        }
        [Fact]
        public async Task Update_IdNotNull_ExistingId()
        {
            var model = new GenreModel
            {
                Id = ExistingId,
                Name = "Mine"
            };
            var result = await genreService.UpdateAsync(model);

            Assert.True(result.Succeeded);
        }
    }
}
