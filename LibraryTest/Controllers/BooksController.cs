using LibraryTest.BLL.Services.Interfaces;
using LibraryTest.CodeData.Enums;
using LibraryTest.Models.Books;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryTest.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }
        [HttpPost]
        public async Task<IActionResult> Add(BookModel model)
        {
            if (model.Category == BookCategories.Classic && model.VolCount == null)
                ModelState.AddModelError("VolCount", "Заполните количество глав");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await bookService.AddAsync(model);
            if (result.Succeeded)
            {
                return Ok();
            }
            return StatusCode(500, new
            {
                message = result.Errors.FirstOrDefault()
            });
        }
        public async Task<IActionResult> GetForEdit(Guid id)
        {
            var book = await bookService.GetForEditAsync(id);
            if (book == null)
                return BadRequest();
            return Ok(book);
        }
        public async Task<IActionResult> GetList(BookFilterModel model)
        {
            var result = await bookService.GetCatalogAsync(model);
            return Ok(result);
        }

        public async Task<IActionResult> GetByClient(Guid id)
        {
            var result = await bookService.GetByClientAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BookModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await bookService.UpdateAsync(model);
            if (result.Succeeded)
            {
                return Ok();
            }
            return StatusCode(500, new
            {
                message = result.Errors.FirstOrDefault()
            });
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await bookService.RemoveAsync(id);
            if (result.Succeeded)
                return Ok();
            return StatusCode(500);
        }
        public async Task<IActionResult> SetClient(BookMoveModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await bookService.SetClientAsync(model);
            if (result.Succeeded)
            {
                return Ok(new { client = result.ResultData });
            }
            return StatusCode(500, new { message = result.Errors.FirstOrDefault() });
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveBookClient(Guid id)
        {
            var result = await bookService.RemoveClientAsync(id);
            if (result.Succeeded)
            {
                return Ok();
            }
            return StatusCode(500, new { message = result.Errors.FirstOrDefault() });
        }
    }
}
