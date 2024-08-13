namespace SaysanPwa.Domain.CommonExceptions;

public abstract class UnProcessableEntityException : Exception
{
    public UnProcessableEntityException(string errorMessage) : base(errorMessage)
    {
        
    }
}
