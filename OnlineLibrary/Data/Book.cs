using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineLibrary.Data
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(400)]
        public string Title { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public virtual ICollection<AuthorToBook> AuthorToBook { get; set; }

    }
}
