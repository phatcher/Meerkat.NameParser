namespace Meerkat.Tools
{
	public interface ILanguageLex : ILex
	{
		ISymbolTable SymbolTable { get; set; }

		bool SkipSpace { get; set; }
	}
}