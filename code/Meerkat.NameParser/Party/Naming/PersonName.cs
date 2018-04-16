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

        /// <summary>
        /// Creates a new instance of the <see cref="PersonName"/> class.
        /// </summary>
        public PersonName()
        {
            familyName = new Name();
        }

        /// <summary>
        /// Get or set the name parser
        /// </summary>
        public INameParser Parser
        {
            get => parser ?? (parser = new SimpleParser());
            set => parser = value;
        }

        /// <copydoc cref="IPersonName.Title" />
        public string Title
        {
            get => title ?? (title = string.Empty);
            set => title = value;
        }

        string IName.Prefix
        {
            get => Title;
            set => Title = value;
        }

        /// <copydoc cref="IPersonName.Given" />
        public string Given
        {
            get => forename ?? (forename = string.Empty);
            set => forename = value;
        }

        string IName.Core
        {
            get => Given;
            set => Given = value;
        }

        /// <copydoc cref="IPersonName.Family" />

        public IName Family => familyName;

        string IPersonName.Letters
        {
            get => letters ?? (letters = string.Empty);
            set => letters = value;
        }

        /// <copydoc cref="IName.Suffix" />

        public string Suffix
        {
            get => Family.Value;
            set => throw new ArgumentException("Cannot directly assign Suffix for a PersonName");
        }

        /// <copydoc cref="IPersonName.ReverseOrder" />

        public bool ReverseOrder { get; set; }

        /// <copydoc cref="IPersonName.Envelope" />

        public string Envelope
        {
            get => envelope ?? (envelope = string.Empty);
            set => envelope = value;
        }

        /// <copydoc cref="IPersonName.Salutation" />

        public string Salutation
        {
            get => salutation ?? (salutation = string.Empty);
            set => salutation = value;
        }

        /// <copydoc cref="IPersonName.Value" />

        public string Value
        {
            get => DisplayValue(string.Empty);
            set => Parse(value);
        }

        /// <summary>
        /// Display name according to the format.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string DisplayValue(string format = null)
        {
            return Parser.Assemble(this, format);
        }

        /// <summary>
        /// Parse the raw name according to the specified format
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        public void Parse(string value, string format = null)
        {
            Parser.Parse(value, this, format);
        }
    }
}