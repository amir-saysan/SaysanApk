namespace SaysanPwa.Domain.SeedWorker;

public class PageResult<TResult>
{
    public PageInfo PageInfo { get; set; } = null!;

    public TResult Result { get; set; } = default!;
}
