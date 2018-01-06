using System;
using System.IO;

namespace Meerkat.Tools
{
    /// <summary>
    /// Implements a generic language lexical analyser that copes with most computer languages
    /// Loosly based on Uni Karlsruhe research from ~1984/86, idea is to touch each character as few times as possible.
    /// </summary>
    public class LanguageLex : ILanguageLex
    {
        private bool pushed;
        private int pushChar;

        public LanguageLex()
        {
            SkipSpace = true;
            AddMissing = false;

            SymbolTable = new SymbolTable();
            Initialized = false;
        }

        /// <summary>
        /// Source of lexemes
        /// </summary>
        public TextReader Source { get; set; }

        /// <summary>
        /// Whether we return white space as a lexeme
        /// </summary>
        public bool SkipSpace { get; set; }

        /// <summary>
        /// Whether we add missing symbols as we find them
        /// </summary>
        public bool AddMissing { get; set; }

        public ISymbolTable SymbolTable { get; set; }

        public int[] BreakChars { get; set; }

        protected bool Initialized { get; set; }

        private int GetChar()
        {
            // TODO: Explore the use of Read with a buffer structure
            if (!pushed)
            {
                return Source.Read();
            }
            pushed = false;
            return pushChar;
        }

        private int Peek()
        {
            return pushed ? pushChar : Source.Peek();
        }

        private void PushChar(int value)
        {
            pushChar = value;
            pushed = true;
        }

        /// <summary>
        /// Returns the next lexeme
        /// </summary>
        /// <returns></returns>
        public ISymbol GetToken()
        {
            var sb = new System.Text.StringBuilder(20);

            ISymbol symbol = new Symbol();
            var nextChar = GetChar();

            while (true)
            {
                // Process based on current character
                if (nextChar.IsLexChar(LexChar.EOF))
                {
                    // Reached the end
                    symbol.Token = TokenClass.EOF;
                    break;
                }

                if (nextChar >= 1 && nextChar <= 31)
                {
                    // Skip early ASCII
                    while (nextChar >= 1 & nextChar <= 31)
                    {
                        nextChar = GetChar();
                    }

                    // Restart loop to see what we have now
                    continue;
                }

                if (nextChar.IsWhiteSpace())
                {
                    // Whitespace
                    if (SkipSpace)
                    {
                        while (nextChar.IsWhiteSpace())
                        {
                            nextChar = GetChar();
                        }

                        // Restart loop to see what we have now
                        continue;
                    }

                    symbol.Token = TokenClass.Whitespace;
                    symbol.Value = (char) nextChar;
                    break;
                }

                if (nextChar.IsDigit())
                {
                    var pointFound = false;
                    var expFound = false;

                    // Try to handle numbers
                    while (true)
                    {
                        if (nextChar.IsDigit())
                        {
                            // Building the number
                            sb.Append((char)nextChar);
                        }
                        else if (nextChar.IsLexChar(LexChar.E, LexChar.Elower))
                        {
                            // Exponent specification
                            if (expFound)
                            {
                                return symbol;
                            }

                            expFound = true;
                            sb.Append((char)nextChar);
                            nextChar = Peek();
                            // Check for a sign on the exponent
                            if (nextChar.IsLexChar(LexChar.Plus, LexChar.Minus))
                            {
                                // Consume the character
                                sb.Append((char)GetChar());
                            }
                        }
                        else if (nextChar.IsLexChar(LexChar.Point))
                        {
                            // Only one point allowed
                            if (pointFound)
                            {
                                return symbol;
                            }

                            pointFound = true;
                            sb.Append((char) nextChar);
                        }
                        else
                        {
                            // Non-valid continuations cause us to exit the loop
                            break;
                        }
                        nextChar = GetChar();
                    }

                    // Push the one back that failed - start of next lexeme
                    PushChar(nextChar);
                    // And set the symbol
                    symbol.Token = TokenClass.Number;
                    // TODO: Do we need more conversions
                    symbol.Value = pointFound ? Convert.ToDouble(sb.ToString()) : Convert.ToInt64(sb.ToString());
                    break;
                }

                if (nextChar.IsLexChar(LexChar.Quote))
                {
                    // Handle quoted strings - NB We don't include the quotes in the string definition

                    // Skip the initial quote
                    nextChar = GetChar();

                    // Continue while we don't have a quote, or we have an escaped quote
                    while (!nextChar.IsLexChar(LexChar.Quote) || Peek().IsLexChar(LexChar.Quote))
                    {
                        // We should not have EOF yet
                        if (nextChar.IsLexChar(LexChar.EOF))
                        {
                            throw new ArgumentException("Unexpected EOF, unterminated string");
                        }

                        // Build the string
                        sb.Append((char)nextChar);
                        // Get the next character
                        nextChar = GetChar();
                    }
                    // And set the symbol
                    symbol.Token = TokenClass.String;
                    symbol.Value = sb.ToString();
                    break;
                }

                if (nextChar.IsLexChar(LexChar.LT))
                {
                    if (Peek().IsLexChar(LexChar.EQ))
                    {
                        symbol.Token = TokenClass.LTE;
                        symbol.Value = "<=";
                        GetChar();
                    }
                    else
                    {
                        symbol.Token = TokenClass.LT;
                        symbol.Value = "<";
                    }
                    break;
                }

                if (nextChar.IsLexChar(LexChar.GT))
                {
                    if (Peek().IsLexChar(LexChar.EQ))
                    {
                        symbol.Token = TokenClass.GTE;
                        symbol.Value = ">=";
                        GetChar();
                    }
                    else
                    {
                        symbol.Token = TokenClass.GT;
                        symbol.Value = ">";
                    }
                    break;
                }

                if (nextChar.IsAlpha() || nextChar.IsLexChar(LexChar.Underscore))
                {
                    // Standard syntax for identifiers
                    while (true)
                    {
                        if (nextChar.IsAlphaNumeric() || nextChar.IsLexChar(LexChar.Underscore))
                        {
                            // Start building the identifier
                            sb.Append((char)nextChar);
                            nextChar = GetChar();
                        }
                        else
                        { 
                            // Non-valid continuations cause us to exit the loop
                            break;
                        }
                    }

                    // Push the one back that failed - start of next lexeme
                    PushChar(nextChar);
                    // And set the symbol
                    symbol.Token = TokenClass.Value;
                    symbol.Value = sb.ToString();
                    break;
                }

                // Everything else is treated as a single character lexeme
                symbol.Token = (TokenClass) nextChar;
                symbol.Value = (char)nextChar;
                break;
            }

            // Check for membership of the symbol table 
            if (symbol.Token == TokenClass.Value)
            {
                if (SymbolTable.Contains(symbol) == false)
                {
                    if (AddMissing)
                    {
                        SymbolTable.Add(symbol);
                    }
                }
                else
                {
                    symbol = SymbolTable[symbol];
                }
            }

            // Tell them the class of the symbol
            return symbol;
        }

        protected virtual void Initialize()
        {
        }
    }
}