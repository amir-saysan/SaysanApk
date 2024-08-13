using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.ProductAggregate;

public interface IProductRepository : IRepository<Product>
{
    public Task<List<Product>> GetProducts(int offset = 0, CancellationToken cancellationToken = default);
    public Task<int> GetAllProductsCount(CancellationToken cancellationToken = default);

    public Task<SysResult<Product>> GetProductById(long id, CancellationToken cancellationToken = default);
    public Task<SysResult<IReadOnlyCollection<ProductCoefficient>>> GetCoefficientsByProductId(long productId, CancellationToken cancellationToken = default);
    public Task<SysResult<IReadOnlyCollection<string>>> GetProductStores(string ids, CancellationToken cancellationToken = default);
    public Task<SysResult<IEnumerable<GetProductsForFactorViewModel>>> GetProductsForFactor(CancellationToken CancellationToken = default);
    public Task<SysResult<HasDiscountViewModel>> ProductHasDiscout(string discountType, long productId, string date, CancellationToken cancellationToken = default);
    public Task<SysResult<int>> GetProductCount(string typeCallProcedure,int fiscalYear, int productId, string? fromDate, string? toDate, CancellationToken cancellationToken = default);
    public Task<SysResult<IEnumerable<SmallOfGoods>>> GetAllSmallOfGoods(string procedureCallType, int fiscalYear, int productId, string? fromDate, string? toDate, long? offset, CancellationToken cancellationToken = default);


    public Task<SysResult<int>> ProfitWithGoodsCount(long fiscalYear, long productId, string? from, string? to);
    public Task<SysResult<IEnumerable<ProductProfit>>> GetAllProductProfits(long fiscalYear, long productId, string? from, string? to, long offset = 0);
    
}