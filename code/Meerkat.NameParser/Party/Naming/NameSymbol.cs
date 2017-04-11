using System.Collections.Generic;
using Meerkat.Tools;

namespace Meerkat.Party.Naming
{
    public class NameSymbol : Symbol
    {
        public NameSymbol()
        {
            Token = TokenClass.Value;
            Properties = new Dictionary<string, object>();
        }

        public NameToken NameType { get; set; }

        public Dictionary<string, object> Properties { get; set; }
    }
}