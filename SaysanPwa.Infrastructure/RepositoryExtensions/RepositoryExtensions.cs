using Microsoft.Data.SqlClient;
using System.Reflection;

namespace SaysanPwa.Infrastructure.RepositoryExtensions;

public static class RepositoryExtensions
{
    public static async Task<T> AsOne<T>(this SqlDataReader reader)
    {
        T instance = Activator.CreateInstance<T>();
        while (await reader.ReadAsync())
        {
            var properties = typeof(T).GetProperties().ToList();
            properties.RemoveAll(p => p.Name.Equals("Notifications"));

            foreach(var property in properties)
            {
                property.SetValue(instance, reader[property.Name]);
            }
        }

        return instance;
    }
}
