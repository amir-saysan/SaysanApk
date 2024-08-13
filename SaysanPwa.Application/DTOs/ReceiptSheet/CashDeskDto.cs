namespace SaysanPwa.Application.DTOs.ReceiptSheet;

public class CashDeskDto : ReceiptionListBaseDto
{
    public int CashDeskId { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; } = "";

    public CashDeskDto() : base()
    {
        
    }

    public CashDeskDto(int cashDeskId, decimal price, string? description) : base()
    {
        CashDeskId = cashDeskId;
        Price = price;
        Description = description;
    }
}