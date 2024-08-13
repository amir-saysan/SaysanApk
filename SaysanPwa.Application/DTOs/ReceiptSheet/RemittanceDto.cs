using System.ComponentModel.DataAnnotations;

namespace SaysanPwa.Application.DTOs.ReceiptSheet;

public class RemittanceDto : ReceiptionListBaseDto
{
    [Required(ErrorMessage = "حساب بانکی نمیتواند خالی باشد.")]
    public int BankAccountId { get; set; }

    [Required(ErrorMessage = "مبلغ نمیتواند خالی باشد")]
    public decimal Price { get; set; }

    public string? RemittanceNumber { get; set; } = string.Empty;
    public string? Description { get; set; } = "";

    public RemittanceDto() : base()
    {
        
    }

    public RemittanceDto(int bankAccountId, decimal price, string? remittanceNumber, string? description) : base()
    {
        BankAccountId = bankAccountId;
        Price = price;
        RemittanceNumber = remittanceNumber;
        Description = description;
    }
}