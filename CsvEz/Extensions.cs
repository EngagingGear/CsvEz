using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CsvEz
{
    public static class Extensions
    {
        public static string TrimStart(this string str, string trimString)
        {
            return str.StartsWith(trimString) ? str.Substring(trimString.Length): str;
        }
        public static string TrimQuotesIfEnclosed(this string str/*, bool trimWhiteSpaces=false*/)
        {
            return str.TrimIfEnclosedWith(Constants.Quotes, Constants.Quotes);
        }

        public static string TrimAccountingNegativeIfEnclosed(this string str)
        {
            string trimmedString;
            if (str.TryTrimIfEnclosedWith(Constants.AccountingNegativeStartSymbol, Constants.AccountingNeEndSymbol, out trimmedString))
            {
                return NumberFormatInfo.CurrentInfo.NegativeSign + trimmedString;
            }
            return str;
        }
        /// <summary>
        /// only trim, if str starts with startTrimString and ends with endTrimString
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startTrimString"></param>
        /// <param name="endTrimString"></param>
        /// <returns></returns>
        public static string TrimIfEnclosedWith(this string str, string startTrimString, string endTrimString)
        {
            string trimmedString;
            str.TryTrimIfEnclosedWith(startTrimString, endTrimString, out trimmedString);
            return trimmedString;
        }
        public static bool TryTrimIfEnclosedWith(this string str, string startTrimString, string endTrimString, out string trimmedString)
        {
            if (str.StartsWith(startTrimString) && str.EndsWith(endTrimString))
            {
                trimmedString = str.Substring(0,str.Length-endTrimString.Length).Substring(startTrimString.Length);
                return true;
            }
            trimmedString = str;
            return false;
        }
        public static List<string> SplitCSVLine(this string line, char splitChar)
        {
            var items = line.Split(splitChar);
            var index = 0;
            var item = string.Empty;
            List<string> split = new List<string>();
            while(index < items.Length)
            {
                item += items[index];
                var quotesCount=item.Count(c=>c == Constants.CharQuotes);
                if(quotesCount % 2 == 0)
                {
                    if(quotesCount>0 && (!item.StartsWith(Constants.Quotes) || !item.EndsWith(Constants.Quotes)))
                    {
                        throw new Exception($"Invalid line: {line}");
                    }

                    split.Add(item);
                    item = string.Empty;
                }
                else
                {
                    item += splitChar;
                }
                index++; 
            }
            if (!string.IsNullOrEmpty(item))
            {
                throw new Exception($"Invalid line: {line}");
            }
            return split;
        }
    }
}
