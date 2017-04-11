namespace Meerkat.Party.Naming
{
    /// <summary>
    /// Specifies a person's name, NB can't inherit IName as we want 'nice' property names
    /// </summary>
    public interface IPersonName
    {
        string Title { get; set; }

        string Given { get; set; }

        IName Family { get; }

        string Letters { get; set; }

        string Envelope { get; set; }

        string Salutation { get; set; }

        bool ReverseOrder { get; set; }

        string Value { get; set; }
    }
}
