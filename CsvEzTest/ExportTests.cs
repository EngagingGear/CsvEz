using CsvEz;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace CsvEzTest
{
    public partial class ExportTests: TestBase
    {
        private readonly Func<SampleDataClass, SampleDataClass, bool> _compInt = (a, b) => a.intField == b.intField;
        private readonly Func<SampleDataClass, SampleDataClass, bool> _compIntStr = (a, b) => a.intField == b.intField && a.stringField == b.stringField;

        [Fact]
        public void ExportInt()
        {
            var data = GetSample1();
            var csv = new CsvEz<SampleDataClass>(null)
                .Column("Int", "intField");
            var result = csv.Export(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.True(MatchExport(result, "Int", "1", "2"));
        }

        [Fact]
        public void ExportIntAndString()
        {
            var data = GetSample1();
            var csv = new CsvEz<SampleDataClass>(null)
                .Column("Int", "intField")
                .Column("Str", "stringField");
            var result = csv.Export(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.True(MatchExport(result, "Int,Str", "1,one", "2,two"));
        }

        [Fact]
        public void ExportIntAndString_ShouldBe_Quoted_AND_SplitCharSemiColon()
        {
            var data = GetSample1();
            var splitChar = ';';
            var csv = new CsvEz<SampleDataClass>(null)
                .Column("Int", "intField")
                .Column("Str", "stringField")
                ////.QuoteFields()
                .SplitChar(splitChar);
            var result = csv.Export(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.True(MatchExport(result, $"Int{splitChar}Str", $"1{splitChar}one", $"2{splitChar}two"));
        }

        [Fact]
        public void ExportIntQuoted()
        {
            var data = GetSample1();
            var csv = new CsvEz<SampleDataClass>(null)
                .Column("Int", "intField");
            //.QuoteFields();
            var result = csv.Export(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.True(MatchExportPipes(result, "Int", "1", "2"));
        }

        [Fact]
        public void ExportIntAndStringQuoted()
        {
            var data = GetSample1();
            var csv = new CsvEz<SampleDataClass>(null)
                .Column("Int", "intField")
                .Column("Str", "stringField");
            //.QuoteFields();
            var result = csv.Export(data).ToList();
            Assert.Empty(csv.Errors);
            Assert.True(MatchExportPipes(result, "Int,Str", "1,one", "2,two"));
        }

        [Fact]
        public void ExportAllFields()
        {
            var data = GetFullSample();
            var csv = new CsvEz<SampleDataClass>(null)
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
            var result = csv.Export(data).ToList();
            Assert.Empty(csv.Errors);
            var dateTimeExpectedField1 = new DateTime(2001, 1, 1, 1, 1, 1);            
            var dateTimeExpectedProperty1 = new DateTime(2011, 11, 11, 11, 11, 11);
            var dateTimeExpectedField2 = new DateTime(2002, 2, 2, 2, 2, 2);
            var dateTimeExpectedProperty2 = new DateTime(2022, 12, 22, 22, 22, 22);
            Assert.True(MatchExportPipes(result,
                    "sbyte,byte,short,ushort,int,uint,long,ulong,char,float,double,bool,decimal,DateTime,DateTimeOffset,string,sbyte,byte,short,ushort,int,uint,long,ulong,char,float,double,bool,decimal,DateTime,DateTimeOffset,string",
                                $"1,1,0,1,1,1,1,1,1,1,1,False,1,{dateTimeExpectedField1},{new DateTimeOffset(dateTimeExpectedField1)},1,11,11,0,11,11,11,11,11,1,1,1,True,11,{dateTimeExpectedProperty1},{new DateTimeOffset(dateTimeExpectedProperty1)},11",
                                $"2,2,0,2,2,2,2,2,2,2,2,False,2,{dateTimeExpectedField2},{new DateTimeOffset(dateTimeExpectedField2)},2,22,22,0,22,22,22,22,22,2,2,2,True,22,{dateTimeExpectedProperty2},{new DateTimeOffset(dateTimeExpectedProperty2)},22"));
        }

        [Fact]
        public void ExportAllFieldsQuoted()
        {
            var data = GetFullSample();
            var csv = new CsvEz<SampleDataClass>(null)
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
            var result = csv.Export(data).ToList();
            Assert.Empty(csv.Errors);
            var dateTimeExpectedField1 = new DateTime(2001, 1, 1, 1, 1, 1);
            var dateTimeExpectedProperty1 = new DateTime(2011, 11, 11, 11, 11, 11);
            var dateTimeExpectedField2 = new DateTime(2002, 2, 2, 2, 2, 2);
            var dateTimeExpectedProperty2 = new DateTime(2022, 12, 22, 22, 22, 22);
            Assert.True(MatchExportPipes(result,
                "sbyte,byte,short,ushort,int,uint,long,ulong,char,float,double,bool,decimal,DateTime,DateTimeOffset,string,sbyte,byte,short,ushort,int,uint,long,ulong,char,float,double,bool,decimal,DateTime,DateTimeOffset,string",
                $"1,1,0,1,1,1,1,1,1,1,1,False,1,{dateTimeExpectedField1},{new DateTimeOffset(dateTimeExpectedField1)},1,11,11,0,11,11,11,11,11,1,1,1,True,11,{dateTimeExpectedProperty1},{new DateTimeOffset(dateTimeExpectedProperty1)},11",
                $"2,2,0,2,2,2,2,2,2,2,2,False,2,{dateTimeExpectedField2},{new DateTimeOffset(dateTimeExpectedField2)},2,22,22,0,22,22,22,22,22,2,2,2,True,22,{dateTimeExpectedProperty2},{new DateTimeOffset(dateTimeExpectedProperty2)},22"));
        }

    }
}
