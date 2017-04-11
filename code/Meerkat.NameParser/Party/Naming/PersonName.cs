using System;

namespace Meerkat.Party.Naming
{
    /// <summary>
    /// Implementation of a person's name
    /// </summary>
    public class PersonName : IPersonName, IName
    {
        private string title;
        private string forename;
        private IName familyName;
        private string letters;
        private string envelope;
        private string salutation;
        private INameParser parser;

        /// <summary>
        /// Simple parser so we always have a strategy, just put the value in the Envelope property
        /// </summary>
        private class SimpleParser : INameParser
        {
            public string Assemble(IName name)
            {
                return Assemble(name, string.Empty);
            }

            public string Assemble(IName name, string format)
            {
                return ((IPersonName)name).Envelope;
            }

            public void Parse(string value, IName name)
            {
                Parse(value, name, string.Empty);
            }

            public void Parse(string value, IName name, string format)
            {
                // Just put it in the envelope
                ((IPersonName)name).Envelope = value;
            }
        }

        public PersonName()
        {
            familyName = new Name();
        }

        public INameParser Parser
        {
            get { return parser ?? (parser = new SimpleParser()); }
            set { parser = value; }
        }

        public string Title
        {
            get { return title ?? (title = string.Empty); }
            set { title = value; }
        }

        string IName.Prefix
        {
            get { return Title; }
            set { Title = value; }
        }

        public string Given
        {
            get { return forename ?? (forename = string.Empty); }
            set { forename = value; }
        }

        string IName.Core {
            get { return Given; }
            set { Given = value; }
        }

        public IName Family
        {
            get { return familyName; }
        }

        public string Letters
        {
            get { return letters ?? (letters = string.Empty); }
            set { letters = value; }
        }

        public string Suffix {
            get { return Family.Value; }
            set { throw new ArgumentException("Cannot directly assign Suffix for a PersonName"); }
        }

        public bool ReverseOrder { get; set; }

        public string Envelope
        {
            get { return envelope ?? (envelope = string.Empty); }
            set { envelope = value; }
        }

        public string Salutation
        {
            get { return salutation ?? (salutation = string.Empty); }
            set { salutation = value; }
        }

        public string Value
        {
            get { return DisplayValue(string.Empty); }
            set { Parse(value); }
        }

        public string DisplayValue(string format = null)
        {
            return Parser.Assemble(this, format);
        }

        public void Parse(string value, string format = null)
        {
            Parser.Parse(value, this, format);
        }
    }
}