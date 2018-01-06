using System.IO;
using Meerkat.Tools;
using NUnit.Framework;

namespace Meerkat.Test.Tools
{
    [TestFixture]
    public class BasicLexTest
    {
        private LanguageLex lex;
        private ISymbol symbol;

        [SetUp]
        public void BuildLexer()
        {
            lex = new LanguageLex();
        }

        [TearDown]
        public void DestroyLexer()
        {
        }

        [Test]
        public void Hello()
        {
            lex.Source = new StringReader("Hello");
            AssertSymbol(TokenClass.Value, "Hello");
            AssertSymbol(TokenClass.EOF, null);
        }

        [Test]
        public void HelloHello()
        {
            lex.Source = new StringReader("Hello Hello");
            AssertSymbol(TokenClass.Value, "Hello");
            ISymbol firstHello = symbol;
            AssertSymbol(TokenClass.Value, "Hello");
            ISymbol secondHello = symbol;
            Assert.That(firstHello, Is.EqualTo(secondHello));
        }

        [Test]
        public void HelloWorld()
        {
            lex.Source = new StringReader("Hello World");
            AssertSymbol(TokenClass.Value, "Hello");
            AssertSymbol(TokenClass.Value, "World");
        }

        [Test]
        public void HelloWorldSpace()
        {
            lex.Source = new StringReader("Hello World");
            lex.SkipSpace = false;
            AssertSymbol(TokenClass.Value, "Hello");
            AssertSymbol(TokenClass.Whitespace, " ");
            AssertSymbol(TokenClass.Value, "World");
        }

        [Test]
        public void QuotedHello()
        {
            lex.Source = new StringReader("\"Hello\"");
            AssertSymbol(TokenClass.String, "Hello");
        }

        [Test]
        public void QuotedHelloWorld()
        {
            lex.Source = new StringReader("\"Hello\" World");
            AssertSymbol(TokenClass.String, "Hello");
            AssertSymbol(TokenClass.Value, "World");
        }

        [Test]
        public void EmptyString()
        {
            lex.Source = new StringReader("\"\"");
            AssertSymbol(TokenClass.String, "");
        }

        [TestCase('=')]
        [TestCase('!')]
        [TestCase('£')]
        [TestCase('$')]
        [TestCase('%')]
        [TestCase('^')]
        [TestCase('&')]
        [TestCase('*')]
        [TestCase('(')]
        [TestCase(')')]
        [TestCase('{')]
        [TestCase('}')]
        [TestCase('[')]
        [TestCase(']')]
        [TestCase('<')]
        [TestCase('>')]
        [TestCase('\\')]
        [TestCase('/')]
        [TestCase('|')]
        [TestCase(']')]
        [TestCase(':')]
        [TestCase('\'')]
        [TestCase('@')]
        [TestCase('#')]
        [TestCase('~')]
        [TestCase('¬')]
        [TestCase('`')]
        public void SingleTokens(char value)
        {
            var str = value.ToString();
            lex.Source = new StringReader(str);
            AssertSymbol((TokenClass) value, str);
        }

        [Test]
        public void DoubleTokens()
        {
            lex.Source = new StringReader(">= <=");
            AssertSymbol(TokenClass.GTE, ">=");
            AssertSymbol(TokenClass.LTE, "<=");
        }

        [Test]
        public void IntegerTest()
        {
            lex.Source = new StringReader("100");
            AssertSymbol(TokenClass.Number, 100);
        }

        [Test]
        public void FloatTest()
        {
            lex.Source = new StringReader("100.1");
            AssertSymbol(TokenClass.Number, 100.1);
        }

        [Test]
        public void ExpFloatTest()
        {
            lex.Source = new StringReader("100.1E10");
            AssertSymbol(TokenClass.Number, 1001000000000.0);
        }

        [Test]
        public void PlusExpFloatTest()
        {
            lex.Source = new StringReader("100.1E+10");
            AssertSymbol(TokenClass.Number, 1001000000000.0);
        }

        [Test]
        public void NegExpFloatTest()
        {
            lex.Source = new StringReader("100.1E-10");
            AssertSymbol(TokenClass.Number, 1.001E-08);
        }

        [Test]
        public void PlusExpression()
        {
            lex.Source = new StringReader("100+20");
            AssertSymbol(TokenClass.Number, 100);
            AssertSymbol(TokenClass.Plus, "+");
            AssertSymbol(TokenClass.Number, 20);
        }

        private void AssertSymbol(TokenClass tokenClass, object value)
        {
            symbol = lex.GetToken();
            Assert.That(tokenClass, Is.EqualTo(symbol.Token), "Symbol class");
            Assert.That(value, Is.EqualTo(symbol.Value), "Symbol value");
        }
    }
}