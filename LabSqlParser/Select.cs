sealed record Select(
	IExpression SelectExpression,
	bool? Distinct,
	Having? Having
) : IExpression {
	public string ToFormattedString() {
		var selectExpression = SelectExpression.ToFormattedString();
		var distinct = Distinct == false ? "" : " DISTINCT";
		var having = Having == null ? "" : Having.ToFormattedString();
		return $"SELECT{distinct} {selectExpression}{having}";
	}
}
