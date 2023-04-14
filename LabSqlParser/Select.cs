namespace LabSqlParser;
sealed record Select(
	IExpression SelectExpression,
	bool Distinct,
	IExpression? Having
) : IExpression {
	public string ToFormattedString() {
		var selectExpression = SelectExpression.ToFormattedString();
		var distinct = Distinct == false ? "" : " DISTINCT";
		var having = Having == null ? "" : $" HAVING {Having.ToFormattedString()}";
		return $"SELECT{distinct} {selectExpression}{having}";
	}
	public void AcceptVisitor(INodeVisitor visitor) {
		visitor.VisitSelect(this);
	}
}
