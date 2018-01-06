using System;
using System.Collections;

namespace Meerkat.Tools
{
    [Serializable]
    public class SymbolTable : CollectionBase, ISymbolTable
    {
        private readonly Hashtable hashTable;
        private bool caseSensitive;

        public SymbolTable()
        {
            hashTable = new Hashtable();
            caseSensitive = true;
        }

        public bool CaseSensitive
        {
            get { return caseSensitive; }
            set { caseSensitive = value; }
        }

        public void Insert(ISymbol value)
        {
            base.InnerList.Add(value);
        }

        public ISymbol this[object key]
        {
            get
            {
                if (key is int)
                {
                    return (ISymbol) List[Convert.ToInt32(key)];
                }

                if (key is ISymbol)
                {
                    key = ((ISymbol)key).Value;
                }
                return (ISymbol)hashTable[SymbolKey(key)];
            }
        }

        public void Add(ISymbol symbol)
        {
            List.Add(symbol);
            hashTable.Add(SymbolKey(symbol.Value), symbol);
        }

        public bool Contains(object key)
        {
            if (key is int)
            {
                var index = Convert.ToInt32(key);
                return index >= 0 && index <= List.Count;
            } 

            if (key is ISymbol)
            {
                key = ((ISymbol)key).Value;
            }
            return hashTable.Contains(SymbolKey(key));
        }

        private object SymbolKey(object key)
        {
            if (CaseSensitive == false && key is string)
            {
                return Convert.ToString(key).ToUpper();
            }
            return key;
        }
    }
}