using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    public class AuthToBookViewModelsController : Controller
    {
        private readonly LibraryDbContext _context;

        public AuthToBookViewModelsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: AuthToBookViewModels
        public async Task<IActionResult> Index()
        {
            var libraryDbContext = _context.AuthToBookViewModel.Include(a => a.Author).Include(a => a.Book);
            return View(await libraryDbContext.ToListAsync());
        }

        // GET: AuthToBookViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AuthToBookViewModel == null)
            {
                return NotFound();
            }

            var authToBookViewModel = await _context.AuthToBookViewModel
                .Include(a => a.Author)
                .Include(a => a.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authToBookViewModel == null)
            {
                return NotFound();
            }

            return View(authToBookViewModel);
        }

        // GET: AuthToBookViewModels/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name");
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            return View();
        }

        // POST: AuthToBookViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AuthorId,BookId")] AuthToBookViewModel authToBookViewModel)
        {
         
                _context.Add(authToBookViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", authToBookViewModel.AuthorId);
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", authToBookViewModel.BookId);
            return View(authToBookViewModel);
        }

        // GET: AuthToBookViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AuthToBookViewModel == null)
            {
                return NotFound();
            }

            var authToBookViewModel = await _context.AuthToBookViewModel.FindAsync(id);
            if (authToBookViewModel == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", authToBookViewModel.AuthorId);
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", authToBookViewModel.BookId);
            return View(authToBookViewModel);
        }

        // POST: AuthToBookViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AuthorId,BookId")] AuthToBookViewModel authToBookViewModel)
        {
            if (id != authToBookViewModel.Id)
            {
                return NotFound();
            }

            
                
               
                    _context.Update(authToBookViewModel);
                    await _context.SaveChangesAsync();
              
                
                return RedirectToAction(nameof(Index));
            
            return View(authToBookViewModel);
        }

        // GET: AuthToBookViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AuthToBookViewModel == null)
            {
                return NotFound();
            }

            var authToBookViewModel = await _context.AuthToBookViewModel
                .Include(a => a.Author)
                .Include(a => a.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authToBookViewModel == null)
            {
                return NotFound();
            }

            return View(authToBookViewModel);
        }

        // POST: AuthToBookViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AuthToBookViewModel == null)
            {
                return Problem("Entity set 'LibraryDbContext.AuthToBookViewModel'  is null.");
            }
            var authToBookViewModel = await _context.AuthToBookViewModel.FindAsync(id);
            if (authToBookViewModel != null)
            {
                _context.AuthToBookViewModel.Remove(authToBookViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthToBookViewModelExists(int id)
        {
          return (_context.AuthToBookViewModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        


    }
}
