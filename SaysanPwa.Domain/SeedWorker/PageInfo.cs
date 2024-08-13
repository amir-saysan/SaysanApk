namespace SaysanPwa.Domain.SeedWorker;

public class PageInfo
{
    public int CurrentPage { get; set; }
    public int ItemPerPage { get; set; }
    public int TotalItem { get; set; }
    public int TotalPage
    {
        get => (int)Math.Ceiling((decimal)TotalItem / ItemPerPage);
    }

    public PageInfo(int currentPage, int itemPerPage, int totalItem) =>
        (CurrentPage, ItemPerPage, TotalItem) = (currentPage, itemPerPage, totalItem);

    public PageInfo()
    {
        
    }
}
