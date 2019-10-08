using System;
using System.Globalization;
using System.Text;

namespace ALZAGRO.AppRendicionGastos.Fwk.ExtensionMethods
{
    public static class StringExtensionMethods
    {

        public static String ToPascalCase(this String textToChange)
        {

            var str = textToChange.Substring(0, 1);
            var str1 = textToChange.Substring(1, textToChange.Length - 1);

            return str.ToUpper() + str1;
        }

        public static String RemoveDiacritics(this String s)
        {
            String normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        public static double ToDouble(this string s) {
            if (double.TryParse(s.Replace('.', ','), out double i)) {
                return i;
            }
            else {
                return 0.0D;
            }
        }
    }
}
