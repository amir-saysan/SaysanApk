using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SaysanPwa.Application.Services
{
    public static class LocationService
    {
        public static async Task<IEnumerable<Region>> GetRegions()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "Location", "regions.json");
            var regionsJson = await File.ReadAllTextAsync(filePath);

            var data = JsonConvert.DeserializeObject<JArray>(regionsJson)!
                .Select(a => new Region(a["Id"]!.Value<int>(), a["Name"]!.Value<string>()!));

            return data;
        }

        public static async Task<IEnumerable<City>> GetCities(int regionId)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "Location", "cities.json");
            var citiesJson = await File.ReadAllTextAsync(filePath);
            var data = JsonConvert.DeserializeObject<JArray>(citiesJson)!
                .Select(a => new City(a["Id"]!.Value<int>(), a["RegionId"]!.Value<int>(), a["Name"]!.Value<string>()!));

            return data.Where(c => c.regionId == regionId);
        }


        public record Region(int id, string name);        
        public record City(int id, int regionId, string name);
    }
}
