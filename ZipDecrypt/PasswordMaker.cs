using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipDecrypt
{
    static class PasswordExtension
    {
        public static readonly List<char> chars;
        public static readonly char startChar;
        public static readonly char endChar;

        static PasswordExtension()
        {
            var list = new List<char>();
            // 英数字のみ
            for (var c = '0'; c <= '9'; ++c)
                list.Add(c);
            for (var c = 'a'; c <= 'z'; ++c)
                list.Add(c);
            for (var c = 'A'; c <= 'Z'; ++c)
                list.Add(c);
            // ASCIIコードの全文字
            //for (var c = (char)0x20; c <= (char)0x7e; ++c)
            //    list.Add(c);
            chars = list;
            startChar = chars[0];
            endChar = chars[chars.Count - 1];
        }

        private static char[] _Next(char[] _password)
        {
            for (var l = _password.Length - 1; l >= 0; l--)
            {
                var i = chars.FindIndex(c => c == _password[l]);
                if (i < chars.Count - 1)
                {
                    _password[l] = chars[++i];
                    break;
                }
                _password[l] = startChar;
                if (l == 0)
                {
                    var __password = new List<char>(_password);
                    __password.Insert(0, startChar);
                    return _Next(__password.ToArray());
                }
            }

            return _password;
        }

        public static string Next(this string password)
        {
            return new string(_Next(password.ToCharArray()));
        }
    }

    public class PasswordMaker
    {
        public IEnumerable<string> NextPassword()
        {
            var startChar = PasswordExtension.startChar;
            var password = new string(new [] { startChar });

            while (true)
            {
                password = password.Trim();
                if (password.Length > 0)
                    yield return password;
                password = password.Next();
            }
        }
    }
}
