using MediatR;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Products;

public class GetProductCountQuery : IRequest<SysResult<int>>
{
    public int FiscalYear { get; set; }
    public int ProductId { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public ProcedureCallType ProcedureCallType { get; set; }


    public GetProductCountQuery(ProcedureCallType procedureCallType, int fiscalYear, int productId, string fromDate, string toDate) =>
        (ProcedureCallType, FiscalYear, ProductId, FromDate, ToDate) = (procedureCallType, fiscalYear, productId, fromDate, toDate);
}

public enum ProcedureCallType
{
    Riz_Kharid_Kala_Count_Record,
    Riz_Forosh_Kala_Count_Record,
    Riz_Kharid_Forosh_Kala_Count_Record
}

public class GetProductCountQueryHandler : IRequestHandler<GetProductCountQuery, SysResult<int>>
{
    private readonly IProductRepository _repository;

    public GetProductCountQueryHandler(IProductRepository repository) => _repository = repository;

    public async Task<SysResult<int>> Handle(GetProductCountQuery request, CancellationToken cancellationToken) =>
        await _repository.GetProductCount(request.ProcedureCallType.ToString(), request.FiscalYear, request.ProductId, request.FromDate, request.ToDate, cancellationToken);
}