using System.ComponentModel.DataAnnotations;

namespace NTLBookStore.Areas.StoreOwner.ViewModels;

public class BookCreate
{
    [StringLength(255)]
    public string Title { get; set; } = null!;
    [StringLength(255)]
    public string Author { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public DateTime ReleaseDate { get; set; }
    public int Page { get; set; } 
    public int CategoryId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price must be bigger than zero.")]
    [DataType(DataType.Currency)]
    public double Price { get; set; }

    public string StoreId { get; set; } = null!;

    public IFormFile UploadImage { get; set; } = null!;
}