namespace LabSqlParser;
sealed record Token(TokenType Type, string Lexeme);
enum TokenType {
	Spaces,
	Identifier,
	Punctuator,
	Number,
}
