using MediatR;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Application.Query.Products;

public class GetAllSmallOfGoodsQuery : IRequest<SysResult<IEnumerable<SmallOfGoods>>>
{
    public GetAllSmallOfGoodsQuery(SmallOfGoodsProcedureCallType procedureCallType, int fiscalYear, int productId, string? fromDate, string? toDate, long? offset)
    {
        ProcedureCallType = procedureCallType;
        FiscalYear = fiscalYear;
        ProductId = productId;
        FromDate = fromDate;
        ToDate = toDate;
        Offset = offset;
    }

    public SmallOfGoodsProcedureCallType ProcedureCallType { get; set; }
    public int FiscalYear { get; set; }
    public int ProductId { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public long? Offset { get; set; }
}

public enum SmallOfGoodsProcedureCallType
{
    Riz_Forosh_Kala,
    Riz_Kharid_Kala,
    Riz_Kharid_Forosh_Kala
}

public class GetAllSmallOfGoodsQueryHandler : IRequestHandler<GetAllSmallOfGoodsQuery, SysResult<IEnumerable<SmallOfGoods>>>
{
    private readonly IProductRepository _repository;

    public GetAllSmallOfGoodsQueryHandler(IProductRepository repository) => _repository = repository;


    public async Task<SysResult<IEnumerable<SmallOfGoods>>> Handle(GetAllSmallOfGoodsQuery request, CancellationToken cancellationToken) =>
        await _repository.GetAllSmallOfGoods(request.ProcedureCallType.ToString(), request.FiscalYear, request.ProductId, request.FromDate, request.ToDate, request.Offset, cancellationToken);
}