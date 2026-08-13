using UnityEngine;

namespace Reign.Core.Extensions
{
    public static class Utility
    {
        public static string Color32ToHexString(Color32 col)
        {
            return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", col.r, col.g, col.b, col.a);
        }

        public static Color32 HexStringToColor32(string hexString)
        {
            hexString = hexString.Trim();

            // Convert given string to only letters and numbers
            hexString = hexString.Replace("0x", "");
            hexString = hexString.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            r = byte.Parse(hexString[..2], System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hexString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hexString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            if (hexString.Length >= 8)
            {
                a = byte.Parse(hexString.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return new Color32(r, g, b, a);
        }
    }
}
