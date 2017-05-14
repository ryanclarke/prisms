using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace Textate
{
    public class TwilioRequest
    {
        public string From { get; private set; }
        public string Body { get; private set; }

        public static async Task<TwilioRequest> FromHttpRequestMessage(HttpRequestMessage request)
        {
            var data = await request.Content.ReadAsStringAsync();
            var formValues = data.Split('&')
                .Select(value => value.Split('='))
                .ToDictionary(pair => Uri.UnescapeDataString(pair[0]).Replace("+", " "),
                              pair => Uri.UnescapeDataString(pair[1]).Replace("+", " "));

            return new TwilioRequest
            {
                From = formValues["From"].Replace("%2B", "").Trim(),
                Body = formValues["Body"],
            };
        }
    }
}