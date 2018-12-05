using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MobileWebAPI.MessageHandlers
{
    public class ApiKeyMessageHandler : DelegatingHandler
    {
        private const string APIKeyToCheck = "VaaaNMobileApi";
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            bool validkey = false;
            IEnumerable<string> requesrHeaders;
            var CheckApiKeyExits = httpRequestMessage.Headers.TryGetValues("APIKey", out requesrHeaders);
            if (CheckApiKeyExits)
            {
                if (requesrHeaders.FirstOrDefault().Equals(APIKeyToCheck))
                {
                    validkey = true;
                }
            }
            if (!validkey)
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Forbidden, "Invalid API Key");
            }

            var responce = await base.SendAsync(httpRequestMessage, cancellationToken);
            return responce;
        }
    }
}