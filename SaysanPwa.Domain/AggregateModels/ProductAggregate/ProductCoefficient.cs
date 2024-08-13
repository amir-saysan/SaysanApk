using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.ProductAggregate;

public class ProductCoefficient : ValueObject
{
    public decimal Coefficients { get; set; }
    public string CoefficientsDescription { get; set; } = string.Empty;
    public string MainUnit { get; set; } = string.Empty;
    public string MinorUnit { get; set; } = string.Empty;
}
