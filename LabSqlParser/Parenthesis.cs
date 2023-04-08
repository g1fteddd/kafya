sealed record Parenthesis(
	IExpression Child
) : IExpression {
	public string ToFormattedString() {
		return $"({Child.ToFormattedString()})";
	}
}
