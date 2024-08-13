using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.SeedWorker;
using SqlServerWrapper.Wrapper.DatabaseManager;
using System.Data;

namespace SaysanPwa.Infrastructure.Repositories;

public class PartnerRepository : IPartnerRepository
{
    private readonly IConfiguration _configuration;
    private readonly IDbManager _dbManager;


    public PartnerRepository(IConfiguration configuration, IDbManager dbManager)
    {
        _configuration = configuration;
        _dbManager = dbManager;
    }
    public async Task<SysResult<bool>> AddAsync(int userId, Partner partner, List<Branch>? branches, CancellationToken cancellationToken = default)
    {
        try
        {
            using (SqlConnection connection = new(_configuration.GetConnectionString("Default")))
            {
                await connection.OpenAsync();

                SqlTransaction transaction;
                transaction = connection.BeginTransaction();

                try
                {
                    long partnerId;
                    using (SqlCommand cmd = new("Apk_Proc_AddNewPartner", connection))
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandType = CommandType.StoredProcedure;
                        var properties = partner.GetType().GetProperties().ToList();
                        properties.RemoveAll(p => p.Name == "ID_tbl_TarafHesab");
                        properties.RemoveAll(p => p.Name == "Notifications");
                        foreach (var item in properties)
                        {
                            cmd.Parameters.AddWithValue($"@{item.Name}", item.GetValue(partner));
                        }
                        partnerId = (long)(decimal)await cmd.ExecuteScalarAsync(cancellationToken);

                        //new(true) : new(false, false, new() { "عملیات ناموفق بود." });
                    }
                    foreach (Branch branch in branches)
                    {
                        using (SqlCommand cmd2 = new("Apk_Proc_AddNew_Partner_Branch", connection))
                        {
                            cmd2.Transaction = transaction;
                            cmd2.CommandType = CommandType.StoredProcedure;
                            var properties = branch.GetType().GetProperties().ToList();
                            properties.RemoveAll(b => b.Name == "Notifications");
                            properties.RemoveAll(b => b.Name == "ID_tbl_Partner_Branch");
                            properties.RemoveAll(b => b.Name == "ID_tbl_TarafHesab");
                            properties.RemoveAll(b => b.Name == "State_Branch");

                            cmd2.Parameters.AddWithValue($"@ID_tbl_TarafHesab", partnerId);
                            cmd2.Parameters.AddWithValue("@ID_tbl_Users", userId);
                            foreach (var item in properties)
                            {
                                cmd2.Parameters.AddWithValue($"@{item.Name}", item.GetValue(branch));
                            }
                            var result = await cmd2.ExecuteNonQueryAsync();
                        }
                    }
                    transaction.Commit();
                    return new(true);
                }
                catch
                {
                    transaction.Rollback();
                    return new(false, false, new() { "مشگلی در افزودن طرف حساب پیش آمد." });
                }

            }
        }
        catch (Exception)
        {

            return new(false, false, new() { "مشگلی در افزودن طرف حساب پیش آمد." });
        }
    }

    public async Task<SysResult<IEnumerable<PartnerGroup>>> GetAllPartnerGroups(CancellationToken cancellationToken = default)
    {
        IEnumerable<PartnerGroup> partnerGroups = await _dbManager.CallProcedureAsync<PartnerGroup>("Apk_Proc_GetAllPartnerGruops", cancellationToken);
        return new(partnerGroups);
    }
    public async Task<SysResult<IEnumerable<PartnerDetailViewModel>>> GetAllAsync(string? searchPattern, int? offset = 0, CancellationToken cancellationToken = default)
    {
        IEnumerable<PartnerDetailViewModel> partners = Enumerable.Empty<PartnerDetailViewModel>();
        if (offset != null)
        {
            partners = await _dbManager.CallProcedureWithParametersAsync<PartnerDetailViewModel>("Apk_Proc_GetAllPartners", new
            {
                Type_Call_Procedure = "get_all_partners_By_Offset",
                SearchPattern = searchPattern,
                Offset = offset
            }, cancellationToken);
        }
        else
        {
            partners = await _dbManager.CallProcedureWithParametersAsync<PartnerDetailViewModel>("Apk_Proc_GetAllPartners", new
            {
                Type_Call_Procedure = "get_all_partners",
                SearchPattern = searchPattern,
            }, cancellationToken);
        }

        return new(partners);
    }

    //TODO: make it works
    public async Task<SysResult<PartnerDetailViewModel>> GetPartnerDetailAsync(long id, CancellationToken cancellationToken = default)
    {
        PartnerDetailViewModel partner = await _dbManager.CallSingleRowProcedureWithParametersAsync<PartnerDetailViewModel>("Apk_Proc_GetAllPartners", new
        {
            ID_tbl_TarafHesab = id,
            Type_Call_Procedure = "get_all_partners_By_Offset"
        }, cancellationToken);

        IEnumerable<Branch> branches = await _dbManager.CallProcedureWithParametersAsync<Branch>("Apk_Proc_Get_Partner_Branches", new
        {
            ID_tbl_TarafHesab = id
        }, cancellationToken);

        partner.Branches = branches.ToList();

        return new(partner);
    }

    public async Task<SysResult<bool>> EditAsync(long userId, Partner partner, List<Branch>? branches, CancellationToken cancellationToken = default)
    {
        bool success = false;
        try
        {
            using (SqlConnection connection = new(_configuration.GetConnectionString("Default")))
            {
                await connection.OpenAsync(cancellationToken);
                SqlTransaction transaction = connection.BeginTransaction();
                var p = new SqlParameter()
                {
                    Direction = ParameterDirection.Output,
                    ParameterName = "@ResultMessage",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 200
                };
                try
                {
                    using (SqlCommand cmd = new("Apk_Proc_EditPartner", connection))
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandType = CommandType.StoredProcedure;
                        partner.ID_tbl_Ostan = partner.ID_tbl_Ostan_Asli;
                        partner.ID_tbl_SharOstan = partner.ID_tbl_SharOstan_Asli;
                        var properties = partner.GetType().GetProperties().ToList();
                        properties.RemoveAll(p => p.Name == "Notifications");
                        properties.RemoveAll(b => b.Name == "Code_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Type_TarafHesab_Samane_Moadyan");
                        properties.RemoveAll(b => b.Name == "National_TarafHesab");
                        properties.RemoveAll(b => b.Name == "State_TarafHesab");
                        properties.RemoveAll(b => b.Name == "State");
                        properties.RemoveAll(b => b.Name == "Tamin_Konande_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Aval_Mnd_TaminKonande");
                        properties.RemoveAll(b => b.Name == "Aval_Type_TaminKonande");
                        properties.RemoveAll(b => b.Name == "Darayi_TaminKonande");
                        properties.RemoveAll(b => b.Name == "Kharidar_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Aval_Mnd_Kharidar");
                        properties.RemoveAll(b => b.Name == "Aval_Type_Kharidar");
                        properties.RemoveAll(b => b.Name == "Darayi_Kharidar");
                        properties.RemoveAll(b => b.Name == "Website_TarafHesab");
                        properties.RemoveAll(b => b.Name == "ID_tbl_Ostan");
                        properties.RemoveAll(b => b.Name == "ID_tbl_SharOstan");
                        properties.RemoveAll(b => b.Name == "CodePosti_Home_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Tell_Home_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Address_Home_TarafHesab");
                        properties.RemoveAll(b => b.Name == "ID_tbl_Ostan1");
                        properties.RemoveAll(b => b.Name == "ID_tbl_SharOstan1");
                        properties.RemoveAll(b => b.Name == "CodePosti_MahaleKar_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Tell_MahaleKar_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Address_MahaleKar_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Bzy_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Karmand_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Aval_Mnd_Karmand");
                        properties.RemoveAll(b => b.Name == "Aval_Type_Karmand");
                        properties.RemoveAll(b => b.Name == "Jensyat_Karmand_TarafHesab");
                        properties.RemoveAll(b => b.Name == "ID_tbl_Ostan_Tv");
                        properties.RemoveAll(b => b.Name == "ID_tbl_SharOstan_Tv");
                        properties.RemoveAll(b => b.Name == "ID_tbl_Ostan_Sd");
                        properties.RemoveAll(b => b.Name == "ID_tbl_SharOstan_Sd");
                        properties.RemoveAll(b => b.Name == "Shomare_Shenasname_Karmand_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Serial_Shenasname_Karmand_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Fathername_Karmand_TarafHesab");
                        properties.RemoveAll(b => b.Name == "MadrakTahsily_Karmand_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Sarbazy_Karmand_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Marrid_Karmand_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Tedad_Farzand_Karmand_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Tedad_Takafol_Karmand_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Dt_Es");
                        properties.RemoveAll(b => b.Name == "Dt_Pay");
                        properties.RemoveAll(b => b.Name == "Number_Bime_Karmand_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Number_BimeTakmily_Karmand_TarafHesab");
                        properties.RemoveAll(b => b.Name == "SabegeBime_Karmand_TarafHesab");
                        properties.RemoveAll(b => b.Name == "Typ_Mal");

                        foreach (var item in properties)
                        {
                            cmd.Parameters.AddWithValue($"@{item.Name}", item.GetValue(partner) ?? default);
                        }
                        cmd.Parameters.Add(p);

                        var result = await cmd.ExecuteReaderAsync(cancellationToken);
                        success = result.RecordsAffected > 0 ? true : false;
                        await result.CloseAsync();
                    }


                    using (SqlCommand cmd = new("Apk_Proc_Edit_Partner_Branch", connection))
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_tbl_Users", userId);
                        foreach (Branch branch in branches)
                        {
                            if (branch.ID_tbl_TarafHesab == 0)
                            {
                                branch.ID_tbl_TarafHesab = partner.ID_tbl_TarafHesab;
                            }
                            var properties = branch.GetType().GetProperties().ToList();
                            properties.RemoveAll(b => b.Name == "Notifications");



                            foreach (var item in properties)
                            {
                                cmd.Parameters.AddWithValue($"@{item.Name}", item.GetValue(branch) ?? default);
                            }
                            var result = await cmd.ExecuteReaderAsync(cancellationToken);
                            await result.CloseAsync();
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@ID_tbl_Users", userId);
                        }
                    }

                    transaction.Commit();
                    return success ? new(true) : new(false, false, new() { p.Value.ToString()! });
                }
                catch
                {
                    transaction.Rollback();
                    return new(false, false, new() { p.Value.ToString()! });
                }


            }
        }
        catch (Exception)
        {

            return new(false, false, new() { "مشگلی در افزودن طرف حساب پیش آمد." });
        }
    }

    public async Task<SysResult<bool>> DeleteBranchById(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _dbManager.ExecuteNonQueryWithParametersAsync("EXEC Apk_Proc_DeleteBranchById @PartnerId", new
            {
                PartnerId = id
            });

            return result >= 0 ? new(true) : new(false);
        }
        catch (Exception ex)
        {
            return new(false, false, new() { ex.Message });
        }
    }

    public async Task<SysResult<bool>> AddNewPartnerGroup(string title, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _dbManager.ExecuteNonQueryWithParametersAsync("EXEC Apk_Proc_AddNewPartnerGroup @GroupTitle", new
            {
                GroupTitle = title
            });
            return new(result >= 0);
        }
        catch (Exception ex)
        {
            return new(false, false, new() { ex.Message });
        }
    }

    public async Task<SysResult<IEnumerable<Branch>>> GetAllPartnerBranchesAsync(long partnerId, CancellationToken cancellationToken = default)
    {
        IEnumerable<Branch> branches = await _dbManager.CallProcedureWithParametersAsync<Branch>("Apk_Proc_Get_Partner_Branches", new
        {
            ID_tbl_TarafHesab = partnerId
        }, cancellationToken);
        return new(branches);
    }

    public async Task<SysResult<IEnumerable<SellReport>>> GetAllSellReportsByPartnerIdAsync(long partnerId, long fiscalYear, long? marketerId, long offset, string? fromDate, string? toDate, CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<SellReport> sellReports = await _dbManager.CallProcedureWithParametersAsync<SellReport>("Apk_Proc_Report_Forosh",
                new
                {
                    Type_Call_Proc = "Select_Top_N_Factor",
                    ID_tbl_SalMaly = fiscalYear,
                    ID_tbl_TarafHesab = partnerId,
                    ID_tbl_Bzy = marketerId,
                    Offset = offset,
                    Date1 = fromDate,
                    Date2 = toDate
                }, cancellationToken);

            return new(sellReports);
        }
        catch (Exception ex)
        {
            return new(null!, false, new() { ex.Message });
        }
    }

    public async Task<SysResult<int>> GetCheckCountByPartnerId(rdoEnglish_V_Ch rdoEnglish_V_Ch, long fiscalYear, long partnerId, string? from, string? to, CancellationToken cancellationToken = default)
    {
        try
        {
            return new(await _dbManager.CallScalarProcedureWithParametersAsync<int>("Apk_Proc_Report_Daryaft_Check", new
            {
                Type_Call_Proc = "Select_Daryaft_Check_Count_Record",
                rdoEnglish_V_Ch = rdoEnglish_V_Ch.ToString(),
                ID_tbl_SalMaly = fiscalYear,
                ID_tbl_TarafHesab = partnerId,
                Date1 = from ?? null!,
                Date2 = to ?? null!
            }));
        }
        catch (Exception ex)
        {
            return new(0, false, new() { ex.Message });
        }
    }

    public async Task<SysResult<IEnumerable<Check>>> GetAllChecks(rdoEnglish_V_Ch rdoEnglish_V_Ch, long fiscalYear, long partnerId, string? from, string? to, long offset, CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<Check> checks = await _dbManager.CallProcedureWithParametersAsync<Check>("Apk_Proc_Report_Daryaft_Check", new
            {
                Type_Call_Proc = "Select_Daryaft_Check",
                rdoEnglish_V_Ch = rdoEnglish_V_Ch.ToString(),
                ID_tbl_SalMaly = fiscalYear,
                ID_tbl_TarafHesab = partnerId,
                Date1 = from ?? null!,
                Date2 = to ?? null!,
                Offset = offset
            }, cancellationToken);

            return new(checks);
        }
        catch (Exception ex)
        {
            return new(null!, false, new() { ex.Message });
        }
    }

    public async Task<SysResult<string>> CanDeleteBranch(int branchId, int partnerId, CancellationToken cancellationToken = default)
    {
        try
        {
            using (SqlConnection connection = new(_configuration.GetConnectionString("Default")))
            {
                using (SqlCommand cmd = new("Apk_Proc_Get_Partner_Branche_When_Deleting", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_tbl_TarafHesab", partnerId);
                    cmd.Parameters.AddWithValue("@ID_tbl_Partner_Branch", branchId);
                    await connection.OpenAsync(cancellationToken);

                    SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                    string preventToDeleteResean = string.Empty;
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        string reasen = (string)reader["Type_Prevent_To_Delete"];
                        string fiscalYear = (string)reader["Title_SalMaly"];

                        preventToDeleteResean = $"به دلیل وجود این شعبه در {reasen} در سال مالی {fiscalYear} امکان حذف این شعبه وجود ندارد.";
                    }

                    return new(preventToDeleteResean, string.IsNullOrEmpty(preventToDeleteResean) ? true : false);
                }
            }
        }
        catch (Exception ex)
        {
            return new("-", false, new() { ex.Message });
        }
    }

    public static T ConvertFromDBVal<T>(object obj)
    {
        if (obj == null || obj == DBNull.Value)
        {
            return default(T)!; // returns the default value for the type
        }
        else
        {
            return (T)obj;
        }
    }

    public async Task<SysResult<IEnumerable<PartnerConsolidationOfSalesOfGoodsToCustomers>>> GetAllPartnerConsolidationOfSalesOfGoodsToCustomers(long partnerId, long fiscalYear, long? marketerId, long offset, string? fromDate, string? toDate, CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<PartnerConsolidationOfSalesOfGoodsToCustomers> list =
                await _dbManager.CallProcedureWithParametersAsync<PartnerConsolidationOfSalesOfGoodsToCustomers>("Apk_Proc_Report_Forosh", new
                {
                    Type_Call_Proc = "Tajm_Forosh_Ba_Msh_Tafkik_Kala",
                    ID_tbl_SalMaly = fiscalYear,
                    ID_tbl_TarafHesab = partnerId,
                    ID_tbl_Bzy = marketerId,
                    Offset = offset,
                    Date1 = fromDate,
                    Date2 = toDate
                }, cancellationToken);
            return new(list);
        }
        catch (Exception ex)
        {
            return new(Enumerable.Empty<PartnerConsolidationOfSalesOfGoodsToCustomers>(), false, new() { ex.Message });
        }
    }

    public async Task<SysResult<int>> GetAllSellReportsByPartnerIdCountAsync(long partnerId, long fiscalYear, long? marketerId, string? fromDate, string? toDate, CancellationToken cancellationToken = default)
    {
        try
        {
            return new(await _dbManager.CallScalarProcedureWithParametersAsync<int>("Apk_Proc_Report_Forosh",
                new
                {
                    Type_Call_Proc = "Select_Top_N_Factor_Count_Record",
                    ID_tbl_SalMaly = fiscalYear,
                    ID_tbl_TarafHesab = partnerId,
                    ID_tbl_Bzy = marketerId,
                    Date1 = fromDate,
                    Date2 = toDate,
                }, cancellationToken));
        }
        catch (Exception ex)
        {
            return new(0, false, new() { ex.Message });
        }
    }

    public async Task<int> GetAllPartnerCountAsync(string? searchPattern, CancellationToken cancellationToken = default)
    {
        int pageCount = await _dbManager.CallScalarProcedureWithParametersAsync<int>("Apk_Proc_GetAllPartners", new
        {
            Type_Call_Procedure = "get_all_partners_Count_Record",
            SearchPattern = searchPattern,
        }, cancellationToken);

        return pageCount;
    }
}