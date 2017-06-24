using Microsoft.WindowsAzure.Storage.Table;

namespace Textate
{
    public class DateRow : TableEntity
    {
    }

    public class CommandRow : TableEntity
    {
        public RecordType RecordType { get; set; }
    }

    public enum RecordType
    {
        DateRow,
        StringRow
    }
}