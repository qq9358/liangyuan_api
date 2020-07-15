namespace Egoal.Redis
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; } = "127.0.0.1:6379,allowAdmin=true";
        public int DatabaseId { get; set; } = -1;
    }
}
