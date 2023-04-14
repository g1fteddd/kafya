namespace LabSqlParser;
interface INodeVisitor {
	void VisitSelect(Select select);
	void VisitNumber(Number number);
	void VisitParenthesis(Parenthesis parenthesis);
	void VisitBinaryOperation(BinaryOperation binaryOperation);
}
