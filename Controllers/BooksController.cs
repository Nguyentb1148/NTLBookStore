using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NTLBookStore.Data;
using NTLBookStore.ViewModels;

namespace NTLBookStore.Controllers;

public class BooksController : Controller
{
    private readonly NTLBookStoreContext _context;

    public BooksController(NTLBookStoreContext context)
    {
        _context = context;
    }

    // GET: Books
    public async Task<IActionResult> Index(SearchViewModel? model)
    {
        var query = _context.Books
            .Include(b => b.Category)
            .Include(b => b.Image)
            .AsQueryable();

        if (model != null && !string.IsNullOrWhiteSpace(model.KeyWord))
        {
            var keyword = model.KeyWord.Trim().ToLower();
            query = query.Where(b => b.Title.ToLower().Contains(keyword)
                                     || b.Category.Name.ToLower().Contains(keyword));
        }

        return View(await query.ToListAsync());
    }

    // GET: Books/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Books == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .Include(b => b.Category)
            .Include(b => b.Store)
            .Include(b => b.Image)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }
    // GET: Books/SearchByBook
    public IActionResult SearchByBook(string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return RedirectToAction(nameof(Index));
        }

        var books = _context.Books
            .Include(b => b.Category)
            .Include(b => b.Image)
            .Where(b => b.Title.ToLower().Contains(keyword.ToLower()))
            .ToList();

        var model = new SearchViewModel
        {
            KeyWord = keyword,
            Books = books
        };

        return View("SearchResult", model);
    }

// GET: Books/SearchByCategory
    public IActionResult SearchByCategory(string category)
    {
        if (string.IsNullOrEmpty(category))
        {
            return RedirectToAction(nameof(Index));
        }

        var books = _context.Books
            .Include(b => b.Category)
            .Include(b => b.Image)
            .Where(b => b.Category.Name.ToLower() == category.ToLower())
            .ToList();

        var model = new SearchViewModel
        {
            BookCategory = category,
            Books = books
        };

        return View("SearchResult", model);
    }

    public IActionResult Help()
    {
        return View();
    }
}
