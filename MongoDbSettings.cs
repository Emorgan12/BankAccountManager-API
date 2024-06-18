namespace MongoDbSettings
{
    public class MongoDBSettings
    {
        public required string Host { get; set; }
        public int Port { get; set; }
        public string ConnectionString
        {
            get
            {
                return $"mongodb://{Host}:{Port}";
            }
        }
    }
}