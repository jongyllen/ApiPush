using ApiPush.Messages;
using log4net;
using RestSharp;
using System.Net.Http;

namespace ApiPush.Push
{
    public class PushSender : IPushSender
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PushSender));

        public void Send(ItemUpdated item, string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest("/", Method.POST);

            // Todo: add auth headers
            //request.AddHeader("header", "value");

            request.AddBody(item);

            IRestResponse result = client.Execute(request);

            if (!IsStatusOk(result))
            {
                throw new HttpRequestException(string.Format("Error calling {0}, statusCode={1}, message={2}", url, (int)result.StatusCode, result.ErrorMessage));
            }

            Log.Info(string.Format("Sent Item {0} to url {1}", item.ItemId, url));
        }

        private static bool IsStatusOk(IRestResponse result)
        {
            return result.ResponseStatus != ResponseStatus.Error && result.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
