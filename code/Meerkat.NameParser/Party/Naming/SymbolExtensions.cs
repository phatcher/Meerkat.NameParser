using Meerkat.Tools;

namespace Meerkat.Party.Naming
{
    public static class SymbolExtensions
    {
        public static bool IsNameType(this ISymbol symbol, NameToken token)
        {
            var nt = symbol as NameSymbol;

            return nt?.NameType == token;
        }
    }
}