using Microsoft.WindowsAzure.Storage.Table;

namespace Textate.Storage
{
    public class CommandTableEntity : TableEntity
    {
        public string Name { get; set; }
        public TableEntityType TableEntityType { get; set; }

        public CommandTableEntity(string name, TableEntityType tableEntityType)
        {
            Name = name;
            TableEntityType = tableEntityType;
        }
    }

    public class DateTableEntity : TableEntity
    {
    }

    public class StringTableEntity: TableEntity
    {
        public string String { get; set; }
    }

    public enum TableEntityType
    {
        Command,
        Date,
        String,
    }
}