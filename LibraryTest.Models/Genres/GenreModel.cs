using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryTest.Models.Genres
{
    public class GenreModel
    {
        public Guid? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
