using ApiPush.Configuration;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using log4net;
using System.Reflection;

namespace ApiPush
{
    public class ApiPushService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ApiPushService));
        private readonly IApiPushServiceConfiguration _configuration;
        private readonly IBus _bus;
        private readonly IAutoSubscriberMessageDispatcher _dispatcher;

        public ApiPushService(IApiPushServiceConfiguration configuration, IBus bus, IAutoSubscriberMessageDispatcher dispatcher)
        {
            _configuration = configuration;
            _dispatcher = dispatcher;
            _bus = bus;
            Log.Info("ApiPushService initialized");
        }

        public bool Start()
        {
            Log.Info("ApiPushService starting");

            var subscriber = new AutoSubscriber(_bus, "name")
            {
                ConfigureSubscriptionConfiguration = c =>
                {
                    c.WithPrefetchCount(_configuration.PrefetchCount);
                    c.WithAutoDelete(_configuration.AutoDelete);
                },
                GenerateSubscriptionId = info => info.ConcreteType.FullName,
                AutoSubscriberMessageDispatcher = _dispatcher
            };

            subscriber.Subscribe(Assembly.GetExecutingAssembly());

            Log.Info("ApiPushService started");
            return true;
        }

        public bool Stop()
        {
            Log.Info("ApiPushService stopped");

            return true;
        }
    }
}