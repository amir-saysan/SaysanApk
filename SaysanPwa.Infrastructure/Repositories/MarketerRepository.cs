using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SaysanPwa.Domain.AggregateModels.MarketerAggregate;
using SaysanPwa.Domain.SeedWorker;
using SaysanPwa.Infrastructure.RepositoryExtensions;
using SqlServerWrapper.Wrapper.DatabaseManager;
using System.Data;
using System.Reflection;

namespace SaysanPwa.Infrastructure.Repositories;

public class MarketerRepository : IMarketerRepository
{
    private readonly IDbManager _dbManager;

    public MarketerRepository(IDbManager dbManager) => _dbManager = dbManager;

    public async Task<SysResult<long>> GetMarketerIdByPartnerIdAsync(long partnerId, CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<Marketer> marketers = await _dbManager.CallProcedureWithParametersAsync<Marketer>("Apk_Proc_Get_Bzy", new
            {
                ID_tbl_TarafHesab = partnerId
            });


            if (marketers.Any())
            {
                Marketer marketer = marketers.First();
                if (marketer.State)
                {
                    return new(marketer.ID_tbl_Bzy);
                }
                else
                {
                    return new(0, false, new() { "حساب بازاریاب فعال نمیباشد." });
                }
            }
            else
            {
                return new(0, false, new() { "کاربر بازاریاب نمیباشد." });
            }

        }

        catch (Exception ex)
        {
            return new(0, false, new() { "مشگلی در سرور رخ داده است. لطفا با پشتیبانی تماس بگیرید." });
        }
    }

}
