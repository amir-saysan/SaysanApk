namespace SaysanPwa.Api.ExceptionHandling;

public class ApplicationException
{
    private List<string> _errorMessages = null!;
    public int StatusCode { get; set; }

    public IReadOnlyCollection<string> ErrorMessages => _errorMessages.AsReadOnly();



    public ApplicationException(int statusCode, List<string> errorMessages = null)
    {
        this.StatusCode = statusCode;
        this._errorMessages = errorMessages;
    }

    public void AddErrorMessage(string message)
    {
        if (_errorMessages == null)
        {
            _errorMessages = new();
        }
        _errorMessages.Add(message);
    }
}
