using System.Collections;

namespace Meerkat.Tools
{
    /// <summary>
    /// Grouping of symbols
    /// </summary>
    public interface ISymbolTable : ICollection
    {
        void Add(ISymbol symbol);

        bool Contains(object key);

        ISymbol this[object key] { get; }
    }
}