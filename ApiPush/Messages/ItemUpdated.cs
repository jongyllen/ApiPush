using System;

namespace ApiPush.Messages
{
    public class ItemUpdated
    {
        public int PartnerId;
        public int ItemId;
        public DateTime? UpdatedAt;

        public ItemUpdated()
        {

        }
    }
}
