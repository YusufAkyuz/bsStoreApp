namespace Entities.Expections;

//sealed keywordu sayesinde bu classın extend edilmeyeceğini bildirmiş oluruz

public sealed class BookNotFoundException : NotFoundException
{
    public BookNotFoundException(int id) : base($"The book with id : {id} not found!")
    {
         
    }
} 