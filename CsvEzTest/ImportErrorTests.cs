using CsvEz;
using CsvEzTest.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace CsvEzTest
{
    public class ImportErrorTests : TestBase
    {
        [Fact]
        public void UnknownField()
        {
            var data = MakeImportList("Int,Str", "1,one", "2,two");
            //bool hasException = false;
            //try
            //{
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField");
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                csv.Column("Str", "unknown");
            });
            Assert.Equal("Field unknown does not exist on type CsvEzTest.SampleDataClass", exception.Message);
            //}
            //catch
            //{
            //    hasException = true;
            //}
        }
        [Fact]
        public void ShouldSkipCoulmnIfSpecified()
        {
            var data = new List<string>
            {
                "Int,SkipColumn,Str", "1,skipData1,one", "2,skipData2,two"
            };
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .SkipColumn("SkipColumn")
                .Column("Str", "stringField");
            var result = csv.Import(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.Equal(result.Count, data.Count - 1);
        }

        [Fact]
        public void EachFieldShouldBeQuotedAndSeparetedBySplitChar()
        {
            var data = MakeImportListPipes("|Int|;|Str|", "|1|;|one|", "|2|;|two|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField")
                ////.QuoteFields()
                .SplitChar(';');
            var result = csv.Import(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.Equal(result.Count, data.Count-1);            
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GenerateNumericFormatFlageTestDataWithQuotes), MemberType = typeof(TestDataGenerator))]
        public void ShouldRespectNumberFormatFlagsForFieldORPropertyValueWithQuote(List<string> data, List<ColumnParameters> columns, List<List<KeyValuePair<string, object>>> memberValuesList)
        {
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass());
            ////csv.QuoteFields();
            foreach (var col in columns)
            {
                csv.Column(col.columnName, col.MemberName, null, col.ImportNumberFormatFlags);
            }
            var result = csv.Import(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.Equal(result.Count, data.Count - 1);
            for (int index1 = 0; index1 < memberValuesList.Count; index1++)
            {
                var memberValues = memberValuesList[index1];
                for (int index = 0; index < memberValues.Count; index++)
                {
                    var memberValue = memberValues[index];
                    Assert.Equal(memberValue.Value, GetValue(result[index1], memberValue.Key));
                }
            }
        }
        [Theory]
        [MemberData(nameof(TestDataGenerator.GenerateNumericFormatFlageTestData), MemberType = typeof(TestDataGenerator))]
        public void ShouldRespectNumberFormatFlagsForFieldORPropertyValue(List<string> data, List<ColumnParameters> columns, List<List<KeyValuePair<string, object>>> memberValuesList)
        {
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass());
            foreach (var col in columns)
            {
                csv.Column(col.columnName, col.MemberName, null, col.ImportNumberFormatFlags);
            }
            var result = csv.Import(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.Equal(result.Count, data.Count - 1);
            for (int index1 = 0; index1 < memberValuesList.Count; index1++)
            {
                var memberValues = memberValuesList[index1];
                for (int index = 0; index < memberValues.Count; index++)
                {
                    var memberValue = memberValues[index];
                    Assert.Equal(memberValue.Value, GetValue(result[index1], memberValue.Key));
                }
            }
        }
        
        private object GetValue(SampleDataClass data, string memberName)
        {
            var type = data.GetType();
            return type.GetField(memberName)?.GetValue(data) ?? type.GetProperty(memberName)?.GetValue(data);            
        }

        [Fact]
        public void QuotedNotStartWithQuote()
        {
            var data = MakeImportListPipes("Int|,|Str|", "|1|,|one|", "|2|,|two|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
              ////  .QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.True(csv.InError);
            Assert.Equal("0: Invalid line: Int\",\"Str\"", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedDataNotStartWithQuote()
        {
            var data = MakeImportListPipes("|Int|,|Str|", "1|,|one|", "|2|,|two|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
            var result = csv.Import(data).ToList();
            Assert.True(csv.InError);
            Assert.Equal("1: Invalid line: 1\",\"one\"", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedNotEndWithQuote()
        {
            var data = MakeImportListPipes("Int|,|Str", "|1|,|one|", "|2|,|two|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
                ////.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.True(csv.InError);
            Assert.Equal("0: Invalid line: Int\",\"Str", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedJunkBetweenCommaHeader1()
        {
            var data = MakeImportListPipes("|Int|,a|Str|", "1|,|one|", "|2|,|two|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
                ////.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.True(csv.InError);
            Assert.Equal("0: Invalid line: \"Int\",a\"Str\"", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedJunkBetweenCommaData1()
        {
            var data = MakeImportListPipes("|Int|,|Str|", "1|,a|one|", "|2|,|two|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
                ////.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.True(csv.InError);
            Assert.Equal("1: Invalid line: 1\",a\"one\"", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedJunkBetweenCommaHeader2()
        {
            var data = MakeImportListPipes("|Int|a,|Str|", "1|,|one|", "|2|,|two|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
                ////.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.True(csv.InError);
            Assert.Equal("0: Invalid line: \"Int\"a,\"Str\"", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedJunkBetweenCommaData2()
        {
            var data = MakeImportListPipes("|Int|,|Str|", "1|a,|one|", "|2|,|two|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
                ////.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.True(csv.InError);
            Assert.Equal("1: Invalid line: 1\"a,\"one\"", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedJunkBetweenMissingCommaHeader()
        {
            var data = MakeImportListPipes("|Int||Str|", "1|,|one|", "|2|,|two|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
                ////.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.True(csv.InError);
            Assert.Equal("Header mismatching number of columns", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedJunkBetweenMissingCommaData()
        {
            var data = MakeImportListPipes("|Int|,|Str|", "1||one|", "|2|,|two|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
                ////.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.True(csv.InError);
            Assert.Equal("1: Invalid line: 1\"\"one\"", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void HeaderColumnWrongNumberTooMany()
        {
            var data = MakeImportList("Int,Str", "1,one", "2,two");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField");
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Header mismatching number of columns", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void HeaderColumnWrongNumberTooFew()
        {
            var data = MakeImportList("Int", "1", "2");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Header mismatching number of columns", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void HeaderColumnWrongTitle()
        {
            var data = MakeImportList("Str", "1", "2");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField");
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Mismatching column header starting at Str (should be Int)", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void DataColumnWrongNumberTooMany()
        {
            var data = MakeImportList("Int", "1", "2,two");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField");
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Mismatching number of columns", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void DataColumnWrongNumberTooFew()
        {
            var data = MakeImportList("Int,Str", "1,one", "2");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Mismatching number of columns", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedHeaderColumnWrongNumberTooMany()
        {
            var data = MakeImportListPipes("|Int|,|Str|", "|1|,|one|", "|2|,|two|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField");
                //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Header mismatching number of columns", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedHeaderColumnWrongNumberTooFew()
        {
            var data = MakeImportListPipes("|Int|", "|1|", "|2|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Header mismatching number of columns", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedHeaderColumnWrongTitle()
        {
            var data = MakeImportListPipes("|Str|", "|1|", "|2|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Mismatching column header starting at Str (should be Int)", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedDataColumnWrongNumberTooMany()
        {
            var data = MakeImportListPipes("|Int|", "|1|", "|2|,|two|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Mismatching number of columns", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedDataColumnWrongNumberTooFew()
        {
            var data = MakeImportListPipes("|Int|,|Str|", "|1|,|one|", "|2|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Mismatching number of columns", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedEscapedQuoteInHeader()
        {
            var data = MakeImportListPipes("|In\"t|,|Str|", "|1|,|one|", "|2|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("In\"t", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("0: Invalid line: \"In\"t\",\"Str\"", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedEscapedQuoteInData()
        {
            var data = MakeImportListPipes("|Int|,|Str|", "|1|,|on\"e|", "|2|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("In\"t", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Mismatching column header starting at Int (should be In\"t)", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedEscapedBackslashInHeader()
        {
            var data = MakeImportListPipes("|In\\t|,|Str|", "|1|,|one|", "|2|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("In\\t", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Mismatching number of columns", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedEscapedBackslashInData()
        {
            var data = MakeImportListPipes("|Int|,|Str|", "|1|,|on\\e|", "|2|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("In\\t", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("on\\e", result[0].stringField);
            Assert.Equal("Mismatching column header starting at Int (should be In\\t)", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedEscapedTrailingCommaInHeader()
        {
            var data = MakeImportListPipes("|Int|,|Str|,", "|1|,|one|", "|2|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Header mismatching number of columns", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedEscapedTrailingCommaInData()
        {
            var data = MakeImportListPipes("|Int|,|Str|", "|1|,|one|,", "|2|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Mismatching number of columns", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedEscapedInvalidInt()
        {
            var data = MakeImportListPipes("|Int|,|Str|", "|aaa|,|one|", "|2|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("In\"t", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Mismatching column header starting at Int (should be In\"t)", csv.Errors.First().Value[0]);
        }

        [Fact]
        public void QuotedEscapedInvalidOutOfRange()
        {
            var data = MakeImportListPipes("|Int|,|Str|", "|5000000000|,|one|", "|2|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("In\"t", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.NotEmpty(csv.Errors);
            Assert.Equal("Mismatching column header starting at Int (should be In\"t)", csv.Errors.First().Value[0]);
        }

    }
}
