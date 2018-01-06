using System.Collections.Generic;
using Newtonsoft.Json;

namespace Meerkat.Tools
{
    public static class SymbolTableExtensions
    {
        public static void Load<T>(this ISymbolTable table, string data)
            where T : ISymbol
        {
            var symbols = JsonConvert.DeserializeObject<IList<T>>(data);

            foreach (var symbol in symbols)
            {
                table.Add(symbol);
            }
        }
    }
}