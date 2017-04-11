namespace Meerkat.Party.Naming
{
    /// <summary>
    /// Structure of a name
    /// </summary>
    public interface IName
    {
        /// <summary>
        /// Prefix for the name, might be a title or 'noise' word we do not want in the collation order
        /// </summary>
        string Prefix { get; set; }

        /// <summary>
        /// The main body of the name, e.g. the item we want to index on
        /// </summary>
        string Core { get; set; }

        /// <summary>
        /// Suffix for the name which is not part of the general name, e.g. a qualification, company classification or generational marker (II, Jr, Snr etc)
        /// </summary>
        string Suffix { get; set; }

        /// <summary>
        /// Retreives and assigns the whole name
        /// </summary>
        /// <remarks>Implementations should use the <see cref="INameParser"/> strategy to parse/construct this</remarks>
        string Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        string DisplayValue(string format = null);

        /// <summary>
        /// Allows a name to parse a raw value into constituent parts.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        void Parse(string value, string format = null);
    }
}