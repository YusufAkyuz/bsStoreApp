using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public abstract record BoookDtoForManipulation
{
    [Required(ErrorMessage = "Title is a required field!")]
    [MinLength(2, ErrorMessage = "Title must consist at least 2 character")]
    [MaxLength(50, ErrorMessage = "Title must consist at max 50 character")]
    public string Title { get; init; }
    
    [Required(ErrorMessage = "Price is a required field!")]
    [Range(10,1000)]
    public decimal Price { get; init; }
}