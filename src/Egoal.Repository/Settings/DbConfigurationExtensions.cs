using Egoal.Extensions;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Egoal.Settings
{
    public static class DbConfigurationExtensions
    {
        public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder, string filePath)
        {
            string connectionString = string.Empty;
            using (var reader = File.OpenText(filePath))
            {
                var appsettings = reader.ReadToEnd().JsonToObject<dynamic>();
                connectionString = appsettings.ConnectionStrings.DefaultConnection;
            }

            return builder.Add(new DbConfigurationSource(connectionString));
        }
    }
}
