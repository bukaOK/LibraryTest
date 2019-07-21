using LibraryTest.CodeData.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryTest.Models.Books
{
    public class BookMoveModel
    {
        public Guid? Id { get; set; }
        [Required]
        public Guid? ClientId { get; set; }
        [Required]
        public Guid? BookId { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        public BookStatuses? NewStatus { get; set; }
    }
}
