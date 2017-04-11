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

        public virtual INameParser Parser
        {
            get { return parser ?? (parser = new SimpleParser()); }
            set { parser = value; }
        }

        public string Prefix {
            get { return prefix ?? (prefix = string.Empty); }
            set { prefix = value; }
        }

        public string Core
        {
            get { return core ?? (core = string.Empty); }
            set { core = value; }
        }

        public string Suffix
        {
            get { return suffix ?? (suffix = string.Empty); }
            set { suffix = value; }
        }

        public string Value
        {
            get { return DisplayValue(); }
            set { Parse(value); }
        }

        public string DisplayValue(string format = null)
        { 
            //TODO: Handle formats
            var fred = (Prefix.Trim() + " " + Core.Trim() + " " + Suffix.Trim()).Trim();
            while (fred.IndexOf("  ") > -1) {
                Core = fred.Replace("  ", " ");
            }
            return fred;
        }

        public void Parse(string value, string format = null)
        {
            Parser.Parse(value, this, format);
        }
    }
}