using System.Configuration;

namespace ApiPush.Configuration
{
    public class ApiPushServiceConfiguration : IApiPushServiceConfiguration
    {
        public bool AutoDelete
        {
            get
            {
                return GetBoolSetting("AutoDelete");
            }
        }

        public ushort PrefetchCount
        {
            get
            {
                return (ushort)GetIntSetting("PrefetchCount");
            }
        }

        public string RabbitMqConnectionString
        {
            get
            {
                return GetConnectionString("Rabbit");
            }
        }

        public int RetryAttempts
        {
            get
            {
                return GetIntSetting("PrefetchCount");
            }
        }

        public int RetryDelayInSeconds
        {
            get
            {
                return GetIntSetting("PrefetchCount");
            }
        }

        private bool GetBoolSetting(string name)
        {
            return bool.Parse(GetSetting(name));
        }

        private int GetIntSetting(string name)
        {
            return int.Parse(GetSetting(name));
        }

        private string GetSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        private string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}