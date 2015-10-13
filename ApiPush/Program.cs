using log4net.Config;
using Topshelf;
using ApiPush.Configuration;
using EasyNetQ.AutoSubscribe;
using ApiPush.Subscriptions;
using ApiPush.Push;

namespace ApiPush
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            IApiPushServiceConfiguration configuration = new ApiPushServiceConfiguration();
            ISubscriptionStorage subscriptionStorage = new ReadonlyJsonSubscriptionStorage();
            IPushSender pushSender = new PushSender();
            IAutoSubscriberMessageDispatcher dispatcher = new MessageDispatcher(subscriptionStorage, pushSender, configuration);

            HostFactory.Run(x =>
            {
                x.Service<ApiPushService>(s =>
                {
                    s.ConstructUsing(() => new ApiPushService(configuration, dispatcher));
                    s.WhenStarted(push => push.Start());
                    s.WhenStopped(push => push.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("Service that consumes events over rabbitMQ and pushes notifications to http endpoints");
                x.SetDisplayName("ApiPush");
                x.SetServiceName("ApiPush");

            });
        }

    }
}
