using Prisms.Core;
using System;
using System.IO;

namespace Prisms.Client.Terminal
{
    public class FlatStorage : IStorage
    {
        private readonly string _path;

        public FlatStorage(string path)
        {
            _path = path;
        }

        public void Write(Shard shard)
        {
            var path = FilePath(shard);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, shard.Data);
        }

        private string FilePath(Shard shard) => 
           Path.Combine(_path, shard.UserId, shard.DataType, shard.TimeStamp.ToString("o").Replace(':', ';').Replace('.', ','));
    }
}