using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SaysanPwa.Application.DTOs.ReceiptSheet;

public class CheckDto : ReceiptionListBaseDto
{
    [Required(ErrorMessage = "لطفا شماره چک را وارد نمایید.")]
    public string CheckNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "لطفا مبلغ چک را وارد نمائید.")]
    public decimal CheckAmount { get; set; }

    [Required(ErrorMessage = "لطفا پشت نمره چک را وارد نمائید.")]
    public int BehindTheCheckMark { get; set; }

    public string? Series { get; set; } = string.Empty;

    public string ReceiveDate { get; set; } = DateTime.Now.ToString("yyyy/MM/dd", new CultureInfo("fa_IR"));
    [Required(ErrorMessage = "لطفا تاریخ سر رسید چک را وارد نمائید.")]

    public string DuDate { get; set; } = string.Empty;
    public string? Description { get; set; } = "";


    public CheckDto() : base()
    {
        
    }

    public CheckDto(string checkNumber, decimal checkAmount, int behindTheCheckMark, string? series, string receiveDate, string duDate, string? description) : base()
    {
        CheckNumber = checkNumber;
        CheckAmount = checkAmount;
        BehindTheCheckMark = behindTheCheckMark;
        Series = series;
        ReceiveDate = receiveDate;
        DuDate = duDate;
        Description = description;
    }
}