using CsvEz;
using System;
using System.Linq;
using Xunit;

namespace CsvEzTest
{
    public partial class ImportTests : TestBase
    {
        [Fact]
        public void ImportInt()
        {
            var data = MakeImportList("Int", "1", "2");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField");
            var result = csv.Import(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.True(CompareList(_compInt, GetSample1(), result));
        }

        [Fact]
        public void ImportIntAndString()
        {
            var data = MakeImportList("Int,Str", "1,one", "2,two");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
            var result = csv.Import(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.True(CompareList(_compIntStr, GetSample1(), result));
        }

        [Fact]
        public void ImportIntQuoted()
        {
            var data = MakeImportListPipes("|Int|", "|1|", "|2|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.True(CompareList(_compInt, GetSample1(), result));
        }

        [Fact]
        public void ImportIntAndStringQuoted()
        {
            var data = MakeImportListPipes("|Int|,|Str|", "|1|,|one|", "|2|,|two|");
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("Int", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.True(CompareList(_compIntStr, GetSample1(), result));
        }

        [Fact]
        public void ImportAllFields()
        {
            var data = MakeImportList(
                "sbyte,byte,short,ushort,int,uint,long,ulong,char,float,double,bool,decimal,DateTime,DateTimeOffset,string,sbyte,byte,short,ushort,int,uint,long,ulong,char,float,double,bool,decimal,DateTime,DateTimeOffset,string",
                $"1,1,0,1,1,1,1,1,1,1,1,False,1,{new DateTime(2001,1,1,1,1,1)},{new DateTimeOffset(new DateTime(2001, 1, 1, 1, 1, 1))},1,11,11,0,11,11,11,11,11,1,1,1,True,11,{new DateTime(2011, 11, 11, 11, 11, 11)},{new DateTimeOffset(new DateTime(2011, 11, 11, 11, 11, 11))},11",
                $"2,2,0,2,2,2,2,2,2,2,2,False,2,{new DateTime(2002,2,2,2,2,2)},{new DateTimeOffset(new DateTime(2002,2,2,2,2,2))},2,22,22,0,22,22,22,22,22,2,2,2,True,22,{new DateTime(2022,12,22,22,22,22)},{new DateTimeOffset(new DateTime(2022,12,22,22,22,22))},22"
            );
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("sbyte", "sbyteField")
                .Column("byte", "byteField")
                .Column("short", "shortField")
                .Column("ushort", "ushortField")
                .Column("int", "intField")
                .Column("uint", "uintField")
                .Column("long", "longField")
                .Column("ulong", "ulongField")
                .Column("char", "charField")
                .Column("float", "floatField")
                .Column("double", "doubleField")
                .Column("bool", "boolField")
                .Column("decimal", "decimalField")
                .Column("DateTime", "dateTimeField")
                .Column("DateTimeOffset", "dateTimeOffsetField")
                .Column("string", "stringField")
                .Column("sbyte", "SbyteProperty")
                .Column("byte", "ByteProperty")
                .Column("short", "ShortProperty")
                .Column("ushort", "UshortProperty")
                .Column("int", "IntProperty")
                .Column("uint", "UintProperty")
                .Column("long", "LongProperty")
                .Column("ulong", "UlongProperty")
                .Column("char", "CharProperty")
                .Column("float", "FloatProperty")
                .Column("double", "DoubleProperty")
                .Column("bool", "BoolProperty")
                .Column("decimal", "DecimalProperty")
                .Column("DateTime", "DateTimeProperty")
                .Column("DateTimeOffset", "DateTimeOffsetProperty")
                .Column("string", "StringProperty");
            var result = csv.Import(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.True(CompareList(_compAll, GetFullSample(), result));
        }

        [Fact]
        public void ImportAllFieldsQuoted()
        {
            var data = MakeImportListPipes(
                "|sbyte|,|byte|,|short|,|ushort|,|int|,|uint|,|long|,|ulong|,|char|,|float|,|double|,|bool|,|decimal|,|DateTime|,|DateTimeOffset|,|string|,|sbyte|,|byte|,|short|,|ushort|,|int|,|uint|,|long|,|ulong|,|char|,|float|,|double|,|bool|,|decimal|,|DateTime|,|DateTimeOffset|,|string|",
                $"|1|,|1|,|0|,|1|,|1|,|1|,|1|,|1|,|1|,|1|,|1|,|False|,|1|,|{new DateTime(2001,1,1,1,1,1)}|,|{new DateTimeOffset(new DateTime(2001, 1, 1, 1, 1, 1))}|,|1|,|11|,|11|,|0|,|11|,|11|,|11|,|11|,|11|,|1|,|1|,|1|,|True|,|11|,|{new DateTime(2011, 11, 11, 11, 11, 11)}|,|{new DateTimeOffset(new DateTime(2011, 11, 11, 11, 11, 11))}|,|11|",
                $"|2|,|2|,|0|,|2|,|2|,|2|,|2|,|2|,|2|,|2|,|2|,|False|,|2|,|{new DateTime(2002, 2, 2, 2, 2, 2)}|,|{new DateTimeOffset(new DateTime(2002, 2, 2, 2, 2, 2))}|,|2|,|22|,|22|,|0|,|22|,|22|,|22|,|22|,|22|,|2|,|2|,|2|,|True|,|22|,|{new DateTime(2022, 12, 22, 22, 22, 22)}|,|{new DateTimeOffset(new DateTime(2022, 12, 22, 22, 22, 22))}|,|22|"
            );
            var csv = new CsvEz<SampleDataClass>(() => new SampleDataClass())
                .Column("sbyte", "sbyteField")
                .Column("byte", "byteField")
                .Column("short", "shortField")
                .Column("ushort", "ushortField")
                .Column("int", "intField")
                .Column("uint", "uintField")
                .Column("long", "longField")
                .Column("ulong", "ulongField")
                .Column("char", "charField")
                .Column("float", "floatField")
                .Column("double", "doubleField")
                .Column("bool", "boolField")
                .Column("decimal", "decimalField")
                .Column("DateTime", "dateTimeField")
                .Column("DateTimeOffset", "dateTimeOffsetField")
                .Column("string", "stringField")
                .Column("sbyte", "SbyteProperty")
                .Column("byte", "ByteProperty")
                .Column("short", "ShortProperty")
                .Column("ushort", "UshortProperty")
                .Column("int", "IntProperty")
                .Column("uint", "UintProperty")
                .Column("long", "LongProperty")
                .Column("ulong", "UlongProperty")
                .Column("char", "CharProperty")
                .Column("float", "FloatProperty")
                .Column("double", "DoubleProperty")
                .Column("bool", "BoolProperty")
                .Column("decimal", "DecimalProperty")
                .Column("DateTime", "DateTimeProperty")
                .Column("DateTimeOffset", "DateTimeOffsetProperty")
                .Column("string", "StringProperty");
            //.QuoteFields();
            var result = csv.Import(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.True(CompareList(_compAll, GetFullSample(), result));
        }


        private readonly Func<SampleDataClass, SampleDataClass, bool> _compInt = (a, b) => a.intField == b.intField;
        private readonly Func<SampleDataClass, SampleDataClass, bool> _compIntStr = (a, b) => a.intField == b.intField && a.stringField == b.stringField;

        private readonly Func<SampleDataClass, SampleDataClass, bool> _compAll = (a, b) =>
            // ReSharper disable CompareOfFloatsByEqualityOperator
            a.sbyteField == b.sbyteField &&
            a.byteField == b.byteField &&
            a.stringField == b.stringField &&
            a.ushortField == b.ushortField &&
            a.intField == b.intField &&
            a.uintField == b.uintField &&
            a.longField == b.longField &&
            a.ulongField == b.ulongField &&
            a.charField == b.charField &&
            a.floatField == b.floatField &&
            a.doubleField == b.doubleField &&
            a.boolField == b.boolField &&
            a.doubleField == b.doubleField &&
            a.dateTimeField == b.dateTimeField &&
            a.dateTimeOffsetField == b.dateTimeOffsetField &&
            a.stringField == b.stringField &&
            a.SbyteProperty == b.SbyteProperty &&
            a.ByteProperty == b.ByteProperty &&
            a.StringProperty == b.StringProperty &&
            a.UshortProperty == b.UshortProperty &&
            a.IntProperty == b.IntProperty &&
            a.UintProperty == b.UintProperty &&
            a.LongProperty == b.LongProperty &&
            a.UlongProperty == b.UlongProperty &&
            a.CharProperty == b.CharProperty &&
            a.FloatProperty == b.FloatProperty &&
            a.DoubleProperty == b.DoubleProperty &&
            a.BoolProperty == b.BoolProperty &&
            a.DoubleProperty == b.DoubleProperty &&
            a.DateTimeProperty == b.DateTimeProperty &&
            a.DateTimeOffsetProperty == b.DateTimeOffsetProperty &&
            a.StringProperty == b.StringProperty;
        // ReSharper restore CompareOfFloatsByEqualityOperator
    }
}
