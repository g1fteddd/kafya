using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace LabSqlParser;
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
				throw new InvalidOperationException();
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
			throw new InvalidOperationException();
		}
	}
}
