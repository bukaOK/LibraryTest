using LibraryTest.BLL.Services.Interfaces;
using LibraryTest.Models.Genres;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryTest.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreService genreService;

        public GenresController(IGenreService genreService)
        {
            this.genreService = genreService;
        }
        public async Task<IActionResult> GetAll()
        {
            var result = await genreService.GetAllAsync();
            return Ok(result);
        }
        public async Task<IActionResult> Get(Guid id)
        {
            var genre = await genreService.GetAsync(id);
            if (genre == null)
                return BadRequest();
            return Ok(genre);
        }
        [HttpPost]
        public async Task<IActionResult> Add(GenreModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.Id = null;
            var result = await genreService.AddAsync(model);
            if (result.Succeeded)
                return Ok(new
                {
                    id = result.ResultData
                });
            return StatusCode(500, new
            {
                message = result.Errors.FirstOrDefault()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(GenreModel model)
        {
            if(model.Id == null)
            {
                ModelState.AddModelError("Id", "Заполните id");
                return BadRequest(ModelState);
            }
            var result = await genreService.UpdateAsync(model);
            if (result.Succeeded)
                return Ok();
            return StatusCode(500, new
            {
                message = result.Errors.FirstOrDefault()
            });
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(Guid id)
        {
            var result = await genreService.RemoveAsync(id);
            if (result.Succeeded)
                return Ok();
            return StatusCode(500, new
            {
                message = result.Errors.FirstOrDefault()
            });
        }
    }
}
