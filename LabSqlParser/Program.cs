using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace LabSqlParser;
enum TokenType {
	Spaces,
	Identifier,
	Punctuator,
	Number,
}
sealed record Token(TokenType Type, string Lexeme);
public class SomeSymbolSkipped : Exception {
	public SomeSymbolSkipped() : base("Пропущен символ") { }
}
static class Lexer {
	public static IEnumerable<Match> GetMatches(Regex rx, string input) {
		var match = rx.Match(input);
		while (match.Success) {
			yield return match;
			match = match.NextMatch();
		}
	}
	public static IEnumerable<Token> GetTokens(string input) {
		var lexemeRx = new Regex("""
			(?<spaces>[ \t\n\r]+)|
			(?<identifier>[a-zA-Z_][a-zA-Z_0-9]*)|
			(?<number>[0-9]+)|
			(?<punc>[()%>])
			""", RegexOptions.IgnorePatternWhitespace);
		var expectedPos = 0;
		foreach (var token in GetMatches(lexemeRx, input)) {
			if (expectedPos != token.Index) {
				throw new SomeSymbolSkipped();
			}
			if (token.Groups["spaces"].Success) {
				yield return new Token(TokenType.Spaces, token.Value);
			}
			if (token.Groups["identifier"].Success) {
				yield return new Token(TokenType.Identifier, token.Value);
			}
			if (token.Groups["punc"].Success) {
				yield return new Token(TokenType.Punctuator, token.Value);
			}
			if (token.Groups["number"].Success) {
				yield return new Token(TokenType.Number, token.Value);
			}
			expectedPos += token.Length;
		}
		if (expectedPos != input.Length) {
			throw new SomeSymbolSkipped();
		}
	}
}
static class Program {
	static void Main() {
		var source = "SELECT ( SELECT DISTINCT 1 HAVING 2 ) % ( 3 ) % ( SELECT 4 ) > ( SELECT DISTINCT 5 ) > 6 HAVING 7";
		var tokens = Lexer.GetTokens(source);
		foreach (var token in tokens) {
			Console.WriteLine($" {token}");
		}
		Console.WriteLine();
	}
}
