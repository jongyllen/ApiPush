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
        public void CanGetPrefetchCount()
        {
            ushort prefetchCount = _config.PrefetchCount;
            Assert.That(prefetchCount, Is.Not.Null);
        }

        [Test]
        public void CanGetAutoDelete()
        {
            bool autoDelete = _config.AutoDelete;
            Assert.That(autoDelete, Is.Not.Null);
        }

        [Test]
        public void CanGetRabbitMqConnectionString()
        {
            string rabbitMqConnectionString = _config.RabbitMqConnectionString;
            Assert.That(rabbitMqConnectionString, Is.Not.Null);
        }

        [Test]
        public void CanGetRetryAttempts()
        {
            int retryAttempts = _config.RetryAttempts;
            Assert.That(retryAttempts, Is.Not.Null);
        }

        [Test]
        public void CanGetRetryDelayInSeconds()
        {
            int retryDelayInSeconds = _config.RetryAttempts;
            Assert.That(retryDelayInSeconds, Is.Not.Null);
        }
    }
}
