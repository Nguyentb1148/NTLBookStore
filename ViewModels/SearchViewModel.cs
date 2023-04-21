using Microsoft.AspNetCore.Mvc.Rendering;
using NTLBookStore.Models;

namespace NTLBookStore.ViewModels;

public class SearchViewModel
{
    public string? Type { get; set; }
    public string Action { get; private set; } = "";

    public string KeyWord { get; set; } = null!;

    public List<Book>? Books { get; set; }
    public SelectList? CategoriesSelectList { get; set; }
    public string? BookCategory { get; set; }

    public static SearchViewModel BookSearchViewModel { get; } = new()
    {
        Action = "/Books"
    };
    public static SearchViewModel CategorySearchViewModel { get; } = new()
    {
        Action = "/Books"
    };
    public static SearchViewModel ProfileSearchViewModel { get; } = new()
    {
        Action = "/Profile"
    };

    public static SearchViewModel OrderSearchViewModel { get; } = new()
    {
        Action = "/StoreOwner/Orders"
    };
}