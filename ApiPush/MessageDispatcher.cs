using ApiPush.Messages;
using EasyNetQ.AutoSubscribe;
using log4net;
using System;
using System.Threading.Tasks;

namespace ApiPush
{
    public class MessageDispatcher : IAutoSubscriberMessageDispatcher
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MessageDispatcher));
        private IConsume<ItemUpdated> _consumer;

        public MessageDispatcher(IConsume<ItemUpdated> consumer)
        {
            _consumer = consumer;
        }
        public void Dispatch<TMessage, TConsumer>(TMessage message)
            where TMessage : class
            where TConsumer : IConsume<TMessage>
        {

            // We could use an IOC container instead and do something along: continer.GetInstance<TConsumer>();
            // We could also skip this dispather completely, the consumer will be used automatically by reflection by easynetq but only using a default parameterless ctor
            if (message is ItemUpdated)
            {
                try
                {
                    _consumer.Consume(message as ItemUpdated);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    throw; // Log and throw, not ideal but will do for now
                }
            }
        }

        public Task DispatchAsync<TMessage, TConsumer>(TMessage message)
            where TMessage : class
            where TConsumer : IConsumeAsync<TMessage>
        {
            throw new NotImplementedException();
        }
    }
}
