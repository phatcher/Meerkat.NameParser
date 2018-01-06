using Meerkat.Tools;
using NUnit.Framework;

namespace Meerkat.Test.Tools
{
    [TestFixture]
    public class SymbolTableTest
    {
        private SymbolTable symbols;

        [SetUp]
        public void BuildTable()
        {
            symbols = new SymbolTable();
        }

        [Test]
        public void FindByToken()
        {
            var symbol = new Symbol { Token = TokenClass.Value, Value = "Hello" };
            symbols.Add(symbol);

            Assert.That(symbols.Contains(symbol), Is.True, "Token locate");
        }

        [TestCase("Hello", true)]
        [TestCase("hello", false)]
        [TestCase("HELLO", false)]
        public void FindByValue(string value, bool found)
        {
            var symbol = new Symbol { Token = TokenClass.Value, Value = "Hello" };
            symbols.Add(symbol);

            Assert.That(symbols.Contains(value), Is.EqualTo(found), "Locate " + value + " differs");
        }

        [TestCase("Hello", true)]
        [TestCase("hello", true)]
        [TestCase("HELLO", true)]
        public void FindByValueCi(string value, bool found)
        {
            var symbol = new Symbol { Token = TokenClass.Value, Value = "Hello" };
            symbols.CaseSensitive = false;
            symbols.Add(symbol);

            Assert.That(symbols.Contains(value), Is.EqualTo(found), "Locate " + value + " differs");
        }

        [Test]
        public void GetByToken()
        {
            var symbol = new Symbol { Token = TokenClass.Value, Value = "Hello" };
            symbols.Add(symbol);

            var candidate = symbols[symbol];

            Assert.That(candidate, Is.SameAs(symbol), "Tokens equal");
        }

        [Test]
        public void GetByValue()
        {
            var symbol = new Symbol { Token = TokenClass.Value, Value = "Hello" };
            symbols.Add(symbol);

            var candidate = symbols["Hello"];

            Assert.That(candidate, Is.SameAs(symbol), "Tokens equal");
        }
    }
}
