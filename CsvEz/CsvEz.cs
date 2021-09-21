
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace CsvEz
{

    public class CsvEz<T> where T: class
    {
        private readonly bool _includeHeaders;
        private readonly Func<T> _generate;
        private readonly List<CsvField> _columns = new();
        private readonly List<FieldInfo> _dataFields;
        private readonly string _dataTypeName;
        //private bool _quoteFields;
        private char _splitChar;
        private readonly List<PropertyInfo> _dataProps;
        public Dictionary<int, List<string>> Errors { get; set; }
        public bool InError => Errors?.Any() ?? false;

        public CsvEz(Func<T> generate = null)
        {
            _generate = generate;
            var dataType = typeof(T);
            _dataTypeName = dataType.FullName;
            _dataFields = dataType.GetFields().ToList();
            _dataProps = dataType.GetProperties().ToList();
            _splitChar = ',';
            _includeHeaders = true;
        }

        public static object Parser<TField>(string val, Func<string, object> parse, NumericFormatFlags? numericFormatFlags = null)
        {

            var filteredValue = val.TrimQuotesIfEnclosed().Replace("\"\"","\"");
            if (numericFormatFlags.HasValue)
            {
                
                if((numericFormatFlags & NumericFormatFlags.AcceptAccountingNegatives) == NumericFormatFlags.AcceptAccountingNegatives )
                {
                    filteredValue = filteredValue
                            .TrimAccountingNegativeIfEnclosed();
                }
                if ((numericFormatFlags & NumericFormatFlags.IgnoreCurrencySymbols) == NumericFormatFlags.IgnoreCurrencySymbols)
                {
                    var negativeSymbol = string.Empty;
                    if(filteredValue.StartsWith(NumberFormatInfo.CurrentInfo.NegativeSign))
                    {
                        negativeSymbol = NumberFormatInfo.CurrentInfo.NegativeSign;
                        filteredValue = filteredValue.TrimStart(negativeSymbol);
                    }
                    filteredValue = negativeSymbol
                        + filteredValue.TrimStart(NumberFormatInfo.CurrentInfo.CurrencySymbol);
                }
                if ((numericFormatFlags & NumericFormatFlags.IgnoreSeparators) == NumericFormatFlags.IgnoreSeparators)
                {
                    filteredValue = filteredValue.Replace(NumberFormatInfo.CurrentInfo.NumberGroupSeparator, string.Empty).Trim();
                }
            }
            return string.IsNullOrWhiteSpace(filteredValue) ? default(TField) : (TField)parse(filteredValue);
        }
        public IEnumerable<string> Export(IEnumerable<T> source)
        {
            var result = new List<string>();
            Errors = new();
            var lineCount = 0;
            if (_includeHeaders)
            {
                result.Add(FormatHeaderLine());
                lineCount++;
            }

            foreach (var item in source)
            {
                var errors = new List<string>();
                var exportedLine = "";
                try
                {
                    exportedLine = FormatOutputLine(item, ref errors);
                }
                catch (Exception e)
                {
                    errors.Add($"{lineCount}: {e.Message}");
                }
                if (errors.Any())
                    Errors[lineCount] = errors;
                lineCount++;
                result.Add(exportedLine);
            }

            return result;
        }

        public IEnumerable<T> Import(IEnumerable<string> source)
        {
            if (_generate == null)
                throw new ArgumentException("Cannot import without specifying a generate function");

            var result = new List<T>();
            Errors = new();
            var lineCount = 0;
            foreach (var item in source)
            {
                var errors = new List<string>();
                try
                {
                    if (lineCount == 0 && _includeHeaders)
                        ConfirmHeaders(item, ref errors);
                    else
                        result.Add(ParseInputLine(item, ref errors));
                }
                catch (Exception e)
                {
                    errors.Add($"{lineCount}: {e.Message}");
                }

                if (errors.Any())
                    Errors[lineCount] = errors;
                lineCount++;
            }

            return result;
        }

        public CsvEz<T> SplitChar(char c)
        {
            _splitChar = c;
            return this;
        }

        public CsvEz<T> Column(string colName, string fieldName = null, string format = null, NumericFormatFlags? numericFormatFlags = null)
        {
            fieldName ??= colName;
            var field = _dataFields.FirstOrDefault(f => f.Name == fieldName);
            var prop = _dataProps.FirstOrDefault(p => p.Name == fieldName);
            if (field == null && prop == null)
                throw new ArgumentException($"Field {fieldName} does not exist on type {_dataTypeName}");
            var fieldType = field?.FieldType;
            var propType = prop?.PropertyType;
            var type = fieldType ?? propType;
            var underlyingType = Nullable.GetUnderlyingType(type);
            var isNullable = underlyingType != null;            
            var typeName = underlyingType?.Name??type.Name;
            Action<T, string> importAction;
            Func<T, string> exportAction;
            var converter = TypeDescriptor.GetConverter(type);            
            switch (typeName)
            {
                case "SByte":
                    importAction = MakeImportAction<sbyte>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop, (format == null) ? null : v => ((sbyte)v).ToString(format));
                    break;
                case "Byte":
                    importAction = MakeImportAction<byte>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop, (format == null) ? null : v => ((byte)v).ToString(format));
                    break;
                case "Int16":
                    importAction = MakeImportAction<short>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop, (format == null) ? null : v => ((short)v).ToString(format));
                    break;
                case "UInt16":
                    importAction = MakeImportAction<ushort>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop, (format == null) ? null : v => ((ushort)v).ToString(format));
                    break;
                case "Int32":
                    importAction = MakeImportAction<int>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop, (format == null) ? null : v => ((int)v).ToString(format));
                    break;
                case "UInt32":
                    importAction = MakeImportAction<uint>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop, (format == null) ? null : v => ((uint)v).ToString(format));
                    break;
                case "Int64":
                    importAction =  MakeImportAction<long>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop, (format == null) ? null : v => ((long)v).ToString(format));
                    break;
                case "UInt64":
                    importAction =  MakeImportAction<ulong>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop, (format == null) ? null : v => ((ulong)v).ToString(format));
                    break;
                case "Char":
                    if (format != null)
                        throw new ArgumentException($"Cannot specify a format for a char property/field: {fieldName}");
                    importAction =  MakeImportAction<char>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop);
                    break;
                case "Single":
                    importAction =  MakeImportAction<float>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop, (format == null) ? null : v => ((float)v).ToString(format));
                    break;
                case "Double":
                    importAction =  MakeImportAction<double>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop, (format == null) ? null : v => ((double)v).ToString(format));
                    break;
                case "Boolean":
                    if (format != null)
                        throw new ArgumentException($"Cannot specify a format for a bool property/field: {fieldName}");
                    importAction = MakeImportAction<bool>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop);
                    break;
                case "Decimal":
                    importAction = MakeImportAction<decimal>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop, (format == null) ? null : v => ((decimal)v).ToString(format));
                    break;
                case "DateTime":
                    importAction = MakeImportAction<DateTime>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop, (format == null) ? null : v => ((DateTime)v).ToString(format));
                    break;
                case "DateTimeOffset":
                    importAction = MakeImportAction<DateTimeOffset>(field, prop, converter.ConvertFromString, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop, (format == null) ? null : v => ((DateTimeOffset)v).ToString(format));
                    break;
                case "String":
                    importAction = (o, val) =>
                    {
                        if (field != null)
                            field.SetValue(o, Parser<string>(val, v=>v));
                        else if(prop != null)
                            prop.SetValue(o, Parser<string>(val, v => v));
                    };//MakeImportAction<string>(field, prop, v => v, isNullable, numericFormatFlags);
                    exportAction = MakeExportAction(field, prop);
                    break;
                default:
                    throw new ArgumentException(
                        $"Field for column {colName}, do not support type {(isNullable ? type.FullName : type.Name)}, please use explicit Field function");
            }

            return Column(colName, importAction, exportAction);
        }

        public CsvEz<T> Column(string colName, Action<T, string> importAction, Func<T, string> exportAction)
        {
            _columns.Add(new CsvField
            {
                ColName = colName,
                Import = importAction,
                Export = exportAction,
            });
            return this;
        }
        public CsvEz<T> SkipColumn(string colName)
        {
            _columns.Add(new CsvFieldToSkip
            {
                ColName = colName
            });
            return this;
        }
        private T ParseInputLine(string item, ref List<string> errors)
        {
            var splitLine = item.SplitCSVLine(_splitChar);
            if (splitLine.Count != _columns.Count)
            {
                errors.Add("Mismatching number of columns");
                return default(T);
            }
            else
            {
                var newItem = _generate();
                for (var colNum = 0; colNum < _columns.Count; colNum++)
                {
                    if (_columns[colNum] is CsvFieldToSkip)
                        continue;
                    try
                    {
                        _columns[colNum].Import(newItem, splitLine[colNum]);
                    }
                    catch (Exception e)
                    {
                        errors.Add($"{_columns[colNum].ColName}, {e.Message}");
                    }
                }
                return newItem;
            }

        }

        private void ConfirmHeaders(string line, ref List<string> errors)
        {
            var splitLine = line.SplitCSVLine(_splitChar);
            if (_columns.Count != splitLine.Count)
            {
                errors.Add("Header mismatching number of columns");
            }
            else
            {
                for (var colNum = 0; colNum < _columns.Count; colNum++)
                {
                    var splitLineColumn = splitLine[colNum].TrimQuotesIfEnclosed();
                    if (_columns[colNum].ColName != splitLineColumn)
                    {
                        errors.Add(
                            $"Mismatching column header starting at {splitLineColumn} (should be {_columns[colNum].ColName})");
                        break;
                    }
                }
            }
        }

        private string FormatOutputLine(T item, ref List<string> errors)
        {
            var result = new List<string>();
            foreach (var col in _columns)
            {
                if (col is CsvFieldToSkip)
                    continue;
                var output = string.Empty;
                try
                {
                    output = col.Export(item);
                }
                catch (Exception e)
                {
                    errors.Add(e.Message);
                }
                result.Add(output);
            }

            return string.Join(_splitChar, result);
        }

        private string FormatHeaderLine()
        {
            return string.Join(_splitChar, _columns.Select(f => f.ColName));
        }

        private Func<T, string> MakeExportAction(FieldInfo field, PropertyInfo prop, Func<object, string> format = null)
        {
            if (field != null)
            {
                return (obj) =>
                {
                    var val = field.GetValue(obj);
                    if (val != null)
                        return format?.Invoke(val) ?? val.ToString();
                    else
                        return "";
                };
            }
            else
            {
                return (obj) =>
                {
                    var val = prop.GetValue(obj);
                    if (val != null)
                        return format?.Invoke(val) ?? val.ToString();
                    else
                        return "";
                };
            }
        }


        private Action<T, string> MakeImportAction<TField>(
            FieldInfo field,
            PropertyInfo prop,
            Func<string, object> parse,
            bool isNullable,
            NumericFormatFlags? numericFormatFlags) 
            where TField: struct
        {            
            if (field != null)
            {
                return (obj, val) =>
                {                    
                    var parsedVal = isNullable 
                        ? Parser<TField?>(val, parse, numericFormatFlags) 
                        : Parser<TField>(val, parse, numericFormatFlags);
                    field.SetValue(obj, parsedVal);
                };
            }
            else
            {
                return (obj, val) =>
                {
                    var parsedVal = isNullable 
                        ? Parser<TField?>(val, parse, numericFormatFlags) 
                        : Parser<TField>(val, parse, numericFormatFlags);
                    prop.SetValue(obj, parsedVal);
                };
            }
        }

        private class CsvField
        {
            public string ColName{ get; init; }
            public Action<T, string> Import { get; init; }
            public Func<T, string> Export { get; init; }
        }
        private class CsvFieldToSkip: CsvField
        {
            public CsvFieldToSkip()
            {
                this.Import = null;
                this.Export = null;
            }
        }
    }
}
