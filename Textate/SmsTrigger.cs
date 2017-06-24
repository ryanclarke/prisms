using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML;

namespace Textate
{
    public static partial class SmsTrigger
    {
        [FunctionName("SmsTrigger")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter log,
            Binder binder)
        {
            var inputHandler = new InputHandler(binder, log);

            var input = await ParseInput(req).ConfigureAwait(false);
            var responseContent = await inputHandler.HandleAsync(input).ConfigureAwait(false);
            return CreateResponse(responseContent);
        }

        public static async Task<Input> ParseInput(HttpRequestMessage request)
        {
            var data = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
            InputHandler.Log.Verbose(data, "Request.Content");
            var formValues = data.Split('&')
                .Select(value => value.Split('='))
                .ToDictionary(pair => Uri.UnescapeDataString(pair[0]).Replace("+", " "),
                              pair => Uri.UnescapeDataString(pair[1]).Replace("+", " "));

            return new Input
            {
                User = "plus" + formValues["From"].Replace("%2B", "").Trim(),
                Command = formValues["Body"].Trim(),
            };
        }

        private static HttpResponseMessage CreateResponse(string message) => new HttpResponseMessage
        {
            Content = new StringContent(
                new MessagingResponse().Message(message).ToString(),
                Encoding.UTF8,
                "application/xml")
        };
    }
}