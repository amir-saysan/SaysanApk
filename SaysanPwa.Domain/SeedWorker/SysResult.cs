namespace SaysanPwa.Domain.SeedWorker;

public class SysResult<TResult>
{
    private List<string> _errorMessages = null!;

    public TResult Result { get; set; } = default(TResult)!;

    public bool Succeeded { get; set; }

    public IReadOnlyCollection<string> ErrorMessages => _errorMessages?.AsReadOnly()!;





    public SysResult()
    {
        
    }

    public SysResult(TResult result) : this(result, true, null!)
    {
        
    }

    public SysResult(TResult result, bool succeeded) : this(result, succeeded, null!)
    {

    }

    public SysResult(TResult result, List<string> errorMessages) : this(result, false, errorMessages)
    {

    }

    public SysResult(TResult result, bool succeeded, List<string> errorMessages = null!)
    {
        this.Result = result;
        this.Succeeded = succeeded;
        this._errorMessages = errorMessages;
    }
}
