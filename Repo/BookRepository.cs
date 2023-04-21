using NTLBookStore.Controllers;
using NTLBookStore.Data;
using NTLBookStore.Models;

namespace NTLBookStore.Repo;

public class BookRepository :IBookRepository
{
    private readonly NTLBookStoreContext _context;

    public BookRepository(NTLBookStoreContext context)
    {
        _context = context;
    }

    public List<Book> GetAllBooks()
    {
        return _context.Books.ToList();
    }

    public List<string> GetCategories()
    {
        return _context.Categories.Select(c => c.Name).ToList();
    }
}

public interface IBookRepository
{
}