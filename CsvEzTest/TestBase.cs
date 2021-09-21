using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace CsvEzTest
{
    public class TestBase
    {
        /// <summary>
        /// Match a result file output against varargs expected results
        /// </summary>
        /// <param name="result"></param>
        /// <param name="outputLines"></param>
        /// <returns></returns>
        public bool MatchExport(List<string> result, params string[] outputLines)
        {
            if (result.Count != outputLines.Length)
                return false;
            for (int i = 0; i < result.Count; i++)
                if (result[i] != outputLines[i])
                    return false;
            return true;
        }

        /// <summary>
        /// Match a result file output against varargs expected results. However, the expected
        /// results replace | with " and ^ with \, this is to make it easier to enter " and \ in
        /// the test string without having to escape them all.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="outputLines"></param>
        /// <returns></returns>
        public bool MatchExportPipes(List<string> result, params string[] outputLines)
        {
            if (result.Count != outputLines.Length)
                return false;
            for (int i = 0; i < result.Count; i++)
                if (result[i] != outputLines[i].Replace("|", "\"").Replace("^", "\\"))
                    return false;
            return true;
        }

        public List<string> MakeImportList(params string[] inputLines)
        {
            return inputLines.ToList();
        }

        public List<string> MakeImportListPipes(params string[] inputLines)
        {
            return inputLines.Select(s => s.Replace("|", "\"")).ToList();
        }

        public List<SampleDataClass> GetSample1()
        {
            var list = new List<SampleDataClass>
            {
                new SampleDataClass() {intField = 1, stringField = "one"},
                new SampleDataClass() {intField = 2, stringField = "two"},
            };
            return list;
        }

        public List<SampleDataClass> GetFullSample()
        {
            return new List<SampleDataClass>
            {
                new SampleDataClass()
                {
                    sbyteField = 1,
                    byteField = 1,
                    ushortField = 1,
                    intField = 1,
                    uintField = 1,
                    longField = 1,
                    ulongField = 1,
                    charField = '1',
                    floatField = 1.0f,
                    doubleField = 1.0d,
                    boolField = false,
                    decimalField = 1,
                    dateTimeField = new DateTime(2001, 1, 1, 1, 1, 1),
                    dateTimeOffsetField = new DateTimeOffset(new DateTime(2001, 1, 1, 1, 1, 1)),
                    stringField = "1",
                    SbyteProperty = 11,
                    ByteProperty = 11,
                    UshortProperty = 11,
                    IntProperty = 11,
                    UintProperty = 11,
                    LongProperty = 11,
                    UlongProperty = 11,
                    CharProperty = '1',
                    FloatProperty = 1.0f,
                    DoubleProperty = 1.0d,
                    BoolProperty = true,
                    DecimalProperty = 11,
                    DateTimeProperty = new DateTime(2011, 11, 11, 11, 11, 11),
                    DateTimeOffsetProperty = new DateTimeOffset(new DateTime(2011, 11, 11, 11, 11, 11)),
                    StringProperty = "11",
                },
                new SampleDataClass()
                {
                    sbyteField = 2,
                    byteField = 2,
                    ushortField = 2,
                    intField = 2,
                    uintField = 2,
                    longField = 2,
                    ulongField = 2,
                    charField = '2',
                    floatField = 2.0f,
                    doubleField = 2.0d,
                    boolField = false,
                    decimalField = 2,
                    dateTimeField = new DateTime(2002, 2, 2, 2, 2, 2),
                    dateTimeOffsetField = new DateTimeOffset(new DateTime(2002, 2, 2, 2, 2, 2)),
                    stringField = "2",
                    SbyteProperty = 22,
                    ByteProperty = 22,
                    UshortProperty = 22,
                    IntProperty = 22,
                    UintProperty = 22,
                    LongProperty = 22,
                    UlongProperty = 22,
                    CharProperty = '2',
                    FloatProperty = 2.0f,
                    DoubleProperty = 2.0d,
                    BoolProperty = true,
                    DecimalProperty = 22,
                    DateTimeProperty = new DateTime(2022, 12, 22, 22, 22, 22),
                    DateTimeOffsetProperty = new DateTimeOffset(new DateTime(2022, 12, 22, 22, 22, 22)),
                    StringProperty = "22",
                },
            };
        }
        public bool CompareList<T>(Func<T, T, bool> comp, List<T> l1, params T[] compareTo)
        {
            return CompareList(comp, l1, compareTo.ToList());
        }

        public bool CompareList<T>(Func<T, T, bool> comp, List<T> list1, List<T> list2)
        {
            if (list1.Count != list2.Count)
                return false;
            for (var i = 0; i < list1.Count; i++)
                if (! comp(list1[i], list2[i]))
                    return false;
            return true;
        }
    }
}
