using System;

namespace CsvEz
{
    [Flags]
    public enum NumericFormatFlags : byte
    {
        AcceptAccountingNegatives = 1,
        IgnoreCurrencySymbols = 2,
        IgnoreSeparators = 4,

        All = 7
    }
}