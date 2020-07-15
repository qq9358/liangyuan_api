using Egoal.Extensions;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Egoal.Redis
{
    public class RedisManager : IDisposable
    {
        private readonly RedisOptions _options;
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        public RedisManager(IOptions<RedisOptions> options)
        {
            _options = options.Value;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_options.ConnectionString);
        }

        public async Task InitConfigAsync()
        {
            var connection = GetConnection();

            var endPoints = connection.GetEndPoints();
            foreach (var endPoint in endPoints)
            {
                var server = connection.GetServer(endPoint);

                await SetConfigAsync(server, "appendonly", "yes");
                //await SetConfigAsync(server, "aof-use-rdb-preamble", "yes");

                await server.ConfigRewriteAsync();
            }
        }

        private async Task SetConfigAsync(IServer server, string name, string value)
        {
            var config = await server.ConfigGetAsync(name);
            if (!config.IsNullOrEmpty() && !config[0].Value.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                await server.ConfigSetAsync(name, value);
            }
        }

        public IDatabase GetDatabase()
        {
            return GetConnection().GetDatabase(_options.DatabaseId);
        }

        public ConnectionMultiplexer GetConnection()
        {
            return _connectionMultiplexer.Value;
        }

        public void Dispose()
        {
            GetConnection().Dispose();
        }
    }
}
