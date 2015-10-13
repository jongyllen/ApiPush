using ApiPush.Configuration;
using ApiPush.Infrastructure;
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
        private readonly IAutoSubscriberMessageDispatcher _dispatcher;

        public ApiPushService(IApiPushServiceConfiguration configuration, IAutoSubscriberMessageDispatcher dispatcher)
        {
            _configuration = configuration;
            _dispatcher = dispatcher;
            Log.Info("ApiPushService initialized");
        }

        public bool Start()
        {
            Log.Info("ApiPushService starting");

            var bus = RabbitHutch.CreateBus(_configuration.RabbitMqConnectionString, register => register.Register<IEasyNetQLogger>(_ => new EasyNetQLogger()));

            var subscriber = new AutoSubscriber(bus, "name")
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