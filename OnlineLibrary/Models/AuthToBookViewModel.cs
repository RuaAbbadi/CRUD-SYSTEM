using OnlineLibrary.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineLibrary.Models
{
    public class AuthToBookViewModel
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int BookId { get; set; }
       
        
        public Author Author { get; set; }
        public Book Book { get; set; }
    }
}
