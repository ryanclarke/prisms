using Prisms.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Prisms.Client.Terminal
{
    public class FlatStorage : IStorage
    {
        private readonly string _path;

        public FlatStorage(string path)
        {
            _path = path;
        }

        public Task<List<Command>> ReadUserCommandsAsync(string userId)
        {
            return Task.FromResult(new List<Command>());
        }

        public Task WriteAsync(Shard shard)
        {
            var path = FilePath(shard);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, shard.Data);
            return Task.CompletedTask;
        }

        private string FilePath(Shard shard) => 
           Path.Combine(_path, shard.UserId, shard.DataType, shard.TimeStamp.ToString("o").Replace(':', ';').Replace('.', ','));
    }
}