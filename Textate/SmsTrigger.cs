using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;

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
            var twilioRequest = await TwilioRequest.FromHttpRequestMessage(req);
            log.Info(await req.Content.ReadAsStringAsync());

            var tableAttributes = new Attribute[]
            {
                new TableAttribute($"plus{twilioRequest.From}", twilioRequest.Body),
                new StorageAccountAttribute("textatestorage_STORAGE")
            };
            var input = await binder.BindAsync<IAsyncCollector<DateRow>>(tableAttributes);
            await input.AddAsync(new DateRow
            {
                PartitionKey = twilioRequest.Body,
                RowKey = DateTime.Now.ToString("s"),
            });
            await input.FlushAsync();
            var output = await binder.BindAsync<IQueryable<DateRow>>(tableAttributes);
            var yesterdayDateString = DateTime.Now.AddMinutes(-2).ToString("s");
            var count = output.Where(r => r.RowKey.CompareTo(yesterdayDateString) >= 0).ToList().Count;

            return new TwilioResponse($"r3: {twilioRequest.Body} ({count})").ToHttpResponseMessage();
        }
    }

    public class DateRow : TableEntity
    {
        public DateRow()
        {

        }
    }
}