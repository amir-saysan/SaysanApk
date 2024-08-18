using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SaysanPwa.Domain.AggregateModels.Factor;
using SaysanPwa.Domain.AggregateModels.PartnerAggregate;
using SaysanPwa.Domain.AggregateModels.ReceiptSheetAggregate;
using SaysanPwa.Domain.AggregateModels.UserAggregate;
using SaysanPwa.Domain.SeedWorker;
using SqlServerWrapper.Wrapper.DatabaseManager;
using System.Data;
using System.Formats.Asn1;
using System.Threading;

namespace SaysanPwa.Infrastructure.Repositories;

public class ReceiptSheetRepository : IReceiptSheetRepository
{
    private readonly IConfiguration _configuration;
    private readonly IDbManager _dbManager;

    public ReceiptSheetRepository(IConfiguration configuration, IDbManager dbManager)
    {
        _configuration = configuration;
        _dbManager = dbManager;
    }

    public async Task<SysResult<bool>> AddReceiptSheet(Tbl_DA tbl_DA, List<tbl_Daryaft_Chek> checks, List<tbl_DA_H> haveleha, List<tbl_DA_K> kartkhan)//, List<Tbl_DA> sandogh
    {
        CancellationToken cancellationToken = default;
        using (SqlConnection connection = new(_configuration.GetConnectionString("Default")))
        {
            await connection.OpenAsync();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                //string Yearr = User.FindFirstValue("FiscalYearBeginDate");
                if (int.Parse(tbl_DA.Dt_DA.Substring(0,4))!=1403)
                {
                    return new(false, false, new List<string> { "!تاریخ دریافت نامعتبر می باشد" });
                }

                long ID_tbl_S1_Str = 0;
                long ID_tbl_DA_Str = 0;
                long ID_tbl_Daryaft_Chek_Str = 0;
                decimal J_DA_K = 0;
                decimal J_DA_H = 0;
                decimal J_DA_Chek = 0;
                decimal? J_DA = 0;
                foreach (var item in kartkhan)
                    J_DA_K += item.Mablag_DA_K;
                foreach (var item in haveleha)
                    J_DA_H += item.Mablag_DA_H;
                foreach (var item in checks)
                    J_DA_Chek += item.Mablagh_Chek;

                if (tbl_DA.ID_tbl_Sandog == null)
                    J_DA = J_DA_K + J_DA_H + J_DA_Chek;
                else
                    J_DA = J_DA_K + J_DA_H + J_DA_Chek + tbl_DA.M_to_S;

                //Console.WriteLine("J_DA:"+J_DA.ToString());
                //Console.WriteLine("J_DA_K:"+J_DA_K.ToString());
                //Console.WriteLine("J_DA_H:"+J_DA_H.ToString());
                //Console.WriteLine("J_DA_Chek:"+J_DA_Chek.ToString());
                //Console.WriteLine("tbl_DA.M_to_S:"+tbl_DA.M_to_S.ToString());

                #region Fetching ID_tbl_DA
                using (SqlCommand command = new("Apk_Proc_Create_ID_tbl_DA", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transaction;
                    command.Parameters.AddWithValue("@ID_tbl_SalMaly", tbl_DA.ID_tbl_SalMaly);

                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            ID_tbl_DA_Str = (long)(decimal)reader["ID_tbl_DA"];
                        }
                    }
                }
                #endregion

                #region Fetching ID_tbl_S1
                using (SqlCommand command = new("Apk_Proc_Create_ID_tbl_S1", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transaction;
                    command.Parameters.AddWithValue("@ID_tbl_SalMaly", tbl_DA.ID_tbl_SalMaly);

                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            ID_tbl_S1_Str = (long)(decimal)reader["ID_tbl_S1"];
                        }
                    }
                }
                #endregion

                #region Add tbl_DA
                using (SqlCommand command = new("Apk_Proc_Add_tbl_DA", connection))
                {
                Console.WriteLine("tbl_DA.Dc_DA:" + tbl_DA.Dc_DA);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transaction;

                    command.Parameters.AddWithValue("@ID_tbl_DA", ID_tbl_DA_Str);
                    command.Parameters.AddWithValue("@ID_tbl_TarafHesab", tbl_DA.ID_tbl_TarafHesab);
                    command.Parameters.AddWithValue("@Typ_DA", tbl_DA.Typ_DA);
                    if (tbl_DA.Typ_DA== "Typ_DA_MSH")
                        command.Parameters.AddWithValue("@Code_HsMt_S2_DA", "110301");
                    if (tbl_DA.Typ_DA == "Typ_DA_TA")
                        command.Parameters.AddWithValue("@Code_HsMt_S2_DA", "210101");
                    if (tbl_DA.Typ_DA == "Typ_DA_KA")
                        command.Parameters.AddWithValue("@Code_HsMt_S2_DA", "210209");
                    if (tbl_DA.Typ_DA == "Typ_DA_TAN_Riy")
                        command.Parameters.AddWithValue("@Code_HsMt_S2_DA", "110105");

                    command.Parameters.AddWithValue("@Type_Tafsir_In_Sayer","" );
                    command.Parameters.AddWithValue("@Dt_DA", tbl_DA.Dt_DA);
                    command.Parameters.AddWithValue("@Dt_C_DA", tbl_DA.Dt_DA);
                    command.Parameters.AddWithValue("@Tm_C_DA", tbl_DA.Tm_C_DA);
                    command.Parameters.AddWithValue("@Dc_DA", tbl_DA.Dc_DA);
                    command.Parameters.AddWithValue("@ID_tbl_Arz", 0);
                    command.Parameters.AddWithValue("@Mbg_Arz", 0);

                    if (tbl_DA.ID_tbl_Sandog != null && tbl_DA.ID_tbl_Sandog > 0 && tbl_DA.M_to_S > 0)
                    {
                        command.Parameters.AddWithValue("@ID_tbl_Sandog", tbl_DA.ID_tbl_Sandog);
                        command.Parameters.AddWithValue("@M_to_S", tbl_DA.M_to_S);
                        command.Parameters.AddWithValue("@Dat_To_S", "");
                        command.Parameters.AddWithValue("@N_R_S", "");
                        command.Parameters.AddWithValue("@DC_To_S", tbl_DA.DC_To_S);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ID_tbl_Sandog", 0);
                        command.Parameters.AddWithValue("@M_to_S", 0);
                        command.Parameters.AddWithValue("@Dat_To_S", "");
                        command.Parameters.AddWithValue("@N_R_S", "");
                        command.Parameters.AddWithValue("@DC_To_S", "");
                    }
                    command.Parameters.AddWithValue("@J_DA_Ch", J_DA_Chek);
                    command.Parameters.AddWithValue("@J_DA_K", J_DA_K);
                    command.Parameters.AddWithValue("@J_DA_H", J_DA_H);
                    command.Parameters.AddWithValue("@J_DA_Sayer", 0);
                    command.Parameters.AddWithValue("@J_DA", J_DA);

                    command.Parameters.AddWithValue("@Use_AvalD", false);
                    command.Parameters.AddWithValue("@C_B_frm", "BG_D");//Barge_Daryaft
                    command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
                    command.Parameters.AddWithValue("@Trans_Frm_Old_DA", false);
                    command.Parameters.AddWithValue("@ID_tbl_Users", tbl_DA.ID_tbl_Users);
                    command.Parameters.AddWithValue("@ID_tbl_SalMaly", tbl_DA.ID_tbl_SalMaly);

                    var result = await command.ExecuteNonQueryAsync(cancellationToken);
                }
                #endregion

                #region Adding to table S1
                using (SqlCommand command = new("Apk_Proc_Add_tbl_S1", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transaction;
                    command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
                    command.Parameters.AddWithValue("@Date_S1", tbl_DA.Dt_DA);
                    command.Parameters.AddWithValue("@Time_S1", tbl_DA.Tm_C_DA);
                    command.Parameters.AddWithValue("@Desc_S1", " برگه دریافت به شماره " + ID_tbl_DA_Str + " - به تاریخ " + tbl_DA.Dt_DA);
                    command.Parameters.AddWithValue("@Type_S1", "سایر سیستم ها");
                    command.Parameters.AddWithValue("@Type_ES1", "Sayer");
                    command.Parameters.AddWithValue("@Vazyat_S1", "موقت");
                    command.Parameters.AddWithValue("@J_S1", J_DA);
                    command.Parameters.AddWithValue("@Date_C_S1", tbl_DA.Dt_DA);
                    command.Parameters.AddWithValue("@Time_C_S1", tbl_DA.Tm_C_DA);
                    command.Parameters.AddWithValue("@ID_tbl_Users", tbl_DA.ID_tbl_Users);
                    command.Parameters.AddWithValue("@ID_tbl_SalMaly", tbl_DA.ID_tbl_SalMaly);

                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
                #endregion

                #region Add_Sandog
                if (tbl_DA.ID_tbl_Sandog != null && tbl_DA.ID_tbl_Sandog > 0 && tbl_DA.M_to_S > 0)
                {
                    using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Transaction = transaction;

                        using (SqlCommand command_Sandog = new(@"select Name_Sandog,Code_Hesab_Sarfasl_Moein_Sandog,Code_Hesab_Sarfasl_Tafsil_Sandog,Name_Hesab_Sarfasl from tbl_Sandog
                                                                        inner join tbl_Sarfasl on tbl_Sandog.Code_Hesab_Sarfasl_Moein_Sandog=tbl_Sarfasl.Code_Hesab_Sarfasl
                                                                        where ID_tbl_Sandog=" + tbl_DA.ID_tbl_Sandog, connection))
                        {
                            command_Sandog.Transaction = transaction;
                            using (var reader_Sandog = await command_Sandog.ExecuteReaderAsync(cancellationToken))
                            {
                                while (await reader_Sandog.ReadAsync(cancellationToken))
                                {
                                    command.Parameters.AddWithValue("@Name_Hst_S2", (reader_Sandog["Name_Sandog"]).ToString());
                                    command.Parameters.AddWithValue("@Code_Hst_S2", (reader_Sandog["Code_Hesab_Sarfasl_Tafsil_Sandog"]).ToString());
                                    command.Parameters.AddWithValue("@Name_HsMt_S2", (reader_Sandog["Name_Hesab_Sarfasl"]).ToString());
                                    command.Parameters.AddWithValue("@Code_HsMt_S2", (reader_Sandog["Code_Hesab_Sarfasl_Moein_Sandog"]).ToString());
                                    command.Parameters.AddWithValue("@Code_HsKt_S2", (reader_Sandog["Code_Hesab_Sarfasl_Moein_Sandog"]).ToString().Substring(0, 4));
                                    command.Parameters.AddWithValue("@Code_HsGt_S2", (reader_Sandog["Code_Hesab_Sarfasl_Moein_Sandog"]).ToString().Substring(0, 2));
                                }
                            }
                        }
                        command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", "tbl_Sandog");
                        command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", tbl_DA.ID_tbl_Sandog);
                        command.Parameters.AddWithValue("@Desc_S2", " واریز به صندوق طی شماره دریافت " + ID_tbl_DA_Str);
                        command.Parameters.AddWithValue("@Bedehkar_S2", tbl_DA.M_to_S);
                        command.Parameters.AddWithValue("@Bestankar_S2", 0);
                        command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz1", 0);
                        command.Parameters.AddWithValue("@Tmbg", 0);
                        command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
                        command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
                        command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم دریافت وجه");
                        command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_DA");
                        command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_DA_Str);
                        command.Parameters.AddWithValue("@ID_tbl_SalMaly", tbl_DA.ID_tbl_SalMaly);

                        var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                }
                #endregion

                #region Add_Kartkhan
                foreach (var Kartkhan_items in kartkhan)
                {
                    using (SqlCommand command = new("Apk_Proc_Add_tbl_DA_K", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@Dat_DA_K", "");
                        command.Parameters.AddWithValue("@ID_tbl_Hesab", Kartkhan_items.ID_tbl_Hesab);
                        command.Parameters.AddWithValue("@Mablag_DA_K", Kartkhan_items.Mablag_DA_K);
                        command.Parameters.AddWithValue("@DC_DA_K", Kartkhan_items.DC_DA_K);
                        command.Parameters.AddWithValue("@Peygiry_Nu", Kartkhan_items.Peygiry_Nu);
                        command.Parameters.AddWithValue("@Traction_Nu", Kartkhan_items.Traction_Nu);
                        command.Parameters.AddWithValue("@ID_tbl_DA", ID_tbl_DA_Str);
                        command.Parameters.AddWithValue("@ID_tbl_SalMaly", tbl_DA.ID_tbl_SalMaly);

                        var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                    //////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////
                    using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Transaction = transaction;

                        using (SqlCommand command_Kart = new(@"select Number_Hesab,Code_Hesab_Sarfasl_Tafsil_Hesab,Code_Hesab_Sarfasl,Name_Hesab_Sarfasl from tbl_Hesab
                                                               inner join tbl_Sarfasl on tbl_Hesab.Parent_ID_tbl_Sarfasl=tbl_Sarfasl.ID_tbl_Sarfasl
                                                               where ID_tbl_Hesab=" + Kartkhan_items.ID_tbl_Hesab, connection))
                        {
                            command_Kart.Transaction = transaction;
                            using (var reader_Kart = await command_Kart.ExecuteReaderAsync(cancellationToken))
                            {
                                while (await reader_Kart.ReadAsync(cancellationToken))
                                {
                                    command.Parameters.AddWithValue("@Name_Hst_S2", (reader_Kart["Number_Hesab"]).ToString());
                                    command.Parameters.AddWithValue("@Code_Hst_S2", (reader_Kart["Code_Hesab_Sarfasl_Tafsil_Hesab"]).ToString());
                                    command.Parameters.AddWithValue("@Name_HsMt_S2", (reader_Kart["Name_Hesab_Sarfasl"]).ToString());
                                    command.Parameters.AddWithValue("@Code_HsMt_S2", (reader_Kart["Code_Hesab_Sarfasl"]).ToString());
                                    command.Parameters.AddWithValue("@Code_HsKt_S2", (reader_Kart["Code_Hesab_Sarfasl"]).ToString().Substring(0, 4));
                                    command.Parameters.AddWithValue("@Code_HsGt_S2", (reader_Kart["Code_Hesab_Sarfasl"]).ToString().Substring(0, 2));
                                }
                            }
                        }
                        command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", "tbl_Hesab");
                        command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", Kartkhan_items.ID_tbl_Hesab);
                        command.Parameters.AddWithValue("@Desc_S2", " واریز به حساب بانکی از طریق کارتخوان طی شماره دریافت " + ID_tbl_DA_Str);
                        command.Parameters.AddWithValue("@Bedehkar_S2", Kartkhan_items.Mablag_DA_K);
                        command.Parameters.AddWithValue("@Bestankar_S2", 0);
                        command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz1", 0);
                        command.Parameters.AddWithValue("@Tmbg", 0);
                        command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
                        command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
                        command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم دریافت وجه");
                        command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_DA");
                        command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_DA_Str);
                        command.Parameters.AddWithValue("@ID_tbl_SalMaly", tbl_DA.ID_tbl_SalMaly);

                        var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                }
                #endregion

                #region Add_Havale
                foreach (var Havale_items in haveleha)
                {
                    using (SqlCommand command = new("Apk_Proc_Add_tbl_DA_H", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@N_H", Havale_items.N_H);
                        command.Parameters.AddWithValue("@Date_H", Havale_items.Date_H);
                        command.Parameters.AddWithValue("@Mablag_DA_H", Havale_items.Mablag_DA_H);
                        command.Parameters.AddWithValue("@Mablag_DA_H_Az", 0);
                        command.Parameters.AddWithValue("@ID_tbl_Hesab", Havale_items.ID_tbl_Hesab);
                        command.Parameters.AddWithValue("@DC_DA_H", Havale_items.DC_DA_H);
                        command.Parameters.AddWithValue("@ID_tbl_DA", ID_tbl_DA_Str);
                        command.Parameters.AddWithValue("@ID_tbl_SalMaly", tbl_DA.ID_tbl_SalMaly);

                        var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                    //////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////
                    using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Transaction = transaction;

                        using (SqlCommand command_Havale = new(@"select Number_Hesab,Code_Hesab_Sarfasl_Tafsil_Hesab,Code_Hesab_Sarfasl,Name_Hesab_Sarfasl from tbl_Hesab
                                                               inner join tbl_Sarfasl on tbl_Hesab.Parent_ID_tbl_Sarfasl=tbl_Sarfasl.ID_tbl_Sarfasl
                                                               where ID_tbl_Hesab=" + Havale_items.ID_tbl_Hesab, connection))
                        {
                            command_Havale.Transaction = transaction;
                            using (var reader_Havale = await command_Havale.ExecuteReaderAsync(cancellationToken))
                            {
                                while (await reader_Havale.ReadAsync(cancellationToken))
                                {
                                    command.Parameters.AddWithValue("@Name_Hst_S2", (reader_Havale["Number_Hesab"]).ToString());
                                    command.Parameters.AddWithValue("@Code_Hst_S2", (reader_Havale["Code_Hesab_Sarfasl_Tafsil_Hesab"]).ToString());
                                    command.Parameters.AddWithValue("@Name_HsMt_S2", (reader_Havale["Name_Hesab_Sarfasl"]).ToString());
                                    command.Parameters.AddWithValue("@Code_HsMt_S2", (reader_Havale["Code_Hesab_Sarfasl"]).ToString());
                                    command.Parameters.AddWithValue("@Code_HsKt_S2", (reader_Havale["Code_Hesab_Sarfasl"]).ToString().Substring(0, 4));
                                    command.Parameters.AddWithValue("@Code_HsGt_S2", (reader_Havale["Code_Hesab_Sarfasl"]).ToString().Substring(0, 2));
                                }
                            }
                        }
                        command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", "tbl_Hesab");
                        command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", Havale_items.ID_tbl_Hesab);
                        command.Parameters.AddWithValue("@Desc_S2", " واریز به حساب بانکی از طریق حواله طی شماره دریافت " + ID_tbl_DA_Str);
                        command.Parameters.AddWithValue("@Bedehkar_S2", Havale_items.Mablag_DA_H);
                        command.Parameters.AddWithValue("@Bestankar_S2", 0);
                        command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz1", 0);
                        command.Parameters.AddWithValue("@Tmbg", 0);
                        command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
                        command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
                        command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم دریافت وجه");
                        command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_DA");
                        command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_DA_Str);
                        command.Parameters.AddWithValue("@ID_tbl_SalMaly", tbl_DA.ID_tbl_SalMaly);

                        var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                }
                #endregion

                #region Add_TarafHesab
                if (J_DA - J_DA_Chek > 0)
                {
                    using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Transaction = transaction;

                        string Code_Str = "";
                        string Name_Str = "";
                        string Query_Str = @"select Code_TarafHesab,Name_TarafHesab 
                                             from tbl_TarafHesab 
		                                     where ID_tbl_TarafHesab=" + tbl_DA.ID_tbl_TarafHesab;
                        if (tbl_DA.Typ_DA == "Typ_DA_MSH")
                        {
                            Code_Str = "110301";
                            Name_Str = "tbl_TarafHesab";
                        }
                        if (tbl_DA.Typ_DA == "Typ_DA_TA")
                        {
                            Code_Str = "210101";
                            Name_Str = "tbl_TarafHesab";
                        }
                        if (tbl_DA.Typ_DA == "Typ_DA_KA")
                        {
                            Code_Str = "210209";
                            Name_Str = "tbl_TarafHesab";
                        }
                        if (tbl_DA.Typ_DA == "Typ_DA_TAN_Riy")
                        {
                            Code_Str = "110105";
                            Name_Str = "tbl_Tankhah";
                            Query_Str = @"select Code_Hesab_Sarfasl_Tafsil_Tankhah as Code_TarafHesab,Name_TarafHesab 
                                          from tbl_Tankhah 
                                          inner join tbl_TarafHesab on tbl_Tankhah.ID_tbl_TarafHesab=tbl_TarafHesab.ID_tbl_TarafHesab
		                                  where tbl_Tankhah.ID_tbl_TarafHesab=" + tbl_DA.ID_tbl_TarafHesab;
                        }
                        using (SqlCommand command_TarafHesab = new(Query_Str, connection))
                        {
                            command_TarafHesab.Transaction = transaction;
                            using (var reader_TarafHesab = await command_TarafHesab.ExecuteReaderAsync(cancellationToken))
                            {
                                while (await reader_TarafHesab.ReadAsync(cancellationToken))
                                {
                                    command.Parameters.AddWithValue("@Name_Hst_S2", (reader_TarafHesab["Name_TarafHesab"]).ToString());
                                    command.Parameters.AddWithValue("@Code_Hst_S2", (reader_TarafHesab["Code_TarafHesab"]).ToString());
                                }
                            }
                        }
                        using (SqlCommand command_Hesab = new(@"select ID_tbl_Sarfasl,Name_Hesab_Sarfasl,Code_Hesab_Sarfasl
		                                                        from tbl_Sarfasl 
		                                                        where Code_Hesab_Sarfasl=N'" + Code_Str + "'", connection))
                        {
                            command_Hesab.Transaction = transaction;
                            using (var reader_Hesab = await command_Hesab.ExecuteReaderAsync(cancellationToken))
                            {
                                while (await reader_Hesab.ReadAsync(cancellationToken))
                                {
                                    command.Parameters.AddWithValue("@Name_HsMt_S2", (reader_Hesab["Name_Hesab_Sarfasl"]).ToString());
                                    command.Parameters.AddWithValue("@Code_HsMt_S2", (reader_Hesab["Code_Hesab_Sarfasl"]).ToString());
                                    command.Parameters.AddWithValue("@Code_HsKt_S2", (reader_Hesab["Code_Hesab_Sarfasl"]).ToString().Substring(0, 4));
                                    command.Parameters.AddWithValue("@Code_HsGt_S2", (reader_Hesab["Code_Hesab_Sarfasl"]).ToString().Substring(0, 2));
                                }
                            }
                        }
                        command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", Name_Str);
                        command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", tbl_DA.ID_tbl_TarafHesab);
                        command.Parameters.AddWithValue("@Desc_S2", " دریافت از طرفحساب طی شماره دریافت " + ID_tbl_DA_Str);
                        command.Parameters.AddWithValue("@Bedehkar_S2", 0);
                        command.Parameters.AddWithValue("@Bestankar_S2", J_DA - J_DA_Chek);
                        command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz1", 0);
                        command.Parameters.AddWithValue("@Tmbg", 0);
                        command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
                        command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
                        command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم دریافت وجه");
                        command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_DA");
                        command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_DA_Str);
                        command.Parameters.AddWithValue("@ID_tbl_SalMaly", tbl_DA.ID_tbl_SalMaly);

                        var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                }
                #endregion

                #region Add_Check
                foreach (var Check_items in checks)
                {
                    using (SqlCommand command = new("Apk_Proc_Add_tbl_Daryaft_Chek", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@ID_tbl_TarafHesab", tbl_DA.ID_tbl_TarafHesab);
                        command.Parameters.AddWithValue("@Shomare_Chek", Check_items.Shomare_Chek);
                        command.Parameters.AddWithValue("@Mablagh_Chek", Check_items.Mablagh_Chek);
                        command.Parameters.AddWithValue("@Serial_Chek", "");
                        command.Parameters.AddWithValue("@Dt_D_Chek", tbl_DA.Dt_DA);
                        command.Parameters.AddWithValue("@Dt_S_Chek", Check_items.Dt_S_Chek);
                        command.Parameters.AddWithValue("@Number_Hesab_Chek", "");
                        command.Parameters.AddWithValue("@PN", 0);
                        command.Parameters.AddWithValue("@ID_tbl_Bank", 1);
                        command.Parameters.AddWithValue("@ID_tbl_BanchBank", 1);
                        command.Parameters.AddWithValue("@Desc_Chek", "");
                        command.Parameters.AddWithValue("@L_rdoPersian_V_Ch", "پاس نشده");
                        command.Parameters.AddWithValue("@Is_Final_Reg", true);
                        command.Parameters.AddWithValue("@Trans_Frm_Old_DCh", false);
                        command.Parameters.AddWithValue("@Is_Khrj_Ch", false);
                        command.Parameters.AddWithValue("@ID_tbl_Users", tbl_DA.ID_tbl_Users);
                        command.Parameters.AddWithValue("@ID_tbl_DA", ID_tbl_DA_Str);
                        command.Parameters.AddWithValue("@ID_tbl_SalMaly", tbl_DA.ID_tbl_SalMaly);

                        ID_tbl_Daryaft_Chek_Str = (long)(decimal)await command.ExecuteScalarAsync(cancellationToken);
                    }
                    //////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////
                    using (SqlCommand command = new("Apk_Proc_Add_tbl_V_Ch", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Transaction = transaction;

                        command.Parameters.AddWithValue("@ID_tbl_Daryaft_Chek", ID_tbl_Daryaft_Chek_Str);
                        command.Parameters.AddWithValue("@Code_Hesab_Sarfasl_Tafsil_Daryaft_Chek","110302001");
                        command.Parameters.AddWithValue("@Code_Hesab_Sarfasl_Moein_Daryaft_Chek", "110302");
                        command.Parameters.AddWithValue("@rdoEnglish_V_Ch", "rdoPas_Nashode");
                        command.Parameters.AddWithValue("@rdoPersian_V_Ch", "پاس نشده");
                        command.Parameters.AddWithValue("@Date_V_Ch", tbl_DA.Dt_DA);
                        command.Parameters.AddWithValue("@Time_V_Ch", tbl_DA.Tm_C_DA);
                        command.Parameters.AddWithValue("@ID_tbl_Users", tbl_DA.ID_tbl_Users);
                        command.Parameters.AddWithValue("@Code_Hesab_Sarfasl_Tafsil_Kh_Chek", "");
                        command.Parameters.AddWithValue("@Name_Hesab_Sarfasl_Tafsil_Kh_Chek", "");
                        command.Parameters.AddWithValue("@Shomare_Vagozari_Kh_Chek", "");
                        command.Parameters.AddWithValue("@Use_AvalD_V_Ch", true);
                        command.Parameters.AddWithValue("@Trans_Frm_Old", false);
                        command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
                        var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                    //////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////
                    using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Transaction = transaction;

                        using (SqlCommand command_Hesab = new(@"select  tbl_Sarfasl.ID_tbl_Sarfasl,
		                                                                tbl_Sarfasl.Name_Hesab_Sarfasl,
		                                                                tbl_Sarfasl.Code_Hesab_Sarfasl,
				                                                        xx.Name_Hesab_Sarfasl as Name_Hesab_Sarfasl_Moein,
				                                                        xx.Code_Hesab_Sarfasl as Code_Hesab_Sarfasl_Moein
		                                                        from tbl_Sarfasl 
		                                                        inner join tbl_Sarfasl as xx on tbl_Sarfasl.Code_Parent_Sarfasl=xx.ID_tbl_Sarfasl
		                                                        where tbl_Sarfasl.Code_Hesab_Sarfasl=N'110302001'", connection))
                        {
                            command_Hesab.Transaction = transaction;
                            using (var reader_Hesab = await command_Hesab.ExecuteReaderAsync(cancellationToken))
                            {
                                while (await reader_Hesab.ReadAsync(cancellationToken))
                                {
                                    command.Parameters.AddWithValue("@Name_Hst_S2", (reader_Hesab["Name_Hesab_Sarfasl"]).ToString());
                                    command.Parameters.AddWithValue("@Code_Hst_S2", (reader_Hesab["Code_Hesab_Sarfasl"]).ToString());
                                    command.Parameters.AddWithValue("@Name_HsMt_S2", (reader_Hesab["Name_Hesab_Sarfasl_Moein"]).ToString());
                                    command.Parameters.AddWithValue("@Code_HsMt_S2", (reader_Hesab["Code_Hesab_Sarfasl_Moein"]).ToString());
                                    command.Parameters.AddWithValue("@Code_HsKt_S2", (reader_Hesab["Code_Hesab_Sarfasl_Moein"]).ToString().Substring(0, 4));
                                    command.Parameters.AddWithValue("@Code_HsGt_S2", (reader_Hesab["Code_Hesab_Sarfasl_Moein"]).ToString().Substring(0, 2));
                                    command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", "tbl_Sarfasl");
                                    command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", long.Parse((reader_Hesab["ID_tbl_Sarfasl"]).ToString()));
                                }
                            }
                        }
                        command.Parameters.AddWithValue("@Desc_S2", " بابت اسناد دریافتنی نزد صندوق به شماره چک " + Check_items.Shomare_Chek);
                        command.Parameters.AddWithValue("@Bedehkar_S2", Check_items.Mablagh_Chek);
                        command.Parameters.AddWithValue("@Bestankar_S2", 0);
                        command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz1", 0);
                        command.Parameters.AddWithValue("@Tmbg", 0);
                        command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
                        command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
                        command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم دریافت وجه");
                        command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_DA");
                        command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_DA_Str);
                        command.Parameters.AddWithValue("@ID_tbl_SalMaly", tbl_DA.ID_tbl_SalMaly);

                        var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                    //////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////
                    using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Transaction = transaction;

                        string Code_Str = "";
                        string Name_Str = "";
                        string Query_Str = @"select Code_TarafHesab,Name_TarafHesab 
                                             from tbl_TarafHesab 
		                                     where ID_tbl_TarafHesab=" + tbl_DA.ID_tbl_TarafHesab;
                        if (tbl_DA.Typ_DA == "Typ_DA_MSH")
                        {
                            Code_Str = "110301";
                            Name_Str = "tbl_TarafHesab";
                        }
                        if (tbl_DA.Typ_DA == "Typ_DA_TA")
                        {
                            Code_Str = "210101";
                            Name_Str = "tbl_TarafHesab";
                        }
                        if (tbl_DA.Typ_DA == "Typ_DA_KA")
                        {
                            Code_Str = "210209";
                            Name_Str = "tbl_TarafHesab";
                        }
                        if (tbl_DA.Typ_DA == "Typ_DA_TAN_Riy")
                        {
                            Code_Str = "110105";
                            Name_Str = "tbl_Tankhah";
                            Query_Str = @"select Code_Hesab_Sarfasl_Tafsil_Tankhah as Code_TarafHesab,Name_TarafHesab 
                                          from tbl_Tankhah 
                                          inner join tbl_TarafHesab on tbl_Tankhah.ID_tbl_TarafHesab=tbl_TarafHesab.ID_tbl_TarafHesab
		                                  where tbl_Tankhah.ID_tbl_TarafHesab=" + tbl_DA.ID_tbl_TarafHesab;
                        }
                        using (SqlCommand command_TarafHesab = new(Query_Str, connection))
                        {
                            command_TarafHesab.Transaction = transaction;
                            using (var reader_TarafHesab = await command_TarafHesab.ExecuteReaderAsync(cancellationToken))
                            {
                                while (await reader_TarafHesab.ReadAsync(cancellationToken))
                                {
                                    command.Parameters.AddWithValue("@Name_Hst_S2", (reader_TarafHesab["Name_TarafHesab"]).ToString());
                                    command.Parameters.AddWithValue("@Code_Hst_S2", (reader_TarafHesab["Code_TarafHesab"]).ToString());
                                }
                            }
                        }
                        using (SqlCommand command_Hesab = new(@"select  ID_tbl_Sarfasl,Name_Hesab_Sarfasl,Code_Hesab_Sarfasl
		                                                        from tbl_Sarfasl 
		                                                        where Code_Hesab_Sarfasl=N'" + Code_Str + "'", connection))
                        {
                            command_Hesab.Transaction = transaction;
                            using (var reader_Hesab = await command_Hesab.ExecuteReaderAsync(cancellationToken))
                            {
                                while (await reader_Hesab.ReadAsync(cancellationToken))
                                {
                                    command.Parameters.AddWithValue("@Name_HsMt_S2", (reader_Hesab["Name_Hesab_Sarfasl"]).ToString());
                                    command.Parameters.AddWithValue("@Code_HsMt_S2", (reader_Hesab["Code_Hesab_Sarfasl"]).ToString());
                                    command.Parameters.AddWithValue("@Code_HsKt_S2", (reader_Hesab["Code_Hesab_Sarfasl"]).ToString().Substring(0, 4));
                                    command.Parameters.AddWithValue("@Code_HsGt_S2", (reader_Hesab["Code_Hesab_Sarfasl"]).ToString().Substring(0, 2));
                                }
                            }
                        }
                        command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", Name_Str);
                        command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", tbl_DA.ID_tbl_TarafHesab);
                        command.Parameters.AddWithValue("@Desc_S2", " بابت اسناد دریافتنی نزد صندوق به شماره چک " + Check_items.Shomare_Chek);
                        command.Parameters.AddWithValue("@Bedehkar_S2", 0);
                        command.Parameters.AddWithValue("@Bestankar_S2", Check_items.Mablagh_Chek);
                        command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz1", 0);
                        command.Parameters.AddWithValue("@Tmbg", 0);
                        command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
                        command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
                        command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
                        command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم دریافت وجه");
                        command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_DA");
                        command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_DA_Str);
                        command.Parameters.AddWithValue("@ID_tbl_SalMaly", tbl_DA.ID_tbl_SalMaly);

                        var result = await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                }
                #endregion
                transaction.Commit();
                return new(true);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex.Message);
                return new(false, false, new List<string> { ex.Message });
            }
        }
    }

    public async Task<IEnumerable<tbl_Hesab>> BankAccounts(CancellationToken cancellationToken = default)
    {
        return await _dbManager.CallProcedureAsync<tbl_Hesab>("Apk_Proc_Get_tbl_Hesab", cancellationToken);
    }

    public async Task<IEnumerable<tbl_Sandog>> CashDesks(CancellationToken cancellationToken = default)
    {
        return await _dbManager.CallProcedureAsync<tbl_Sandog>("Apk_Proc_Get_tbl_Sandog", cancellationToken);
    }



	public async Task<List<ReceiptSheet>> GetAllReceiptSheetsCount(int fiscalYear, int UserId, string Date1, string Date2, CancellationToken cancellationToken = default)
	{
		IEnumerable<ReceiptSheet> Factor = await _dbManager.CallProcedureWithParametersAsync<ReceiptSheet>("Apk_Proc_Get_List_tbl_DA", new
		{
			ID_tbl_SalMaly = fiscalYear,
			ID_tbl_Users = UserId,
			@Dt_F1 = Date1,
			@Dt_F2 = Date2,
		});
		return new(Factor);
	}


	public async Task<List<ReceiptSheet>> GetReceiptSheetPrint(long? Id_tbl_DA, long? fiscalYear, int? UserId, int? Offset, string? Date1, string? Date2, object parameters = null!, CancellationToken cancellationToken = default)
	{
		IEnumerable<ReceiptSheet> Factor = await _dbManager.CallProcedureWithParametersAsync<ReceiptSheet>("Apk_Proc_Report_Daryaft", new
		{
			Type_Call_Proc = "Print_Daryaft",
			ID_tbl_SalMaly = fiscalYear,
			ID_tbl_DA = Id_tbl_DA,
			ID_tbl_Users = UserId,
			Date1 = Date1,
			Date2 = Date2,
			Offset = Offset,

		});
		return new(Factor);
	}
}
