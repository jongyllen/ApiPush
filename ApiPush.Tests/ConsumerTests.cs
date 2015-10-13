using ApiPush.Configuration;
using ApiPush.Messages;
using ApiPush.Push;
using ApiPush.Subscriptions;
using NSubstitute;
using NUnit.Framework;
using Polly.Utilities;
using System;

namespace ApiPush.Tests
{
    [TestFixture]
    public class ConsumerTests
    {
        private int _partnerId = 1;
        private Consumer _consumer;
        private IPushSender _pushSender;
        private ISubscriptionStorage _subscriptionStorage;
        private IApiPushServiceConfiguration _apiPushServiceConfiguration;

        private ItemUpdated _itemUpdated;

        [SetUp]
        public void Setup()
        {
            _apiPushServiceConfiguration = Substitute.For<IApiPushServiceConfiguration>();
            _apiPushServiceConfiguration.RetryDelayInSeconds.Returns(30);
            _apiPushServiceConfiguration.RetryAttempts.Returns(3);

            _pushSender = Substitute.For<IPushSender>();
            _subscriptionStorage = Substitute.For<ISubscriptionStorage>();
            _consumer = new Consumer(_subscriptionStorage, _pushSender, _apiPushServiceConfiguration);

            _itemUpdated = new ItemUpdated
            {
                ItemId = 1,
                PartnerId = 1,
                UpdatedAt = DateTime.Now
            };
        }

        [Test]
        public void should_not_send_push_if_no_subscription_exists()
        {
            _subscriptionStorage.ByPartnerId(_partnerId).Returns(null as Subscription);

            _consumer.Consume(new ItemUpdated
            {
                ItemId = 1,
                PartnerId = _partnerId,
                UpdatedAt = DateTime.Now
            });

            _pushSender.DidNotReceiveWithAnyArgs().Send(Arg.Any<ItemUpdated>(), Arg.Any<string>());
        }

        [Test, ExpectedException]
        public void should_throw_if_malformatted_uri()
        {
            var subscription = new Subscription { Endpoint = "Fake" };
            _subscriptionStorage.ByPartnerId(_partnerId).Returns(subscription);

            _consumer.Consume(_itemUpdated);
        }

        [Test]
        public void should_send_push_if_subscription_exists()
        {
            var subscription = new Subscription { Endpoint = "http://localhost" };
            _subscriptionStorage.ByPartnerId(_partnerId).Returns(subscription);

            _consumer.Consume(_itemUpdated);

            _pushSender.Received().Send(_itemUpdated, subscription.Endpoint);
        }

        [Test]
        public void should_retry_send_three_times_and_then_throw()
        {
            var retries = 0;
          
            var subscription = new Subscription { Endpoint = "http://localhost" };
            _subscriptionStorage.ByPartnerId(_partnerId).Returns(subscription);

            SystemClock.Sleep = timespan =>
            {
                Assert.That(timespan.TotalSeconds, Is.EqualTo(_apiPushServiceConfiguration.RetryDelayInSeconds * ++retries));
            };

            _pushSender
                .WhenForAnyArgs(x => x.Send(null, null))
                .Do(x => { throw new Exception(); });

            var exception = Assert.Throws<RetryPolicyException>(() => _consumer.Consume(_itemUpdated));
            Assert.That(exception.RetryCount, Is.EqualTo(3));

            _pushSender.ReceivedWithAnyArgs(4).Send(null, null);
        }
    }
}
