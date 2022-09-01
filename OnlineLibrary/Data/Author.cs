using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Data
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<AuthorToBook> AuthorToBook { get; set; }


    }
}
