using LibraryTest.CodeData.Enums;
using LibraryTest.DAL.Core;
using LibraryTest.DAL.Entities;
using LibraryTest.DAL.Infrastructure;
using LibraryTest.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.DAL.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<Client> GetByPhoneAsync(string phone)
        {
            return await Table.FirstOrDefaultAsync(x => x.Phone == phone);
        }
        public async Task<(int clientsCount, IList<Client> clients)> FilterAsync(string name, ClientTypes clientType, int startIndex, int limit)
        {
            var query = Table.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(name))
                // Без GIN
                query = query.Where(x => EF.Functions.ToTsVector("russian", x.Name).Matches(EF.Functions.ToTsQuery("russian", name)));

            if (clientType == ClientTypes.Taked)
            {
                query = DbContext.BookMoves.AsNoTracking().Where(x => x.EndDate < DateTime.UtcNow)
                    .Join(DbContext.Books.AsNoTracking(), x => x.BookId, y => y.Id, (x, y) => y)
                    .Join(query, x => x.ClientId, y => y.Id, (x, y) => y);
            }
            else if(clientType == ClientTypes.Expired)
            {
                query = DbContext.BookMoves.AsNoTracking().Where(x => x.EndDate >= DateTime.UtcNow)
                    .Join(DbContext.Books.AsNoTracking(), x => x.BookId, y => y.Id, (x, y) => y)
                    .Join(query, x => x.ClientId, y => y.Id, (x, y) => y);
            }

            var clientsCount = await query.CountAsync();
            var clients = await query.Skip(startIndex).Take(limit).ToListAsync();
            return (clientsCount, clients);
        }
        public async Task<bool> IsExistAsync(Guid id)
        {
            return await Table.AnyAsync(x => x.Id == id);
        }
    }
}
