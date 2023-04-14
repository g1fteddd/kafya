using System.IO;
namespace LabSqlParser;
sealed class DebugPrintingVisitor : INodeVisitor {
	readonly TextWriter output;
	int indent;
	public DebugPrintingVisitor(TextWriter output, int indent = 0) {
		this.output = output;
		this.indent = indent;
	}
	public void WriteLine(INode node) {
		WriteNode(node);
		Write("\n");
	}
	void WriteNode(INode node) {
		node.AcceptVisitor(this);
	}
	void Write(string s) {
		output.Write(s);
	}
	void WriteIndent() {
		Write(new string(' ', indent * 4));
	}
	void INodeVisitor.VisitSelect(Select select) {
		Write($"new {nameof(Select)}(\n");
		{
			indent += 1;
			WriteIndent();
			WriteNode(select.SelectExpression);
			Write(",\n");
			WriteIndent();
			if (select.Distinct) {
				Write($"{nameof(select.Distinct)}: true");
			}
			else {
				Write($"{nameof(select.Distinct)}: false");
			}
			Write(",\n");
			WriteIndent();
			if (select.Having is not null) {
				WriteNode(select.Having);
			}
			else {
				Write($"{nameof(select.Having)}: null");
			}
			Write("\n");
			indent -= 1;
		}
		WriteIndent();
		Write(")");
	}
	void INodeVisitor.VisitNumber(Number number) {
		Write($"new {nameof(Number)}(\"{number.Lexeme}\")");
	}
	void INodeVisitor.VisitBinaryOperation(BinaryOperation binaryOperation) {
		Write($"new {nameof(BinaryOperation)}(\n");
		{
			indent += 1;
			WriteIndent();
			WriteNode(binaryOperation.Left);
			Write(",\n");
			WriteIndent();
			Write($"{nameof(BinaryOperator)}.{binaryOperation.Operator}");
			Write(",\n");
			WriteIndent();
			WriteNode(binaryOperation.Right);
			Write("\n");
			indent -= 1;
		}
		WriteIndent();
		Write(")");
	}
	void INodeVisitor.VisitParenthesis(Parenthesis parenthesis) {
		Write($"new {nameof(Parenthesis)}(\n");
		{
			indent += 1;
			WriteIndent();
			WriteNode(parenthesis.Child);
			Write("\n");
			indent -= 1;
		}
		WriteIndent();
		Write(")");
	}
}
