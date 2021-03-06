﻿using LibraryTest.DAL.Core;
using LibraryTest.DAL.Entities;
using LibraryTest.DAL.Infrastructure;
using LibraryTest.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTest.DAL.Repositories
{
    public class BookMoveRepository : Repository<BookMove>, IBookMoveRepository
    {
        public BookMoveRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
