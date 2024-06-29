using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public record BookDtoForUpdate : BoookDtoForManipulation
{ 
    [Required]
    public int Id { get; set; }
    
}