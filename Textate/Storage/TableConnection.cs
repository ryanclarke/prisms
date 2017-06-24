using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Textate.Storage
{
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

        public Task<IQueryable<CommandTableEntity>> GetUserCommands()
        {
            return GetPartition<CommandTableEntity>("CustomUserCommands");
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