namespace ProductAPI.Infrastructure.Persistence
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;

    }
}
