using ApiPush.Messages;

namespace ApiPush.Push
{
    public interface IPushSender
    {
        void Send(ItemUpdated item, string url);
    }
}