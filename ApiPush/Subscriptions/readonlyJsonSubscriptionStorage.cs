
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ApiPush.Subscriptions
{
    public class ReadonlyJsonSubscriptionStorage : ISubscriptionStorage
    {
        private readonly List<Subscription> allSubscriptions;

        public ReadonlyJsonSubscriptionStorage()
        {
            allSubscriptions = LoadJson();
        }
        public Subscription ByPartnerId(int partnerId)
        {
            return allSubscriptions.FirstOrDefault(x => x.PartnerId == partnerId);
        }

        private List<Subscription> LoadJson()
        {
            Subscriptions items = JsonConvert.DeserializeObject<Subscriptions>(File.ReadAllText("subscriptions.json"));

            return items.SubscriptionList;
            
        }
    }

   
}