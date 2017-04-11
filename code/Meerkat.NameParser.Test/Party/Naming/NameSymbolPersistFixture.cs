using System.Collections.Generic;
using Meerkat.Party.Naming;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Meerkat.Test.Party.Naming
{
    [TestFixture]
    public class NameSymbolPersistFixture
    {
        [Test]
        public void PersistNameSymbols()
        {
            var symbols = new List<NameSymbol>
            {
                new NameSymbol
                {
                    NameType = NameToken.Title,
                    Value = "Mr",
                    Properties = new Dictionary<string, object>
                    {
                        ["Gender"] = "M"
                    }
                },
                new NameSymbol
                {
                    NameType = NameToken.Academic, Value = "BSc"
                },
            };

            var content = JsonConvert.SerializeObject(symbols, new Newtonsoft.Json.Converters.StringEnumConverter());

            //Assert.That(content, Is.EqualTo("Foo"), "content differs");
        }
    }
}