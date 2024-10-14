using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class UtilsNumber
    {
        private static readonly int charA = Convert.ToInt32('a');
        private static readonly string strFormat = "{0:0.##}{1}";
        private static readonly float minValueFormat = 10000.0f;

        private static readonly Dictionary<int, string> units = new Dictionary<int, string>
        {
            {0, ""},
            {1, "K"},
            {2, "M"},
            {3, "B"},
            {4, "T"},
        };

        public static string Format(float value)
        {
            // If value less than min value need to format, return rounded number
            if (value < minValueFormat)
            {
                return Mathf.RoundToInt(value).ToString();
            }

            var n = (int)Math.Log(value, 1000);
            var m = value / Math.Pow(1000, n);
            var unit = string.Empty;

            if (n < units.Count)
            {
                unit = units[n];
            }
            else
            {
                var unitInt = n - units.Count;
                var secondUnit = unitInt % 26;
                var firstUnit = unitInt / 26;
                unit = $"{Convert.ToChar(firstUnit + charA)}{Convert.ToChar(secondUnit + charA)}";
            }

            // Math.Floor(m * 100) / 100) fixes rounding errors
            return string.Format(strFormat, (Math.Floor(m * 100) * 0.01f), unit);
        }
    }
}
