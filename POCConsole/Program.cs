using System;
using System.ComponentModel;

namespace POCConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int a;
            int? b;

            var converter = TypeDescriptor.GetConverter(typeof(int));
            a = (int)converter.ConvertFromString("2");
            Console.WriteLine(a);

            var converter2 = TypeDescriptor.GetConverter(typeof(int?));
            b = (int?)converter2.ConvertFromString("2");
            Console.WriteLine(b);

            var x = default(DateTimeOffset);
            Console.WriteLine(x);

            var y = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0).ToUniversalTime());
            Console.WriteLine(y);

            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalDigits);
            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator);
            Console.WriteLine(string.Join(",", System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyGroupSizes));
            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyNegativePattern);
            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyPositivePattern);
            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol);
            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.DigitSubstitution);
            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.NaNSymbol);
            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.NegativeSign);
            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.PositiveSign);
            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalDigits);
            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.NumberGroupSeparator);
            Console.WriteLine(string.Join(",", System.Globalization.NumberFormatInfo.CurrentInfo.NumberGroupSizes));
            Console.WriteLine(System.Globalization.NumberFormatInfo.CurrentInfo.NumberNegativePattern);

            string s = "1,223.23";
            var converter3 = TypeDescriptor.GetConverter(typeof(float));
            //float value = (float)converter3.ConvertFromString(null,System.Globalization.CultureInfo.CurrentCulture, s);
            float value = float.Parse(s);
            Console.WriteLine(value);

            string s1 = "Rs 2000";
            Console.WriteLine(s1.Substring("Rs".Length));
            Console.WriteLine(default(string)==null);
        }
    }
}
