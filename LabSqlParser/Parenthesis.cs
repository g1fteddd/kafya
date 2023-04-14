namespace LabSqlParser;
sealed record Parenthesis(
	IExpression Child
) : IExpression {
	public string ToFormattedString() {
		return $"({Child.ToFormattedString()})";
	}
	public void AcceptVisitor(INodeVisitor visitor) {
		visitor.VisitParenthesis(this);
	}
}
