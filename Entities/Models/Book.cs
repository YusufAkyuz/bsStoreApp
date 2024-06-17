using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Book
{
    [Key]
    public int Id { get; set; }
        
    [Column(TypeName = "varchar(100)")]
    public string Title { get; set; }
        
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
}