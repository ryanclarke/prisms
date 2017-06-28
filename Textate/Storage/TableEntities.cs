using Microsoft.WindowsAzure.Storage.Table;

namespace Textate.Storage
{
    public class CommandTableEntity : TableEntity
    {
        public string Shortcut { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TableEntityType TableEntityType { get; set; }

        public CommandTableEntity(string shortcut, string name, string description, TableEntityType tableEntityType)
        {
            Shortcut = shortcut;
            Name = name;
            Description = description;
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