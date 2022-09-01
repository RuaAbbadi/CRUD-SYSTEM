using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
