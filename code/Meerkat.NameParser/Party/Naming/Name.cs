namespace Meerkat.Party.Naming
{
    /// <summary>
    /// Basic implementation of a name
    /// </summary>
    public class Name : IName
    {
        private string prefix;
        private string core;
        private string suffix;

        private INameParser parser;

        /// <summary>
        /// Simple parser so we always have a strategy, just put the value in the Name property
        /// </summary>
        private class SimpleParser : INameParser
        {
            public string Assemble(IName name)
            {
                return Assemble(name, string.Empty);
            }

            public string Assemble(IName name, string format)
            {
                return name.Prefix + name.Core + name.Suffix;
            }

            public void Parse(string value, IName name)
            {
                Parse(value, name, string.Empty);
            }

            public void Parse(string value, IName name, string format)
            {
                // Just put it in the name
                name.Core = value;
            }
        }

        /// <summary>
        /// Get or set the name parser.
        /// </summary>
        public virtual INameParser Parser
        {
            get => parser ?? (parser = new SimpleParser());
            set => parser = value;
        }

        /// <copydoc cref="IName.Prefix" />

        public string Prefix
        {
            get => prefix ?? (prefix = string.Empty);
            set => prefix = value;
        }

        /// <copydoc cref="IName.Core" />

        public string Core
        {
            get => core ?? (core = string.Empty);
            set => core = value;
        }

        /// <copydoc cref="IName.Suffix" />

        public string Suffix
        {
            get => suffix ?? (suffix = string.Empty);
            set => suffix = value;
        }

        /// <copydoc cref="IName.Value" />

        public string Value
        {
            get => DisplayValue();
            set => Parse(value);
        }

        /// <copydoc cref="IName.DisplayValue" />

        public string DisplayValue(string format = null)
        { 
            //TODO: Handle formats
            var fred = (Prefix.Trim() + " " + Core.Trim() + " " + Suffix.Trim()).Trim();
            while (fred.IndexOf("  ") > -1)
            {
                Core = fred.Replace("  ", " ");
            }
            return fred;
        }

        /// <copydoc cref="IName.Parse" />

        public void Parse(string value, string format = null)
        {
            Parser.Parse(value, this, format);
        }
    }
}