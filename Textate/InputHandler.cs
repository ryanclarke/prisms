using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Textate
{
    public class InputHandler
    {
        public static Binder Binder => SmsTrigger.Binder;
        public static TraceWriter Log => SmsTrigger.Log;

        public async Task<string> HandleAsync(Input input)
        {
            var tableAttributes = new Attribute[]
            {
                new TableAttribute($"plus{input.User}", input.Command),
                new StorageAccountAttribute("textatestorage_STORAGE")
            };
            var inputTable = await Binder.BindAsync<IAsyncCollector<DateRow>>(tableAttributes).ConfigureAwait(false);
            await inputTable.AddAsync(new DateRow
            {
                PartitionKey = input.Command,
                RowKey = DateTime.Now.ToString("s"),
            }).ConfigureAwait(false);
            await inputTable.FlushAsync().ConfigureAwait(false);
            var outputTable = await Binder.BindAsync<IQueryable<DateRow>>(tableAttributes).ConfigureAwait(false);
            var yesterdayDateString = DateTime.Now.AddMinutes(-2).ToString("s");
            var count = outputTable.Where(r => r.RowKey.CompareTo(yesterdayDateString) >= 0).ToList().Count;

            return $"{input.Command} ({count})";
        }
    }
}