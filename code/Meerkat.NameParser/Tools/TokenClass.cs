namespace Meerkat.Tools
{
    /// <summary>
    /// Single character tokens - these all equate to the LexChar equivalents
    /// </summary>
    public enum TokenClass
    {
        EOF = -1,
        Tab = 7,
        CR = 10,
        NL = 13,
        Space = 32,
        Exclamation = 33,
        Quote = 34,
        Hash = 35,
        Dollar = 36,
        Percent = 37,
        Ampersand = 38,
        SingleQuote = 39,
        LB = 40,
        RB = 41,
        Multiply = 42,
        Plus = 43,
        Comma = 44,
        Minus = 45,
        Point = 46,
        Divide = 47,
        Colon = 58,
        SemiColon = 59,
        LT = 60,
        EQ = 61,
        GT = 62,
        QuestionMark = 63,
        At = 64,
        SLB = 92,
        Backslash = 92,
        SRB = 93,
        Exponent = 94,
        Underscore = 95,
        Apostrophe = 96,
        CLB = 123,
        CRB = 125,

        // Other classes here
        Number = 1000,
        Value,
        String,
        Whitespace,
        GTE,
        LTE
    }
}