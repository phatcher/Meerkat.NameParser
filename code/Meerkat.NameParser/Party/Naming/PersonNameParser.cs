using System;
using System.Collections.Generic;
using System.IO;

using Meerkat.Tools;

namespace Meerkat.Party.Naming
{
    /// <summary>
    /// Parses people names into their components of Title, Given, Prefix, Family, Suffix and Letters
    /// </summary>
    public class PersonNameParser : INameParser
    {
        private string title;
        private string familyname;
        private string givenname;
        private string prefix;
        private string suffix;
        private bool processed = false;

        private int firstSurname;

        private int lastForename;
        private int lastTitle;

        protected bool guard;

        public PersonNameParser()
        {
            Symbols = new List<ISymbol>();
            Lex = new NameLexer();
        }

        protected IList<ISymbol> Symbols { get; set; }

        public NameLexer Lex { get; }

        protected int Count
        {
            get { return Symbols.Count; }
        }

        protected int FirstTitle { get; set; }

        protected int FirstForename { get; set; }

        protected int FirstPrefix { get; set; }

        protected int FirstSurname
        {
            get { return firstSurname; }
            set { firstSurname = Math.Max(value, 0); }
        }

        protected int FirstSuffix { get; set; }

        protected int FirstLetters { get; set; }

        protected virtual int LastTitle
        {
            get
            {
                if (FirstTitle == 0)
                {
                    return -1;
                }
                if (lastTitle > 0)
                {
                    return lastTitle;
                }
                if (FirstForename > 0)
                {
                    return FirstForename - 1;
                }

                // No forenames, so delegate it
                return LastForename;
            }
        }

        protected virtual int LastForename
        {
            get
            {
                if (FirstForename == 0)
                {
                    return -1;
                }
                if (lastForename > 0)
                {
                    return lastForename;
                }
                if (FirstPrefix > 0)
                {
                    return Math.Min(FirstPrefix, FirstSurname) - 1;
                }
                return FirstSurname - 1;
            }
        }

        protected virtual int LastLetters
        {
            get { return FirstLetters == 0 ? -1 : Count - 1; }
        }

        protected virtual int LastPrefix
        {
            get { return FirstPrefix == 0 ? -1 : FirstSurname - 1; }
        }

        protected virtual int LastSurname
        {
            get
            {
                if (FirstSuffix > 0)
                {
                    return FirstSuffix - 1;
                }
                if (FirstLetters > 0)
                {
                    return FirstLetters - 1;
                }

                return Count == 0 || lastForename != 0 ? -1 : Count;
            }
        }

        protected virtual int LastSuffix
        {
            get
            {
                if (FirstSuffix == 0)
                {
                    return -1;
                }
                return FirstLetters > 0 ? FirstLetters - 1 : Count;
            }
        }

        protected virtual int LastRange { get; set; }

        protected int LastSurnameHint { get; set; }

        protected int SurnameHint { get; set; }

        protected IPersonName Person { get; set; }

        public string Assemble(IName name, string format = null)
        {
            var person = (IPersonName) name;
            string fred = null;

            // Set a default format
            if (string.IsNullOrEmpty(format))
            {
                format = "TFS";
            }

            for (var i = 0; i <= format.Length - 1; i++)
            {
                switch (format[i])
                {
                    case 'T':
                        fred +=  person.Title.Trim() + " ";
                        break;
                    case 'S':
                        fred += person.Family.Value.Trim() + " ";
                        break;
                    case 'F':
                        fred += person.Given.Trim() + " ";
                        break;
                }
            }

            while (fred.IndexOf("  ") > -1)
            {
                fred = fred.Replace("  ", " ");
            }

            return fred.Trim();
        }

        public void Parse(string name, IName person, string format = null)
        {
            if (guard)
            {
                return;
            }
            guard = true;

            try
            {
                if (!(person is IPersonName))
                {
                    throw new ArgumentException("Cannot parse as object is not an IPersonName");
                }

                // Assign the person
                Person = (IPersonName) person;

                // Initialise the person and ourselves
                Initialise();

                // Process the supplied name
                Lex.Source = new StringReader(name);

                var symbol = Lex.GetToken();
                while (symbol.Token != TokenClass.EOF)
                {
                    switch (symbol.Token)
                    {
                        case TokenClass.Comma:
                            SurnameHint += 1;
                            break;

                        case TokenClass.Value:
                            var nameSymbol = symbol as NameSymbol;
                            if (nameSymbol == null)
                            {
                                AddValue(symbol);
                            }
                            else
                            {
                                switch (nameSymbol.NameType)
                                {
                                    case NameToken.Title:
                                        AddTitle(nameSymbol);
                                        break;

                                    case NameToken.Given:
                                        AddForename(symbol);
                                        break;

                                    case NameToken.Prefix:
                                        AddPrefix(symbol);
                                        break;

                                    case NameToken.Family:
                                        AddSurname(symbol);
                                        break;

                                    case NameToken.Suffix:
                                        AddSuffix(symbol);
                                        break;

                                    case NameToken.Civil:
                                    case NameToken.Military:
                                    case NameToken.Academic:
                                    case NameToken.Professional:
                                        AddLetters(symbol);
                                        break;

                                    case NameToken.Multiple:
                                        AddMultiple(nameSymbol);
                                        break;
                                }
                            }
                            break;

                        case TokenClass.Minus:
                            AddMinus(symbol);
                            break;

                        case TokenClass.SingleQuote:
                        case TokenClass.Apostrophe:
                            AddSurname(symbol);
                            break;
                    }
                    symbol = Lex.GetToken();
                }

                AdjustHints();

                // Rebuild the name from the parsed components
                ConstructName();

                // Tidy up the elements
                TidyName();

                // Push the values back into the name
                AssignName(lastTitle == 0 ? "TFS" : "FTS");
            }
            finally
            {
                guard = false;
            }
        }

        protected virtual void AddMultiple(NameSymbol symbol)
        {
            // NB We don't add the symbol here as we will be re-dispatching it and the target will add it
            if (symbol.NameType == NameToken.Multiple)
            {
                switch (symbol.Value.ToString())
                {
                    case "SC":
                        if ((FirstForename == 0) | (FirstForename == Count))
                        {
                            symbol.NameType = NameToken.Title;
                            AddTitle(symbol);
                        }
                        else
                        {
                            symbol.NameType = NameToken.Civil;
                            AddLetters(symbol);
                        }
                        break;
                }
            }
        }

        protected virtual void AddTitle(NameSymbol symbol)
        {
            // Add it to the collection
            Symbols.Add(symbol);
            if (FirstTitle == 0)
            {
                FirstTitle = Count;
            }

            if (symbol.Value.ToString() == "The")
            {
                if (Count == 2)
                {
                    // Special case, we probably have <name> The <title>
                    lastForename = Count - 1;
                }
                else
                {
                    // Special case which implies the next word must be of class <Title>
                    FirstForename = Count + 2;
                }
            }
            else if (symbol.Value.ToString() == "of")
            {
                var prev = Symbol(Count - 1) as NameSymbol;
                // Nasty as it can be part of <Title> or <Surname>
                if (prev != null && prev.NameType == NameToken.Title)
                {
                    // Special case which implies the next work must be of class <Title>
                    FirstForename = Count + 2;
                }
                else
                {
                    // Just set the first surname position and hope something else fixes it
                    FirstSurname = Count - 1;
                }
            }
            else
            {
                // Move it forwards and hope it gets resolved later
                FirstForename = Count + 1;
            }
        }

        protected virtual void AddMinus(ISymbol symbol)
        {
            // Add it to the collection
            Symbols.Add(symbol);

            // Attempts to cope with hyphenated surnames but first checks for title and initials
            // Also tries to avoid breaking Jean-Paul Revet etc
            var prev = Symbol(Count - 1) as ISymbol;

            // First check for academic titles early in the name e.g. Dipl-Ing
            if (prev.IsNameType(NameToken.Academic))
            {
                if (Count == 2)
                {
                    // Reset the title
                    FirstTitle = 1;
                    // Reset the letters info
                    FirstLetters = 0;
                    // Kick the forename foreward passed here
                    FirstForename = Count + 2;
                }
            }
            else if ((Count - FirstForename) >= 2 && FirstSurname == 0 && Convert.ToString(prev.Value).Length > 1 && !prev.IsNameType(NameToken.Title))
            {
                // Quick check for multi-barrelled surname
                if (Count > 2 && Symbol(Count - 2).Token == TokenClass.Minus)
                {
                    FirstSurname = Count - 3;
                }
                else
                {
                    FirstSurname = Count - 1;
                }
            }

            else
            {
                LastRange = Count;
            }
        }

        protected virtual void AddPrefix(ISymbol symbol)
        {
            // Add it to the collection
            Symbols.Add(symbol);
            if (FirstPrefix == 0)
            {
                FirstPrefix = Count;
            }
            if (FirstSurname == 0 || FirstSurname == Count)
            {
                FirstSurname = Count + 1;
            }
        }

        protected virtual void AddForename(ISymbol symbol)
        {
            // Add it to the collection
            Symbols.Add(symbol);
            if (FirstForename == 0)
            {
                FirstForename = Count;
            }
        }

        protected virtual void AddSurname(ISymbol symbol)
        {
            // Add it to the collection
            Symbols.Add(symbol);

            if (((symbol.Token == TokenClass.SingleQuote) | (symbol.Token == TokenClass.Apostrophe)) & (Count > 1))
            {
                // Mark the next one as a surname
                LastSurnameHint = Count + 1;

                if (FirstSurname == 0)
                {
                    FirstSurname = Count - 1;
                }
            }
            else if (FirstSurname == 0)
            {
                FirstSurname = Count;
            }
        }

        protected virtual void AddSuffix(ISymbol symbol)
        {
            // Add it to the collection
            Symbols.Add(symbol);
            if (FirstSuffix == 0)
            {
                FirstSuffix = Count;
            }
        }

        protected virtual void AddLetters(ISymbol symbol)
        {
            // Add it to the collection
            Symbols.Add(symbol);
            if (FirstLetters == 0)
            {
                FirstLetters = Count;
            }
        }

        protected virtual void AddValue(ISymbol symbol)
        {
            // Assume it's a forename
            AddForename(symbol);
        }

        /// <summary>
        /// Modify where we think elements of the name start and finish based on current information
        /// </summary>
        protected virtual void AdjustHints()
        {
            // Special case when only titles found
            if (LastTitle >= Count)
            {
                FirstSurname = Count + 1;
                return;
            }

            // Special case when only suffix found
            if (FirstSuffix == 1 && LastSuffix != -1)
            {
                FirstSurname = Count + 1;
                return;
            }

            // Funny case <name> The <title> (..., <title>)
            if (lastForename != 0)
            {
                lastTitle = Count;
                FirstForename = 1;
                FirstSurname = 0;
                FirstLetters = 0;
            }
            else
            {
                // Otherwise, assume an unrecognised <title> token
                if (FirstTitle > 1)
                {
                    FirstTitle = 1;
                }

                // No recognised Surname
                if (FirstSurname == 0)
                {
                    FirstSurname = SurnameHint == 0 ? LastSurname : SurnameHint;

                    if ((LastRange == FirstSurname - 1) & (FirstSurname != LastSurname))
                    {
                        FirstSurname = LastRange - 1;
                    }
                }

                // Fix-up for odd stuff
                if (FirstSurname > LastSurname)
                {
                    FirstSurname = LastSurname;
                }
            }
        }

        /// <summary>
        /// Put the components back into the name
        /// </summary>
        /// <param name="format"></param>
        protected virtual void AssignName(string format)
        {
            Person.Title = title;
            Person.Given = givenname;

            var fn = Person.Family;
            fn.Prefix = prefix;
            fn.Core = familyname;
            fn.Suffix = suffix;

            // NB Presumes we are using PersonName
            Person.Envelope = Assemble((IName) Person, format);
        }

        protected virtual void ConstructName()
        {
            // Produce the name components, we use a guard variable to avoid recursive calls
            // Title...
            for (var i = FirstTitle; i <= LastTitle; i++)
            {
                title += Convert.ToString(Symbol(i).Value) + " ";
            }

            // Forename...
            for (var i = FirstForename; i <= LastForename; i++)
            {
                givenname += Convert.ToString(Symbol(i).Value) + " ";
            }

            // Prefix...
            for (var i = FirstPrefix; i <= LastPrefix; i++)
            {
                prefix += Convert.ToString(Symbol(i).Value) + " ";
            }

            // Surname
            for (var i = FirstSurname; i <= LastSurname; i++)
            {
                familyname += Convert.ToString(Symbol(i).Value) + " ";
            }

            // Suffix
            for (var i = FirstSuffix; i <= LastSuffix; i++)
            {
                suffix += Convert.ToString(Symbol(i).Value) + " ";
            }
        }

        protected void Initialise()
        {
            Symbols = new List<ISymbol>();

            title = string.Empty;
            givenname = string.Empty;
            prefix = string.Empty;
            familyname = string.Empty;
            suffix = string.Empty;

            FirstTitle = 0;
            FirstForename = 0;
            FirstPrefix = 0;
            FirstSurname = 0;
            FirstSuffix = 0;

            SurnameHint = 0;
            LastRange = 0;
        }

        protected virtual void TidyName()
        {
            title = title.Replace(" - ", "-");
            title = title.Trim();

            givenname = givenname.Replace(" - ", "-");
            givenname = givenname.Trim();

            prefix = prefix.Trim();

            prefix = prefix.Replace(" - ", "-");
            prefix = prefix.Replace(" ' ", "'");
            prefix = prefix.Replace(" ` ", "`");
            prefix = prefix.Trim();

            familyname = familyname.Replace(" - ", "-");
            familyname = familyname.Replace(" ' ", "'");
            familyname = familyname.Replace(" ` ", "`");
            familyname = familyname.Trim();

            familyname = familyname.Trim();

            suffix = suffix.Trim();
        }

        private ISymbol Symbol(int index)
        {
            // Adjusts the logic so we can use 1-based logic due to convert from VB
            return Symbols[index - 1];
        }
    }
}