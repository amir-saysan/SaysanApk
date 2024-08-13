using AutoMapper;
using MediatR;
using SaysanPwa.Application.DTOs.Partner;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;
using System.ComponentModel.DataAnnotations;


namespace SaysanPwa.Application.Commands.Partnership
{
    public class EditPartnerCommand : IRequest<SysResult<bool>>
    {
        public long ID_tbl_Partner_Branch { get; set; }
        public long ID_tbl_TarafHesab { get; set; }
        public string Type_TarafHesab { get; set; } = null!;
        public int ID_tbl_Group_TF { get; set; }
        public string Name_TarafHesab { get; set; } = null!;
        public long Code_TarafHesab { get; set; }
        public string CodeMelli_TarafHesab { get; set; } = string.Empty;
        public string CodeEgtesad_TarafHesab { get; set; } = string.Empty;
        public int ID_tbl_Job { get; set; }
        public string BirthDay_TarafHesab { get; set; } = string.Empty;
        public string Marrid_Date_TarafHesab { get; set; } = string.Empty;
        public bool Jelogiri_Had_Etebar_TarafHesab { get; set; }
        public string ChelPhone_TarafHesab { get; set; } = string.Empty;
        public string Fax_TarafHesab { get; set; } = string.Empty;
        public string Email_TarafHesab { get; set; } = string.Empty;
        public int ID_tbl_Ostan_Asli { get; set; }
        public int ID_tbl_SharOstan_Asli { get; set; }
        public string CodePosti_Asli { get; set; } = string.Empty;
        public string Tell_Asli { get; set; } = string.Empty;
        public string Address_Asli { get; set; } = string.Empty;
        public int ID_tbl_Bank { get; set; }
        public int ID_tbl_BanchBank { get; set; }
        public int ID_tbl_TypeHesab { get; set; }
        public string HesabNumber_TarafHesab { get; set; } = string.Empty;
        public decimal Sagf_Eteb_Snd_TarafHesab { get; set; } = 0;
        public decimal Sagf_Eteb_Ngd_TarafHesab { get; set; } = 0;
        public string? Title_Job { get; set; }
        public string? Name_Group { get; set; }
        public string? Name_Ostan { get; set; }
        public string? Name_SharOstan { get; set; }

        public string Location_TarafHesab { get; set; } = string.Empty;

        public List<EditBranchDto>? Branches { get; set; } = new();


        public long UserId { get; set; }
    }

    public class EditPartnerCommandHandler : IRequestHandler<EditPartnerCommand, SysResult<bool>>
    {
        private readonly IPartnerRepository _repository;
        private readonly IMapper _mapper;

        public EditPartnerCommandHandler(IMapper mapper, IPartnerRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<SysResult<bool>> Handle(EditPartnerCommand request, CancellationToken cancellationToken)
        {
            Partner partner = _mapper.Map<Partner>(request);
            List<Branch> branches = _mapper.Map<List<Branch>>(request.Branches);
            var result = await _repository.EditAsync(request.UserId, partner, branches);
            return result;
        }
    }
}
