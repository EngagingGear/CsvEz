using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvEz;

namespace CSVSample
{
    class Program
    {
        static void Main(string[] args)
        {
            new ConsoleTestExportThenImportSample().Run();
        }
    }
    class ConsoleTestImportExcelExportedNumberThousandSeparated
    {
        public void Run()
        {
            var csv = new CsvEz<Sample>(() => new Sample())
                .Column("Text", "Text")
                .Column("Number", "Number");
            var desktopFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var importedLines = File.ReadAllLines(Path.Combine(desktopFolderPath, "Book4.csv"));// Book1.csv
            csv.Import(importedLines);
            if (csv.Errors.Any())
            {
                Console.WriteLine("Import errors:);");
                foreach(var error in csv.Errors)
                {
                    Console.WriteLine($"{error.Key}\t{string.Join($"{Environment.NewLine}{error.Key}\t", error.Value)}");
                }
                
            }
        }
        class Sample
        {
            public string Text { get; set; }
            public float Number { get; set; }
        }
    }
    class ConsoleTestExportThenImportSample
    {
        public void Run()
        {
            // Configure the file mapping
            var csv = new CsvEz<Sample>(() => new Sample())
                .Column("User Id", "id")
                .Column("User's Name", "name")
                .Column("Date", "when", "MM/dd/yyyy")
                .Column("Time",
                    (s, v) =>
                        {
                            var t = DateTime.Parse(v);
                            s.when += new TimeSpan(0, t.Hour, t.Minute, t.Second);
                        },
                    (s) => s.when.ToString("hh:mm:ss"))
                ;

            // Get data and export it
            var data = GetSampleData();
            var exportedLines = csv.Export(data);
            Console.WriteLine("The export file looks like this:");
            Console.WriteLine(string.Join(Environment.NewLine, exportedLines));
            File.WriteAllLines("sample.csv", exportedLines);
            if (csv.Errors.Any())
            {
                Console.WriteLine("Export errors:);");
                Console.WriteLine($"{string.Join(Environment.NewLine, csv.Errors)}");
            }

            // Now import that same data
            var importedLines = File.ReadAllLines("sample.csv");
            csv.Import(importedLines);
            if (csv.Errors.Any())
            {
                Console.WriteLine("Import errors:);");
                Console.WriteLine($"{string.Join(Environment.NewLine, csv.Errors)}");
            }
        }
        private List<Sample> GetSampleData()
        {
            return new()
            {
                new() {id = 1, name = "John", when = new DateTime(2001, 1, 1, 1, 2, 3)},
                new() {id = 2, name = "Mary", when = new DateTime(2002, 2, 2, 1, 2, 3)},
                new() {id = 3, name = "Tom", when = new DateTime(2003, 3, 3, 1, 2, 3)}
            };
        }
        class Sample
        {
            public int id;
            public string name;
            public DateTime when;
        }
    }
}

