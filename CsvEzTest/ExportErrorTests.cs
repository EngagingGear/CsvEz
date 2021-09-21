using CsvEz;
using System.Linq;
using Xunit;

namespace CsvEzTest
{
    public class ExportErrorTests : TestBase
    {
        [Fact]
        public void EscapeHeader()
        {
            var data = GetSample1();
            var csv = new CsvEz<SampleDataClass>(null)
                .Column("Int\"", "intField");
            //.QuoteFields();
            var result = csv.Export(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.Collection(result,
                s => s.Equals("Int\""),
                s => s.Equals("1"),
                s => s.Equals("2")
                );
            ////Assert.True(MatchExportPipes(result, "|Int^||", "|1|", "|2|"));
        }
        [Fact]
        public void EscapeBody()
        {
            var data = GetSample1();
            data[1].stringField = "hello\"there";
            var csv = new CsvEz<SampleDataClass>(null)
                .Column("Int", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Export(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.Collection(result,
                s => s.Equals("Int,Str"),
                s => s.Equals("1,one"),
                s => s.Equals("2,hello\"there")
                );
            ////Assert.True(MatchExportPipes(result, "Int,Str", "1,one", "2,hello|there"));
        }
        [Fact]
        public void EscapeHeaderBackslash()
        {
            var data = GetSample1();
            var csv = new CsvEz<SampleDataClass>(null)
                .Column("Int\\", "intField");
            //.QuoteFields();
            var result = csv.Export(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.Collection(result,
                s => s.Equals("Int\\"),
                s => s.Equals("1"),
                s => s.Equals("2")
                );
            ////Assert.True(MatchExportPipes(result, "|Int^^|", "|1|", "|2|"));
        }
        [Fact]
        public void EscapeBodyBackslash()
        {
            var data = GetSample1();
            data[1].stringField = "hello\\there";
            var csv = new CsvEz<SampleDataClass>(null)
                .Column("Int", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Export(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.Collection(result,
                s => s.Equals("Int,Str"),
                s => s.Equals("1,one"),
                s => s.Equals("2,hello\\there")
                );
            ////Assert.True(MatchExportPipes(result, "|Int|,|Str|", "|1|,|one|", "|2|,|hello^^there|"));
        }

    }
}
