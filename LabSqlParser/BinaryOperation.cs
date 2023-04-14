namespace LabSqlParser;
sealed record BinaryOperation(
	IExpression Left,
	BinaryOperator Operator,
	IExpression Right
) : IExpression {
	public string ToFormattedString() {
		switch (Operator) {
			case BinaryOperator.Multiplicative:
				return $"{Left.ToFormattedString()} % {Right.ToFormattedString()}";
			case BinaryOperator.Relational:
				return $"{Left.ToFormattedString()} > {Right.ToFormattedString()}";
			default:
				return "";
		}
	}
	public void AcceptVisitor(INodeVisitor visitor) {
		visitor.VisitBinaryOperation(this);
	}
}
