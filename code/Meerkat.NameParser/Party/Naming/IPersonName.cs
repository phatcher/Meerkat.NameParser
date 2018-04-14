namespace Meerkat.Party.Naming
{
    /// <summary>
    /// Specifies a person's name, NB can't inherit IName as we want 'nice' property names
    /// </summary>
    public interface IPersonName
    {
        /// <summary>
        /// Get or set the title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Get or set the given name.
        /// </summary>
        /// <remarks>We don't use forename as this implies an ordering</remarks>
        string Given { get; set; }

        /// <summary>
        /// Get or set the family name.
        /// </summary>
        /// <remarks>We don't use surname as this implies an ordering</remarks>
        IName Family { get; }

        /// <summary>
        /// Get or set the post-nominals
        /// </summary>
        string Letters { get; set; }

        /// <summary>
        /// Get or set the envelope name
        /// </summary>
        string Envelope { get; set; }

        /// <summary>
        /// Get or set the salutation
        /// </summary>
        string Salutation { get; set; }

        /// <summary>
        /// Get or set the order of given/family name
        /// </summary>
        bool ReverseOrder { get; set; }

        /// <summary>
        /// Get or set the raw name
        /// </summary>
        string Value { get; set; }
    }
}
