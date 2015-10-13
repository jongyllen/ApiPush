using ApiPush.Configuration;
using ApiPush.Messages;
using ApiPush.Push;
using ApiPush.Subscriptions;
using EasyNetQ.AutoSubscribe;
using log4net;
using Polly;
using System;
using System.Diagnostics;

namespace ApiPush
{
    public class Consumer : IConsume<ItemUpdated>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Consumer));
        private readonly IPushSender _pushSender;
        private readonly ISubscriptionStorage _subscriptions;
        private readonly IApiPushServiceConfiguration _configuration;

        private int _retryCount;

        public Consumer(ISubscriptionStorage subscriptions, IPushSender pushSender, IApiPushServiceConfiguration configuration)
        {
            _subscriptions = subscriptions;
            _pushSender = pushSender;
            _configuration = configuration;
        }

        public void Consume(ItemUpdated item)
        {
            Subscription subscription = _subscriptions.ByPartnerId(item.PartnerId);

            if (subscription == null)
            {
                Log.Info("No subscription found for partner " + item.PartnerId);
                return;
            }

            ValidateUrl(subscription.Endpoint);

            string url = subscription.Endpoint;

            Send(item, url);

        }

        private void ValidateUrl(string endpoint)
        {
            var uri = new Uri(endpoint);
        }

        private void Send(ItemUpdated item, string url)
        {
            var timer = Stopwatch.StartNew();
            try
            {
                var retryPolicy = Policy
                   .Handle<Exception>()
                   .WaitAndRetry(_configuration.RetryAttempts, retryAttempt => TimeSpan.FromSeconds(_configuration.RetryDelayInSeconds * retryAttempt),
                   (exception, timespan) => {
                       _retryCount++;
                       Log.Info(string.Format("Failed to push item, will retry in {0} seconds", timespan.TotalSeconds), exception);
                   });

                retryPolicy.Execute(() =>
                {
                    _pushSender.Send(item, url);
                });
            }
            catch (Exception ex)
            {
                throw new RetryPolicyException(_retryCount, ex);
            }
            finally
            {
                timer.Stop();
                Log.Info("Send ItemUpdated took " + timer.ElapsedMilliseconds + " milliseconds");
            }
        }
    }
}
