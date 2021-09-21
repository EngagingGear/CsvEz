using CsvEz;

namespace CsvEzTest.Generators
{
    public class ColumnParameters
    {
        public string columnName { get; set; }
        public string MemberName { get; set; }

        public string ExportFormat { get; set; }

        public NumericFormatFlags? ImportNumberFormatFlags { get; set; }
    }
}