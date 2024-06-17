using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EFCore.Config;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasData(
            new Book()
            {
                Id = 1, Title = "CS1", Price = 75
            },
            new Book()
            {
                Id = 2, Title = "CS2", Price = 150
            },
            new Book()
            {
                Id = 3, Title = "CS3", Price = 100
            }
        );
    }
}