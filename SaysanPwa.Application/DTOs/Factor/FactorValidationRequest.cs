namespace SaysanPwa.Application.DTOs.Factor;

public record FactorValidationRequest
{
    public long PartnerId { get; set; }
    public long BranchId { get; set; }
    public bool AgentValidation { get; set; } = false;
    public IEnumerable<FactorValidationProduct> Products { get; set; } = null!;
}

public record FactorValidationProduct
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = null!;
}