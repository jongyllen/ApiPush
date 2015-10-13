using ApiPush.Messages;
using EasyNetQ;
using log4net.Config;
using System;
using System.Configuration;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var connectionString = ConfigurationManager.ConnectionStrings["Rabbit"].ConnectionString;

            IBus bus = RabbitHutch.CreateBus(connectionString, register => register.Register<IEasyNetQLogger>(_ => new EasyNetQLogger()));

            Console.WriteLine("Enter a partner Id to publish as updated. 0 quits");
            int itemId = int.Parse(Console.ReadLine());

            while (itemId > 0)
            {

                var message = new ItemUpdated
                {
                    PartnerId = itemId,
                    UpdatedAt = DateTime.Now,
                    ItemId = 1
                };

                bus.Publish(message);

                Console.WriteLine("Message published");
                Console.WriteLine("");

                Console.WriteLine("Enter a partner Id to publish as updated. 0 quits");
                itemId = int.Parse(Console.ReadLine());
            } 
            
        }
    }
}
