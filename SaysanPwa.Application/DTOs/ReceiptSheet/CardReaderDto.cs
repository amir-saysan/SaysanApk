namespace SaysanPwa.Application.DTOs.ReceiptSheet;

public class CardReaderDto : ReceiptionListBaseDto
{
    public int BankAccountId { get; set; }
    public decimal Price { get; set; }
    public string? IssueTracking { get; set; } = string.Empty;
    public string? TransactionSeries { get; set; } = string.Empty;
    public string? Description { get; set; } = "";

    public CardReaderDto() : base()
    {
        
    }

    public CardReaderDto(int bankAccountId, decimal price, string? issueTracking, string? transactionSeries, string? description) : base()
    {
        BankAccountId = bankAccountId;
        Price = price;
        IssueTracking = issueTracking;
        TransactionSeries = transactionSeries;
        Description = description;
    }
}