namespace Entities.Expections;

public abstract class NotFoundException : Exception
{
    //base sayesinde super classın contructorunu kullan demiş oluruz
    protected NotFoundException(string message) : base(message)
    {
        
    }
}
