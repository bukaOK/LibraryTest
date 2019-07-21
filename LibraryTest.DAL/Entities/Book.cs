using LibraryTest.CodeData.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LibraryTest.DAL.Entities
{
    public class Book : GuidEntity
    {
        [Required]
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public int PagesCount { get; set; }
        [ForeignKey("Genre")]
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
        [ForeignKey("Client")]
        public Guid? ClientId { get; set; }
        public Client Client { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}