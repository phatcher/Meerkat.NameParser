using System.Globalization;
using System.Linq;

namespace Meerkat.Tools
{
    public static class LexExtensions
    {
        public static bool IsLexChar(this int value, LexChar lex)
        {
            return value == (int) lex;
        }

        public static bool IsLexChar(this int value, params LexChar[] lexes)
        {
            return lexes.Select(lex => value.IsLexChar(lex)).Any(result => result);
        }

        public static bool InLexCharRange(this int value, LexChar lowLex, LexChar highLex)
        {
            return value >= (int)lowLex && value <= (int) highLex;
        }

        public static bool IsWhiteSpace(this int value)
        {
            return value.IsLexChar(LexChar.Tab, LexChar.Space);
        }

        public static bool IsDigit(this int value)
        {
            return value.InLexCharRange(LexChar.Zero, LexChar.Nine);
        }

        public static bool IsAlpha(this int value)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory((char)value);
            return category == UnicodeCategory.LowercaseLetter || category == UnicodeCategory.UppercaseLetter;
        }

        public static bool IsAlphaNumeric(this int value)
        {
            return value.IsAlpha() || value.IsDigit();
        }
    }
}
