using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.Factor;

public interface IFactorRepository : IRepository<Factor>
{
    Task<SysResult<IEnumerable<GetPartnersForFactorViewModel>>> GetPartnerAndBranches(object parameters = null!, CancellationToken cancellationToken = default);
    Task<SysResult<bool>> AddPreFactor(PreFactor preFactor, IEnumerable<FactorItem> factorItems, CancellationToken cancellationToken = default);
    Task<SysResult<bool>> AddSaleFactor(SaleFactor saleFactor, IEnumerable<FactorItem> factorItems, CancellationToken cancellationToken = default);
    Task<SysResult<bool>> AddSaleServiceFactor(SaleServiceFactor saleServiceFactor, IEnumerable<ServiceFactorItem> factorItems, CancellationToken cancellationToken = default);
    Task<SysResult<FactorPricing>> GetCustomPriceForProduct(TypeCallProcedure typeCallProcedure, long partnerId, long productId, int fiscalYear, CancellationToken cancellationToken = default);
    Task<IEnumerable<PartnerInServiceFactor>> GetAllServicePartnetsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Services>> GetAllServices(CancellationToken cancellationToken);
    Task<List<Factor>> GetFactors(int fiscalYear, int UserId, string Date1, string Date2 ,object parameters = null!, CancellationToken cancellationToken = default);
    Task<SysResult<IEnumerable<SaleFactor>>> GetSaleFactors(int fiscalYear, int UserId, string Date1, string Date2, object parameters = null!, CancellationToken cancellationToken = default);
    Task<List<SaleFactor>> GetAllSaleFactorsCount(int fiscalYear, int UserId, string Date1, string Date2, CancellationToken cancellationToken = default);
    Task<SysResult<IEnumerable<SaleServiceFactor>>> GetServiceSaleFactors(int fiscalYear, int UserId, string Date1, string Date2, object parameters = null!, CancellationToken cancellationToken = default);
    Task<List<SaleServiceFactor>> GetAllServiceSaleFactorsCount(int fiscalYear, int UserId, string Date1, string Date2, CancellationToken cancellationToken = default);
    Task<List<Factor>> GetFactorPrints(long? PishForoshID, long? fiscalYear, int? UserId, int? Offset, int? ID_tbl_Bzy, string? Date1, string? Date2, object parameters = null!, CancellationToken cancellationToken = default);
    Task<List<ReturnedInvoice>> GetReturnedInvoices(int fiscalYear, int UserId, string Date1, string Date2, object parameters = null!, CancellationToken cancellationToken = default);
    Task<SysResult<bool>> AddReturnedInvoice(ReturnedInvoice returnedInvoice, IEnumerable<FactorItem> factorItems, CancellationToken cancellationToken = default);
    Task<List<SaleFactor>> GetSaleFactorPrint(long? ForoshID, long? fiscalYear, int? UserId, int? Offset, int? ID_tbl_Bzy, string? Date1, string? Date2, object parameters = null!, CancellationToken cancellationToken = default);
    Task<List<SaleServiceFactor>> GetServiceSaleFactorPrint(long? ForoshID, long? fiscalYear, int? UserId, int? Offset, int? ID_tbl_Bzy, string? Date1, string? Date2, object parameters = null!, CancellationToken cancellationToken = default);
	Task<List<ReturnedInvoice>> GetReturnedFactorPrint(long? ForoshID, long? fiscalYear, int? UserId, int? Offset, int? ID_tbl_Bzy, string? Date1, string? Date2, object parameters = null!, CancellationToken cancellationToken = default);
	Task<List<SaleServiceFactorDetailViewModel>> GetServiceSaleFactorDetailAsync(long id, int SalMalyId, CancellationToken cancellationToken = default);
    Task<SysResult<bool>> EditSaleServiceFactor(SaleServiceFactor saleServiceFactor, IEnumerable<ServiceFactorItem> factorItems, CancellationToken cancellationToken = default);
}

public enum TypeCallProcedure
{
    Select_Price_IN_FF,
    Select_Price_IN_FF1,
    Select_Last_Bah_Kala_to_next_year
}