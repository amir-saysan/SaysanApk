using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SaysanPwa.Domain.AggregateModels.LocationAggregate;
using SaysanPwa.Domain.SeedWorker;
using SqlServerWrapper.Wrapper.DatabaseManager;
using System.Data;

namespace SaysanPwa.Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDbManager _dbManager;

        public CityRepository(IConfiguration configuration, IDbManager dbManager)
        {
            _configuration = configuration;
            _dbManager = dbManager;
        }
        public async Task<SysResult<IEnumerable<City>>> GetCitiesAsync(int regionId, CancellationToken cancellationToken = default)
        {
            IEnumerable<City> cities = await _dbManager.CallProcedureWithParametersAsync<City>("Apk_Proc_GetCities", new
            {
                ID_tbl_Ostan = regionId
            }, cancellationToken);

            return new(cities);
        }

        public async Task<SysResult<string>> GetCityNameById(int cityId)
        {
            try
            {
                using (SqlConnection connection = new(_configuration.GetConnectionString("Default")))
                {
                    using (SqlCommand cmd = new("Apk_Proc_GetCities", connection))
                    {
                        cmd.Parameters.AddWithValue("@ID_tbl_SharOstan", cityId);
                        cmd.CommandType = CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        var reader = await cmd.ExecuteScalarAsync();
                        return new((string)reader!);
                    }
                }
            }
            catch (Exception ex)
            {
                return new(string.Empty, false, new() { ex.Message });
            }
        }
    }
}
// 80