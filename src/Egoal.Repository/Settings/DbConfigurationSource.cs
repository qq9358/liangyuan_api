using Microsoft.Extensions.Configuration;

namespace Egoal.Settings
{
    public class DbConfigurationSource : IConfigurationSource
    {
        private readonly string _connectionString;

        public DbConfigurationSource(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DbConfigurationProvider(_connectionString);
        }
    }
}
