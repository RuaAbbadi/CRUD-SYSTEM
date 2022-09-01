using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    public class CategoryController : Controller
    {
        private readonly LibraryDbContext _db;
        public CategoryController(LibraryDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var categories = await _db.Categories.Select(pc => new BookCategoryViewModel() { Id = pc.Id, Name = pc.Name, Description = pc.Description }).ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookCategoryViewModel input)
        {
            if (ModelState.IsValid)
            {
                var category = new Category() { Name = input.Name, Description = input.Description };
                _db.Categories.Add(category);
                await _db.SaveChangesAsync();
                TempData["Success"] = "The Category Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(input);
            }
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyCategory(string name)
        {
            if (_db.Categories.Where(p => p.Name == name).Any())
            {
                return Json($"Category {name} is already in use.");
            }
            return Json(true);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var productCategory = await _db.Categories.FirstOrDefaultAsync(pc => pc.Id == id);
            if (productCategory == null)
            {
                return NotFound();
            }
            var vm = new BookCategoryViewModel() { Id = productCategory.Id, Name = productCategory.Name, Description = productCategory.Description };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookCategoryViewModel input)
        {
            if (ModelState.IsValid)
            {
                var category = new Category() { Id = input.Id, Name = input.Name, Description = input.Description };
                _db.Categories.Update(category);
                await _db.SaveChangesAsync();
                TempData["Success"] = "The Category Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(input);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var productCategory = await _db.Categories.FirstOrDefaultAsync(pc => pc.Id == id);
            if (productCategory == null)
            {
                return NotFound();
            }
            var vm = new BookCategoryViewModel() { Id = productCategory.Id, Name = productCategory.Name, Description = productCategory.Description };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BookCategoryViewModel input)
        {
            var category = new Category() { Id = input.Id, Name = input.Name, Description = input.Description };
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            TempData["Success"] = "The Category Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> TestAjaxLoad()
        {

            var cats = await _db.Books.Select(c => new { c.Id, c.Title }).ToListAsync();
            var selectValues = new SelectList(cats, "Id", "Title");
            ViewBag.BooksList = selectValues;  
            return View();

        }


        public async Task<IActionResult> LoadBooksInfoPartial(int id)
        {

            var books = await _db.Books.Include(x => x.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (books != null)
            {
                return PartialView("_BookPartialView", books);
            }
            else
            {
                return PartialView("_ItemNotFound");
            }

        }


     
    }



}

