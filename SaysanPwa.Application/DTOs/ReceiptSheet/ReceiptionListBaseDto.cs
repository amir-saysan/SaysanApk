namespace SaysanPwa.Application.DTOs.ReceiptSheet;

public class ReceiptionListBaseDto
{
    public Guid ItemId { get; set; }

    public ReceiptionListBaseDto()
    {
        ItemId = Guid.NewGuid();
    }
}
