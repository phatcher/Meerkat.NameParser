using Meerkat.Tools;

namespace Meerkat.Party.Naming
{
    /// <summary>
    /// Parser people names when the string is presented FamilyName first
    /// </summary>
    public class ReversePersonNameParser : PersonNameParser
    {
        protected override int LastForename
        {
            get { return FirstForename == 0 ? -1 : Count; }
        }

        protected override int LastSurname
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
                if (Count == 0)
                {
                    return -1;
                }
                if (FirstTitle > 0)
                {
                    return FirstTitle - 1;
                }
                if (LastSurnameHint > 0)
                {
                    return LastSurnameHint;
                }
                return FirstForename > 0 ? FirstForename - 1 : FirstSurname;
            }
        }

        protected override int LastSuffix
        {
            get
            {
                if (FirstSuffix == 0)
                {
                    return -1;
                }
                if (FirstLetters > 0)
                {
                    return FirstLetters - 1;
                }
                if (Count == 0)
                {
                    return -1;
                }
                if (FirstTitle > 0)
                {
                    return FirstTitle - 1;
                }
                return FirstForename > 0 ? FirstForename - 1 : FirstSuffix;
            }
        }

        protected override void AddValue(ISymbol symbol)
        {
            // Add it to the collection
            Symbols.Add(symbol);

            // Preferrentially assume it's a surname, unless we already have one
            if (FirstSurname == 0)
            {
                FirstSurname = Count;
            }
        }

        protected override void AddMinus(ISymbol symbol)
        {
            // Add it to the collection
            Symbols.Add(symbol);

            // Cope with double-barrelled surname
            if (Count == 2)
            {
                FirstForename = Count + 2;
            }
        }

        protected override void AdjustHints()
        {
            // No recognised Forename
            if (FirstForename == 0)
            {
                if (LastTitle == -1)
                {
                    FirstForename = LastSurname + 1;
                }
                else
                {
                    FirstForename = LastTitle + 1;
                }
            }
        }
    }
}
