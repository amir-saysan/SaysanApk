using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SaysanPwa.Application.Utilities.DateAndTime;
using SaysanPwa.Domain.AggregateModels.Factor;
using SaysanPwa.Domain.AggregateModels.FiscalYear;
using SaysanPwa.Domain.AggregateModels.ProductAggregate;
using SaysanPwa.Domain.AggregateModels.UserAggregate;
using SaysanPwa.Domain.SeedWorker;
using SqlServerWrapper.Wrapper.DatabaseManager;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SaysanPwa.Infrastructure.Repositories;
public class FactorRepository : IFactorRepository
{
	private readonly IConfiguration _configuration;
	private readonly IDbManager _dbManager;

	public FactorRepository(IConfiguration configuration, IDbManager dbManager)
	{
		_configuration = configuration;
		_dbManager = dbManager;
	}
	public static T ConvertFromDBVal<T>(object obj)
	{
		if (obj == null || obj == DBNull.Value)
		{
			return default(T); // returns the default value for the type
		}
		else
		{
			return (T)obj;
		}
	}

	struct struct_FactorItem
	{
		public long ID_tbl_Kala;
		public int ID_tbl_Anbar;
		public decimal Mablagh_Koll_Gabl_Az_Takhfif;
		public decimal Mablagh_Koll_Pas_Az_Takhfif;
		public decimal Mablagh_Bahaye_Tamam_Shode;
		public decimal Fi_Sadere;
		public decimal Mablagh_Sadere;
		public decimal Mnd_Fi;
		public decimal Mnd_Megdar;
		public decimal Mnd_Mablagh;
		public bool Mogayese_Shode_Ast;
	}
	public async Task<SysResult<IEnumerable<GetPartnersForFactorViewModel>>> GetPartnerAndBranches(object parameters = null!, CancellationToken cancellationToken = default)
	{
		IEnumerable<GetPartnersForFactorViewModel> partnersAndBranches = Enumerable.Empty<GetPartnersForFactorViewModel>();
		if (parameters == null)
		{
			partnersAndBranches = await _dbManager.CallProcedureAsync<GetPartnersForFactorViewModel>("Apk_Proc_Get_Branch_And_Partner_For_Factor", cancellationToken);
		}
		else
		{
			partnersAndBranches = await _dbManager.CallProcedureWithParametersAsync<GetPartnersForFactorViewModel>("Apk_Proc_Get_Branch_And_Partner_For_Factor",
				parameters, cancellationToken);
		}

		return new(partnersAndBranches);
	}
	public async Task<SysResult<bool>> AddPreFactor(PreFactor preFactor, IEnumerable<FactorItem> factorItems, CancellationToken cancellationToken = default)
	{
		using (SqlConnection connection = new(_configuration.GetConnectionString("Default")))
		{
			await connection.OpenAsync();
			SqlTransaction transaction = connection.BeginTransaction();
			try
			{
				long factorId = 0;

				using (SqlCommand command = new SqlCommand("Apk_Proc_Add_tbl_PF", connection))
				{
					command.Transaction = transaction;
					command.CommandType = CommandType.StoredProcedure;
					var properties = preFactor.GetType().GetProperties().ToList();
					properties.RemoveAll(b => b.Name == "Notifications");
					//properties.RemoveAll(b => b.Name == "Type_tbl_F");
					properties.RemoveAll(b => b.Name == "ID_tbl_PF");
					foreach (var p in properties)
					{
						command.Parameters.AddWithValue($"@{p.Name}", p.GetValue(preFactor));
					}
					await Console.Out.WriteLineAsync("before execution");
					factorId = (long)(decimal)await command.ExecuteScalarAsync(cancellationToken);
					await Console.Out.WriteLineAsync("after execution");
				}
				foreach (var item in factorItems)
				{
					using (SqlCommand command = new("APK_Proc_Add_tbl_F_Aglm", connection))
					{
						command.Transaction = transaction;
						command.CommandType = CommandType.StoredProcedure;
						var properties = item.GetType().GetProperties().ToList();
						properties.RemoveAll(b => b.Name == "Notifications");
						//properties.RemoveAll(b => b.Name == "ID_tbl_F");
						item.ID_tbl_F = factorId;
						item.Type_tbl_F = "tbl_PF";
						await Console.Out.WriteLineAsync("ID_tbl_F: " + factorId.ToString());
						//command.Parameters.AddWithValue("@ID_tbl_F", factorId);
						foreach (var p in properties)
						{

							if (p.PropertyType.Equals(typeof(decimal)))
							{
								command.Parameters.AddWithValue($"@{p.Name}", (decimal)p.GetValue(item));
							}
							else
							{
								command.Parameters.AddWithValue($"@{p.Name}", p.GetValue(item));
							}
							Console.WriteLine($"@{p.Name}:" + $" {p.GetValue(item)}");
						}
						await Console.Out.WriteLineAsync($"before execution of {item.ID_tbl_Kala}");
						var result = await command.ExecuteNonQueryAsync();
						await Console.Out.WriteLineAsync($"after execution of {item.ID_tbl_Kala}");
					}
				}

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
	public async Task<SysResult<FactorPricing>> GetCustomPriceForProduct(TypeCallProcedure typeCallProcedure, long partnerId, long productId, int fiscalYear, CancellationToken cancellationToken = default)
	{
		var date = DateTime.Now.ToPersianData().Substring(0, 10);
		IEnumerable<FactorPricing> factorPricing = await _dbManager.CallProcedureWithParametersAsync<FactorPricing>("Proc_tbl_Pricing", new
		{
			Type_Call_Proc = typeCallProcedure.ToString(),
			ID_tbl_Pricing = partnerId,
			ID_tbl_Kala = productId,
			ID_tbl_SalMaly = fiscalYear,
			Datee = date
		}, cancellationToken);

		if (factorPricing.Any())
		{
			return new(factorPricing.ToList()[0]);
		}
		else
		{
			return new(new()
			{
				Fi_Ba_Haz = -1,
				Mnd_Fi = -1,
				Mnd_Megdar = -1,
				Price = -1,
				Type_Price = -1
			});
		}
	}
	public async Task<List<Factor>> GetFactors(int fiscalYear, int UserId, string Date1, string Date2, object parameters = null!, CancellationToken cancellationToken = default)
	{
		IEnumerable<Factor> Factor = await _dbManager.CallProcedureWithParametersAsync<Factor>("Apk_Proc_Get_List_tbl_PF", new
		{
			ID_tbl_SalMaly = fiscalYear,
			ID_tbl_Users = UserId,
			@Dt_F1 = Date1,
			@Dt_F2 = Date2,
		});
		return new(Factor);
	}
	public async Task<List<Factor>> GetFactorPrints(long? PishForoshID, long? fiscalYear, int? UserId, int? Offset, int? ID_tbl_Bzy, string? Date1, string? Date2, object parameters = null!, CancellationToken cancellationToken = default)
	{
		IEnumerable<Factor> Factor = await _dbManager.CallProcedureWithParametersAsync<Factor>("Apk_Proc_Report_PishForosh", new
		{
			Type_Call_Proc = "Select_Top_N_Factor",
			ID_tbl_SalMaly = fiscalYear,
			ID_tbl_F = PishForoshID,
			ID_tbl_TarafHesab = UserId,
			Date1 = Date1,
			Date2 = Date2,
			ID_tbl_Bzy = ID_tbl_Bzy,
			Offset = Offset,

		});
		return new(Factor);
	}


	public async Task<SysResult<bool>> AddSaleFactor(SaleFactor saleFactor, IEnumerable<FactorItem> factorItems, CancellationToken cancellationToken = default)
	{
		using (SqlConnection connection = new(_configuration.GetConnectionString("Default")))
		{
			await connection.OpenAsync();
			SqlTransaction transaction = connection.BeginTransaction();
			try
			{
				#region Checking product availibility
				using (SqlCommand command = new("Proc_Mnd_Kala", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;

					decimal salesCount = 0.0M;

					int row = 1;
					foreach (var item in factorItems)
					{
						salesCount = 0.0M;
						foreach (var subItem in factorItems)
						{
							if ((item.ID_tbl_Kala == subItem.ID_tbl_Kala) && (item.ID_tbl_Anbar == subItem.ID_tbl_Anbar))
							{
								salesCount += subItem.Tedad;
							}
						}
						Console.WriteLine(salesCount.ToString());

						command.Parameters.Clear();
						command.Parameters.AddWithValue("@Type_Call_Proc", "One_To_Date");
						command.Parameters.AddWithValue("@ID_tbl_Kala ", item.ID_tbl_Kala);
						command.Parameters.AddWithValue("@ID_tbl_Anbar ", item.ID_tbl_Anbar);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly ", item.ID_tbl_SalMaly);

						string whereString = "'" + saleFactor.FiscalYearBeginDate + "' AND '" + saleFactor.Dt_F + "'";
						command.Parameters.AddWithValue("@where", whereString);

						using (var reader = await command.ExecuteReaderAsync(cancellationToken))
						{
							decimal reminderOfProduct = 0.0M;
							bool allowToNegetive = false;
							while (await reader.ReadAsync(cancellationToken))
							{
								reminderOfProduct = (decimal)reader["Mnd_Kala"];
								allowToNegetive = (bool)reader["Alw_To_Mnus"];
							}

							Console.WriteLine("reminderOfProduct  " + reminderOfProduct.ToString());
							Console.WriteLine("allowToNegetive  " + allowToNegetive.ToString());
							if (salesCount > reminderOfProduct)
							{
								if (allowToNegetive == false)
								{
									return new(false, false, new() { $" موجود کالای ردیف {row} جهت فروش منفی می باشد." });
								}
							}
						}
					}
				}
				#endregion

				SaleFactorHesab saleFactorHesab = new();
				#region Fetch_Accounts_For_Factor
				Console.WriteLine("Before proc_Comp - Sett_F");
				using (SqlCommand command = new("Proc_Comp", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;

					command.Parameters.AddWithValue("@Type_Call_Proc", "Sett_F");
					command.Parameters.AddWithValue("@ID_tbl", 1);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleFactor.ID_tbl_SalMaly);
					command.Parameters.AddWithValue("@Where_Query", "-");

					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync())
						{
							if ((long)reader["ID_tbl_Hesab_F_D"] == 0 ||
								(long)reader["ID_tbl_Hesab_AV_F_D"] == 0 ||
								(long)reader["ID_tbl_Hesab_Bah_F_D"] == 0 ||
								(long)reader["ID_tbl_Hesab_Jayeze_F_D"] == 0 ||
								(long)reader["ID_tbl_Hesab_Mal_F_D"] == 0 ||
								(long)reader["ID_tbl_Hesab_Tkh_F_D"] == 0)
							{
								return new(false, false, new() { "حساب های فروش خدمت تنظیم نشده است." });
							}

							saleFactorHesab.chkGenerate_And_Sanad_Baha_Kala = (bool)reader["chkGenerate_And_Sanad_Baha_Kala"];
							saleFactorHesab.Cd_Moen_F_D = (string)reader["Cd_Moen_F_D"];
							saleFactorHesab.Nam_Moen_F_D = (string)reader["Nam_Moen_F_D"];
							saleFactorHesab.Cd_Tafsl_F_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_F_D"]);
							saleFactorHesab.Nam_Tafsl_F_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_F_D"]);
							saleFactorHesab.Name_tbl_Hesab_F_D = (string)reader["Name_tbl_Hesab_F_D"];
							saleFactorHesab.ID_tbl_Hesab_F_D = (long)reader["ID_tbl_Hesab_F_D"];

							saleFactorHesab.Cd_Moen_Mal_F_D = (string)reader["Cd_Moen_Mal_F_D"];
							saleFactorHesab.Nam_Moen_Mal_F_D = (string)reader["Nam_Moen_Mal_F_D"];
							saleFactorHesab.Cd_Tafsl_Mal_F_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_Mal_F_D"]);
							saleFactorHesab.Nam_Tafsl_Mal_F_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_Mal_F_D"]);
							saleFactorHesab.Name_tbl_Hesab_Mal_F_D = (string)reader["Name_tbl_Hesab_Mal_F_D"];
							saleFactorHesab.ID_tbl_Hesab_Mal_F_D = (long)reader["ID_tbl_Hesab_Mal_F_D"];

							saleFactorHesab.Cd_Moen_AV_F_D = (string)reader["Cd_Moen_AV_F_D"];
							saleFactorHesab.Nam_Moen_AV_F_D = (string)reader["Nam_Moen_AV_F_D"];
							saleFactorHesab.Cd_Tafsl_AV_F_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_AV_F_D"]);
							saleFactorHesab.Nam_Tafsl_AV_F_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_AV_F_D"]);
							saleFactorHesab.Name_tbl_Hesab_AV_F_D = (string)reader["Name_tbl_Hesab_AV_F_D"];
							saleFactorHesab.ID_tbl_Hesab_AV_F_D = (long)reader["ID_tbl_Hesab_AV_F_D"];

							saleFactorHesab.Cd_Moen_Tkh_F_D = (string)reader["Cd_Moen_Tkh_F_D"];
							saleFactorHesab.Nam_Moen_Tkh_F_D = (string)reader["Nam_Moen_Tkh_F_D"];
							saleFactorHesab.Cd_Tafsl_Tkh_F_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_Tkh_F_D"]);
							saleFactorHesab.Nam_Tafsl_Tkh_F_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_Tkh_F_D"]);
							saleFactorHesab.Name_tbl_Hesab_Tkh_F_D = (string)reader["Name_tbl_Hesab_Tkh_F_D"];
							saleFactorHesab.ID_tbl_Hesab_Tkh_F_D = (long)reader["ID_tbl_Hesab_Tkh_F_D"];

							saleFactorHesab.Cd_Moen_Bah_F_D = (string)reader["Cd_Moen_Bah_F_D"];
							saleFactorHesab.Nam_Moen_Bah_F_D = (string)reader["Nam_Moen_Bah_F_D"];
							saleFactorHesab.Cd_Tafsl_Bah_F_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_Bah_F_D"]);
							saleFactorHesab.Nam_Tafsl_Bah_F_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_Bah_F_D"]);
							saleFactorHesab.Name_tbl_Hesab_Bah_F_D = (string)reader["Name_tbl_Hesab_Bah_F_D"];
							saleFactorHesab.ID_tbl_Hesab_Bah_F_D = (long)reader["ID_tbl_Hesab_Bah_F_D"];

							saleFactorHesab.Cd_Moen_Pors_F_D = (string)reader["Cd_Moen_Pors_F_D"];
							saleFactorHesab.Nam_Moen_Pors_F_D = (string)reader["Nam_Moen_Pors_F_D"];
							saleFactorHesab.Cd_Tafsl_Pors_F_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_Pors_F_D"]);
							saleFactorHesab.Nam_Tafsl_Pors_F_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_Pors_F_D"]);
							saleFactorHesab.Name_tbl_Hesab_Pors_F_D = (string)reader["Name_tbl_Hesab_Pors_F_D"];
							saleFactorHesab.ID_tbl_Hesab_Pors_F_D = (long)reader["ID_tbl_Hesab_Pors_F_D"];

							saleFactorHesab.Cd_Moen_Jayeze_F_D = (string)reader["Cd_Moen_Jayeze_F_D"];
							saleFactorHesab.Nam_Moen_Jayeze_F_D = (string)reader["Nam_Moen_Jayeze_F_D"];
							saleFactorHesab.Cd_Tafsl_Jayeze_F_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_Jayeze_F_D"]);
							saleFactorHesab.Nam_Tafsl_Jayeze_F_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_Jayeze_F_D"]);
							saleFactorHesab.Name_tbl_Hesab_Jayeze_F_D = (string)reader["Name_tbl_Hesab_Jayeze_F_D"];
							saleFactorHesab.ID_tbl_Hesab_Jayeze_F_D = (long)reader["ID_tbl_Hesab_Jayeze_F_D"];
						}
					}
				}
				Console.WriteLine("After proc_Comp - Sett_F");
				#endregion

				int i = 0;
				decimal Mablagh = 0;
				decimal Mablagh_Koll = 0;
				decimal Mablagh_Bahaye_Tamam_Shode = 0;
				long ID_tbl_FF_Str = 0;
				long N_F_Str = 0;
				long ID_tbl_S1_Str = 0;
				decimal Profit_Factor = 0;
				string partnerName = string.Empty;
				long partnerCode = 0;
				struct_FactorItem[] struct_FactorItem_Arr = new struct_FactorItem[factorItems.Count()];

				foreach (var item in factorItems)
				{
					struct_FactorItem_Arr[i].ID_tbl_Kala = item.ID_tbl_Kala;
					struct_FactorItem_Arr[i].ID_tbl_Anbar = item.ID_tbl_Anbar;
					struct_FactorItem_Arr[i].Mablagh_Koll_Gabl_Az_Takhfif = item.Tedad * item.Fi_Bed_Haz;
					struct_FactorItem_Arr[i].Mablagh_Koll_Pas_Az_Takhfif = item.Tedad * item.Fi_Ba_Haz;
					struct_FactorItem_Arr[i].Mablagh_Bahaye_Tamam_Shode = 0;
					struct_FactorItem_Arr[i].Fi_Sadere = 0;
					struct_FactorItem_Arr[i].Mablagh_Sadere = 0;
					struct_FactorItem_Arr[i].Mnd_Fi = 0;
					struct_FactorItem_Arr[i].Mnd_Megdar = 0;
					struct_FactorItem_Arr[i].Mnd_Mablagh = 0;
					struct_FactorItem_Arr[i].Mogayese_Shode_Ast = false;
					i++;
				}

				#region Genereate_Baha_Kala
				i = 0;
				Mablagh_Bahaye_Tamam_Shode = 0;
				foreach (var item in factorItems)
				{
					Mablagh_Koll += struct_FactorItem_Arr[i].Mablagh_Koll_Pas_Az_Takhfif;
					using (SqlCommand command = new("Proc_Kala", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						// مقدار دهی مبالغ بهای تمام شده در صورت منفی بودن کالا
						struct_FactorItem_Arr[i].Mablagh_Bahaye_Tamam_Shode = item.Tedad * item.Fi_Ba_Haz;
						struct_FactorItem_Arr[i].Fi_Sadere = item.Fi_Ba_Haz;
						struct_FactorItem_Arr[i].Mablagh_Sadere = item.Tedad * item.Fi_Ba_Haz;
						struct_FactorItem_Arr[i].Mnd_Fi = item.Fi_Ba_Haz;
						struct_FactorItem_Arr[i].Mnd_Megdar = item.Tedad;
						struct_FactorItem_Arr[i].Mnd_Mablagh = (item.Tedad * item.Fi_Ba_Haz);

						item.Fi_Sadere = struct_FactorItem_Arr[i].Fi_Sadere;
						item.Mablagh_Sadere = struct_FactorItem_Arr[i].Mablagh_Sadere;
						item.Mnd_Fi = struct_FactorItem_Arr[i].Mnd_Fi;
						item.Mnd_Megdar = struct_FactorItem_Arr[i].Mnd_Megdar;
						item.Mnd_Mablagh = struct_FactorItem_Arr[i].Mnd_Mablagh;

						Profit_Factor += 0;
						// مقدار دهی مبالغ بهای تمام شده در صورت منفی بودن کالا

						command.Parameters.AddWithValue("@Type_Call_Proc", "Select_Last_Bah_Kala");
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", item.ID_tbl_SalMaly);
						command.Parameters.AddWithValue("@ID_tbl_Kala", item.ID_tbl_Kala);
						command.Parameters.AddWithValue("@ID_tbl_Users", saleFactor.ID_tbl_Users);
						command.Parameters.AddWithValue("@where", "");
						using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
						{
							if (reader.HasRows)
							{
								while (await reader.ReadAsync(cancellationToken))
								{
									struct_FactorItem_Arr[i].Mablagh_Bahaye_Tamam_Shode = item.Tedad * (decimal)reader["Mnd_Fi"];
									struct_FactorItem_Arr[i].Fi_Sadere = (decimal)reader["Mnd_Fi"];
									struct_FactorItem_Arr[i].Mablagh_Sadere = item.Tedad * (decimal)reader["Mnd_Fi"];
									struct_FactorItem_Arr[i].Mnd_Fi = (decimal)reader["Mnd_Fi"];
									struct_FactorItem_Arr[i].Mnd_Megdar = (decimal)reader["Mnd_Megdar"] - item.Tedad;
									struct_FactorItem_Arr[i].Mnd_Mablagh = (decimal)reader["Mnd_Mablagh"] - (item.Tedad * (decimal)reader["Mnd_Fi"]);

									item.Fi_Sadere = struct_FactorItem_Arr[i].Fi_Sadere;
									item.Mablagh_Sadere = struct_FactorItem_Arr[i].Mablagh_Sadere;
									item.Mnd_Fi = struct_FactorItem_Arr[i].Mnd_Fi;
									item.Mnd_Megdar = struct_FactorItem_Arr[i].Mnd_Megdar;
									item.Mnd_Mablagh = struct_FactorItem_Arr[i].Mnd_Mablagh;

									Mablagh_Bahaye_Tamam_Shode += item.Tedad * (decimal)reader["Mnd_Fi"];
									Profit_Factor += struct_FactorItem_Arr[i].Mablagh_Koll_Pas_Az_Takhfif - (item.Tedad * (decimal)reader["Mnd_Fi"]);
								}
							}
							else
							{
								await reader.CloseAsync();
								using (SqlCommand command1 = new("Proc_Kala", connection))
								{
									command1.CommandType = CommandType.StoredProcedure;
									command1.Transaction = transaction;

									command1.Parameters.AddWithValue("@Type_Call_Proc", "Select_Last_Bah_Kala_From_Eftetahye");
									command1.Parameters.AddWithValue("@ID_tbl_SalMaly", item.ID_tbl_SalMaly);
									command1.Parameters.AddWithValue("@ID_tbl_Kala", item.ID_tbl_Kala);
									command1.Parameters.AddWithValue("@ID_tbl_Users", saleFactor.ID_tbl_Users);
									command1.Parameters.AddWithValue("@where", "");
									using (var reader1 = await command1.ExecuteReaderAsync(cancellationToken))
									{
										if (reader1.HasRows)
										{
											while (await reader1.ReadAsync(cancellationToken))
											{
												struct_FactorItem_Arr[i].Mablagh_Bahaye_Tamam_Shode = item.Tedad * (decimal)reader1["Last_Bah_Tam_Kala"];
												struct_FactorItem_Arr[i].Fi_Sadere = (decimal)reader1["Last_Bah_Tam_Kala"];
												struct_FactorItem_Arr[i].Mablagh_Sadere = item.Tedad * (decimal)reader1["Last_Bah_Tam_Kala"];
												struct_FactorItem_Arr[i].Mnd_Fi = (decimal)reader1["Last_Bah_Tam_Kala"];
												struct_FactorItem_Arr[i].Mnd_Megdar = (decimal)reader1["Avl_Td_Kala"] - item.Tedad;
												struct_FactorItem_Arr[i].Mnd_Mablagh = ((decimal)reader1["Avl_Td_Kala"] * (decimal)reader1["Last_Bah_Tam_Kala"]) - (item.Tedad * (decimal)reader1["Last_Bah_Tam_Kala"]);

												item.Fi_Sadere = struct_FactorItem_Arr[i].Fi_Sadere;
												item.Mablagh_Sadere = struct_FactorItem_Arr[i].Mablagh_Sadere;
												item.Mnd_Fi = struct_FactorItem_Arr[i].Mnd_Fi;
												item.Mnd_Megdar = struct_FactorItem_Arr[i].Mnd_Megdar;
												item.Mnd_Mablagh = struct_FactorItem_Arr[i].Mnd_Mablagh;

												Mablagh_Bahaye_Tamam_Shode += item.Tedad * (decimal)reader1["Last_Bah_Tam_Kala"];
												Profit_Factor += struct_FactorItem_Arr[i].Mablagh_Koll_Pas_Az_Takhfif - (item.Tedad * (decimal)reader1["Last_Bah_Tam_Kala"]);
											}
										}
										else
										{
											Mablagh_Bahaye_Tamam_Shode += item.Tedad * item.Fi_Ba_Haz;
										}
									}
								}
							}
						}
					}
					i++;
				}
				#endregion

				#region Adding new sale factor
				using (SqlCommand command = new SqlCommand("Apk_Proc_Add_tbl_FF", connection))
				{
					command.Transaction = transaction;
					command.CommandType = CommandType.StoredProcedure;
					var properties = saleFactor.GetType().GetProperties().ToList();
					properties.RemoveAll(b => b.Name == "Notifications");
					properties.RemoveAll(b => b.Name == "ID_tbl_FF");
					properties.RemoveAll(b => b.Name == "FiscalYearBeginDate");
					properties.RemoveAll(b => b.Name == "FiscalYearEndDate");
					properties.RemoveAll(b => b.Name == "FiscalYearTitle");
					properties.RemoveAll(b => b.Name == "Name_TarafHesab");
					properties.RemoveAll(b => b.Name == "Code_Kala");
					properties.RemoveAll(b => b.Name == "Shenase_Kala");
					properties.RemoveAll(b => b.Name == "Vahede_Name");
					properties.RemoveAll(b => b.Name == "Name_Anbar");
					properties.RemoveAll(b => b.Name == "Code_Anbar");
					properties.RemoveAll(b => b.Name == "Tedad");
					properties.RemoveAll(b => b.Name == "Fi");
					properties.RemoveAll(b => b.Name == "Mablagh");
					properties.RemoveAll(b => b.Name == "Name_Branch");
					properties.RemoveAll(b => b.Name == "ChelPhone_TarafHesab");
					properties.RemoveAll(b => b.Name == "Code_TarafHesab");
					properties.RemoveAll(b => b.Name == "Name_Bzy");
					properties.RemoveAll(b => b.Name == "Name_Kala");
					properties.RemoveAll(b => b.Name == "BarCode_Kala");
					foreach (var p in properties)
					{
						command.Parameters.AddWithValue($"@{p.Name}", p.GetValue(saleFactor));
					}
					//command.Parameters.AddWithValue("@BG_F", saleFactor.J_F);
					ID_tbl_FF_Str = (long)(decimal)await command.ExecuteScalarAsync(cancellationToken);
				}
				foreach (var item in factorItems)
				{
					using (SqlCommand command = new("APK_Proc_Add_tbl_F_Aglm", connection))
					{
						command.Transaction = transaction;
						command.CommandType = CommandType.StoredProcedure;
						var properties = item.GetType().GetProperties().ToList();
						properties.RemoveAll(b => b.Name == "Notifications");
						properties.RemoveAll(b => b.Name == "Type_tbl_F");

						item.ID_tbl_F = ID_tbl_FF_Str;
						item.ID_tbl_SalMaly = saleFactor.ID_tbl_SalMaly;
						command.Parameters.AddWithValue("@Type_tbl_F", "tbl_FF");
						foreach (var p in properties)
						{
							command.Parameters.AddWithValue($"@{p.Name}", p.GetValue(item));
						}

						var result = await command.ExecuteNonQueryAsync();
					}
				}
				#endregion

				#region Fetching Number Inserted Sale Factor
				using (SqlCommand command = new("select N_F from tbl_FF where ID_tbl_FF=" + ID_tbl_FF_Str, connection))
				{
					command.Transaction = transaction;
					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							N_F_Str = (long)reader["N_F"];
						}
					}
				}
				#endregion

				#region fetchin partner name by id
				using (SqlCommand command = new("select Name_TarafHesab,Code_TarafHesab from tbl_TarafHesab where ID_tbl_TarafHesab = @PartnerId", connection))
				{
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@PartnerId", saleFactor.ID_tbl_TarafHesab);
					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							partnerName = (string)reader["Name_TarafHesab"];
							partnerCode = (long)reader["Code_TarafHesab"];
						}
					}
				}
				#endregion

				#region Fetching ID_tbl_S1
				using (SqlCommand command = new("Apk_Proc_Create_ID_tbl_S1", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleFactor.ID_tbl_SalMaly);

					using (var reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							ID_tbl_S1_Str = (long)(decimal)reader["ID_tbl_S1"];
						}
					}
				}
				#endregion

				#region Adding to table S1
				using (SqlCommand command = new("Apk_Proc_Add_tbl_S1", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@Date_S1", saleFactor.Dt_F);
					command.Parameters.AddWithValue("@Time_S1", saleFactor.Tm_C_F);
					command.Parameters.AddWithValue("@Desc_S1", " بابت فاکتور فروش داخلی به شماره " + N_F_Str + " - طرف حساب " + partnerName);
					command.Parameters.AddWithValue("@Type_S1", "سایر سیستم ها");
					command.Parameters.AddWithValue("@Type_ES1", "Sayer");
					command.Parameters.AddWithValue("@Vazyat_S1", "موقت");
					command.Parameters.AddWithValue("@J_S1", 0);
					command.Parameters.AddWithValue("@Date_C_S1", saleFactor.Dt_F);
					command.Parameters.AddWithValue("@Time_C_S1", saleFactor.Tm_C_F);
					command.Parameters.AddWithValue("@ID_tbl_Users", saleFactor.ID_tbl_Users);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleFactor.ID_tbl_SalMaly);

					await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Updating ID_tbl_S1 in tbl_ff
				using (SqlCommand command = new("Apk_Proc_Update_tbl_FF_ID_tbl_S1", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;

					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@ID_tbl_FF", ID_tbl_FF_Str);

					await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Updating Profit_Factor in tbl_ff
				using (SqlCommand command = new("UPDATE tbl_FF SET Pf_F=" + Profit_Factor + " WHERE ID_tbl_FF = " + ID_tbl_FF_Str, connection))
				{
					command.Transaction = transaction;
					await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Partner_as_the_Bedehkar_of_the_document
				if (Mablagh_Bahaye_Tamam_Shode > 0)
				{
					string Name_Hesab_Sarfasl = "";
					using (SqlCommand command = new("select Name_Hesab_Sarfasl from tbl_Sarfasl where Code_Hesab_Sarfasl=N'110301'", connection))
					{
						command.Transaction = transaction;
						using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
						{
							while (await reader.ReadAsync(cancellationToken))
							{
								Name_Hesab_Sarfasl = (string)reader["Name_Hesab_Sarfasl"];
							}
						}
					}

					Mablagh = saleFactor.JM_F + saleFactor.JAv_F + Mablagh_Koll - saleFactor.Tkh_N_F;
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", partnerName);
						command.Parameters.AddWithValue("@Code_Hst_S2", partnerCode.ToString());
						command.Parameters.AddWithValue("@Name_HsMt_S2", Name_Hesab_Sarfasl);
						command.Parameters.AddWithValue("@Code_HsMt_S2", "110301");
						command.Parameters.AddWithValue("@Code_HsKt_S2", "1103");
						command.Parameters.AddWithValue("@Code_HsGt_S2", "11");
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", "tbl_TarafHesab");
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", saleFactor.ID_tbl_TarafHesab);
						command.Parameters.AddWithValue("@Desc_S2", " بابت فاکتور فروش داخلی به شماره " + N_F_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", Mablagh);
						command.Parameters.AddWithValue("@Bestankar_S2", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleFactor.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Account_Sale_as_the_Bestankar_of_the_document
				using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;

					command.Parameters.AddWithValue("@Name_Hst_S2", saleFactorHesab.Nam_Tafsl_F_D);
					command.Parameters.AddWithValue("@Code_Hst_S2", saleFactorHesab.Cd_Tafsl_F_D);
					command.Parameters.AddWithValue("@Name_HsMt_S2", saleFactorHesab.Nam_Moen_F_D);
					command.Parameters.AddWithValue("@Code_HsMt_S2", saleFactorHesab.Cd_Moen_F_D);
					command.Parameters.AddWithValue("@Code_HsKt_S2", saleFactorHesab.Cd_Moen_F_D.Substring(0, 4));
					command.Parameters.AddWithValue("@Code_HsGt_S2", saleFactorHesab.Cd_Moen_F_D.Substring(0, 2));
					command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", saleFactorHesab.Name_tbl_Hesab_F_D);
					command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", saleFactorHesab.ID_tbl_Hesab_F_D);
					command.Parameters.AddWithValue("@Desc_S2", " بابت فاکتور فروش داخلی به شماره " + N_F_Str + " - طرف حساب " + partnerName);
					command.Parameters.AddWithValue("@Bedehkar_S2", 0);
					command.Parameters.AddWithValue("@Bestankar_S2", Mablagh_Koll);
					command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
					command.Parameters.AddWithValue("@Mbg_Arz1", 0);
					command.Parameters.AddWithValue("@Tmbg", 0);
					command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش");
					command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF");
					command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_Str);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleFactor.ID_tbl_SalMaly);

					var result = await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Malyat_as_the_Bestankar_of_the_document
				if (saleFactor.JM_F > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", saleFactorHesab.Nam_Tafsl_Mal_F_D);
						command.Parameters.AddWithValue("@Code_Hst_S2", saleFactorHesab.Cd_Tafsl_Mal_F_D);
						command.Parameters.AddWithValue("@Name_HsMt_S2", saleFactorHesab.Nam_Moen_Mal_F_D);
						command.Parameters.AddWithValue("@Code_HsMt_S2", saleFactorHesab.Cd_Moen_Mal_F_D);
						command.Parameters.AddWithValue("@Code_HsKt_S2", saleFactorHesab.Cd_Moen_Mal_F_D.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", saleFactorHesab.Cd_Moen_Mal_F_D.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", saleFactorHesab.Name_tbl_Hesab_Mal_F_D);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", saleFactorHesab.ID_tbl_Hesab_Mal_F_D);
						command.Parameters.AddWithValue("@Desc_S2", " مالیات بابت فاکتور فروش داخلی به شماره " + N_F_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", 0);
						command.Parameters.AddWithValue("@Bestankar_S2", saleFactor.JM_F);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleFactor.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Avarez_as_the_Bestankar_of_the_document
				if (saleFactor.JAv_F > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", saleFactorHesab.Nam_Tafsl_AV_F_D);
						command.Parameters.AddWithValue("@Code_Hst_S2", saleFactorHesab.Cd_Tafsl_AV_F_D);
						command.Parameters.AddWithValue("@Name_HsMt_S2", saleFactorHesab.Nam_Moen_AV_F_D);
						command.Parameters.AddWithValue("@Code_HsMt_S2", saleFactorHesab.Cd_Moen_AV_F_D);
						command.Parameters.AddWithValue("@Code_HsKt_S2", saleFactorHesab.Cd_Moen_AV_F_D.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", saleFactorHesab.Cd_Moen_AV_F_D.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", saleFactorHesab.Name_tbl_Hesab_AV_F_D);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", saleFactorHesab.ID_tbl_Hesab_AV_F_D);
						command.Parameters.AddWithValue("@Desc_S2", " عوارض بابت فاکتور فروش داخلی به شماره " + N_F_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", 0);
						command.Parameters.AddWithValue("@Bestankar_S2", saleFactor.JAv_F);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleFactor.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Takhfif_as_the_Bedehkar_of_the_document
				if (saleFactor.Tkh_N_F > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", saleFactorHesab.Nam_Tafsl_Tkh_F_D);
						command.Parameters.AddWithValue("@Code_Hst_S2", saleFactorHesab.Cd_Tafsl_Tkh_F_D);
						command.Parameters.AddWithValue("@Name_HsMt_S2", saleFactorHesab.Nam_Moen_Tkh_F_D);
						command.Parameters.AddWithValue("@Code_HsMt_S2", saleFactorHesab.Cd_Moen_Tkh_F_D);
						command.Parameters.AddWithValue("@Code_HsKt_S2", saleFactorHesab.Cd_Moen_Tkh_F_D.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", saleFactorHesab.Cd_Moen_Tkh_F_D.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", saleFactorHesab.Name_tbl_Hesab_Tkh_F_D);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", saleFactorHesab.ID_tbl_Hesab_Tkh_F_D);
						command.Parameters.AddWithValue("@Desc_S2", " تخفيف نقدی بابت فاکتور فروش داخلی به شماره " + N_F_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", saleFactor.Tkh_N_F);
						command.Parameters.AddWithValue("@Bestankar_S2", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleFactor.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Baha_as_the_Bedehkar_of_the_document
				if (saleFactorHesab.chkGenerate_And_Sanad_Baha_Kala == true)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", saleFactorHesab.Nam_Tafsl_Bah_F_D);
						command.Parameters.AddWithValue("@Code_Hst_S2", saleFactorHesab.Cd_Tafsl_Bah_F_D);
						command.Parameters.AddWithValue("@Name_HsMt_S2", saleFactorHesab.Nam_Moen_Bah_F_D);
						command.Parameters.AddWithValue("@Code_HsMt_S2", saleFactorHesab.Cd_Moen_Bah_F_D);
						command.Parameters.AddWithValue("@Code_HsKt_S2", saleFactorHesab.Cd_Moen_Bah_F_D.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", saleFactorHesab.Cd_Moen_Bah_F_D.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", saleFactorHesab.Name_tbl_Hesab_Bah_F_D);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", saleFactorHesab.ID_tbl_Hesab_Bah_F_D);
						command.Parameters.AddWithValue("@Desc_S2", " بهای تمام شده کالای فروش رفته داخلی به شماره " + N_F_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", Math.Round(Mablagh_Bahaye_Tamam_Shode));
						command.Parameters.AddWithValue("@Bestankar_S2", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleFactor.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Stock_as_the_Bestankar_of_the_document
				bool flagg = false;
				if (saleFactorHesab.chkGenerate_And_Sanad_Baha_Kala == true)
				{
					for (i = 0; i < struct_FactorItem_Arr.Length; i++)
					{
						using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
						{
							command.CommandType = CommandType.StoredProcedure;
							command.Transaction = transaction;
							Mablagh = 0;
							flagg = false;
							for (int jj = 0; jj < struct_FactorItem_Arr.Length; jj++)
							{
								if ((struct_FactorItem_Arr[i].ID_tbl_Anbar == struct_FactorItem_Arr[jj].ID_tbl_Anbar)
									&& struct_FactorItem_Arr[jj].Mogayese_Shode_Ast == false)
								{
									flagg = true;
									struct_FactorItem_Arr[jj].Mogayese_Shode_Ast = true;
									Mablagh += struct_FactorItem_Arr[jj].Mablagh_Bahaye_Tamam_Shode;
								}
							}
							if (Mablagh != 0 || flagg == true)
							{
								//===========================================================================
								using (SqlCommand command_Stock_Information = new(@"select tbl_Anbar.*,Name_Hesab_Sarfasl from tbl_Anbar 
                                              inner join tbl_Sarfasl on tbl_Anbar.Code_Hesab_Sarfasl_Moein_Anbar=tbl_Sarfasl.Code_Hesab_Sarfasl
                                              where ID_tbl_Anbar=" + struct_FactorItem_Arr[i].ID_tbl_Anbar, connection))
								{
									command_Stock_Information.Transaction = transaction;
									using (var reader_Stock_Information = await command_Stock_Information.ExecuteReaderAsync(cancellationToken))
									{
										while (await reader_Stock_Information.ReadAsync(cancellationToken))
										{
											command.Parameters.AddWithValue("@Name_Hst_S2", (reader_Stock_Information["Name_Anbar"]).ToString());
											command.Parameters.AddWithValue("@Code_Hst_S2", (reader_Stock_Information["Code_Anbar"]).ToString());
											command.Parameters.AddWithValue("@Name_HsMt_S2", (reader_Stock_Information["Name_Hesab_Sarfasl"]).ToString());
											command.Parameters.AddWithValue("@Code_HsMt_S2", (reader_Stock_Information["Code_Hesab_Sarfasl_Moein_Anbar"]).ToString());
											command.Parameters.AddWithValue("@Code_HsKt_S2", (reader_Stock_Information["Code_Hesab_Sarfasl_Moein_Anbar"]).ToString().Substring(0, 4));
											command.Parameters.AddWithValue("@Code_HsGt_S2", (reader_Stock_Information["Code_Hesab_Sarfasl_Moein_Anbar"]).ToString().Substring(0, 2));
											command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", "tbl_Anbar");
											command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", long.Parse((reader_Stock_Information["ID_tbl_Anbar"]).ToString()));
										}
									}
								}
								//===========================================================================

								command.Parameters.AddWithValue("@Desc_S2", " بابت فاکتور فروش داخلی به شماره " + N_F_Str + " - طرف حساب " + partnerName);
								command.Parameters.AddWithValue("@Bedehkar_S2", 0);
								command.Parameters.AddWithValue("@Bestankar_S2", Mablagh);
								command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
								command.Parameters.AddWithValue("@Mbg_Arz1", 0);
								command.Parameters.AddWithValue("@Tmbg", 0);
								command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
								command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
								command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
								command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
								command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش");
								command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF");
								command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_Str);
								command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleFactor.ID_tbl_SalMaly);

								var result = await command.ExecuteNonQueryAsync(cancellationToken);
							}
							command.Parameters.Clear();
						}
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
	public async Task<SysResult<IEnumerable<SaleFactor>>> GetSaleFactors(int fiscalYear, int UserId, string Date1, string Date2, object parameters = null!, CancellationToken cancellationToken = default)
	{
		IEnumerable<SaleFactor> saleFactors = Enumerable.Empty<SaleFactor>();
		if (parameters == null)
		{
			saleFactors = await _dbManager.CallProcedureWithParametersAsync<SaleFactor>("Apk_Proc_Get_List_tbl_FF",
				new
				{
					ID_tbl_SalMaly = fiscalYear,
					ID_tbl_Users = UserId,
					@Dt_F1 = Date1,
					@Dt_F2 = Date2,
				});
		}
		else
		{
			saleFactors = await _dbManager.CallProcedureWithParametersAsync<SaleFactor>("Apk_Proc_Get_List_tbl_FF",
				parameters, cancellationToken);
		}

		return new(saleFactors);
	}
	public async Task<List<SaleFactor>> GetAllSaleFactorsCount(int fiscalYear, int UserId, string Date1, string Date2, CancellationToken cancellationToken = default)
	{
		IEnumerable<SaleFactor> Factor = await _dbManager.CallProcedureWithParametersAsync<SaleFactor>("Apk_Proc_Get_List_tbl_FF", new
		{
			ID_tbl_SalMaly = fiscalYear,
			ID_tbl_Users = UserId,
			@Dt_F1 = Date1,
			@Dt_F2 = Date2,
		});
		return new(Factor);
	}
	public async Task<List<SaleFactor>> GetSaleFactorPrint(long? ForoshID, long? fiscalYear, int? UserId, int? Offset, int? ID_tbl_Bzy, string? Date1, string? Date2, object parameters = null!, CancellationToken cancellationToken = default)
	{
		IEnumerable<SaleFactor> Factor = await _dbManager.CallProcedureWithParametersAsync<SaleFactor>("Apk_Proc_Report_Forosh", new
		{
			Type_Call_Proc = "Select_Top_N_Factor",
			ID_tbl_SalMaly = fiscalYear,
			ID_tbl_F = ForoshID,
			ID_tbl_TarafHesab = UserId,
			Date1 = Date1,
			Date2 = Date2,
			ID_tbl_Bzy = ID_tbl_Bzy,
			Offset = Offset,

		});
		return new(Factor);
	}


	struct struct_ServiceFactorItem
	{
		public long ID_tbl_Khedmat;
		public long ID_tbl_TarafHesab_Moj_Rdf;
		public decimal Pors_Dr_Rdf;
		public decimal Pors_Mbg_Rdf;
		public bool Mogayese_Shode_Ast;
	}
	public async Task<SysResult<bool>> AddSaleServiceFactor(SaleServiceFactor saleServiceFactor, IEnumerable<ServiceFactorItem> factorItems, CancellationToken cancellationToken = default)
	{
		//Console.WriteLine(JsonSerializer.Serialize(saleServiceFactor));
		Console.WriteLine(JsonSerializer.Serialize(factorItems));
		using (SqlConnection connection = new(_configuration.GetConnectionString("Default")))
		{
			await connection.OpenAsync();
			SqlTransaction transaction = connection.BeginTransaction();

			try
			{
				ServiceSaleFactorHesab ServiceSaleFactorHesab = new();
				#region Fetch_Accounts_For_Service_Sale_Factor
				using (SqlCommand command = new("Proc_Comp", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;

					command.Parameters.AddWithValue("@Type_Call_Proc", "Sett_F_khed");
					command.Parameters.AddWithValue("@ID_tbl", 1);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);
					command.Parameters.AddWithValue("@Where_Query", "-");

					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync())
						{
							if ((long)reader["ID_tbl_Hesab_DrAmad_Khed"] == 0 ||
								(long)reader["ID_tbl_Hesab_Mal_Khed"] == 0 ||
								(long)reader["ID_tbl_Hesab_Av_Khed"] == 0 ||
								(long)reader["ID_tbl_Hesab_Tkhf_Khed"] == 0 ||
								(long)reader["ID_tbl_Hesab_PorsMoj_Khed"] == 0 ||
								(long)reader["ID_tbl_Hesab_Bargasht_Khed"] == 0)
							{
								return new(false, false, new() { "حساب های فروش خدمت تنظیم نشده است." });
							}
							ServiceSaleFactorHesab.Cd_Moen_Hesab_DrAmad_Khed = (string)reader["Cd_Moen_Hesab_DrAmad_Khed"];
							ServiceSaleFactorHesab.Nam_Moen_Hesab_DrAmad_Khed = (string)reader["Nam_Moen_Hesab_DrAmad_Khed"];
							ServiceSaleFactorHesab.Cd_Tafsl_Hesab_DrAmad_Khed = ConvertFromDBVal<string>(reader["Cd_Tafsl_Hesab_DrAmad_Khed"]);
							ServiceSaleFactorHesab.Nam_Tafsl_Hesab_DrAmad_Khed = ConvertFromDBVal<string>(reader["Nam_Tafsl_Hesab_DrAmad_Khed"]);
							ServiceSaleFactorHesab.Name_tbl_Hesab_DrAmad_Khed = (string)reader["Name_tbl_Hesab_DrAmad_Khed"];
							ServiceSaleFactorHesab.ID_tbl_Hesab_DrAmad_Khed = (long)reader["ID_tbl_Hesab_DrAmad_Khed"];

							ServiceSaleFactorHesab.Cd_Moen_Hesab_Mal_Khed = (string)reader["Cd_Moen_Hesab_Mal_Khed"];
							ServiceSaleFactorHesab.Nam_Moen_Hesab_Mal_Khed = (string)reader["Nam_Moen_Hesab_Mal_Khed"];
							ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Mal_Khed = ConvertFromDBVal<string>(reader["Cd_Tafsl_Hesab_Mal_Khed"]);
							ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Mal_Khed = ConvertFromDBVal<string>(reader["Nam_Tafsl_Hesab_Mal_Khed"]);
							ServiceSaleFactorHesab.Name_tbl_Hesab_Mal_Khed = (string)reader["Name_tbl_Hesab_Mal_Khed"];
							ServiceSaleFactorHesab.ID_tbl_Hesab_Mal_Khed = (long)reader["ID_tbl_Hesab_Mal_Khed"];

							ServiceSaleFactorHesab.Cd_Moen_Hesab_Av_Khed = (string)reader["Cd_Moen_Hesab_Av_Khed"];
							ServiceSaleFactorHesab.Nam_Moen_Hesab_Av_Khed = (string)reader["Nam_Moen_Hesab_Av_Khed"];
							ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Av_Khed = ConvertFromDBVal<string>(reader["Cd_Tafsl_Hesab_Av_Khed"]);
							ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Av_Khed = ConvertFromDBVal<string>(reader["Nam_Tafsl_Hesab_Av_Khed"]);
							ServiceSaleFactorHesab.Name_tbl_Hesab_Av_Khed = (string)reader["Name_tbl_Hesab_Av_Khed"];
							ServiceSaleFactorHesab.ID_tbl_Hesab_Av_Khed = (long)reader["ID_tbl_Hesab_Av_Khed"];

							ServiceSaleFactorHesab.Cd_Moen_Hesab_Tkhf_Khed = (string)reader["Cd_Moen_Hesab_Tkhf_Khed"];
							ServiceSaleFactorHesab.Nam_Moen_Hesab_Tkhf_Khed = (string)reader["Nam_Moen_Hesab_Tkhf_Khed"];
							ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Tkhf_Khed = ConvertFromDBVal<string>(reader["Cd_Tafsl_Hesab_Tkhf_Khed"]);
							ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Tkhf_Khed = ConvertFromDBVal<string>(reader["Nam_Tafsl_Hesab_Tkhf_Khed"]);
							ServiceSaleFactorHesab.Name_tbl_Hesab_Tkhf_Khed = (string)reader["Name_tbl_Hesab_Tkhf_Khed"];
							ServiceSaleFactorHesab.ID_tbl_Hesab_Tkhf_Khed = (long)reader["ID_tbl_Hesab_Tkhf_Khed"];

							ServiceSaleFactorHesab.Cd_Moen_Hesab_PorsMoj_Khed = (string)reader["Cd_Moen_Hesab_PorsMoj_Khed"];
							ServiceSaleFactorHesab.Nam_Moen_Hesab_PorsMoj_Khed = (string)reader["Nam_Moen_Hesab_PorsMoj_Khed"];
							ServiceSaleFactorHesab.Cd_Tafsl_Hesab_PorsMoj_Khed = ConvertFromDBVal<string>(reader["Cd_Tafsl_Hesab_PorsMoj_Khed"]);
							ServiceSaleFactorHesab.Nam_Tafsl_Hesab_PorsMoj_Khed = ConvertFromDBVal<string>(reader["Nam_Tafsl_Hesab_PorsMoj_Khed"]);
							ServiceSaleFactorHesab.Name_tbl_Hesab_PorsMoj_Khed = ConvertFromDBVal<string>(reader["Name_tbl_Hesab_PorsMoj_Khed"]);
							ServiceSaleFactorHesab.ID_tbl_Hesab_PorsMoj_Khed = (long)reader["ID_tbl_Hesab_PorsMoj_Khed"];

							ServiceSaleFactorHesab.Cd_Moen_Hesab_Bargasht_Khed = (string)reader["Cd_Moen_Hesab_Bargasht_Khed"];
							ServiceSaleFactorHesab.Nam_Moen_Hesab_Bargasht_Khed = (string)reader["Nam_Moen_Hesab_Bargasht_Khed"];
							ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Bargasht_Khed = ConvertFromDBVal<string>(reader["Cd_Tafsl_Hesab_Bargasht_Khed"]);
							ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Bargasht_Khed = ConvertFromDBVal<string>(reader["Nam_Tafsl_Hesab_Bargasht_Khed"]);
							ServiceSaleFactorHesab.Name_tbl_Hesab_Bargasht_Khed = (string)reader["Name_tbl_Hesab_Bargasht_Khed"];
							ServiceSaleFactorHesab.ID_tbl_Hesab_Bargasht_Khed = (long)reader["ID_tbl_Hesab_Bargasht_Khed"];

							ServiceSaleFactorHesab.chkSbt_Snd_Pors_Khed = (bool)reader["chkSbt_Snd_Pors_Khed"];
						}
					}
				}
				#endregion

				int i = 0;
				decimal Mablagh = 0;
				decimal Mablagh_Koll = 0;
				decimal Mablagh_Jam_Porsant = 0;
				long ID_tbl_FF_KHed_Str = 0;
				long N_FF_KHed_Str = 0;
				long ID_tbl_S1_Str = 0;
				string partnerName = string.Empty;
				long partnerCode = 0;
				struct_ServiceFactorItem[] struct_FactorItem_Arr = new struct_ServiceFactorItem[factorItems.Count()];

				foreach (var item in factorItems)
				{
					Mablagh_Koll += item.Tedad * item.Fi_Ba_Haz;
					Mablagh_Jam_Porsant += Math.Round(item.Pors_Mbg_Rdf);
					struct_FactorItem_Arr[i].ID_tbl_Khedmat = item.ID_tbl_Khedmat;
					struct_FactorItem_Arr[i].ID_tbl_TarafHesab_Moj_Rdf = item.ID_tbl_TarafHesab_Moj_Rdf;
					struct_FactorItem_Arr[i].Pors_Dr_Rdf = item.Pors_Dr_Rdf;
					struct_FactorItem_Arr[i].Pors_Mbg_Rdf = Math.Round(item.Pors_Mbg_Rdf);
					struct_FactorItem_Arr[i].Mogayese_Shode_Ast = false;
					i++;
				}

				#region Adding new sale factor
				using (SqlCommand command = new SqlCommand("Apk_Proc_Add_tbl_FF_KHed", connection))
				{
					command.Transaction = transaction;
					command.CommandType = CommandType.StoredProcedure;
					var properties = saleServiceFactor.GetType().GetProperties().ToList();
					properties.RemoveAll(b => b.Name == "Notifications");
					properties.RemoveAll(b => b.Name == "ID_tbl_FF_KHed");
					properties.RemoveAll(b => b.Name == "FiscalYearBeginDate");
					properties.RemoveAll(b => b.Name == "FiscalYearEndDate");
					properties.RemoveAll(b => b.Name == "FiscalYearTitle");
					properties.RemoveAll(b => b.Name == "Tedad");
					properties.RemoveAll(b => b.Name == "Fi");
					properties.RemoveAll(b => b.Name == "Mablagh");
					properties.RemoveAll(b => b.Name == "Name_Branch");
					properties.RemoveAll(b => b.Name == "Name_TarafHesab");
					properties.RemoveAll(b => b.Name == "Code_TarafHesab");
					properties.RemoveAll(b => b.Name == "ChelPhone_TarafHesab");
					properties.RemoveAll(b => b.Name == "Name_Bzy");
					properties.RemoveAll(b => b.Name == "JM_F");
					properties.RemoveAll(b => b.Name == "Name_TarafHesab_Moj_Rdf");
					properties.RemoveAll(b => b.Name == "Code_TarafHesab_Moj_Rdf");
					properties.RemoveAll(b => b.Name == "Name_Khedmat");
					properties.RemoveAll(b => b.Name == "Cde_Khedmat");
					properties.RemoveAll(b => b.Name == "Shenase_Khedmat");
					foreach (var p in properties)
					{
						command.Parameters.AddWithValue($"@{p.Name}", p.GetValue(saleServiceFactor));
					}
					ID_tbl_FF_KHed_Str = (long)(decimal)await command.ExecuteScalarAsync(cancellationToken);
				}
				foreach (var item in factorItems)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_F_Aglm_Khed", connection))
					{
						command.Transaction = transaction;
						command.CommandType = CommandType.StoredProcedure;
						var properties = item.GetType().GetProperties().ToList();
						properties.RemoveAll(b => b.Name == "Notifications");
						properties.RemoveAll(b => b.Name == "Type_tbl_F");

						item.ID_tbl_F = ID_tbl_FF_KHed_Str;
						item.ID_tbl_SalMaly = saleServiceFactor.ID_tbl_SalMaly;
						command.Parameters.AddWithValue("@Type_tbl_F", "tbl_FF_KHed");
						foreach (var p in properties)
						{
							command.Parameters.AddWithValue($"@{p.Name}", p.GetValue(item));
						}

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Fetching Number Inserted Sale Factor
				using (SqlCommand command = new("select N_FF_KHed from tbl_FF_KHed where ID_tbl_FF_KHed=" + ID_tbl_FF_KHed_Str, connection))
				{
					command.Transaction = transaction;
					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							N_FF_KHed_Str = (long)reader["N_FF_KHed"];
						}
					}
				}
				#endregion

				#region fetchin partner name by id
				using (SqlCommand command = new("select Name_TarafHesab,Code_TarafHesab from tbl_TarafHesab where ID_tbl_TarafHesab = @PartnerId", connection))
				{
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@PartnerId", saleServiceFactor.ID_tbl_TarafHesab);
					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							partnerName = (string)reader["Name_TarafHesab"];
							partnerCode = (long)reader["Code_TarafHesab"];
						}
					}
				}
				#endregion

				#region Fetching ID_tbl_S1
				using (SqlCommand command = new("Apk_Proc_Create_ID_tbl_S1", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

					using (var reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							ID_tbl_S1_Str = (long)(decimal)reader["ID_tbl_S1"];
						}
					}
				}
				#endregion

				#region Adding to table S1
				using (SqlCommand command = new("Apk_Proc_Add_tbl_S1", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@Date_S1", saleServiceFactor.Dt_FF_KHed);
					command.Parameters.AddWithValue("@Time_S1", saleServiceFactor.Tm_C_FF_KHed);
					command.Parameters.AddWithValue("@Desc_S1", " بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
					command.Parameters.AddWithValue("@Type_S1", "سایر سیستم ها");
					command.Parameters.AddWithValue("@Type_ES1", "Sayer");
					command.Parameters.AddWithValue("@Vazyat_S1", "موقت");
					command.Parameters.AddWithValue("@J_S1", 0);
					command.Parameters.AddWithValue("@Date_C_S1", saleServiceFactor.Dt_FF_KHed);
					command.Parameters.AddWithValue("@Time_C_S1", saleServiceFactor.Tm_C_FF_KHed);
					command.Parameters.AddWithValue("@ID_tbl_Users", saleServiceFactor.ID_tbl_Users);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

					var result = await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Updating ID_tbl_S1 in tbl_FF_KHed
				using (SqlCommand command = new("update tbl_FF_KHed set ID_tbl_S1=@ID_tbl_S1 where ID_tbl_FF_KHed=@ID_tbl_FF_KHed", connection))
				{
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@ID_tbl_FF_KHed", ID_tbl_FF_KHed_Str);

					var result = await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Partner_as_the_Bedehkar_of_the_document
				string Name_Hesab_Sarfasl = "";
				using (SqlCommand command = new("select Name_Hesab_Sarfasl from tbl_Sarfasl where Code_Hesab_Sarfasl=N'110301'", connection))
				{
					command.Transaction = transaction;
					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							Name_Hesab_Sarfasl = (string)reader["Name_Hesab_Sarfasl"];
						}
					}
				}

				Mablagh = saleServiceFactor.JM_FF_KHed + saleServiceFactor.JAv_FF_KHed + Mablagh_Koll - saleServiceFactor.Tkh_N_FF_KHed;
				using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;

					command.Parameters.AddWithValue("@Name_Hst_S2", partnerName);
					command.Parameters.AddWithValue("@Code_Hst_S2", partnerCode.ToString());
					command.Parameters.AddWithValue("@Name_HsMt_S2", Name_Hesab_Sarfasl);
					command.Parameters.AddWithValue("@Code_HsMt_S2", "110301");
					command.Parameters.AddWithValue("@Code_HsKt_S2", "1103");
					command.Parameters.AddWithValue("@Code_HsGt_S2", "11");
					command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", "tbl_TarafHesab");
					command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", saleServiceFactor.ID_tbl_TarafHesab);
					command.Parameters.AddWithValue("@Desc_S2", " بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
					command.Parameters.AddWithValue("@Bedehkar_S2", Mablagh);
					command.Parameters.AddWithValue("@Bestankar_S2", 0);
					command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
					command.Parameters.AddWithValue("@Mbg_Arz1", 0);
					command.Parameters.AddWithValue("@Tmbg", 0);
					command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
					command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
					command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

					var result = await command.ExecuteNonQueryAsync(cancellationToken);
				}

				#endregion

				#region Account_Sale_as_the_Bestankar_of_the_document
				using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;

					command.Parameters.AddWithValue("@Name_Hst_S2", ServiceSaleFactorHesab.Nam_Tafsl_Hesab_DrAmad_Khed);
					command.Parameters.AddWithValue("@Code_Hst_S2", ServiceSaleFactorHesab.Cd_Tafsl_Hesab_DrAmad_Khed);
					command.Parameters.AddWithValue("@Name_HsMt_S2", ServiceSaleFactorHesab.Nam_Moen_Hesab_DrAmad_Khed);
					command.Parameters.AddWithValue("@Code_HsMt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_DrAmad_Khed);
					command.Parameters.AddWithValue("@Code_HsKt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_DrAmad_Khed.Substring(0, 4));
					command.Parameters.AddWithValue("@Code_HsGt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_DrAmad_Khed.Substring(0, 2));
					command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.Name_tbl_Hesab_DrAmad_Khed);
					command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.ID_tbl_Hesab_DrAmad_Khed);
					command.Parameters.AddWithValue("@Desc_S2", " بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
					command.Parameters.AddWithValue("@Bedehkar_S2", 0);
					command.Parameters.AddWithValue("@Bestankar_S2", Mablagh_Koll);
					command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
					command.Parameters.AddWithValue("@Mbg_Arz1", 0);
					command.Parameters.AddWithValue("@Tmbg", 0);
					command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
					command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
					command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

					var result = await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Malyat_as_the_Bestankar_of_the_document
				if (saleServiceFactor.JM_FF_KHed > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Mal_Khed);
						command.Parameters.AddWithValue("@Code_Hst_S2", ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Mal_Khed);
						command.Parameters.AddWithValue("@Name_HsMt_S2", ServiceSaleFactorHesab.Nam_Moen_Hesab_Mal_Khed);
						command.Parameters.AddWithValue("@Code_HsMt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Mal_Khed);
						command.Parameters.AddWithValue("@Code_HsKt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Mal_Khed.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Mal_Khed.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.Name_tbl_Hesab_Mal_Khed);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.ID_tbl_Hesab_Mal_Khed);
						command.Parameters.AddWithValue("@Desc_S2", " مالیات بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", 0);
						command.Parameters.AddWithValue("@Bestankar_S2", saleServiceFactor.JM_FF_KHed);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Avarez_as_the_Bestankar_of_the_document
				Console.WriteLine("saleServiceFactor.JAv_FF_KHed " + saleServiceFactor.JAv_FF_KHed.ToString());
				if (saleServiceFactor.JAv_FF_KHed > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Av_Khed);
						command.Parameters.AddWithValue("@Code_Hst_S2", ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Av_Khed);
						command.Parameters.AddWithValue("@Name_HsMt_S2", ServiceSaleFactorHesab.Nam_Moen_Hesab_Av_Khed);
						command.Parameters.AddWithValue("@Code_HsMt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Av_Khed);
						command.Parameters.AddWithValue("@Code_HsKt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Av_Khed.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Av_Khed.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.Name_tbl_Hesab_Av_Khed);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.ID_tbl_Hesab_Av_Khed);
						command.Parameters.AddWithValue("@Desc_S2", " عوارض بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", 0);
						command.Parameters.AddWithValue("@Bestankar_S2", saleServiceFactor.JAv_FF_KHed);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
						Console.WriteLine("JAv_FF_KHed " + result.ToString());
					}
				}
				#endregion

				#region Takhfif_as_the_Bedehkar_of_the_document
				Console.WriteLine("saleServiceFactor.Tkh_N_FF_KHed " + saleServiceFactor.Tkh_N_FF_KHed.ToString());
				if (saleServiceFactor.Tkh_N_FF_KHed > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Tkhf_Khed);
						command.Parameters.AddWithValue("@Code_Hst_S2", ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Tkhf_Khed);
						command.Parameters.AddWithValue("@Name_HsMt_S2", ServiceSaleFactorHesab.Nam_Moen_Hesab_Tkhf_Khed);
						command.Parameters.AddWithValue("@Code_HsMt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Tkhf_Khed);
						command.Parameters.AddWithValue("@Code_HsKt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Tkhf_Khed.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Tkhf_Khed.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.Name_tbl_Hesab_Tkhf_Khed);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.ID_tbl_Hesab_Tkhf_Khed);
						command.Parameters.AddWithValue("@Desc_S2", " تخفيف نقدی بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", saleServiceFactor.Tkh_N_FF_KHed);
						command.Parameters.AddWithValue("@Bestankar_S2", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Porsant_Bedehkar
				if (Mablagh_Jam_Porsant > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						if (ServiceSaleFactorHesab.Nam_Tafsl_Hesab_PorsMoj_Khed != null)
							command.Parameters.AddWithValue("@Name_Hst_S2", ServiceSaleFactorHesab.Nam_Tafsl_Hesab_PorsMoj_Khed);
						else
							command.Parameters.AddWithValue("@Name_Hst_S2", "");

						if (ServiceSaleFactorHesab.Cd_Tafsl_Hesab_PorsMoj_Khed != null)
							command.Parameters.AddWithValue("@Code_Hst_S2", ServiceSaleFactorHesab.Cd_Tafsl_Hesab_PorsMoj_Khed);
						else
							command.Parameters.AddWithValue("@Code_Hst_S2", "");

						command.Parameters.AddWithValue("@Name_HsMt_S2", ServiceSaleFactorHesab.Nam_Moen_Hesab_PorsMoj_Khed);
						command.Parameters.AddWithValue("@Code_HsMt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_PorsMoj_Khed);
						command.Parameters.AddWithValue("@Code_HsKt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_PorsMoj_Khed.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_PorsMoj_Khed.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.Name_tbl_Hesab_PorsMoj_Khed);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.ID_tbl_Hesab_PorsMoj_Khed);
						command.Parameters.AddWithValue("@Desc_S2", " ثبت هزینه پورسانت بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", Mablagh_Jam_Porsant);
						command.Parameters.AddWithValue("@Bestankar_S2", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Porsant_Mojri_Bedehkar
				if (ServiceSaleFactorHesab.chkSbt_Snd_Pors_Khed == true)
				{
					bool flagg = false;
					for (i = 0; i < struct_FactorItem_Arr.Length; i++)
					{
						if (struct_FactorItem_Arr[i].ID_tbl_TarafHesab_Moj_Rdf > 0)
						{
							using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
							{
								command.CommandType = CommandType.StoredProcedure;
								command.Transaction = transaction;
								Mablagh = 0;
								flagg = false;
								for (int jj = 0; jj < struct_FactorItem_Arr.Length; jj++)
								{
									if ((struct_FactorItem_Arr[i].ID_tbl_TarafHesab_Moj_Rdf == struct_FactorItem_Arr[jj].ID_tbl_TarafHesab_Moj_Rdf)
										&& struct_FactorItem_Arr[jj].Mogayese_Shode_Ast == false)
									{
										flagg = true;
										struct_FactorItem_Arr[jj].Mogayese_Shode_Ast = true;
										Mablagh += struct_FactorItem_Arr[jj].Pors_Mbg_Rdf;
									}
								}
								if (Mablagh > 0 && flagg == true)
								{
									//===========================================================================
									using (SqlCommand command_tbl_TarafHesab = new(@"select ID_tbl_TarafHesab,Name_TarafHesab,Code_TarafHesab from tbl_TarafHesab where ID_tbl_TarafHesab=" + struct_FactorItem_Arr[i].ID_tbl_TarafHesab_Moj_Rdf, connection))
									{
										command_tbl_TarafHesab.Transaction = transaction;
										using (var reader_tbl_TarafHesab = await command_tbl_TarafHesab.ExecuteReaderAsync(cancellationToken))
										{
											while (await reader_tbl_TarafHesab.ReadAsync(cancellationToken))
											{
												Console.WriteLine("ID_tbl_TarafHesab :" + (reader_tbl_TarafHesab["ID_tbl_TarafHesab"]).ToString());
												command.Parameters.AddWithValue("@Name_Hst_S2", (reader_tbl_TarafHesab["Name_TarafHesab"]).ToString());
												command.Parameters.AddWithValue("@Code_Hst_S2", (reader_tbl_TarafHesab["Code_TarafHesab"]).ToString());
												command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", "tbl_TarafHesab");
												command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", long.Parse((reader_tbl_TarafHesab["ID_tbl_TarafHesab"]).ToString()));
											}
										}
									}
									//===========================================================================
									using (SqlCommand command_Hesab_sarfasl = new(@"select Name_Hesab_Sarfasl,ID_tbl_Sarfasl from tbl_Sarfasl where Code_Hesab_Sarfasl=N'210209'", connection))
									{
										command_Hesab_sarfasl.Transaction = transaction;
										using (var reader_Hesab_sarfasl = await command_Hesab_sarfasl.ExecuteReaderAsync(cancellationToken))
										{
											while (await reader_Hesab_sarfasl.ReadAsync(cancellationToken))
											{
												command.Parameters.AddWithValue("@Name_HsMt_S2", (reader_Hesab_sarfasl["Name_Hesab_Sarfasl"]).ToString());
												command.Parameters.AddWithValue("@Code_HsMt_S2", "210209");
												command.Parameters.AddWithValue("@Code_HsKt_S2", "2102");
												command.Parameters.AddWithValue("@Code_HsGt_S2", "21");
											}
										}
									}
									//===========================================================================
									command.Parameters.AddWithValue("@Desc_S2", " بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
									command.Parameters.AddWithValue("@Bedehkar_S2", 0);
									command.Parameters.AddWithValue("@Bestankar_S2", Mablagh);
									command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
									command.Parameters.AddWithValue("@Mbg_Arz1", 0);
									command.Parameters.AddWithValue("@Tmbg", 0);
									command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
									command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
									command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
									command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
									command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
									command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
									command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
									command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

									var result = await command.ExecuteNonQueryAsync(cancellationToken);
								}
								command.Parameters.Clear();
							}
						}
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
	public async Task<IEnumerable<Services>> GetAllServices(CancellationToken cancellationToken)
	{
		IEnumerable<Services> services = await _dbManager.CallProcedureAsync<Services>("Apk_Proc_GetServices", cancellationToken);
		return services;
	}
	public async Task<SysResult<IEnumerable<SaleServiceFactor>>> GetServiceSaleFactors(int fiscalYear, int UserId, string Date1, string Date2, object parameters = null!, CancellationToken cancellationToken = default)
	{
		IEnumerable<SaleServiceFactor> saleFactors = Enumerable.Empty<SaleServiceFactor>();
		if (parameters == null)
		{
			saleFactors = await _dbManager.CallProcedureWithParametersAsync<SaleServiceFactor>("Apk_Proc_Get_List_tbl_FF_KHed",
				new
				{
					ID_tbl_SalMaly = fiscalYear,
					ID_tbl_Users = UserId,
					@Dt_F1 = Date1,
					@Dt_F2 = Date2,
				});
		}
		else
		{
			saleFactors = await _dbManager.CallProcedureWithParametersAsync<SaleServiceFactor>("Apk_Proc_Get_List_tbl_FF_KHed",
				parameters, cancellationToken);
		}

		return new(saleFactors);
	}
	public async Task<List<SaleServiceFactor>> GetAllServiceSaleFactorsCount(int fiscalYear, int UserId, string Date1, string Date2, CancellationToken cancellationToken = default)
	{
		IEnumerable<SaleServiceFactor> Factor = await _dbManager.CallProcedureWithParametersAsync<SaleServiceFactor>("Apk_Proc_Get_List_tbl_FF_KHed", new
		{
			ID_tbl_SalMaly = fiscalYear,
			ID_tbl_Users = UserId,
			@Dt_F1 = Date1,
			@Dt_F2 = Date2,
		});
		return new(Factor);
	}
	public async Task<List<SaleServiceFactor>> GetServiceSaleFactorPrint(long? ForoshID, long? fiscalYear, int? UserId, int? OFfset, int? IDBzy, string? From, string? To, object parameters = null!, CancellationToken cancellationToken = default)
	{
		IEnumerable<SaleServiceFactor> Factor = await _dbManager.CallProcedureWithParametersAsync<SaleServiceFactor>("Apk_Proc_Report_Forosh_Khedmat", new
		{
			Type_Call_Proc = "Select_Top_N_Factor",
			ID_tbl_SalMaly = fiscalYear,
			ID_tbl_F = ForoshID,
			ID_tbl_TarafHesab = UserId,
			Date1 = From,
			Date2 = To,
			ID_tbl_Bzy = IDBzy,
			Offset = OFfset,
		});
		return new(Factor);
	}
	public async Task<IEnumerable<PartnerInServiceFactor>> GetAllServicePartnetsAsync(CancellationToken cancellationToken)
	{
		IEnumerable<PartnerInServiceFactor> partnerInServiceFactors = await _dbManager.CallProcedureAsync<PartnerInServiceFactor>("Apk_Proc_GetPartner_IN_ServiceFactor", cancellationToken);
		return partnerInServiceFactors;
	}



	public async Task<SysResult<bool>> AddReturnedInvoice(ReturnedInvoice returnedInvoice, IEnumerable<FactorItem> factorItems, CancellationToken cancellationToken = default)
	{
		using (SqlConnection connection = new(_configuration.GetConnectionString("Default")))
		{
			await connection.OpenAsync();
			SqlTransaction transaction = connection.BeginTransaction();
			try
			{
				SaleReturnedInvoiceHesab ReturnedInvoiceHesab = new();
				#region Fetch_Accounts_For_Factor
				Console.WriteLine("Before proc_Comp - Sett_F");
				using (SqlCommand command = new("Proc_Comp", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;

					command.Parameters.AddWithValue("@Type_Call_Proc", "Sett_F");
					command.Parameters.AddWithValue("@ID_tbl", 1);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", returnedInvoice.ID_tbl_SalMaly);
					command.Parameters.AddWithValue("@Where_Query", "-");

					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync())
						{
							if ((long)reader["ID_tbl_Hesab_BF_D"] == 0 ||
								(long)reader["ID_tbl_Hesab_AV_F_D"] == 0 ||
								(long)reader["ID_tbl_Hesab_Bah_F_D"] == 0 ||
								(long)reader["ID_tbl_Hesab_Jayeze_F_D"] == 0 ||
								(long)reader["ID_tbl_Hesab_Mal_F_D"] == 0 ||
								(long)reader["ID_tbl_Hesab_Tkh_F_D"] == 0)
							{
								return new(false, false, new() { "حساب های فروش تنظیم نشده است." });
							}
							ReturnedInvoiceHesab.Cd_Moen_BF_D = (string)reader["Cd_Moen_BF_D"];
							ReturnedInvoiceHesab.Nam_Moen_BF_D = (string)reader["Nam_Moen_BF_D"];
							ReturnedInvoiceHesab.Cd_Tafsl_BF_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_BF_D"]);
							ReturnedInvoiceHesab.Nam_Tafsl_BF_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_BF_D"]);
							ReturnedInvoiceHesab.Name_tbl_Hesab_BF_D = (string)reader["Name_tbl_Hesab_BF_D"];
							ReturnedInvoiceHesab.ID_tbl_Hesab_BF_D = (long)reader["ID_tbl_Hesab_BF_D"];

							ReturnedInvoiceHesab.Cd_Moen_Mal_F_D = (string)reader["Cd_Moen_Mal_F_D"];
							ReturnedInvoiceHesab.Nam_Moen_Mal_F_D = (string)reader["Nam_Moen_Mal_F_D"];
							ReturnedInvoiceHesab.Cd_Tafsl_Mal_F_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_Mal_F_D"]);
							ReturnedInvoiceHesab.Nam_Tafsl_Mal_F_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_Mal_F_D"]);
							ReturnedInvoiceHesab.Name_tbl_Hesab_Mal_F_D = (string)reader["Name_tbl_Hesab_Mal_F_D"];
							ReturnedInvoiceHesab.ID_tbl_Hesab_Mal_F_D = (long)reader["ID_tbl_Hesab_Mal_F_D"];

							ReturnedInvoiceHesab.Cd_Moen_AV_F_D = (string)reader["Cd_Moen_AV_F_D"];
							ReturnedInvoiceHesab.Nam_Moen_AV_F_D = (string)reader["Nam_Moen_AV_F_D"];
							ReturnedInvoiceHesab.Cd_Tafsl_AV_F_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_AV_F_D"]);
							ReturnedInvoiceHesab.Nam_Tafsl_AV_F_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_AV_F_D"]);
							ReturnedInvoiceHesab.Name_tbl_Hesab_AV_F_D = (string)reader["Name_tbl_Hesab_AV_F_D"];
							ReturnedInvoiceHesab.ID_tbl_Hesab_AV_F_D = (long)reader["ID_tbl_Hesab_AV_F_D"];

							ReturnedInvoiceHesab.Cd_Moen_Tkh_F_D = (string)reader["Cd_Moen_Tkh_F_D"];
							ReturnedInvoiceHesab.Nam_Moen_Tkh_F_D = (string)reader["Nam_Moen_Tkh_F_D"];
							ReturnedInvoiceHesab.Cd_Tafsl_Tkh_F_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_Tkh_F_D"]);
							ReturnedInvoiceHesab.Nam_Tafsl_Tkh_F_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_Tkh_F_D"]);
							ReturnedInvoiceHesab.Name_tbl_Hesab_Tkh_F_D = (string)reader["Name_tbl_Hesab_Tkh_F_D"];
							ReturnedInvoiceHesab.ID_tbl_Hesab_Tkh_F_D = (long)reader["ID_tbl_Hesab_Tkh_F_D"];

							ReturnedInvoiceHesab.Cd_Moen_Bah_F_D = (string)reader["Cd_Moen_Bah_F_D"];
							ReturnedInvoiceHesab.Nam_Moen_Bah_F_D = (string)reader["Nam_Moen_Bah_F_D"];
							ReturnedInvoiceHesab.Cd_Tafsl_Bah_F_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_Bah_F_D"]);
							ReturnedInvoiceHesab.Nam_Tafsl_Bah_F_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_Bah_F_D"]);
							ReturnedInvoiceHesab.Name_tbl_Hesab_Bah_F_D = (string)reader["Name_tbl_Hesab_Bah_F_D"];
							ReturnedInvoiceHesab.ID_tbl_Hesab_Bah_F_D = (long)reader["ID_tbl_Hesab_Bah_F_D"];

							ReturnedInvoiceHesab.Cd_Moen_Pors_F_D = (string)reader["Cd_Moen_Pors_F_D"];
							ReturnedInvoiceHesab.Nam_Moen_Pors_F_D = (string)reader["Nam_Moen_Pors_F_D"];
							ReturnedInvoiceHesab.Cd_Tafsl_Pors_F_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_Pors_F_D"]);
							ReturnedInvoiceHesab.Nam_Tafsl_Pors_F_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_Pors_F_D"]);
							ReturnedInvoiceHesab.Name_tbl_Hesab_Pors_F_D = (string)reader["Name_tbl_Hesab_Pors_F_D"];
							ReturnedInvoiceHesab.ID_tbl_Hesab_Pors_F_D = (long)reader["ID_tbl_Hesab_Pors_F_D"];

							ReturnedInvoiceHesab.Cd_Moen_Jayeze_F_D = (string)reader["Cd_Moen_Jayeze_F_D"];
							ReturnedInvoiceHesab.Nam_Moen_Jayeze_F_D = (string)reader["Nam_Moen_Jayeze_F_D"];
							ReturnedInvoiceHesab.Cd_Tafsl_Jayeze_F_D = ConvertFromDBVal<string>(reader["Cd_Tafsl_Jayeze_F_D"]);
							ReturnedInvoiceHesab.Nam_Tafsl_Jayeze_F_D = ConvertFromDBVal<string>(reader["Nam_Tafsl_Jayeze_F_D"]);
							ReturnedInvoiceHesab.Name_tbl_Hesab_Jayeze_F_D = (string)reader["Name_tbl_Hesab_Jayeze_F_D"];
							ReturnedInvoiceHesab.ID_tbl_Hesab_Jayeze_F_D = (long)reader["ID_tbl_Hesab_Jayeze_F_D"];
						}
					}
				}
				Console.WriteLine("After proc_Comp - Sett_F");
				#endregion

				int i = 0;
				decimal Mablagh = 0;
				decimal Mablagh_Koll = 0;
				decimal Mablagh_Bahaye_Tamam_Shode = 0;
				long ID_tbl_FBB_Str = 0;
				long N_FBB_Str = 0;
				long ID_tbl_S1_Str = 0;
				string partnerName = string.Empty;
				long partnerCode = 0;
				struct_FactorItem[] struct_FactorItem_Arr = new struct_FactorItem[factorItems.Count()];

				foreach (var item in factorItems)
				{
					struct_FactorItem_Arr[i].ID_tbl_Kala = item.ID_tbl_Kala;
					struct_FactorItem_Arr[i].ID_tbl_Anbar = item.ID_tbl_Anbar;
					struct_FactorItem_Arr[i].Mablagh_Koll_Gabl_Az_Takhfif = item.Tedad * item.Fi_Bed_Haz;
					struct_FactorItem_Arr[i].Mablagh_Koll_Pas_Az_Takhfif = item.Tedad * item.Fi_Ba_Haz;
					struct_FactorItem_Arr[i].Mablagh_Bahaye_Tamam_Shode = 0;
					struct_FactorItem_Arr[i].Fi_Sadere = 0;
					struct_FactorItem_Arr[i].Mablagh_Sadere = 0;
					struct_FactorItem_Arr[i].Mnd_Fi = 0;
					struct_FactorItem_Arr[i].Mnd_Megdar = 0;
					struct_FactorItem_Arr[i].Mnd_Mablagh = 0;
					struct_FactorItem_Arr[i].Mogayese_Shode_Ast = false;
					i++;
				}

				#region Genereate_Baha_Kala
				i = 0;
				foreach (var item in factorItems)
				{
					Mablagh_Koll += struct_FactorItem_Arr[i].Mablagh_Koll_Pas_Az_Takhfif;
					Mablagh_Bahaye_Tamam_Shode = 0;
					using (SqlCommand command = new("Proc_Kala", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						// مقدار دهی مبالغ بهای تمام شده در صورت منفی بودن کالا
						struct_FactorItem_Arr[i].Mablagh_Bahaye_Tamam_Shode = item.Tedad * item.Fi_Ba_Haz;
						struct_FactorItem_Arr[i].Fi_Sadere = item.Fi_Ba_Haz;
						struct_FactorItem_Arr[i].Mablagh_Sadere = item.Tedad * item.Fi_Ba_Haz;
						struct_FactorItem_Arr[i].Mnd_Fi = item.Fi_Ba_Haz;
						struct_FactorItem_Arr[i].Mnd_Megdar = item.Tedad;
						struct_FactorItem_Arr[i].Mnd_Mablagh = (item.Tedad * item.Fi_Ba_Haz);

						item.Fi_Sadere = struct_FactorItem_Arr[i].Fi_Sadere;
						item.Mablagh_Sadere = struct_FactorItem_Arr[i].Mablagh_Sadere;
						item.Mnd_Fi = struct_FactorItem_Arr[i].Mnd_Fi;
						item.Mnd_Megdar = struct_FactorItem_Arr[i].Mnd_Megdar;
						item.Mnd_Mablagh = struct_FactorItem_Arr[i].Mnd_Mablagh;

						Mablagh_Bahaye_Tamam_Shode += item.Tedad * item.Fi_Ba_Haz;
						// مقدار دهی مبالغ بهای تمام شده در صورت منفی بودن کالا

						command.Parameters.AddWithValue("@Type_Call_Proc", "Select_Last_Bah_Kala");
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", item.ID_tbl_SalMaly);
						command.Parameters.AddWithValue("@ID_tbl_Kala", item.ID_tbl_Kala);
						command.Parameters.AddWithValue("@ID_tbl_Users", returnedInvoice.ID_tbl_Users);
						command.Parameters.AddWithValue("@where", "");
						using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
						{

							if (reader.HasRows)
							{
								while (await reader.ReadAsync(cancellationToken))
								{
									struct_FactorItem_Arr[i].Mablagh_Bahaye_Tamam_Shode = item.Tedad * (decimal)reader["Mnd_Fi"];
									struct_FactorItem_Arr[i].Fi_Sadere = (decimal)reader["Mnd_Fi"];
									struct_FactorItem_Arr[i].Mablagh_Sadere = item.Tedad * (decimal)reader["Mnd_Fi"];
									struct_FactorItem_Arr[i].Mnd_Fi = (decimal)reader["Mnd_Fi"];
									struct_FactorItem_Arr[i].Mnd_Megdar = (decimal)reader["Mnd_Megdar"] + item.Tedad;
									struct_FactorItem_Arr[i].Mnd_Mablagh = (decimal)reader["Mnd_Mablagh"] + (item.Tedad * (decimal)reader["Mnd_Fi"]);

									item.Fi_Sadere = struct_FactorItem_Arr[i].Fi_Sadere;
									item.Mablagh_Sadere = struct_FactorItem_Arr[i].Mablagh_Sadere;
									item.Mnd_Fi = struct_FactorItem_Arr[i].Mnd_Fi;
									item.Mnd_Megdar = struct_FactorItem_Arr[i].Mnd_Megdar;
									item.Mnd_Mablagh = struct_FactorItem_Arr[i].Mnd_Mablagh;

									Mablagh_Bahaye_Tamam_Shode += item.Tedad * (decimal)reader["Mnd_Fi"];
								}
							}
							else
							{
								await reader.CloseAsync();
								using (SqlCommand command1 = new("Proc_Kala", connection))
								{
									command1.CommandType = CommandType.StoredProcedure;
									command1.Transaction = transaction;

									command1.Parameters.AddWithValue("@Type_Call_Proc", "Select_Last_Bah_Kala_From_Eftetahye");
									command1.Parameters.AddWithValue("@ID_tbl_SalMaly", item.ID_tbl_SalMaly);
									command1.Parameters.AddWithValue("@ID_tbl_Kala", item.ID_tbl_Kala);
									command1.Parameters.AddWithValue("@ID_tbl_Users", returnedInvoice.ID_tbl_Users);
									command1.Parameters.AddWithValue("@where", "");
									using (var reader1 = await command1.ExecuteReaderAsync(cancellationToken))
									{
										if (reader1.HasRows)
										{
											while (await reader1.ReadAsync(cancellationToken))
											{
												struct_FactorItem_Arr[i].Mablagh_Bahaye_Tamam_Shode = item.Tedad * (decimal)reader1["Last_Bah_Tam_Kala"];
												struct_FactorItem_Arr[i].Fi_Sadere = (decimal)reader1["Last_Bah_Tam_Kala"];
												struct_FactorItem_Arr[i].Mablagh_Sadere = item.Tedad * (decimal)reader1["Last_Bah_Tam_Kala"];
												struct_FactorItem_Arr[i].Mnd_Fi = (decimal)reader1["Last_Bah_Tam_Kala"];
												struct_FactorItem_Arr[i].Mnd_Megdar = (decimal)reader1["Avl_Td_Kala"] + item.Tedad;
												struct_FactorItem_Arr[i].Mnd_Mablagh = ((decimal)reader1["Avl_Td_Kala"] * (decimal)reader1["Last_Bah_Tam_Kala"]) + (item.Tedad * (decimal)reader1["Last_Bah_Tam_Kala"]);

												item.Fi_Sadere = struct_FactorItem_Arr[i].Fi_Sadere;
												item.Mablagh_Sadere = struct_FactorItem_Arr[i].Mablagh_Sadere;
												item.Mnd_Fi = struct_FactorItem_Arr[i].Mnd_Fi;
												item.Mnd_Megdar = struct_FactorItem_Arr[i].Mnd_Megdar;
												item.Mnd_Mablagh = struct_FactorItem_Arr[i].Mnd_Mablagh;

												Mablagh_Bahaye_Tamam_Shode += item.Tedad * (decimal)reader1["Last_Bah_Tam_Kala"];
											}
										}
									}
								}
							}
						}
					}
					i++;
				}
				#endregion

				#region Adding new Returnsale factor
				using (SqlCommand command = new SqlCommand("Apk_Proc_Add_tbl_FBB", connection))
				{
					command.Transaction = transaction;
					command.CommandType = CommandType.StoredProcedure;
					var properties = returnedInvoice.GetType().GetProperties().ToList();
					properties.RemoveAll(b => b.Name == "ID_tbl_FBB");
					properties.RemoveAll(b => b.Name == "FiscalYearBeginDate");
					properties.RemoveAll(b => b.Name == "FiscalYearEndDate");
					properties.RemoveAll(b => b.Name == "FiscalYearTitle");
					properties.RemoveAll(b => b.Name == "Name_TarafHesab");
					properties.RemoveAll(b => b.Name == "Code_TarafHesab");
					properties.RemoveAll(b => b.Name == "ChelPhone_TarafHesab");
					properties.RemoveAll(b => b.Name == "Name_Bzy");
					properties.RemoveAll(b => b.Name == "Username");
					properties.RemoveAll(b => b.Name == "Name_Kala");
					properties.RemoveAll(b => b.Name == "BarCode_Kala");
					properties.RemoveAll(b => b.Name == "Code_Kala");
					properties.RemoveAll(b => b.Name == "Shenase_Kala");
					properties.RemoveAll(b => b.Name == "Vahede_Name");
					properties.RemoveAll(b => b.Name == "Name_Anbar");
					properties.RemoveAll(b => b.Name == "Code_Anbar");
					properties.RemoveAll(b => b.Name == "Tedad");
					properties.RemoveAll(b => b.Name == "Fi");
					properties.RemoveAll(b => b.Name == "Mablagh");
					properties.RemoveAll(b => b.Name == "Name_Branch");
					foreach (var p in properties)
					{
						command.Parameters.AddWithValue($"@{p.Name}", p.GetValue(returnedInvoice));
					}
					ID_tbl_FBB_Str = (long)(decimal)await command.ExecuteScalarAsync(cancellationToken);
				}
				foreach (var item in factorItems)
				{
					using (SqlCommand command = new("APK_Proc_Add_tbl_F_Aglm", connection))
					{
						command.Transaction = transaction;
						command.CommandType = CommandType.StoredProcedure;
						var properties = item.GetType().GetProperties().ToList();
						properties.RemoveAll(b => b.Name == "Notifications");
						properties.RemoveAll(b => b.Name == "Type_tbl_F");

						item.ID_tbl_F = ID_tbl_FBB_Str;
						item.ID_tbl_SalMaly = returnedInvoice.ID_tbl_SalMaly;
						command.Parameters.AddWithValue("@Type_tbl_F", "tbl_FBB");
						foreach (var p in properties)
						{
							command.Parameters.AddWithValue($"@{p.Name}", p.GetValue(item));
						}

						var result = await command.ExecuteNonQueryAsync();
					}
				}
				#endregion

				#region Fetching Number Inserted Sale Factor
				using (SqlCommand command = new("select N_FBB from tbl_FBB where ID_tbl_FBB=" + ID_tbl_FBB_Str, connection))
				{
					command.Transaction = transaction;
					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							N_FBB_Str = (long)reader["N_FBB"];
						}
					}
				}
				#endregion

				#region fetchin partner name by id
				using (SqlCommand command = new("select Name_TarafHesab,Code_TarafHesab from tbl_TarafHesab where ID_tbl_TarafHesab = @PartnerId", connection))
				{
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@PartnerId", returnedInvoice.ID_tbl_TarafHesab);
					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							partnerName = (string)reader["Name_TarafHesab"];
							partnerCode = (long)reader["Code_TarafHesab"];
						}
					}
				}
				#endregion

				#region Fetching ID_tbl_S1
				using (SqlCommand command = new("Apk_Proc_Create_ID_tbl_S1", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", returnedInvoice.ID_tbl_SalMaly);

					using (var reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							ID_tbl_S1_Str = (long)(decimal)reader["ID_tbl_S1"];
						}
					}
				}
				#endregion

				#region Adding to table S1
				using (SqlCommand command = new("Apk_Proc_Add_tbl_S1", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@Date_S1", returnedInvoice.Dt_FBB);
					command.Parameters.AddWithValue("@Time_S1", returnedInvoice.Tm_C_FBB);
					command.Parameters.AddWithValue("@Desc_S1", " بابت فاکتور برگشت از فروش داخلی به شماره " + N_FBB_Str + " - طرف حساب " + partnerName);
					command.Parameters.AddWithValue("@Type_S1", "سایر سیستم ها");
					command.Parameters.AddWithValue("@Type_ES1", "Sayer");
					command.Parameters.AddWithValue("@Vazyat_S1", "موقت");
					command.Parameters.AddWithValue("@J_S1", 0);
					command.Parameters.AddWithValue("@Date_C_S1", returnedInvoice.Dt_FBB);
					command.Parameters.AddWithValue("@Time_C_S1", returnedInvoice.Tm_C_FBB);
					command.Parameters.AddWithValue("@ID_tbl_Users", returnedInvoice.ID_tbl_Users);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", returnedInvoice.ID_tbl_SalMaly);

					await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Updating ID_tbl_S1 in tbl_FBB
				using (SqlCommand command = new("update tbl_FBB set ID_tbl_S1=@ID_tbl_S1 where ID_tbl_FBB=@ID_tbl_FBB", connection))
				{
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@ID_tbl_FBB", ID_tbl_FBB_Str);

					var result = await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Partner_as_the_Bedehkar_of_the_document
				if (Mablagh_Bahaye_Tamam_Shode > 0)
				{
					string Name_Hesab_Sarfasl = "";
					using (SqlCommand command = new("select Name_Hesab_Sarfasl from tbl_Sarfasl where Code_Hesab_Sarfasl=N'110301'", connection))
					{
						command.Transaction = transaction;
						using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
						{
							while (await reader.ReadAsync(cancellationToken))
							{
								Name_Hesab_Sarfasl = (string)reader["Name_Hesab_Sarfasl"];
							}
						}
					}

					Mablagh = returnedInvoice.JM_FBB + returnedInvoice.JAv_FBB + Mablagh_Koll - returnedInvoice.Tkh_N_FBB;
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", partnerName);
						command.Parameters.AddWithValue("@Code_Hst_S2", partnerCode.ToString());
						command.Parameters.AddWithValue("@Name_HsMt_S2", Name_Hesab_Sarfasl);
						command.Parameters.AddWithValue("@Code_HsMt_S2", "110301");
						command.Parameters.AddWithValue("@Code_HsKt_S2", "1103");
						command.Parameters.AddWithValue("@Code_HsGt_S2", "11");
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", "tbl_TarafHesab");
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", returnedInvoice.ID_tbl_TarafHesab);
						command.Parameters.AddWithValue("@Desc_S2", " بابت فاکتور برگشت از فروش داخلی به شماره " + N_FBB_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", Mablagh);
						command.Parameters.AddWithValue("@Bestankar_S2", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور برگشت از فروش");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FBB");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FBB_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", returnedInvoice.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Account_Sale_as_the_Bestankar_of_the_document
				using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;

					command.Parameters.AddWithValue("@Name_Hst_S2", ReturnedInvoiceHesab.Nam_Tafsl_BF_D);
					command.Parameters.AddWithValue("@Code_Hst_S2", ReturnedInvoiceHesab.Cd_Tafsl_BF_D);
					command.Parameters.AddWithValue("@Name_HsMt_S2", ReturnedInvoiceHesab.Nam_Moen_BF_D);
					command.Parameters.AddWithValue("@Code_HsMt_S2", ReturnedInvoiceHesab.Cd_Moen_BF_D);
					command.Parameters.AddWithValue("@Code_HsKt_S2", ReturnedInvoiceHesab.Cd_Moen_BF_D.Substring(0, 4));
					command.Parameters.AddWithValue("@Code_HsGt_S2", ReturnedInvoiceHesab.Cd_Moen_BF_D.Substring(0, 2));
					command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ReturnedInvoiceHesab.Name_tbl_Hesab_BF_D);
					command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ReturnedInvoiceHesab.ID_tbl_Hesab_BF_D);
					command.Parameters.AddWithValue("@Desc_S2", " بابت فاکتور برگشت از فروش داخلی به شماره " + N_FBB_Str + " - طرف حساب " + partnerName);
					command.Parameters.AddWithValue("@Bedehkar_S2", 0);
					command.Parameters.AddWithValue("@Bestankar_S2", Mablagh_Koll);
					command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
					command.Parameters.AddWithValue("@Mbg_Arz1", 0);
					command.Parameters.AddWithValue("@Tmbg", 0);
					command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور برگشت از فروش");
					command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FBB");
					command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FBB_Str);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", returnedInvoice.ID_tbl_SalMaly);

					var result = await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Malyat_as_the_Bestankar_of_the_document
				if (returnedInvoice.JM_FBB > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", ReturnedInvoiceHesab.Nam_Tafsl_Mal_F_D);
						command.Parameters.AddWithValue("@Code_Hst_S2", ReturnedInvoiceHesab.Cd_Tafsl_Mal_F_D);
						command.Parameters.AddWithValue("@Name_HsMt_S2", ReturnedInvoiceHesab.Nam_Moen_Mal_F_D);
						command.Parameters.AddWithValue("@Code_HsMt_S2", ReturnedInvoiceHesab.Cd_Moen_Mal_F_D);
						command.Parameters.AddWithValue("@Code_HsKt_S2", ReturnedInvoiceHesab.Cd_Moen_Mal_F_D.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", ReturnedInvoiceHesab.Cd_Moen_Mal_F_D.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ReturnedInvoiceHesab.Name_tbl_Hesab_Mal_F_D);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ReturnedInvoiceHesab.ID_tbl_Hesab_Mal_F_D);
						command.Parameters.AddWithValue("@Desc_S2", " مالیات بابت فاکتور برگشت از فروش داخلی به شماره " + N_FBB_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", 0);
						command.Parameters.AddWithValue("@Bestankar_S2", returnedInvoice.JM_FBB);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور برگشت از فروش");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FBB");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FBB_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", returnedInvoice.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Avarez_as_the_Bestankar_of_the_document
				if (returnedInvoice.JAv_FBB > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", ReturnedInvoiceHesab.Nam_Tafsl_AV_F_D);
						command.Parameters.AddWithValue("@Code_Hst_S2", ReturnedInvoiceHesab.Cd_Tafsl_AV_F_D);
						command.Parameters.AddWithValue("@Name_HsMt_S2", ReturnedInvoiceHesab.Nam_Moen_AV_F_D);
						command.Parameters.AddWithValue("@Code_HsMt_S2", ReturnedInvoiceHesab.Cd_Moen_AV_F_D);
						command.Parameters.AddWithValue("@Code_HsKt_S2", ReturnedInvoiceHesab.Cd_Moen_AV_F_D.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", ReturnedInvoiceHesab.Cd_Moen_AV_F_D.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ReturnedInvoiceHesab.Name_tbl_Hesab_AV_F_D);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ReturnedInvoiceHesab.ID_tbl_Hesab_AV_F_D);
						command.Parameters.AddWithValue("@Desc_S2", " عوارض بابت فاکتور برگشت از فروش داخلی به شماره " + N_FBB_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", 0);
						command.Parameters.AddWithValue("@Bestankar_S2", returnedInvoice.JAv_FBB);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور برگشت از فروش");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FBB");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FBB_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", returnedInvoice.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Takhfif_as_the_Bedehkar_of_the_document
				if (returnedInvoice.Tkh_N_FBB > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", ReturnedInvoiceHesab.Nam_Tafsl_Tkh_F_D);
						command.Parameters.AddWithValue("@Code_Hst_S2", ReturnedInvoiceHesab.Cd_Tafsl_Tkh_F_D);
						command.Parameters.AddWithValue("@Name_HsMt_S2", ReturnedInvoiceHesab.Nam_Moen_Tkh_F_D);
						command.Parameters.AddWithValue("@Code_HsMt_S2", ReturnedInvoiceHesab.Cd_Moen_Tkh_F_D);
						command.Parameters.AddWithValue("@Code_HsKt_S2", ReturnedInvoiceHesab.Cd_Moen_Tkh_F_D.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", ReturnedInvoiceHesab.Cd_Moen_Tkh_F_D.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ReturnedInvoiceHesab.Name_tbl_Hesab_Tkh_F_D);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ReturnedInvoiceHesab.ID_tbl_Hesab_Tkh_F_D);
						command.Parameters.AddWithValue("@Desc_S2", " تخفيف نقدی بابت فاکتور برگشت از فروش داخلی به شماره " + N_FBB_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", returnedInvoice.Tkh_N_FBB);
						command.Parameters.AddWithValue("@Bestankar_S2", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور برگشت از فروش");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FBB");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FBB_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", returnedInvoice.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Baha_as_the_Bedehkar_of_the_document
				using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;

					command.Parameters.AddWithValue("@Name_Hst_S2", ReturnedInvoiceHesab.Nam_Tafsl_Bah_F_D);
					command.Parameters.AddWithValue("@Code_Hst_S2", ReturnedInvoiceHesab.Cd_Tafsl_Bah_F_D);
					command.Parameters.AddWithValue("@Name_HsMt_S2", ReturnedInvoiceHesab.Nam_Moen_Bah_F_D);
					command.Parameters.AddWithValue("@Code_HsMt_S2", ReturnedInvoiceHesab.Cd_Moen_Bah_F_D);
					command.Parameters.AddWithValue("@Code_HsKt_S2", ReturnedInvoiceHesab.Cd_Moen_Bah_F_D.Substring(0, 4));
					command.Parameters.AddWithValue("@Code_HsGt_S2", ReturnedInvoiceHesab.Cd_Moen_Bah_F_D.Substring(0, 2));
					command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ReturnedInvoiceHesab.Name_tbl_Hesab_Bah_F_D);
					command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ReturnedInvoiceHesab.ID_tbl_Hesab_Bah_F_D);
					command.Parameters.AddWithValue("@Desc_S2", " بهای تمام شده کالای برگشت از فروش رفته داخلی به شماره " + N_FBB_Str + " - طرف حساب " + partnerName);
					command.Parameters.AddWithValue("@Bedehkar_S2", Math.Round(Mablagh_Bahaye_Tamam_Shode));
					command.Parameters.AddWithValue("@Bestankar_S2", 0);
					command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
					command.Parameters.AddWithValue("@Mbg_Arz1", 0);
					command.Parameters.AddWithValue("@Tmbg", 0);
					command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور برگشت از فروش");
					command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FBB");
					command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FBB_Str);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", returnedInvoice.ID_tbl_SalMaly);

					var result = await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Stock_as_the_Bestankar_of_the_document
				bool flagg = false;
				for (i = 0; i < struct_FactorItem_Arr.Length; i++)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;
						Mablagh = 0;
						flagg = false;
						for (int jj = 0; jj < struct_FactorItem_Arr.Length; jj++)
						{
							if ((struct_FactorItem_Arr[i].ID_tbl_Anbar == struct_FactorItem_Arr[jj].ID_tbl_Anbar)
								&& struct_FactorItem_Arr[jj].Mogayese_Shode_Ast == false)
							{
								flagg = true;
								struct_FactorItem_Arr[jj].Mogayese_Shode_Ast = true;
								Mablagh += struct_FactorItem_Arr[jj].Mablagh_Bahaye_Tamam_Shode;
							}
						}
						if (Mablagh != 0 || flagg == true)
						{
							//===========================================================================
							using (SqlCommand command_Stock_Information = new(@"select tbl_Anbar.*,Name_Hesab_Sarfasl from tbl_Anbar 
                                              inner join tbl_Sarfasl on tbl_Anbar.Code_Hesab_Sarfasl_Moein_Anbar=tbl_Sarfasl.Code_Hesab_Sarfasl
                                              where ID_tbl_Anbar=" + struct_FactorItem_Arr[i].ID_tbl_Anbar, connection))
							{
								command_Stock_Information.Transaction = transaction;
								using (var reader_Stock_Information = await command_Stock_Information.ExecuteReaderAsync(cancellationToken))
								{
									while (await reader_Stock_Information.ReadAsync(cancellationToken))
									{
										command.Parameters.AddWithValue("@Name_Hst_S2", (reader_Stock_Information["Name_Anbar"]).ToString());
										command.Parameters.AddWithValue("@Code_Hst_S2", (reader_Stock_Information["Code_Anbar"]).ToString());
										command.Parameters.AddWithValue("@Name_HsMt_S2", (reader_Stock_Information["Name_Hesab_Sarfasl"]).ToString());
										command.Parameters.AddWithValue("@Code_HsMt_S2", (reader_Stock_Information["Code_Hesab_Sarfasl_Moein_Anbar"]).ToString());
										command.Parameters.AddWithValue("@Code_HsKt_S2", (reader_Stock_Information["Code_Hesab_Sarfasl_Moein_Anbar"]).ToString().Substring(0, 4));
										command.Parameters.AddWithValue("@Code_HsGt_S2", (reader_Stock_Information["Code_Hesab_Sarfasl_Moein_Anbar"]).ToString().Substring(0, 2));
										command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", "tbl_Anbar");
										command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", long.Parse((reader_Stock_Information["ID_tbl_Anbar"]).ToString()));
									}
								}
							}
							//===========================================================================

							command.Parameters.AddWithValue("@Desc_S2", " بابت فاکتور برگشت از فروش داخلی به شماره " + N_FBB_Str + " - طرف حساب " + partnerName);
							command.Parameters.AddWithValue("@Bedehkar_S2", 0);
							command.Parameters.AddWithValue("@Bestankar_S2", Mablagh);
							command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
							command.Parameters.AddWithValue("@Mbg_Arz1", 0);
							command.Parameters.AddWithValue("@Tmbg", 0);
							command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
							command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
							command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
							command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
							command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور برگشت از فروش");
							command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FBB");
							command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FBB_Str);
							command.Parameters.AddWithValue("@ID_tbl_SalMaly", returnedInvoice.ID_tbl_SalMaly);

							var result = await command.ExecuteNonQueryAsync(cancellationToken);
						}
						command.Parameters.Clear();
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
	public async Task<List<ReturnedInvoice>> GetReturnedFactorPrint(long? ForoshID, long? fiscalYear, int? UserId, int? Offset, int? ID_tbl_Bzy, string? Date1, string? Date2, object parameters = null!, CancellationToken cancellationToken = default)
	{
		IEnumerable<ReturnedInvoice> Factor = await _dbManager.CallProcedureWithParametersAsync<ReturnedInvoice>("Apk_Proc_Report_BargashtForosh", new
		{
			Type_Call_Proc = "Select_Top_N_Factor",
			ID_tbl_SalMaly = fiscalYear,
			ID_tbl_F = ForoshID,
			ID_tbl_TarafHesab = UserId,
			Date1 = Date1,
			Date2 = Date2,
			ID_tbl_Bzy = ID_tbl_Bzy,
			Offset = Offset,
		});
		return new(Factor);
	}
	public async Task<List<ReturnedInvoice>> GetReturnedInvoices(int fiscalYear, int UserId, string Date1, string Date2, object parameters = null!, CancellationToken cancellationToken = default)
	{

		//IEnumerable<ReturnedInvoice> ReturnedInvoice = await _dbManager.CallProcedureWithParametersAsync<ReturnedInvoice>("Apk_Proc_Get_List_tbl_FBB", new
		//{
		//    ID_tbl_SalMaly = fiscalYear,
		//    ID_tbl_Users = UserId,
		//    @Dt_F1 = Date1,
		//    @Dt_F2 = Date2,
		//});
		//return new(ReturnedInvoice);

		IEnumerable<ReturnedInvoice> ReturnedInvoice = Enumerable.Empty<ReturnedInvoice>();
		if (parameters == null)
		{
			ReturnedInvoice = await _dbManager.CallProcedureWithParametersAsync<ReturnedInvoice>("Apk_Proc_Get_List_tbl_FBB",
				new
				{
					ID_tbl_SalMaly = fiscalYear,
					ID_tbl_Users = UserId,
					@Dt_F1 = Date1,
					@Dt_F2 = Date2,
				});
		}
		else
		{
			ReturnedInvoice = await _dbManager.CallProcedureWithParametersAsync<ReturnedInvoice>("Apk_Proc_Get_List_tbl_FBB",
				parameters, cancellationToken);
		}

		return new(ReturnedInvoice);
	}

	//public async Task<SysResult<List<SaleServiceFactorDetailViewModel>>> GetServiceSaleFactorDetailAsync(long id, int SalMalyId, CancellationToken cancellationToken = default)
	//{
	//	int? SalMaly = null;
	//	long? TarafHesab = null;
	//	string? From = null;
	//	string? to = null;
	//	long? ID_Bzy = null;
	//	int? offSet = null;

	//	IEnumerable<SaleServiceFactorDetailViewModel> partner = await _dbManager.CallSingleRowProcedureWithParametersAsync<SaleServiceFactorDetailViewModel>("Apk_Proc_Report_Forosh_Khedmat", new
	//	{
	//		Type_Call_Proc = "Load_Factor",
	//		ID_tbl_F = id,
	//		ID_tbl_SalMaly = SalMalyId,
	//		ID_tbl_TarafHesab = TarafHesab,
	//		Date1 = From,
	//		Date2 = to,
	//		ID_tbl_Bzy = ID_Bzy,
	//		Offset = offSet,
	//	});

	//	return new(partner);

	//}





	////////////////////////////////////////////ویرایش فاکتور خدمت
	public async Task<List<SaleServiceFactorDetailViewModel>> GetServiceSaleFactorDetailAsync(long id, int SalMalyId, CancellationToken cancellationToken = default)
	{
		int? SalMaly = null;
		long? TarafHesab = null;
		string? From = null;
		string? to = null;
		long? ID_Bzy = null;
		int? offSet = null;
		IEnumerable<SaleServiceFactorDetailViewModel> Factor = await _dbManager.CallProcedureWithParametersAsync<SaleServiceFactorDetailViewModel>("Apk_Proc_Report_Forosh_Khedmat", new
		{
			Type_Call_Proc = "Load_Factor",
			ID_tbl_F = id,
			ID_tbl_SalMaly = SalMalyId,
			ID_tbl_TarafHesab = TarafHesab,
			Date1 = From,
			Date2 = to,
			ID_tbl_Bzy = ID_Bzy,
			Offset = offSet,

		});
		return new(Factor);
	}



	public async Task<SysResult<bool>> EditSaleServiceFactor(SaleServiceFactor saleServiceFactor, IEnumerable<ServiceFactorItem> factorItems, CancellationToken cancellationToken = default)
	{
		//Console.WriteLine(JsonSerializer.Serialize(saleServiceFactor));
		Console.WriteLine(JsonSerializer.Serialize(factorItems));
		using (SqlConnection connection = new(_configuration.GetConnectionString("Default")))
		{
			await connection.OpenAsync();
			SqlTransaction transaction = connection.BeginTransaction();

			try
			{
				ServiceSaleFactorHesab ServiceSaleFactorHesab = new();
				#region Fetch_Accounts_For_Service_Sale_Factor
				using (SqlCommand command = new("Proc_Comp", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;

					command.Parameters.AddWithValue("@Type_Call_Proc", "Sett_F_khed");
					command.Parameters.AddWithValue("@ID_tbl", 1);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);
					command.Parameters.AddWithValue("@Where_Query", "-");

					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync())
						{
							if ((long)reader["ID_tbl_Hesab_DrAmad_Khed"] == 0 ||
								(long)reader["ID_tbl_Hesab_Mal_Khed"] == 0 ||
								(long)reader["ID_tbl_Hesab_Av_Khed"] == 0 ||
								(long)reader["ID_tbl_Hesab_Tkhf_Khed"] == 0 ||
								(long)reader["ID_tbl_Hesab_PorsMoj_Khed"] == 0 ||
								(long)reader["ID_tbl_Hesab_Bargasht_Khed"] == 0)
							{
								return new(false, false, new() { "حساب های فروش خدمت تنظیم نشده است." });
							}
							ServiceSaleFactorHesab.Cd_Moen_Hesab_DrAmad_Khed = (string)reader["Cd_Moen_Hesab_DrAmad_Khed"];
							ServiceSaleFactorHesab.Nam_Moen_Hesab_DrAmad_Khed = (string)reader["Nam_Moen_Hesab_DrAmad_Khed"];
							ServiceSaleFactorHesab.Cd_Tafsl_Hesab_DrAmad_Khed = ConvertFromDBVal<string>(reader["Cd_Tafsl_Hesab_DrAmad_Khed"]);
							ServiceSaleFactorHesab.Nam_Tafsl_Hesab_DrAmad_Khed = ConvertFromDBVal<string>(reader["Nam_Tafsl_Hesab_DrAmad_Khed"]);
							ServiceSaleFactorHesab.Name_tbl_Hesab_DrAmad_Khed = (string)reader["Name_tbl_Hesab_DrAmad_Khed"];
							ServiceSaleFactorHesab.ID_tbl_Hesab_DrAmad_Khed = (long)reader["ID_tbl_Hesab_DrAmad_Khed"];

							ServiceSaleFactorHesab.Cd_Moen_Hesab_Mal_Khed = (string)reader["Cd_Moen_Hesab_Mal_Khed"];
							ServiceSaleFactorHesab.Nam_Moen_Hesab_Mal_Khed = (string)reader["Nam_Moen_Hesab_Mal_Khed"];
							ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Mal_Khed = ConvertFromDBVal<string>(reader["Cd_Tafsl_Hesab_Mal_Khed"]);
							ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Mal_Khed = ConvertFromDBVal<string>(reader["Nam_Tafsl_Hesab_Mal_Khed"]);
							ServiceSaleFactorHesab.Name_tbl_Hesab_Mal_Khed = (string)reader["Name_tbl_Hesab_Mal_Khed"];
							ServiceSaleFactorHesab.ID_tbl_Hesab_Mal_Khed = (long)reader["ID_tbl_Hesab_Mal_Khed"];

							ServiceSaleFactorHesab.Cd_Moen_Hesab_Av_Khed = (string)reader["Cd_Moen_Hesab_Av_Khed"];
							ServiceSaleFactorHesab.Nam_Moen_Hesab_Av_Khed = (string)reader["Nam_Moen_Hesab_Av_Khed"];
							ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Av_Khed = ConvertFromDBVal<string>(reader["Cd_Tafsl_Hesab_Av_Khed"]);
							ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Av_Khed = ConvertFromDBVal<string>(reader["Nam_Tafsl_Hesab_Av_Khed"]);
							ServiceSaleFactorHesab.Name_tbl_Hesab_Av_Khed = (string)reader["Name_tbl_Hesab_Av_Khed"];
							ServiceSaleFactorHesab.ID_tbl_Hesab_Av_Khed = (long)reader["ID_tbl_Hesab_Av_Khed"];

							ServiceSaleFactorHesab.Cd_Moen_Hesab_Tkhf_Khed = (string)reader["Cd_Moen_Hesab_Tkhf_Khed"];
							ServiceSaleFactorHesab.Nam_Moen_Hesab_Tkhf_Khed = (string)reader["Nam_Moen_Hesab_Tkhf_Khed"];
							ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Tkhf_Khed = ConvertFromDBVal<string>(reader["Cd_Tafsl_Hesab_Tkhf_Khed"]);
							ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Tkhf_Khed = ConvertFromDBVal<string>(reader["Nam_Tafsl_Hesab_Tkhf_Khed"]);
							ServiceSaleFactorHesab.Name_tbl_Hesab_Tkhf_Khed = (string)reader["Name_tbl_Hesab_Tkhf_Khed"];
							ServiceSaleFactorHesab.ID_tbl_Hesab_Tkhf_Khed = (long)reader["ID_tbl_Hesab_Tkhf_Khed"];

							ServiceSaleFactorHesab.Cd_Moen_Hesab_PorsMoj_Khed = (string)reader["Cd_Moen_Hesab_PorsMoj_Khed"];
							ServiceSaleFactorHesab.Nam_Moen_Hesab_PorsMoj_Khed = (string)reader["Nam_Moen_Hesab_PorsMoj_Khed"];
							ServiceSaleFactorHesab.Cd_Tafsl_Hesab_PorsMoj_Khed = ConvertFromDBVal<string>(reader["Cd_Tafsl_Hesab_PorsMoj_Khed"]);
							ServiceSaleFactorHesab.Nam_Tafsl_Hesab_PorsMoj_Khed = ConvertFromDBVal<string>(reader["Nam_Tafsl_Hesab_PorsMoj_Khed"]);
							ServiceSaleFactorHesab.Name_tbl_Hesab_PorsMoj_Khed = ConvertFromDBVal<string>(reader["Name_tbl_Hesab_PorsMoj_Khed"]);
							ServiceSaleFactorHesab.ID_tbl_Hesab_PorsMoj_Khed = (long)reader["ID_tbl_Hesab_PorsMoj_Khed"];

							ServiceSaleFactorHesab.Cd_Moen_Hesab_Bargasht_Khed = (string)reader["Cd_Moen_Hesab_Bargasht_Khed"];
							ServiceSaleFactorHesab.Nam_Moen_Hesab_Bargasht_Khed = (string)reader["Nam_Moen_Hesab_Bargasht_Khed"];
							ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Bargasht_Khed = ConvertFromDBVal<string>(reader["Cd_Tafsl_Hesab_Bargasht_Khed"]);
							ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Bargasht_Khed = ConvertFromDBVal<string>(reader["Nam_Tafsl_Hesab_Bargasht_Khed"]);
							ServiceSaleFactorHesab.Name_tbl_Hesab_Bargasht_Khed = (string)reader["Name_tbl_Hesab_Bargasht_Khed"];
							ServiceSaleFactorHesab.ID_tbl_Hesab_Bargasht_Khed = (long)reader["ID_tbl_Hesab_Bargasht_Khed"];

							ServiceSaleFactorHesab.chkSbt_Snd_Pors_Khed = (bool)reader["chkSbt_Snd_Pors_Khed"];
						}
					}
				}
				#endregion

				int i = 0;
				decimal Mablagh = 0;
				decimal Mablagh_Koll = 0;
				decimal Mablagh_Jam_Porsant = 0;
				long ID_tbl_FF_KHed_Str = 0;
				long N_FF_KHed_Str = 0;
				long ID_tbl_S1_Str = 0;
				string partnerName = string.Empty;
				long partnerCode = 0;
				struct_ServiceFactorItem[] struct_FactorItem_Arr = new struct_ServiceFactorItem[factorItems.Count()];

				foreach (var item in factorItems)
				{
					Mablagh_Koll += item.Tedad * item.Fi_Ba_Haz;
					Mablagh_Jam_Porsant += Math.Round(item.Pors_Mbg_Rdf);
					struct_FactorItem_Arr[i].ID_tbl_Khedmat = item.ID_tbl_Khedmat;
					struct_FactorItem_Arr[i].ID_tbl_TarafHesab_Moj_Rdf = item.ID_tbl_TarafHesab_Moj_Rdf;
					struct_FactorItem_Arr[i].Pors_Dr_Rdf = item.Pors_Dr_Rdf;
					struct_FactorItem_Arr[i].Pors_Mbg_Rdf = Math.Round(item.Pors_Mbg_Rdf);
					struct_FactorItem_Arr[i].Mogayese_Shode_Ast = false;
					i++;
				}

				#region Adding new sale factor
				using (SqlCommand command = new SqlCommand("Apk_Proc_Add_tbl_FF_KHed", connection))
				{
					command.Transaction = transaction;
					command.CommandType = CommandType.StoredProcedure;
					var properties = saleServiceFactor.GetType().GetProperties().ToList();
					properties.RemoveAll(b => b.Name == "Notifications");
					properties.RemoveAll(b => b.Name == "ID_tbl_FF_KHed");
					properties.RemoveAll(b => b.Name == "FiscalYearBeginDate");
					properties.RemoveAll(b => b.Name == "FiscalYearEndDate");
					properties.RemoveAll(b => b.Name == "FiscalYearTitle");
					properties.RemoveAll(b => b.Name == "Tedad");
					properties.RemoveAll(b => b.Name == "Fi");
					properties.RemoveAll(b => b.Name == "Mablagh");
					properties.RemoveAll(b => b.Name == "Name_Branch");
					properties.RemoveAll(b => b.Name == "Name_TarafHesab");
					properties.RemoveAll(b => b.Name == "Code_TarafHesab");
					properties.RemoveAll(b => b.Name == "ChelPhone_TarafHesab");
					properties.RemoveAll(b => b.Name == "Name_Bzy");
					properties.RemoveAll(b => b.Name == "JM_F");
					properties.RemoveAll(b => b.Name == "Name_TarafHesab_Moj_Rdf");
					properties.RemoveAll(b => b.Name == "Code_TarafHesab_Moj_Rdf");
					properties.RemoveAll(b => b.Name == "Name_Khedmat");
					properties.RemoveAll(b => b.Name == "Cde_Khedmat");
					properties.RemoveAll(b => b.Name == "Shenase_Khedmat");
					foreach (var p in properties)
					{
						command.Parameters.AddWithValue($"@{p.Name}", p.GetValue(saleServiceFactor));
					}
					ID_tbl_FF_KHed_Str = (long)(decimal)await command.ExecuteScalarAsync(cancellationToken);
				}
				foreach (var item in factorItems)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_F_Aglm_Khed", connection))
					{
						command.Transaction = transaction;
						command.CommandType = CommandType.StoredProcedure;
						var properties = item.GetType().GetProperties().ToList();
						properties.RemoveAll(b => b.Name == "Notifications");
						properties.RemoveAll(b => b.Name == "Type_tbl_F");

						item.ID_tbl_F = ID_tbl_FF_KHed_Str;
						item.ID_tbl_SalMaly = saleServiceFactor.ID_tbl_SalMaly;
						command.Parameters.AddWithValue("@Type_tbl_F", "tbl_FF_KHed");
						foreach (var p in properties)
						{
							command.Parameters.AddWithValue($"@{p.Name}", p.GetValue(item));
						}

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Fetching Number Inserted Sale Factor
				using (SqlCommand command = new("select N_FF_KHed from tbl_FF_KHed where ID_tbl_FF_KHed=" + ID_tbl_FF_KHed_Str, connection))
				{
					command.Transaction = transaction;
					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							N_FF_KHed_Str = (long)reader["N_FF_KHed"];
						}
					}
				}
				#endregion

				#region fetchin partner name by id
				using (SqlCommand command = new("select Name_TarafHesab,Code_TarafHesab from tbl_TarafHesab where ID_tbl_TarafHesab = @PartnerId", connection))
				{
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@PartnerId", saleServiceFactor.ID_tbl_TarafHesab);
					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							partnerName = (string)reader["Name_TarafHesab"];
							partnerCode = (long)reader["Code_TarafHesab"];
						}
					}
				}
				#endregion

				#region Fetching ID_tbl_S1
				using (SqlCommand command = new("Apk_Proc_Create_ID_tbl_S1", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

					using (var reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							ID_tbl_S1_Str = (long)(decimal)reader["ID_tbl_S1"];
						}
					}
				}
				#endregion

				#region Adding to table S1
				using (SqlCommand command = new("Apk_Proc_Add_tbl_S1", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@Date_S1", saleServiceFactor.Dt_FF_KHed);
					command.Parameters.AddWithValue("@Time_S1", saleServiceFactor.Tm_C_FF_KHed);
					command.Parameters.AddWithValue("@Desc_S1", " بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
					command.Parameters.AddWithValue("@Type_S1", "سایر سیستم ها");
					command.Parameters.AddWithValue("@Type_ES1", "Sayer");
					command.Parameters.AddWithValue("@Vazyat_S1", "موقت");
					command.Parameters.AddWithValue("@J_S1", 0);
					command.Parameters.AddWithValue("@Date_C_S1", saleServiceFactor.Dt_FF_KHed);
					command.Parameters.AddWithValue("@Time_C_S1", saleServiceFactor.Tm_C_FF_KHed);
					command.Parameters.AddWithValue("@ID_tbl_Users", saleServiceFactor.ID_tbl_Users);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

					var result = await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Updating ID_tbl_S1 in tbl_FF_KHed
				using (SqlCommand command = new("update tbl_FF_KHed set ID_tbl_S1=@ID_tbl_S1 where ID_tbl_FF_KHed=@ID_tbl_FF_KHed", connection))
				{
					command.Transaction = transaction;
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@ID_tbl_FF_KHed", ID_tbl_FF_KHed_Str);

					var result = await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Partner_as_the_Bedehkar_of_the_document
				string Name_Hesab_Sarfasl = "";
				using (SqlCommand command = new("select Name_Hesab_Sarfasl from tbl_Sarfasl where Code_Hesab_Sarfasl=N'110301'", connection))
				{
					command.Transaction = transaction;
					using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
					{
						while (await reader.ReadAsync(cancellationToken))
						{
							Name_Hesab_Sarfasl = (string)reader["Name_Hesab_Sarfasl"];
						}
					}
				}

				Mablagh = saleServiceFactor.JM_FF_KHed + saleServiceFactor.JAv_FF_KHed + Mablagh_Koll - saleServiceFactor.Tkh_N_FF_KHed;
				using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;

					command.Parameters.AddWithValue("@Name_Hst_S2", partnerName);
					command.Parameters.AddWithValue("@Code_Hst_S2", partnerCode.ToString());
					command.Parameters.AddWithValue("@Name_HsMt_S2", Name_Hesab_Sarfasl);
					command.Parameters.AddWithValue("@Code_HsMt_S2", "110301");
					command.Parameters.AddWithValue("@Code_HsKt_S2", "1103");
					command.Parameters.AddWithValue("@Code_HsGt_S2", "11");
					command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", "tbl_TarafHesab");
					command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", saleServiceFactor.ID_tbl_TarafHesab);
					command.Parameters.AddWithValue("@Desc_S2", " بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
					command.Parameters.AddWithValue("@Bedehkar_S2", Mablagh);
					command.Parameters.AddWithValue("@Bestankar_S2", 0);
					command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
					command.Parameters.AddWithValue("@Mbg_Arz1", 0);
					command.Parameters.AddWithValue("@Tmbg", 0);
					command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
					command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
					command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

					var result = await command.ExecuteNonQueryAsync(cancellationToken);
				}

				#endregion

				#region Account_Sale_as_the_Bestankar_of_the_document
				using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Transaction = transaction;

					command.Parameters.AddWithValue("@Name_Hst_S2", ServiceSaleFactorHesab.Nam_Tafsl_Hesab_DrAmad_Khed);
					command.Parameters.AddWithValue("@Code_Hst_S2", ServiceSaleFactorHesab.Cd_Tafsl_Hesab_DrAmad_Khed);
					command.Parameters.AddWithValue("@Name_HsMt_S2", ServiceSaleFactorHesab.Nam_Moen_Hesab_DrAmad_Khed);
					command.Parameters.AddWithValue("@Code_HsMt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_DrAmad_Khed);
					command.Parameters.AddWithValue("@Code_HsKt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_DrAmad_Khed.Substring(0, 4));
					command.Parameters.AddWithValue("@Code_HsGt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_DrAmad_Khed.Substring(0, 2));
					command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.Name_tbl_Hesab_DrAmad_Khed);
					command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.ID_tbl_Hesab_DrAmad_Khed);
					command.Parameters.AddWithValue("@Desc_S2", " بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
					command.Parameters.AddWithValue("@Bedehkar_S2", 0);
					command.Parameters.AddWithValue("@Bestankar_S2", Mablagh_Koll);
					command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
					command.Parameters.AddWithValue("@Mbg_Arz1", 0);
					command.Parameters.AddWithValue("@Tmbg", 0);
					command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
					command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
					command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
					command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
					command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
					command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
					command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

					var result = await command.ExecuteNonQueryAsync(cancellationToken);
				}
				#endregion

				#region Malyat_as_the_Bestankar_of_the_document
				if (saleServiceFactor.JM_FF_KHed > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Mal_Khed);
						command.Parameters.AddWithValue("@Code_Hst_S2", ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Mal_Khed);
						command.Parameters.AddWithValue("@Name_HsMt_S2", ServiceSaleFactorHesab.Nam_Moen_Hesab_Mal_Khed);
						command.Parameters.AddWithValue("@Code_HsMt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Mal_Khed);
						command.Parameters.AddWithValue("@Code_HsKt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Mal_Khed.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Mal_Khed.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.Name_tbl_Hesab_Mal_Khed);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.ID_tbl_Hesab_Mal_Khed);
						command.Parameters.AddWithValue("@Desc_S2", " مالیات بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", 0);
						command.Parameters.AddWithValue("@Bestankar_S2", saleServiceFactor.JM_FF_KHed);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Avarez_as_the_Bestankar_of_the_document
				Console.WriteLine("saleServiceFactor.JAv_FF_KHed " + saleServiceFactor.JAv_FF_KHed.ToString());
				if (saleServiceFactor.JAv_FF_KHed > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Av_Khed);
						command.Parameters.AddWithValue("@Code_Hst_S2", ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Av_Khed);
						command.Parameters.AddWithValue("@Name_HsMt_S2", ServiceSaleFactorHesab.Nam_Moen_Hesab_Av_Khed);
						command.Parameters.AddWithValue("@Code_HsMt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Av_Khed);
						command.Parameters.AddWithValue("@Code_HsKt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Av_Khed.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Av_Khed.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.Name_tbl_Hesab_Av_Khed);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.ID_tbl_Hesab_Av_Khed);
						command.Parameters.AddWithValue("@Desc_S2", " عوارض بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", 0);
						command.Parameters.AddWithValue("@Bestankar_S2", saleServiceFactor.JAv_FF_KHed);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
						Console.WriteLine("JAv_FF_KHed " + result.ToString());
					}
				}
				#endregion

				#region Takhfif_as_the_Bedehkar_of_the_document
				Console.WriteLine("saleServiceFactor.Tkh_N_FF_KHed " + saleServiceFactor.Tkh_N_FF_KHed.ToString());
				if (saleServiceFactor.Tkh_N_FF_KHed > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						command.Parameters.AddWithValue("@Name_Hst_S2", ServiceSaleFactorHesab.Nam_Tafsl_Hesab_Tkhf_Khed);
						command.Parameters.AddWithValue("@Code_Hst_S2", ServiceSaleFactorHesab.Cd_Tafsl_Hesab_Tkhf_Khed);
						command.Parameters.AddWithValue("@Name_HsMt_S2", ServiceSaleFactorHesab.Nam_Moen_Hesab_Tkhf_Khed);
						command.Parameters.AddWithValue("@Code_HsMt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Tkhf_Khed);
						command.Parameters.AddWithValue("@Code_HsKt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Tkhf_Khed.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_Tkhf_Khed.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.Name_tbl_Hesab_Tkhf_Khed);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.ID_tbl_Hesab_Tkhf_Khed);
						command.Parameters.AddWithValue("@Desc_S2", " تخفيف نقدی بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", saleServiceFactor.Tkh_N_FF_KHed);
						command.Parameters.AddWithValue("@Bestankar_S2", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Porsant_Bedehkar
				if (Mablagh_Jam_Porsant > 0)
				{
					using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Transaction = transaction;

						if (ServiceSaleFactorHesab.Nam_Tafsl_Hesab_PorsMoj_Khed != null)
							command.Parameters.AddWithValue("@Name_Hst_S2", ServiceSaleFactorHesab.Nam_Tafsl_Hesab_PorsMoj_Khed);
						else
							command.Parameters.AddWithValue("@Name_Hst_S2", "");

						if (ServiceSaleFactorHesab.Cd_Tafsl_Hesab_PorsMoj_Khed != null)
							command.Parameters.AddWithValue("@Code_Hst_S2", ServiceSaleFactorHesab.Cd_Tafsl_Hesab_PorsMoj_Khed);
						else
							command.Parameters.AddWithValue("@Code_Hst_S2", "");

						command.Parameters.AddWithValue("@Name_HsMt_S2", ServiceSaleFactorHesab.Nam_Moen_Hesab_PorsMoj_Khed);
						command.Parameters.AddWithValue("@Code_HsMt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_PorsMoj_Khed);
						command.Parameters.AddWithValue("@Code_HsKt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_PorsMoj_Khed.Substring(0, 4));
						command.Parameters.AddWithValue("@Code_HsGt_S2", ServiceSaleFactorHesab.Cd_Moen_Hesab_PorsMoj_Khed.Substring(0, 2));
						command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.Name_tbl_Hesab_PorsMoj_Khed);
						command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", ServiceSaleFactorHesab.ID_tbl_Hesab_PorsMoj_Khed);
						command.Parameters.AddWithValue("@Desc_S2", " ثبت هزینه پورسانت بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
						command.Parameters.AddWithValue("@Bedehkar_S2", Mablagh_Jam_Porsant);
						command.Parameters.AddWithValue("@Bestankar_S2", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
						command.Parameters.AddWithValue("@Mbg_Arz1", 0);
						command.Parameters.AddWithValue("@Tmbg", 0);
						command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
						command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
						command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
						command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
						command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
						command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
						command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

						var result = await command.ExecuteNonQueryAsync(cancellationToken);
					}
				}
				#endregion

				#region Porsant_Mojri_Bedehkar
				if (ServiceSaleFactorHesab.chkSbt_Snd_Pors_Khed == true)
				{
					bool flagg = false;
					for (i = 0; i < struct_FactorItem_Arr.Length; i++)
					{
						if (struct_FactorItem_Arr[i].ID_tbl_TarafHesab_Moj_Rdf > 0)
						{
							using (SqlCommand command = new("Apk_Proc_Add_tbl_S2", connection))
							{
								command.CommandType = CommandType.StoredProcedure;
								command.Transaction = transaction;
								Mablagh = 0;
								flagg = false;
								for (int jj = 0; jj < struct_FactorItem_Arr.Length; jj++)
								{
									if ((struct_FactorItem_Arr[i].ID_tbl_TarafHesab_Moj_Rdf == struct_FactorItem_Arr[jj].ID_tbl_TarafHesab_Moj_Rdf)
										&& struct_FactorItem_Arr[jj].Mogayese_Shode_Ast == false)
									{
										flagg = true;
										struct_FactorItem_Arr[jj].Mogayese_Shode_Ast = true;
										Mablagh += struct_FactorItem_Arr[jj].Pors_Mbg_Rdf;
									}
								}
								if (Mablagh > 0 && flagg == true)
								{
									//===========================================================================
									using (SqlCommand command_tbl_TarafHesab = new(@"select ID_tbl_TarafHesab,Name_TarafHesab,Code_TarafHesab from tbl_TarafHesab where ID_tbl_TarafHesab=" + struct_FactorItem_Arr[i].ID_tbl_TarafHesab_Moj_Rdf, connection))
									{
										command_tbl_TarafHesab.Transaction = transaction;
										using (var reader_tbl_TarafHesab = await command_tbl_TarafHesab.ExecuteReaderAsync(cancellationToken))
										{
											while (await reader_tbl_TarafHesab.ReadAsync(cancellationToken))
											{
												Console.WriteLine("ID_tbl_TarafHesab :" + (reader_tbl_TarafHesab["ID_tbl_TarafHesab"]).ToString());
												command.Parameters.AddWithValue("@Name_Hst_S2", (reader_tbl_TarafHesab["Name_TarafHesab"]).ToString());
												command.Parameters.AddWithValue("@Code_Hst_S2", (reader_tbl_TarafHesab["Code_TarafHesab"]).ToString());
												command.Parameters.AddWithValue("@Name_tbl_Self_Hesab_S2", "tbl_TarafHesab");
												command.Parameters.AddWithValue("@ID_tbl_Self_Hesab_S2", long.Parse((reader_tbl_TarafHesab["ID_tbl_TarafHesab"]).ToString()));
											}
										}
									}
									//===========================================================================
									using (SqlCommand command_Hesab_sarfasl = new(@"select Name_Hesab_Sarfasl,ID_tbl_Sarfasl from tbl_Sarfasl where Code_Hesab_Sarfasl=N'210209'", connection))
									{
										command_Hesab_sarfasl.Transaction = transaction;
										using (var reader_Hesab_sarfasl = await command_Hesab_sarfasl.ExecuteReaderAsync(cancellationToken))
										{
											while (await reader_Hesab_sarfasl.ReadAsync(cancellationToken))
											{
												command.Parameters.AddWithValue("@Name_HsMt_S2", (reader_Hesab_sarfasl["Name_Hesab_Sarfasl"]).ToString());
												command.Parameters.AddWithValue("@Code_HsMt_S2", "210209");
												command.Parameters.AddWithValue("@Code_HsKt_S2", "2102");
												command.Parameters.AddWithValue("@Code_HsGt_S2", "21");
											}
										}
									}
									//===========================================================================
									command.Parameters.AddWithValue("@Desc_S2", " بابت فاکتور فروش خدمات به شماره " + N_FF_KHed_Str + " - طرف حساب " + partnerName);
									command.Parameters.AddWithValue("@Bedehkar_S2", 0);
									command.Parameters.AddWithValue("@Bestankar_S2", Mablagh);
									command.Parameters.AddWithValue("@ID_tbl_Arz1", 0);
									command.Parameters.AddWithValue("@Mbg_Arz1", 0);
									command.Parameters.AddWithValue("@Tmbg", 0);
									command.Parameters.AddWithValue("@ID_tbl_Arz_Mabna", 0);
									command.Parameters.AddWithValue("@Mbg_Arz_Mabna", 0);
									command.Parameters.AddWithValue("@Mbg_Arz_Riyal", 0);
									command.Parameters.AddWithValue("@ID_tbl_S1", ID_tbl_S1_Str);
									command.Parameters.AddWithValue("@Create_By_Frm_P_s2", "فرم فاکتور فروش خدمات");
									command.Parameters.AddWithValue("@Created_By_Tb_N_S2", "tbl_FF_KHed");
									command.Parameters.AddWithValue("@Created_By_Tb_I_S2", ID_tbl_FF_KHed_Str);
									command.Parameters.AddWithValue("@ID_tbl_SalMaly", saleServiceFactor.ID_tbl_SalMaly);

									var result = await command.ExecuteNonQueryAsync(cancellationToken);
								}
								command.Parameters.Clear();
							}
						}
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
}