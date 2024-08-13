
using MediatR;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;

namespace SaysanPwa.Application.Query.Factor;

public record FactorValidationQuery : IRequest<FactorValidationResult>
{
    public long PartnerId { get; set; }
    public long BranchId { get; set; }
    public bool IsServiceValidation { get; set; } = false;
    public bool AgentValidation { get; set; } = false;
    public IEnumerable<FactorValidationProduct> Products { get; set; } = null!;
}

public class FactorValidationQueryHandler : IRequestHandler<FactorValidationQuery, FactorValidationResult>
{
    private readonly IFactorRepository _factorRepository;
    private readonly IProductRepository _productRepository;

    public FactorValidationQueryHandler(IProductRepository productRepository, IFactorRepository factorRepository)
    {
        _productRepository = productRepository;
        _factorRepository = factorRepository;
    }

    public async Task<FactorValidationResult> Handle(FactorValidationQuery request, CancellationToken cancellationToken)
    {
        FactorValidationResult result = new();
        var partners = (await _factorRepository.GetPartnerAndBranches()).Result;
        if (request.AgentValidation)
        {
            Console.WriteLine("Agent validation.");
            if (!partners.Any(p => p.ID_tbl_TarafHesab == request.PartnerId || !p.Karmand_TarafHesab))
            {
                result.result = false;
                result.Fails!.Add("طرفحساب حذف شده یا وجود ندارد.");
            }
            if (request.BranchId != 0) // branch 0 means main(asli) branch so we dont check if branch exists => موز 
            {
                if (!partners.Any(b => b.ID_tbl_Partner_Branch == request.PartnerId))
                {
                    result.result = false;
                    result.Fails!.Add("شعبه طرفحساب حذف شده یا وجود ندارد.");
                }
            }
        }
        else
        {
            await Console.Out.WriteLineAsync("Partner validation.");
            if (!partners.Any(p => p.ID_tbl_TarafHesab == request.PartnerId || !p.Kharidar_TarafHesab))
            {
                result.result = false;
                result.Fails!.Add("طرفحساب حذف شده یا وجود ندارد.");
            }
            if (request.BranchId != 0) // branch 0 means main(asli) branch so we dont check if branch exists => موز 
            {
                if (!partners.Any(b => b.ID_tbl_Partner_Branch == request.PartnerId))
                {
                    result.result = false;
                    result.Fails!.Add("شعبه طرفحساب حذف شده یا وجود ندارد.");
                }
            }
        }

        if (request.IsServiceValidation)
        {
            await Console.Out.WriteLineAsync("inside of the service validation.");
            var services = await _factorRepository.GetAllServices(cancellationToken);
            foreach(var p in request.Products)
            {
                if (!services.Any(s => s.ID_tbl_Khedmat == p.ProductId))
                {
                    result.result = false;
                    result.Fails!.Add("خدمت " + p.ProductName + " وجود ندارد یا حذف شده.");
                }
            }
        }
        else
        {
            var products = (await _productRepository.GetProductsForFactor()).Result;
            foreach (var p in request.Products)
            {
                if (!products.Any(r => r.ID_tbl_Kala == p.ProductId))
                {
                    result.result = false;
                    result.Fails!.Add("محصول " + p.ProductName + " وجود ندارد یا حذف شده");
                }
            }
        }

        return result;
    }
}
