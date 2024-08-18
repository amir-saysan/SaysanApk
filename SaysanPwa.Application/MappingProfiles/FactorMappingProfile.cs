using AutoMapper;
using SaysanPwa.Application.Commands.Factor;
using SaysanPwa.Application.Commands.Partnership;
using SaysanPwa.Application.DTOs.Factor;
using SaysanPwa.Application.DTOs.Partner;
using SaysanPwa.Application.DTOs.Product;
using SaysanPwa.Application.DTOs.ReceiptSheet;
using SaysanPwa.Application.DTOs.ReturnedInvoice;
using SaysanPwa.Application.Query.Factor;
using SaysanPwa.Domain.AggregateModels.Factor;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;

namespace SaysanPwa.Application.MappingProfiles;

public class FactorMappingProfile : Profile
{
    public FactorMappingProfile()
    {
        CreateMap<GetPartnersForFactorViewModel, GetPartnersForFactorDto>();
        CreateMap<AddPreFactorDto, AddFactorCommand>();
        CreateMap<AddSaleFactorDto, AddSaleFactorCommand>();
        CreateMap<AddFactorCommand, PreFactor>();
        CreateMap<AddFactorItemDto, FactorItem>();

        CreateMap<GetProductsForFactorViewModel, GetProductsForFactorDto>();

        CreateMap<FactorValidationRequest, FactorValidationQuery>();
        CreateMap<AddSaleFactorCommand, SaleFactor>();
    

        CreateMap<Domain.AggregateModels.Factor.Services, ServicesForSearchDto>();



        CreateMap<AddServiceSaleFactorDto, AddServiceSaleFactorCommand>();
        CreateMap<AddServiceSaleFactorCommand, SaleServiceFactor>();
        CreateMap<AddSaleServiceFactorItemDto, ServiceFactorItem>();
        CreateMap<Factor, FactorDto>();
        //CreateMap<AddSaleServiceFactorItemDto, ServiceFactorItem>();
        CreateMap<SaleServiceFactor, GetServiceSaleFactorDto>();
        CreateMap<SaleFactor, GetSaleFactorDto>();
        CreateMap<AddReturnedInvoiceCommand, ReturnedInvoice>();
        CreateMap<AddReturnedInvoiceDto, AddReturnedInvoiceCommand>();

		CreateMap<ReturnedInvoice, ReturnedInvoiceDto>();


		CreateMap<SaleServiceFactorDetailViewModel, ServiceSaleFactorDetailDto>();
		CreateMap<ServiceSaleFactorDetailDto, EditServiceSaleFactorDto>();
		CreateMap<EditServiceSaleFactorDto, EditServiceSaleFactorCommand>();
		CreateMap< EditServiceSaleFactorCommand, SaleServiceFactor> ();
		CreateMap<EditServiceSaleFactorDto, EditServiceSaleFactorCommand>();

		CreateMap<ReceiptSheet, GetReceiptSheetDto>();


		CreateMap<TaxSale, TaxSaleDto>();

	}
}