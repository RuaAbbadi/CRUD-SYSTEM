using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models
{
    public class CreateAuthorViewModel
    {
        [Key]
        [Display(Name = "Id Number")]
        public int Id { get; set; }

        [Required]
        [MaxLength(400)]
        [Display(Name = "The Author Name", Prompt = "Please Enter Author Name")]
        [Remote(action: "VerifyAuthor", controller: "Author")]
        public string Name { get; set; }
    }
}
