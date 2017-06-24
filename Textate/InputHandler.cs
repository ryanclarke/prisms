using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Linq;
using System.Threading.Tasks;
using Textate.Storage;

namespace Textate
{
    public class InputHandler
    {
        public static Binder Binder => SmsTrigger.Binder;
        public static TraceWriter Log => SmsTrigger.Log;

        private TableConnection tableConnection;

        public async Task<string> HandleAsync(Input input)
        {
            tableConnection = new TableConnection(Binder, input.User);

            await tableConnection.WriteRow(new DateTableEntity
            {
                PartitionKey = input.Command,
                RowKey = DateTime.Now.ToString("s")
            }).ConfigureAwait(false);

            var outputTable = await tableConnection.GetPartition<DateTableEntity>(input.Command).ConfigureAwait(false);
            var yesterdayDateString = DateTime.Now.AddMinutes(-2).ToString("s");
            var count = outputTable.Where(r => r.RowKey.CompareTo(yesterdayDateString) >= 0).ToList().Count;

            return $"{input.Command} ({count})";
        }
    }
}