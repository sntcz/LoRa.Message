using System;
using System.Collections.Generic;
using System.Text;

namespace LoRa.Message
{
    public static class ConvertExtension
    {
        /// <summary>
        /// Converts an array of bytes to its equivalent string representation that is encoded as hex string.
        /// </summary>
        /// <param name="data">An array of bytes.</param>
        /// <returns>The string representation of the contents of <paramref name="data"/>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="data"/> is null.</exception>
        public static string ToHexString(this byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            StringBuilder hex = new StringBuilder(data.Length * 2);
            for (int i = 0; i < data.Length; i++)
                hex.AppendFormat("{0:X2}", data[i]);
            return hex.ToString();
        }

        /// <summary>
        /// Converts a span of an array of bytes to its equivalent string representation that is encoded as hex string.
        /// </summary>
        /// <param name="data">A span of an array of bytes.</param>
        /// <returns>The string representation of the contents of <paramref name="data"/>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="data"/> is null.</exception>
        public static string ToHexString(this Span<byte> data)
        {
            StringBuilder hex = new StringBuilder(data.Length * 2);
            for (int i = 0; i < data.Length; i++)            
                hex.AppendFormat("{0:X2}", data[i]);
            return hex.ToString();
        }

        /// <summary>
        /// Converts an array of bytes to its equivalent string representation that is reversed and encoded as hex string.
        /// </summary>
        /// <param name="data">An array of bytes.</param>
        /// <returns>The string representation of the contents of <paramref name="data"/>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="data"/> is null.</exception>
        public static string ToReverseHexString(this byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            StringBuilder hex = new StringBuilder(data.Length * 2);
            for (int i = data.Length - 1; i >= 0; i--)
                hex.AppendFormat("{0:X2}", data[i]);
            return hex.ToString();
        }

        /// <summary>
        /// Converts a span of an array of bytes to its equivalent string representation that is reversed and encoded as hex string.
        /// </summary>
        /// <param name="data">A span of an array of bytes.</param>
        /// <returns>The string representation of the contents of <paramref name="data"/>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="data"/> is null.</exception>
        public static string ToReverseHexString(this Span<byte> data)
        {
            StringBuilder hex = new StringBuilder(data.Length * 2);
            for (int i = data.Length - 1; i >= 0; i--)
                hex.AppendFormat("{0:X2}", data[i]);
            return hex.ToString();
        }

        /// <summary>
        /// Converts an array of bytes to its equivalent string representation that is encoded with base-64 digits.
        /// </summary>
        /// <param name="data">An array of bytes.</param>
        /// <returns>The string representation, in base 64, of the contents of <paramref name="data"/>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="data"/> is null.</exception>
        public static string ToBase64String(this byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Converts a span of an array of bytes to its equivalent string representation that is encoded with base-64 digits.
        /// </summary>
        /// <param name="data">A span of an array of bytes.</param>
        /// <returns>The string representation, in base 64, of the contents of <paramref name="data"/>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="data"/> is null.</exception>
        public static string ToBase64String(this Span<byte> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            return Convert.ToBase64String(data.ToArray());
        }

        public static byte[] FromHexString(this string hex)
        {
            if (String.IsNullOrEmpty(hex))
                return Array.Empty<byte>();
            if (hex.Length % 2 != 0)
                throw new ArgumentException("");
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static byte[] FromBase64String(this string hex)
        {
            return Convert.FromBase64String(hex);
        }

       


        }
}
