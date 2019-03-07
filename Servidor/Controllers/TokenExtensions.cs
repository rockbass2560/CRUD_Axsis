using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Servidor.Controllers
{
    public static class TokenExtensions
    {
        public static (bool, string) TryRetrieveToken(this HttpRequestMessage request)
        {
            string token = null;
            bool result = false;

            if (request.Headers.TryGetValues("Authorization", out IEnumerable<string> authHeaders)
                && authHeaders.Count() > 0)
            {
                var bearerToken = authHeaders.ElementAt(0);
                token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
                result = true;
            }

            return (result, token);
        }

        public static string GetDomain(this HttpContext context)
        {
            Uri url = context.Request.Url;
            return url.AbsoluteUri.Replace(url.PathAndQuery, string.Empty);
        }
    }
}