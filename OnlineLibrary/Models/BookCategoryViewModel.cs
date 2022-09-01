using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models
{
    public class BookCategoryViewModel
    {
        [Key]
        [Display(Name = "Id Number")]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Category Name", Prompt = "Please Enter Category Name")]
        [Remote(action: "VerifyCategory", controller: "Category")]
        public string Name { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Category Description", Prompt = "Please Enter Category Description")]
        public string Description { get; set; }
    }
}
