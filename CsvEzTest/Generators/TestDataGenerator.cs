using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvEzTest.Generators
{
    public class TestDataGenerator
    {
        public static IEnumerable<object[]> GenerateNumericFormatFlageTestDataWithQuotes()
        {
            yield return new object[]
            {
                new List<string>
                {
                    "\"Float\"",
                    "\"-1,234.56\"",
                    "\"1,234.56\"",
                    $"\"-{NumberFormatInfo.CurrentInfo.CurrencySymbol}1,234.56\"",
                    $"\"{NumberFormatInfo.CurrentInfo.CurrencySymbol}1,234.56\"",
                },
                new List<ColumnParameters>
                {
                    new ColumnParameters
                    {
                        columnName = "Float",
                        MemberName = "doubleField",
                        ImportNumberFormatFlags = CsvEz.NumericFormatFlags.All
                    }
                },
                new List<List<KeyValuePair<string, object>>>{
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("doubleField",-1234.56),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("doubleField",1234.56),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("doubleField",-1234.56),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("doubleField",1234.56),
                    }
                }

            };
        }
        public static IEnumerable<object[]> GenerateNumericFormatFlageTestData()
        {

            yield return new object[]
            {
                new List<string>
                {
                    "SByte,Str",
                    "\"(100)\",negative one hundred",
                    "\"(101)\", with spaces",
                    "-1,minus one",
                    "0,zero"
                },
                new List<ColumnParameters>
                {
                    new ColumnParameters
                    { 
                        columnName = "SByte",
                        MemberName = "sbyteField",
                        ImportNumberFormatFlags = CsvEz.NumericFormatFlags.AcceptAccountingNegatives
                    },
                    new ColumnParameters
                    {
                        columnName = "Str",
                        MemberName = "stringField",
                    }
                },
                new List<List<KeyValuePair<string, object>>>{
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("sbyteField",(sbyte)-100),
                        new KeyValuePair<string, object>("stringField","negative one hundred"),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("sbyteField",(sbyte)-101),
                        new KeyValuePair<string, object>("stringField"," with spaces"),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("sbyteField",(sbyte)-1),
                        new KeyValuePair<string, object>("stringField","minus one"),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("sbyteField",(sbyte)0),
                        new KeyValuePair<string, object>("stringField","zero"),
                    }
                }

            };

            yield return new object[]
            {
                new List<string>
                {
                    "SByte,Str",
                    $"\"-{NumberFormatInfo.CurrentInfo.CurrencySymbol}100\",negative one hundred",
                    $"\"{NumberFormatInfo.CurrentInfo.CurrencySymbol}101\", with spaces",
                    $"\"{NumberFormatInfo.CurrentInfo.CurrencySymbol}-1\",minus one",
                    $"\"{NumberFormatInfo.CurrentInfo.CurrencySymbol}0\",zero"
                },
                new List<ColumnParameters>
                {
                    new ColumnParameters
                    {
                        columnName = "SByte",
                        MemberName = "sbyteField",
                        ImportNumberFormatFlags = CsvEz.NumericFormatFlags.IgnoreCurrencySymbols
                    },
                    new ColumnParameters
                    {
                        columnName = "Str",
                        MemberName = "stringField",
                    }
                },
                new List<List<KeyValuePair<string, object>>>{
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("sbyteField",(sbyte)-100),
                        new KeyValuePair<string, object>("stringField","negative one hundred"),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("sbyteField",(sbyte)101),
                        new KeyValuePair<string, object>("stringField"," with spaces"),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("sbyteField",(sbyte)-1),
                        new KeyValuePair<string, object>("stringField","minus one"),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("sbyteField",(sbyte)0),
                        new KeyValuePair<string, object>("stringField","zero"),
                    }
                }
            };

            yield return new object[]
            {
                new List<string>
                {
                    "Short,Str",
                    "\"(100)\",negative one hundred",
                    "\"(101)\", with spaces",
                    "-1,minus one",
                    "0,zero"
                },
                new List<ColumnParameters>
                {
                    new ColumnParameters
                    {
                        columnName = "Short",
                        MemberName = "shortField",
                        ImportNumberFormatFlags = CsvEz.NumericFormatFlags.AcceptAccountingNegatives
                    },
                    new ColumnParameters
                    {
                        columnName = "Str",
                        MemberName = "stringField",
                    }
                },
                new List<List<KeyValuePair<string, object>>>{
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("shortField",(short)-100),
                        new KeyValuePair<string, object>("stringField","negative one hundred"),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("shortField",(short)-101),
                        new KeyValuePair<string, object>("stringField"," with spaces"),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("shortField",(short)-1),
                        new KeyValuePair<string, object>("stringField","minus one"),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("shortField",(short)0),
                        new KeyValuePair<string, object>("stringField","zero"),
                    }
                }

            };

            yield return new object[]
            {
                new List<string>
                {
                    "Short,Str",
                    $"\"-{NumberFormatInfo.CurrentInfo.CurrencySymbol}100\",negative one hundred",
                    $"\"{NumberFormatInfo.CurrentInfo.CurrencySymbol}101\", with spaces",
                    $"\"{NumberFormatInfo.CurrentInfo.CurrencySymbol}-1\",minus one",
                    $"\"{NumberFormatInfo.CurrentInfo.CurrencySymbol}0\",zero"
                },
                new List<ColumnParameters>
                {
                    new ColumnParameters
                    {
                        columnName = "Short",
                        MemberName = "shortField",
                        ImportNumberFormatFlags = CsvEz.NumericFormatFlags.IgnoreCurrencySymbols
                    },
                    new ColumnParameters
                    {
                        columnName = "Str",
                        MemberName = "stringField",
                    }
                },
                new List<List<KeyValuePair<string, object>>>{
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("shortField",(short)-100),
                        new KeyValuePair<string, object>("stringField","negative one hundred"),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("shortField",(short)101),
                        new KeyValuePair<string, object>("stringField"," with spaces"),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("shortField",(short)-1),
                        new KeyValuePair<string, object>("stringField","minus one"),
                    },
                    new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("shortField",(short)0),
                        new KeyValuePair<string, object>("stringField","zero"),
                    }
                }

            };
        }

        public static List<string> MakeImportList(params string[] inputLines)
        {
            return inputLines.ToList();
        }
    }
}
