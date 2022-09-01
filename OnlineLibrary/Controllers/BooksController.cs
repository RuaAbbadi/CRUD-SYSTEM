using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using System.Drawing;

namespace OnlineLibrary.Controllers
{
    public class BooksController :Controller
    {
        private readonly LibraryDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

     
        public BooksController(LibraryDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            var books = await _db.Books.Select(pc => new BookItemViewModel
            {
                Id = pc.Id,
                Title = pc.Title,
                Image = pc.Image,
                Publisher = pc.Publisher,
                Description = pc.Description,
                CategoryName = pc.Category.Name
            }).ToListAsync();
            return View(books);
        }

        public async Task<IActionResult> Create()
        {
            await FillLookups();
            return View();
        }


        private async Task FillLookups()
        {
            var BookList = await _db.Categories.ToListAsync();
            var categorySelect = new SelectList(BookList, "Id", "Name");
            ViewBag.CategorySelect = categorySelect;
        }

        private Boolean IsVaildExtension(string extension)
        {
            // 1) vaild extensions 
            var vaildExtensions = new List<string>() { ".jpg", ".png", ".gif" };
            if (vaildExtensions.Contains(extension.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookViewModel input)
        {
            if (ModelState.IsValid)
            {
                var extension = Path.GetExtension(input.ImageFile.FileName);
                if (IsVaildExtension(extension))
                {

                    string newFileName = SaveBookImage(input.ImageFile);
                    var book = new Book()
                    {
                        CategoryId = input.CategoryId,
                        Title = input.Title,
                        Publisher = input.Publisher,
                        Description = input.Description,
                        Image = newFileName,

                    };
                    _db.Books.Add(book);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Book Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("ImageFile", "Image file has not vaild extension!!!");

                }
            }
            await FillLookups();
            return View(input);
        }

        private string SaveBookImage(IFormFile file)
        {

            var wwwrootPath = _webHostEnvironment.WebRootPath;
           

            var imagePath = Path.Combine(wwwrootPath, "BooksImages");
          

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);

            }
            var extension = Path.GetExtension(file.FileName);

            string newFileName = $"{Guid.NewGuid()}{extension}";
         

            string newFilePath = Path.Combine(imagePath, newFileName);
          

            using (FileStream stream = new FileStream(newFilePath
                , FileMode.Create))
            {

                file.CopyToAsync(stream);

                stream.Close();
            }


            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyToAsync(ms);
                SaveThumpBookImage(140, 100, newFileName, ms);
            }
            return newFileName; 

        }
        private void SaveThumpBookImage(int width, int height, string fileName, Stream resourceImage)
        {

            var wwwrootPath = _webHostEnvironment.WebRootPath; 

            var imagePath = Path.Combine(wwwrootPath, "thumpBookImages");
        

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);

            }

            string newFileName = fileName;
         

            string newFilePath = Path.Combine(imagePath, newFileName);
          


            var image = Image.FromStream(resourceImage);
            var thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);


            thumb.Save(newFilePath);
        }


        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyBook(string Title)
        {
            if (!_db.Books.Where(p => p.Title == Title).Any())
            {
                return Json($"Book {Title} is already in use.");
            }
            return Json(true);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var book = await _db.Books.FirstOrDefaultAsync(pc => pc.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            var vm = new CreateBookViewModel()
            {
                Title=book.Title,
                Description=book.Description,
                Publisher=book.Publisher,
                CategoryId = book.CategoryId, CurrentImageUrl=book.Image
            };
            await FillLookups();
            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateBookViewModel input)
        {

            var book = new Book()
                 {
                     CategoryId = input.CategoryId,
                     Title = input.Title,
                     Publisher = input.Publisher,
                     Description = input.Description,
                    Image=input.CurrentImageUrl, Id=id
            };

            if (ModelState.IsValid)
            {
                if (input.ImageFile != null)
                {
                    var wwwrootPath = _webHostEnvironment.WebRootPath;

                    var imagePath = Path.Combine(wwwrootPath, "BooksImages");

                    var extension = Path.GetExtension(input.ImageFile.FileName);

                    string newFileName = $"{Guid.NewGuid()}{extension}";

                    string newFilePath = Path.Combine(imagePath, newFileName);
                    using (FileStream stream = new FileStream(newFilePath, FileMode.Create))
                    {

                       await input.ImageFile.CopyToAsync(stream);

                        stream.Close();
                    }

                    book.Image = newFileName;

                }

                _db.Books.Update(book);
                await _db.SaveChangesAsync();
                TempData["Success"] = "The Book Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                await FillLookups();
                return View(input);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _db.Books.FirstOrDefaultAsync(pc => pc.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            var vm = new CreateBookViewModel() {
                Id = book.Id, 
                Title = book.Title,
                Description=book.Description,
                CurrentImageUrl=book.Image
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CreateBookViewModel input)
        {
            var book = new Book() { Id = input.Id, Title = input.Title ,Description=input.Description,Image=input.CurrentImageUrl };
            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            TempData["Success"] = "The Book Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DataTable()
        {
            var books = await _db.Books.Select(pc => new BookItemViewModel() { 
                Id = pc.Id, Title = pc.Title, Description = pc.Description,
                Image = pc.Image, Publisher=pc.Publisher,
                CategoryName=pc.Category.Name
            }).ToListAsync();
            return View(books);
        }


        public async Task<IActionResult> Details(int id )
        {
            if (id == null || _db.Books == null)
            {
                return NotFound();
            }
            var result = await _db.Books.Select(b=> new BookItemViewModel
            {
                Id=b.Id,
                Title = b.Title,
                Description = b.Description,    
                Image=b.Image,
                Publisher=b.Publisher,  
            }).FirstOrDefaultAsync(b=> b.Id == id);

            if (result == null)
            {
                return NotFound();
            }
            return View(result);
           
        }





    }
}

