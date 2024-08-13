namespace SaysanPwa.Domain.CommonExceptions;

public abstract class AccessDeniedException : Exception
{
    public AccessDeniedException(string errorMessage) : base(errorMessage)
    {
        
    }
}
