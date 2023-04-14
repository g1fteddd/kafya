using System;
using System.Collections.Generic;
using System.Linq;
namespace LabSqlParser;
sealed class Parser {
	readonly IReadOnlyList<Token> tokens;
	int pos;
	public Parser(IReadOnlyList<Token> tokens) {
		this.tokens = tokens;
	}
	#region common
	Exception MakeError(string message) {
		throw new InvalidOperationException($"{message} в {pos}");
	}
	void ReadNextToken() {
		pos++;
	}
	Token CurrentToken => tokens[pos];
	void Expect(string s) {
		if (CurrentToken.Lexeme != s) {
			throw MakeError($"Ожидалось {s} Получено {CurrentToken.Lexeme}");
		}
		ReadNextToken();
	}
	bool SkipIf(string s) {
		if (CurrentToken.Lexeme == s) {
			ReadNextToken();
			return true;
		}
		return false;
	}
	void ExpectEof() {
		if (CurrentToken.Type != TokenType.EndOfFile) {
			throw MakeError($"Ожидался конец файла, получен {CurrentToken}");
		}
	}
	#endregion
	public static Select Parse(IEnumerable<Token> tokens) {
		var tokenList = tokens
						.Where(token => token.Type != TokenType.Spaces)
						.Append(new Token(TokenType.EndOfFile, ""))
						.ToList();
		var parser = new Parser(tokenList);
		var select = parser.ParseSelect();
		parser.ExpectEof();
		return select;
	}
	IExpression ParseExpression() {
		var left = ParseMultiplicative();
		while (SkipIf(">")) {
			var right = ParseMultiplicative();
			left = new BinaryOperation(left, BinaryOperator.Relational, right);
		}
		return left;
	}
	IExpression ParseMultiplicative() {
		var left = ParsePrimary();
		while (true) {
			if (SkipIf("%")) {
				var right = ParsePrimary();
				left = new BinaryOperation(left, BinaryOperator.Multiplicative, right);
				continue;
			}
			break;
		}
		return left;
	}
	IExpression ParsePrimary() {
		if (SkipIf("(")) {
			var primary = CurrentToken.Lexeme == "SELECT" ? ParseSelect() : ParseExpression();
			Expect(")");
			return new Parenthesis(primary);
		}
		if (CurrentToken.Type == TokenType.Number) {
			return ParseNumber();
		}
		throw MakeError("Ожидалось число или идентификатор или выражение в скобках.");
	}
	Select ParseSelect() {
		Expect("SELECT");
		var distinct = false;
		if (SkipIf("DISTINCT")) {
			distinct = true;
		}
		var column = ParseExpression();
		IExpression? having = null;
		if (SkipIf("HAVING")) {
			having = ParseExpression();
		}
		return new Select(column, distinct, having);
	}
	Number ParseNumber() {
		if (CurrentToken.Type != TokenType.Number) {
			throw MakeError($"Ожидался Number. Получен {CurrentToken.Type}. {CurrentToken.Lexeme}");
		}
		var number = new Number(CurrentToken.Lexeme);
		ReadNextToken();
		return number;
	}
}
