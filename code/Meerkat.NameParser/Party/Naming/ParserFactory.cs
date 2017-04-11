using System;

using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Meerkat.Tools;

namespace Meerkat.Party.Naming
{
    public static class ParserFactory
    {
        private static readonly IDictionary<string, SymbolTable> tables;

        static ParserFactory()
        {
            tables = new Dictionary<string, SymbolTable>();
        }

        public static PersonNameParser StandardPersonParser(bool reverse = false)
        {
            // Excluded due to case differenc
            // 	{ "NameType": "Civil", "Value": "LT", "Properties":{ "Group": 1, "Order": 4, "Name": "Lady of the Thistle", "Gender": "F" } },

            var parser = reverse ? new ReversePersonNameParser() : new PersonNameParser();
            parser.Lex.SymbolTable = StandardSymbolTable("PersonNames");

            return parser;
        }

        public static ISymbolTable StandardSymbolTable(string name, bool cache = true)
        {
            var resourcePath = "Meerkat.Party.Naming." + name + ".json";

            Func<string, SymbolTable> f = (x) =>
            {
                using (var reader = new StreamReader(GetEmbeddedResourceStream(x)))
                {
                    var data = reader.ReadToEnd();
                    return (SymbolTable) SymbolTable(data);
                }
            };

            return SymbolTable(resourcePath, cache, f);
        }

        public static ISymbolTable FileSymbolTable(string fileName, bool cache = true)
        {
            Func<string, SymbolTable> f = (x) =>
            {
                var data = File.ReadAllText(x);

                return (SymbolTable) SymbolTable(data);

            };

            return SymbolTable(fileName, cache, f);
        }

        public static ISymbolTable SymbolTable(string data)
        {
            var table = new SymbolTable
            {
                CaseSensitive = false
            };

            table.Load<NameSymbol>(data);

            return table;
        }

        private static ISymbolTable SymbolTable(string name, bool cache, Func<string, SymbolTable> func)
        {
            SymbolTable st;
            if (cache)
            {
                if (!tables.TryGetValue(name, out st))
                {
                    st = func(name);
                    tables[name] = st;
                }
            }
            else
            {
                st = func(name);
            }

            return st;
        }

        /// <summary>
        /// Get the list of all emdedded resources in the assembly.
        /// </summary>
        /// <returns>An array of fully qualified resource names</returns>
        private static string[] GetEmbeddedResourceNames()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceNames();
        }

        /// <summary>
        /// Takes the full name of a resource and loads it in to a stream.
        /// </summary>
        /// <param name="resourceName">Assuming an embedded resource is a file
        /// called info.png and is located in a folder called Resources, it
        /// will be compiled in to the assembly with this fully qualified
        /// name: Full.Assembly.Name.Resources.info.png. That is the string
        /// that you should pass to this method.</param>
        /// <returns></returns>
        private static Stream GetEmbeddedResourceStream(string resourceName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        }
    }
}