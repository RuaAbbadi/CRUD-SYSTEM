using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    public class AuthorController : Controller
    {
        private readonly LibraryDbContext _db;

        public AuthorController(LibraryDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var Auth = await _db.Authors.Select(pc => new CreateAuthorViewModel() {Id = pc.Id,Name = pc.Name }).ToListAsync();
            return View(Auth);
        }

        public async Task<IActionResult> Create()
        {
          
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateAuthorViewModel input)
        {
            if (ModelState.IsValid)
            {
                var Auth = new Author() { Name = input.Name };
                _db.Authors.Add(Auth);
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
        public IActionResult VerifyAuthor(string name)
        {
            if (_db.Authors.Where(p => p.Name == name).Any())
            {
                return Json($"Author's {name} is already in use.");
            }
            return Json(true);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Auth = await _db.Authors.FirstOrDefaultAsync(pc => pc.Id == id);
            if (Auth == null)
            {
                return NotFound();
            }
            var vm = new CreateAuthorViewModel() { Id = Auth.Id, Name = Auth.Name };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateAuthorViewModel input)
        {
            if (ModelState.IsValid)
            {
                var Auth = new Author() { Id=input.Id,Name = input.Name };
                _db.Authors.Update(Auth);
                await _db.SaveChangesAsync();
                TempData["Success"] = "The Author Name Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(input);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var Author = await _db.Authors.FirstOrDefaultAsync(pc => pc.Id == id);
            if (Author == null)
            {
                return NotFound();
            }
            var vm = new CreateAuthorViewModel() { Id = Author.Id, Name = Author.Name };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CreateAuthorViewModel input)
        {
            var Auth = new Author() { Id = input.Id, Name = input.Name };
            _db.Authors.Remove(Auth);
            await _db.SaveChangesAsync();
            TempData["Success"] = "The Author Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }


    }
}
