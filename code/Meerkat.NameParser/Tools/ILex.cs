using System.IO;

namespace Meerkat.Tools
{
	public interface ILex
	{
		TextReader Source { get; set; }

		ISymbol GetToken();
	}
}