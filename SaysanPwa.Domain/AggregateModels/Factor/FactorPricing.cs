namespace SaysanPwa.Domain.AggregateModels.Factor;

public class FactorPricing
{
    public int Type_Price { get; set; } //0,1,2
    public decimal Price { get; set; }
    public decimal Fi_Ba_Haz { get; set; }
    public decimal Mnd_Fi { get; set; }
    public decimal Mnd_Megdar { get; set; }
}