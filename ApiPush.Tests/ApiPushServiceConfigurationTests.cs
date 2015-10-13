using ApiPush.Configuration;
using NUnit.Framework;

namespace ApiPush.Tests
{
    [TestFixture]
    public class ApiPushServiceConfigurationTests
    {
        IApiPushServiceConfiguration _config;

        [SetUp]
        public void Setup()
        {
            _config = new ApiPushServiceConfiguration();
        }

        [Test]
        public void can_get_prefetch_count()
        {
            ushort prefetchCount = _config.PrefetchCount;
            Assert.That(prefetchCount, Is.Not.Null);
        }

        [Test]
        public void can_get_auto_delete()
        {
            bool autoDelete = _config.AutoDelete;
            Assert.That(autoDelete, Is.Not.Null);
        }

        [Test]
        public void can_get_rabbitMq_connectionstring()
        {
            string rabbitMqConnectionString = _config.RabbitMqConnectionString;
            Assert.That(rabbitMqConnectionString, Is.Not.Null);
        }

        [Test]
        public void can_get_retry_attempts()
        {
            int retryAttempts = _config.RetryAttempts;
            Assert.That(retryAttempts, Is.Not.Null);
        }

        [Test]
        public void can_get_retry_delay_in_seconds()
        {
            int retryDelayInSeconds = _config.RetryAttempts;
            Assert.That(retryDelayInSeconds, Is.Not.Null);
        }
    }
}
