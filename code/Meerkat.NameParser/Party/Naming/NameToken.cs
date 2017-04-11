namespace Meerkat.Party.Naming
{
    /// <summary>
    /// Define the elements of a person/company name so we can identify the lexemes
    /// </summary>
    public enum NameToken
    {
        Title = 2000,
        Given = 2001,
        Prefix = 2002,
        Family = 2003,
        Suffix = 2004,
        Academic = 2005,
        Civil = 2006,
        Military = 2007,
        Professional = 2008,
        Multiple = 2009
    }
}