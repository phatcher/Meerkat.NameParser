using System;

namespace Meerkat.Tools
{
    [Serializable]
    public class Symbol : ISymbol
    {
        private object value;

        public TokenClass Token { get; set; }

        public object Value
        {
            get { return value; }
            set
            {
                if (value is char)
                {
                    value = value.ToString();
                }

                if (value is string)
                {
                    // Intern the value as comparison will happen frequently
                    this.value = string.Intern(Convert.ToString(value));
                }
                else
                {
                    this.value = value;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!ReferenceEquals(GetType(), obj.GetType()))
            {
                return false;
            }

            // Must work as we are the same type
            var other = (Symbol)obj;

            // Now compare the fields
            if (Token.Equals(other.Token) == false)
            {
                return false;
            }

            // Objects are equal
            return Equals(value, other.Value);
        }
    }
}