using LibraryTest.CodeData.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryTest.Models.Books
{
    public class BookModel
    {
        public Guid? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        public string ISBN { get; set; }
        [Required]
        public int? Year { get; set; }
        [Required]
        public int? PagesCount { get; set; }
        [Required]
        public Guid? GenreId { get; set; }
        [Required]
        public BookCategories? Category { get; set; }
        /// <summary>
        /// Если категория - классика
        /// </summary>
        public int? VolCount { get; set; }
        /// <summary>
        /// Если категория - сборник рассказов
        /// </summary>
        public IList<StoryModel> Stories { get; set; }
    }
}
