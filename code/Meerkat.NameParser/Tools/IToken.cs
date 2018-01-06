namespace Meerkat.Tools
{
    public interface IToken
    {
        /// <summary>
        /// Class of the symbol, application dependent
        /// </summary>
        TokenClass Token { get; set; }

        /// <summary>
        /// Value of token, e.g. the keyword/variable name/constant
        /// </summary>
        object Value { get; set; }
    }
}