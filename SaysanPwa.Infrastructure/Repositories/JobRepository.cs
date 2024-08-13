using SaysanPwa.Domain.AggregateModels.JobAggregate;
using SaysanPwa.Domain.SeedWorker;
using SqlServerWrapper.Wrapper.DatabaseManager;


namespace SaysanPwa.Infrastructure.Repositories;

public class JobRepository : IJobRepository
{
    private readonly IDbManager _dbManager;
    public JobRepository(IDbManager dbManager) => _dbManager = dbManager;

    public async Task<SysResult<bool>> AddNewJob(string title, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _dbManager.ExecuteNonQueryWithParametersAsync("EXEC Apk_Proc_AddNewJob @JobTitle", new
            {
                JobTitle = title
            });
            return new(result >= 0);
        }
        catch (Exception ex)
        {
            return new(false, false, new() { ex.Message });
        }
    }

    public async Task<SysResult<IEnumerable<Job>>> GetAllJobs(CancellationToken cancellationToken)
    {
        IEnumerable<Job> jobs = await _dbManager.CallProcedureAsync<Job>("Apk_Proc_GetAllJobs", cancellationToken);
        return new(jobs);
    }
}