namespace SaysanPwa.Application.DTOs.Factor;

public class FactorValidationResult
{
    public bool result { get; set; } = true;
    public List<string>? Fails { get; set; }

    public FactorValidationResult()
    {
        Fails = new List<string>();
    }
}

