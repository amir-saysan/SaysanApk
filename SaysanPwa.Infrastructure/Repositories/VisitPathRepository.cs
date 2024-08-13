using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SaysanPwa.Domain.AggregateModels.Factor;
using SaysanPwa.Domain.AggregateModels.UserAggregate;
using SaysanPwa.Domain.AggregateModels.VisitPathAggregate;
using SaysanPwa.Domain.SeedWorker;
using SqlServerWrapper.Wrapper.DatabaseManager;
using System.Data;
using System.Globalization;

namespace SaysanPwa.Infrastructure.Repositories;

public class VisitPathRepository : IVisitPathRepository
{
    private readonly IConfiguration _configuration;
    private readonly IDbManager _dbManager;

    public VisitPathRepository(IConfiguration configuration, IDbManager dbManager)
    {
        _configuration = configuration;
        _dbManager = dbManager;
    }

    public async Task<SysResult<IEnumerable<GetVisitPathForUserModel>>> GetPathesForUser(long idBazaryab, string date, CancellationToken cancellationToken = default)
    {
        IEnumerable<GetVisitPathForUserModel> paths =
            await _dbManager.CallProcedureWithParametersAsync<GetVisitPathForUserModel>("Apk_Proc_Get_ToVisit_Branch_Per_Partner_Daily",
            new
            {
                ID_tbl_Bzy = idBazaryab,
                Date_Defenition = date
            });
        return new(paths);
    }
    public async Task<SysResult<bool>> PathVisitedAsync(long ID_tbl_Bzy, long ID_tbl_TarafHesab, long ID_tbl_Partner_Branch, string description, CancellationToken cancellationToken = default)
    {
        using(SqlConnection connection = new(_configuration.GetConnectionString("Default")))
        {
            using(SqlCommand command = new("Apk_Proc_Add_Aglm_ToVisit_Branch_Per_Partner_Daily", connection))
            {
                SqlParameter p = new ()
                {
                    Direction = ParameterDirection.Output,
                    ParameterName = "@ResultMessage",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 200
                };
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID_tbl_Bzy", ID_tbl_Bzy);
                command.Parameters.AddWithValue("@ID_tbl_TarafHesab", ID_tbl_TarafHesab);
                command.Parameters.AddWithValue("@ID_tbl_Partner_Branch", ID_tbl_Partner_Branch);
                command.Parameters.AddWithValue("@Description_Visited", description ?? "");

                PersianCalendar pc = new PersianCalendar();
                DateTime now = DateTime.Now;
                string Date_Visited = $"{pc.GetYear(now)}/{pc.GetMonth(now).ToString("D2")}/{pc.GetDayOfMonth(now).ToString("D2")}";
                command.Parameters.AddWithValue("@Date_Visited", Date_Visited);

                string Time_Visited = DateTime.Now.ToString("HH:mm:ss");
                command.Parameters.AddWithValue("@Time_Visited", Time_Visited);

                command.Parameters.Add(p);
                await connection.OpenAsync();
                //try
                //{
                    int reader = await command.ExecuteNonQueryAsync();
                    return new(true);
               // }
                //catch
                //{
                //    return new(false, false, new() { p.Value.ToString()!});
                //}
            }
        }
    }
}