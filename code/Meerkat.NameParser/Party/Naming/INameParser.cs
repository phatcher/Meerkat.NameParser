namespace Meerkat.Party.Naming
{
    /// <summary>
    /// Strategy pattern for parsing a name
    /// </summary>
    public interface INameParser
    {
        /// <summary>
        /// Reassembles a name from its components with the eventual structure being based on the format specification
        /// </summary>
        /// <param name="name">The name to build the value from</param>
        /// <param name="format">Format for the value, e.g. PNS implies the supplied format is prefix, name, suffix</param>
        /// <returns></returns>
        string Assemble(IName name, string format = null);

        /// <summary>
        /// Parses a value into an IName structure with the value being treated according to the format specification
        /// </summary>
        /// <param name="value">The value to be parsed</param>
        /// <param name="name">The name to put the parsed value into</param>
        /// <param name="format">Format for the value, e.g. PNS implies the supplied format is prefix, name, suffix</param>
        void Parse(string value, IName name, string format = null);
    }
}