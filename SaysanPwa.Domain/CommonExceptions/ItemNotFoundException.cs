namespace SaysanPwa.Domain.CommonExceptions;

public abstract class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string errorMessage) : base(errorMessage)
    {
        
    }
}
