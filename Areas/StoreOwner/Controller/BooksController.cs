using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NTLBookStore.Areas.StoreOwner.ViewModels;
using NTLBookStore.Data;
using NTLBookStore.Helpers;
using NTLBookStore.Models;

namespace NTLBookStore.Areas.StoreOwner.Controller;

    [Area("StoreOwner")]
    [Authorize(Roles = Roles.StoreOwner)]
    public class BooksController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly NTLBookStoreContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        private string? _storeId;

        private string StoreId
        {
            get
            {
                _storeId = _storeId ?? userManager.GetUserId(User);
                return _storeId;
            }
        }

        public BooksController(NTLBookStoreContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Books
                .Include(b => b.Category)
                .Where(b => b.StoreId == StoreId);

            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Image)
                .FirstOrDefaultAsync(b => b.Id == id && b.StoreId == StoreId);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            var model = new BookCreate()
            {
                StoreId = StoreId,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreate model)
        {
            if (ModelState.IsValid)
            {
                var extension = Path.GetExtension(model.UploadImage.FileName);
                var book = new Book()
                {
                    Title = model.Title,
                    Author = model.Author,
                    ReleaseDate = model.ReleaseDate,
                    Page = model.Page,
                    Description = model.Description,
                    Price = model.Price,
                    StoreId = StoreId, // Set the StoreId property to the current user's Id
                    CategoryId = model.CategoryId,
                    Image = new(model.UploadImage.FileName, extension),
                };
                _context.Add(book);

                //var randomFileName = $"{Path.GetRandomFileName()}.{extension}";
                //var imagePath = Path.Combine(FileUploadHelper.BookImageDirectory, randomFileName);
                //var imageHref = Path.Combine(FileUploadHelper.BookImageHref, randomFileName);
                using (var stream = new FileStream(book.Image.Path, FileMode.CreateNew))
                {
                    await model.UploadImage.CopyToAsync(stream);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", model.CategoryId);
            return View(model);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                _context.Update(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }