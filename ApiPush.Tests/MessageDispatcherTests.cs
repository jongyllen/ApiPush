using ApiPush.Configuration;
using ApiPush.Messages;
using ApiPush.Push;
using ApiPush.Subscriptions;
using NSubstitute;
using NUnit.Framework;
using System;

namespace ApiPush.Tests
{
    [TestFixture]
    public class MessageDispatcherTests
    {
        IApiPushServiceConfiguration _apiPushServiceConfiguration;
        ISubscriptionStorage _subscriptionStorage;
        IPushSender _pushSender;
        MessageDispatcher dispatcher;

        [SetUp]
        public void Setup()
        {
            _apiPushServiceConfiguration = Substitute.For<IApiPushServiceConfiguration>();
            _pushSender = Substitute.For<IPushSender>();
            _subscriptionStorage = Substitute.For<ISubscriptionStorage>();

            dispatcher = new MessageDispatcher(_subscriptionStorage, _pushSender, _apiPushServiceConfiguration);
        }

        [Test]
        public void can_dispatch_itemUpdated_message()
        {
            var message = new ItemUpdated();
            dispatcher.Dispatch<ItemUpdated, Consumer>(message);
        }

        [Test]
        public void can_dispatch_null_message()
        {
            var message = new ItemUpdated();
            dispatcher.Dispatch<ItemUpdated, Consumer>(null);
        }

        [Test, ExpectedException]
        public void throws_whenitem_updated_message_throws()
        {
            var message = new ItemUpdated();
            _subscriptionStorage
                .WhenForAnyArgs(x => x.ByPartnerId(message.PartnerId))
                .Do(x => { throw new Exception(); });

            dispatcher.Dispatch<ItemUpdated, Consumer>(message);

        }
    }
}
