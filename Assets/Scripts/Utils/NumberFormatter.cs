using System.Globalization;

namespace Utils
{
    public static class NumberFormatter
    {
        private static readonly string[] Suffixes =
        {
            "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No"
        };

        public static string Format(double value)
        {
            if (value < 1000)
            {
                return value.ToString("0");
            }

            var suffixIndex = 0;

            while (value >= 1000 && suffixIndex < Suffixes.Length - 1)
            {
                value /= 1000;
                suffixIndex++;
            }

            return value.ToString("0.#", CultureInfo.InvariantCulture) + Suffixes[suffixIndex];
        }
    }
}