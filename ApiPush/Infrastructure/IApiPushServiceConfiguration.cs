namespace ApiPush.Configuration
{
    public interface IApiPushServiceConfiguration
    {
        bool AutoDelete { get; }
        ushort PrefetchCount { get; }
        string RabbitMqConnectionString { get; }
        int RetryAttempts { get; }
        int RetryDelayInSeconds { get; }
    }
}
