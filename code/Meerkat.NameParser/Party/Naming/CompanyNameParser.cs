using System.Collections;
using System.IO;
using Meerkat.Tools;

namespace Meerkat.Party.Naming
{
    /// <summary>
    /// Parse a string containing a company name into its components
    /// </summary>
    public class CompanyNameParser : INameParser
    {
        private IName company;
        protected ArrayList symbols;
        protected readonly NameLexer Lex;

        public CompanyNameParser()
        {
            Lex = new NameLexer();
        }

        private IName Company
        {
            get => company;
            set => company = value;
        }

        private void Initialise()
        {
        }


        private void AdjustHints()
        {
        }


        private void ConstructName()
        {
        }


        private void TidyName()
        {
        }

        public string Assemble(IName name)
        {
            return Assemble(name, string.Empty);
        }

        public string Assemble(IName name, string format)
        {
            return string.Empty;
        }

        public void Parse(string name, IName value)
        {
            Parse(name, company, string.Empty);
        }

        public void Parse(string name, IName value, string format)
        {
            // Assign the company
            Company = company;

            // Initialise the person and ourselves
            Initialise();

            // Process the supplied name
            Lex.Source = new StringReader(name);

            var symbol = Lex.GetToken();
            while (symbol.Token != TokenClass.EOF) {
                switch (symbol.Token) {
                    case TokenClass.Value:
                        var nt = symbol as NameSymbol;
                        
                        switch (nt.NameType)
                        {
                            case NameToken.Title:
                                break;
                            case NameToken.Prefix:
                                break;
                            case NameToken.Suffix:
                                break;
                        }

                        break;
                }
                symbol = Lex.GetToken();
            }

            AdjustHints();

            // Rebuild the name from the parsed components
            ConstructName();

            // Tidy up the elements
            TidyName();
        }
    }
}