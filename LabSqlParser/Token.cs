namespace LabSqlParser;
sealed record Token(
	TokenType Type,
	string Lexeme
);
