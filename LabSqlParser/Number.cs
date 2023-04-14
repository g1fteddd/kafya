namespace LabSqlParser;
sealed record Number(
	string Lexeme
) : IExpression {
	public string ToFormattedString() {
		return Lexeme;
	}
	public void AcceptVisitor(INodeVisitor visitor) {
		visitor.VisitNumber(this);
	}
}
