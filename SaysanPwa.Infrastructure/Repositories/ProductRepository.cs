using Microsoft.Extensions.Configuration;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using SaysanPwa.Domain.SeedWorker;
using SqlServerWrapper.Wrapper.DatabaseManager;
using System.Data;
using System.Runtime.CompilerServices;

namespace SaysanPwa.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IConfiguration _configuration;
    private readonly IDbManager _dbManager;

    public ProductRepository(IConfiguration configuration, IDbManager dbManager)
    {
        _configuration = configuration;
        _dbManager = dbManager;
    }

    public async Task<SysResult<IEnumerable<ProductProfit>>> GetAllProductProfits(long fiscalYear, long productId, string? from, string? to, long offset = 0)
    {
        try
        {
            IEnumerable<ProductProfit> productProfits = await _dbManager.CallProcedureWithParametersAsync<ProductProfit>("Apk_Proc_Report_Kala_Profit_Prize", new
            {
                Type_Call_Proc = "Sod_Ba_Kala",
                ID_tbl_SalMaly = fiscalYear,
                ID_tbl_Kala = productId,
                Date1 = from,
                Date2 = to,
                Offset = offset
            });

            return new(productProfits, true);

        }
        catch (Exception ex)
        {
            return new(Enumerable.Empty<ProductProfit>(), false, new() { ex.Message });
        }
    }

    public async Task<int> GetAllProductsCount(CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _dbManager.CallScalarProcedureWithParametersAsync<int>("Apk_Proc_GetAllProducts", new
            {
                Type_call_procedure = "get_all_products_Count_Record"
            });

            return result;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    public async Task<SysResult<IEnumerable<SmallOfGoods>>> GetAllSmallOfGoods(string procedureCallType, int fiscalYear, int productId, string? fromDate, string? toDate, long? offset, CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<SmallOfGoods> smallOfGoods = await _dbManager.CallProcedureWithParametersAsync<SmallOfGoods>("Apk_Proc_Report_Kala_Kharid_Forosh",
                new
                {
                    Type_Call_Proc = procedureCallType,
                    ID_tbl_SalMaly = fiscalYear,
                    ID_tbl_Kala = productId,
                    Date1 = fromDate,
                    Date2 = toDate,
                    Offset = offset
                }, cancellationToken);

            return new(smallOfGoods);
        }
        catch (Exception ex)
        {
            return new(Enumerable.Empty<SmallOfGoods>(), false, new() { ex.Message });
        }
    }

    public async Task<SysResult<IReadOnlyCollection<ProductCoefficient>>> GetCoefficientsByProductId(long productId, CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<ProductCoefficient> productCoefficients = await _dbManager.CallProcedureWithParametersAsync<ProductCoefficient>("Apk_Proc_Get_Coefficients_By_ProductId", new
            {
                ProductId = productId
            }, cancellationToken);

            return new(productCoefficients.ToList().AsReadOnly());

        }
        catch (Exception ex)
        {
            return new(null!, false, new() { ex.Message });
        }
    }

    public async Task<SysResult<Product>> GetProductById(long id, CancellationToken cancellationToken = default)
    {
        return new(await _dbManager.CallSingleRowProcedureWithParametersAsync<Product>("Apk_Proc_GetProductById", new { ProductId = id }, cancellationToken));
    }

    public async Task<SysResult<int>> GetProductCount(string typeCallProcedure, int fiscalYear, int productId, string? fromDate, string? toDate, CancellationToken cancellationToken = default)
    {
        try
        {
            return new(await _dbManager.CallScalarProcedureWithParametersAsync<int>("Apk_Proc_Report_Kala_Kharid_Forosh", new
            {
                Type_Call_Proc = typeCallProcedure,
                ID_tbl_SalMaly = fiscalYear,
                ID_tbl_Kala = productId,
                Date1 = fromDate,
                Date2 = toDate
            }, cancellationToken));
        }
        catch (Exception ex)
        {
            return new(0, false, new() { ex.Message });
        }
    }

    public async Task<List<Product>> GetProducts(int offset = 0, CancellationToken cancellationToken = default)
    {
        IEnumerable<Product> products = await _dbManager.CallProcedureWithParametersAsync<Product>("Apk_Proc_GetAllProducts", new
        {
            Type_call_procedure = "get_all_products",
            Offset = offset
        }, cancellationToken);
        return new(products);
    }

    public async Task<SysResult<IEnumerable<GetProductsForFactorViewModel>>> GetProductsForFactor(CancellationToken cancellationToken = default)
    {
        IEnumerable<GetProductsForFactorViewModel> products = await _dbManager.CallProcedureWithParametersAsync<GetProductsForFactorViewModel>("Apk_Proc_GetAllProducts",
            new
            {
                Type_call_procedure = "get_all_products"
            },
            cancellationToken);
        return new(products);
    }

    public async Task<SysResult<IReadOnlyCollection<string>>> GetProductStores(string ids, CancellationToken cancellationToken = default)
    {
        try
        {
            List<Store> stores = new();
            foreach (var id in ids.Split("-"))
            {
                Store store = await _dbManager.CallSingleRowProcedureWithParametersAsync<Store>("Apk_Proc_GetStoreById", new
                {
                    ID_tbl_Anbar = id
                });

                stores.Add(store);
            }

            return new(stores.Select(s => s.Name).ToList().AsReadOnly());
        }
        catch (Exception ex)
        {
            return new(null!, false, new() { ex.Message });
        }
    }

    public async Task<SysResult<HasDiscountViewModel>> ProductHasDiscout(string discountType, long productId, string date, CancellationToken cancellationToken = default)
    {
        return new(await _dbManager.CallSingleRowProcedureWithParametersAsync<HasDiscountViewModel>("Apk_Proc_Discount_Product", new
        {
            TypeDiscount = discountType,
            ID_tbl_Kala = productId,
            Date = date
        }, cancellationToken));
    }

    public async Task<SysResult<int>> ProfitWithGoodsCount(long fiscalYear, long productId, string? from, string? to)
    {
        try
        {
            return new(await _dbManager.CallScalarProcedureWithParametersAsync<int>("Apk_Proc_Report_Kala_Profit_Prize", new
            {
                Type_Call_Proc = "Sod_Ba_Kala_Count_Record",
                ID_tbl_SalMaly = fiscalYear,
                ID_tbl_Kala = productId,
                Date1 = from,
                Date2 = to
            }));

        }
        catch (Exception ex)
        {
            return new(-1, false, new() { ex.Message });
        }
    }
}