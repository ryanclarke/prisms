using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Threading.Tasks;

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

            await tableConnection.WriteRow(new DateRow
            {
                PartitionKey = input.Command,
                RowKey = DateTime.Now.ToString("s")
            }).ConfigureAwait(false);

            var outputTable = await tableConnection.GetPartition<DateRow>(input.Command).ConfigureAwait(false);
            var yesterdayDateString = DateTime.Now.AddMinutes(-2).ToString("s");
            var count = outputTable.Where(r => r.RowKey.CompareTo(yesterdayDateString) >= 0).ToList().Count;

            return $"{input.Command} ({count})";
        }
    }

    public class TableConnection
    {
        private Binder binder;
        private string tableKey;

        public TableConnection(Binder binder, string user)
        {
            this.binder = binder;
            tableKey = user;
        }

        public Task<IQueryable<T>> GetPartition<T>(string partitionKey) where T : TableEntity
        {
            return binder.BindAsync<IQueryable<T>>(CreateTableAttributes(partitionKey));
        }

        public Task<IAsyncCollector<T>> GetWritablePartition<T>(string partitionKey) where T : TableEntity
        {
            return binder.BindAsync<IAsyncCollector<T>>(CreateTableAttributes(partitionKey));
        }

        public Task<IQueryable<CommandRow>> GetUserCommands()
        {
            return GetPartition<CommandRow>("CustomUserCommands");
        }

        public async Task WriteRow<T>(T row) where T : TableEntity
        {
            var inputTable = await GetWritablePartition<T>(row.PartitionKey).ConfigureAwait(false);
            await inputTable.AddAsync(row).ConfigureAwait(false);
            await inputTable.FlushAsync().ConfigureAwait(false);
        }

        private Attribute[] CreateTableAttributes(string partitionKey)
        {
            return new Attribute[]
            {
                new TableAttribute(tableKey, partitionKey),
                new StorageAccountAttribute("textatestorage_STORAGE")
            };
        }
    }
}