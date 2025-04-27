namespace Fahem.Settings
{
    public class MongoDbSettings
    {
        public string ClusterAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"mongodb+srv://{Username}:{Password}@{ClusterAddress}/?retryWrites=true&w=majority&appName={DatabaseName}Cluster";
            }
        }
    }


}
