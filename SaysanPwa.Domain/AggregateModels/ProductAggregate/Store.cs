namespace SaysanPwa.Domain.AggregateModels.ProductAggregate;

public class Store
{
    public string Name { get; set; } = string.Empty;

    public Store(string name) => Name = name;

    public Store()
    {
        
    }
}