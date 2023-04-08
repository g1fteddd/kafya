sealed record Number(
	string Lexeme
) : IExpression {
	public string ToFormattedString() {
		return Lexeme;
	}
}
