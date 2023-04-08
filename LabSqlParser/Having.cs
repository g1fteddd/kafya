sealed record Having(
	IExpression Expression
) : INode {
	public string ToFormattedString() {
		return $" HAVING {Expression.ToFormattedString()}";
	}
}
