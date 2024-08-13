using Quartz;

namespace SaysanPwa.Api.Quartz;

public class QuartzJobs
{
    public static IReadOnlyDictionary<IJobDetail, IReadOnlyCollection<ITrigger>> Jobs =>
        new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();
}
