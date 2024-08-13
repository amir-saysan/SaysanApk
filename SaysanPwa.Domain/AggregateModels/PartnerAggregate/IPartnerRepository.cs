using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.PartnerAggregate;

public interface IPartnerRepository : IRepository<Partner>
{
    public Task<SysResult<bool>> AddAsync(int userId, Partner partner, List<Branch>? branches = null, CancellationToken cancellationToken = default);
    public Task<SysResult<IEnumerable<PartnerDetailViewModel>>> GetAllAsync(string? searchPattern, int? offset = 0, CancellationToken cancellationToken = default);
    public Task<int> GetAllPartnerCountAsync(string? searchPattern,CancellationToken cancellationToken = default);
    public Task<SysResult<IEnumerable<PartnerGroup>>> GetAllPartnerGroups(CancellationToken cancellationToken = default);
    public Task<SysResult<PartnerDetailViewModel>> GetPartnerDetailAsync(long id, CancellationToken cancellationToken = default);
    public Task<SysResult<bool>> EditAsync(long userId, Partner partner, List<Branch>? branches = null, CancellationToken cancellationToken = default);
    public Task<SysResult<bool>> DeleteBranchById(long id, CancellationToken cancellationToken = default);
    public Task<SysResult<bool>> AddNewPartnerGroup(string title, CancellationToken cancellationToken = default);
    public Task<SysResult<IEnumerable<Branch>>> GetAllPartnerBranchesAsync(long partnerId, CancellationToken cancellationToken = default);


    public Task<SysResult<int>> GetAllSellReportsByPartnerIdCountAsync(long partnerId, long fiscalYear, long? marketerId, string? fromDate, string? toDate, CancellationToken cancellationToken = default);
    public Task<SysResult<IEnumerable<SellReport>>> GetAllSellReportsByPartnerIdAsync(long partnerId, long fiscalYear, long? marketerId, long offset, string? fromDate, string? toDate, CancellationToken cancellationToken = default);
    public Task<SysResult<int>> GetCheckCountByPartnerId(rdoEnglish_V_Ch rdoEnglish_V_Ch, long fiscalYear, long partnerId, string? from, string? to, CancellationToken cancellationToken = default);
    public Task<SysResult<IEnumerable<Check>>> GetAllChecks(rdoEnglish_V_Ch rdoEnglish_V_Ch, long fiscalYear, long partnerId, string? from, string? to, long offset, CancellationToken cancellationToken = default);
    public Task<SysResult<string>> CanDeleteBranch(int branchId, int partnerId, CancellationToken cancellationToken = default);
    public Task<SysResult<IEnumerable<PartnerConsolidationOfSalesOfGoodsToCustomers>>> GetAllPartnerConsolidationOfSalesOfGoodsToCustomers(long partnerId, long fiscalYear, long? marketerId, long offset, string? fromDate, string? toDate, CancellationToken cancellationToken = default);
}

public enum rdoEnglish_V_Ch
{
    rdoPas_Shode,
    rdoPas_Nashode,
    rdoDar_Jarayan,
    rdoBargashti
}