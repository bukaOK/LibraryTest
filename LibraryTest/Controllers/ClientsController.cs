using LibraryTest.BLL.Services.Interfaces;
using LibraryTest.CodeData.Enums;
using LibraryTest.Models.Clients;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryTest.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientService clientService;

        public ClientsController(IClientService clientService)
        {
            this.clientService = clientService;
        }
        [HttpPost]
        public async Task<IActionResult> Add(ClientModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await clientService.AddAsync(model);
            if (result.Succeeded)
            {
                return Ok();
            }
            return StatusCode(500);
        }
        public async Task<IActionResult> Get(Guid id)
        {
            var book = await clientService.GetAsync(id);
            if (book == null)
                return BadRequest();
            return Ok(book);
        }
        public async Task<IActionResult> GetByPhone(string phone)
        {
            var client = await clientService.GetByPhoneAsync(phone);
            return Ok(client);
        }
        public async Task<IActionResult> GetList(int page, string name, ClientTypes clientType)
        {
            var clientsModel = await clientService.GetClientsAsync(page, name, clientType);
            return Ok(clientsModel);
        }
        /// <summary>
        /// Поиск для выбора клиента для книги
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<IActionResult> Search(string name)
        {
            var clients = await clientService.GetClientsAsync(1, name, ClientTypes.All);
            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ClientModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await clientService.UpdateAsync(model);
            if (result.Succeeded)
            {
                return Ok();
            }
            return StatusCode(500, new { message = result.Errors.FirstOrDefault() });
        }
        [HttpDelete]
        public async Task<IActionResult> Remove(Guid id)
        {
            var result = await clientService.RemoveAsync(id);
            if (result.Succeeded)
                return Ok();
            return StatusCode(500, new { message = result.Errors.FirstOrDefault() });
        }
    }
}
