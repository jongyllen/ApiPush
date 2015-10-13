
namespace ApiPush.Subscriptions
{
    public interface ISubscriptionStorage
    {
        Subscription ByPartnerId(int partnerId);
    }
}
