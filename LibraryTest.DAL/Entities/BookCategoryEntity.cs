using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LibraryTest.DAL.Entities
{
    public abstract class BookCategoryEntity
    {
        [Key, ForeignKey("Book")]
        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}
