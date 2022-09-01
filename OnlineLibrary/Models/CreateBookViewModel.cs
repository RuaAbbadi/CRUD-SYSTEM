using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models
{
    public class CreateBookViewModel
    {
        [Key]
        [Display(Name = "Id Number")]
        public int Id { get; set; }
        [Required]
        [MaxLength(400)]
        [Display(Name = "The Book Name", Prompt = "Please Enter Book Name")]
        [Remote(action: "VerifyBook", controller: "Books")]

        public string Title { get; set; }

        [Required]
        [Display(Name = "The Book Image")]
        public IFormFile ImageFile { get; set; }

        [Required]
        [Display(Name = "The Book Publisher")]
        public string Publisher { get; set; }

        [Required]
        [MaxLength(2000)]
        [Display(Name = "The Book Description")]
        public string Description { get; set; }

        [Display(Name = "Category Name")]
        public int CategoryId { get; set; }

        public string? CurrentImageUrl { get; set; }
    }
}
