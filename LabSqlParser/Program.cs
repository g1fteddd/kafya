using System;
namespace LabSqlParser;
static class Program {
	static void Main() {
		var source = "SELECT ( SELECT DISTINCT 1 HAVING 2 ) % ( 3 ) % ( SELECT 4 ) > ( SELECT DISTINCT 5 ) > 6 HAVING 7";
		var tokens = Lexer.GetTokens(source);
		foreach (var token in tokens) {
			Console.WriteLine($" {token}");
		}
		Console.WriteLine();
		var tree = new Select(
			new BinaryOperation(
				new BinaryOperation(
					new BinaryOperation(
						new BinaryOperation(
							new Parenthesis(
								new Select(
									new Number("1"),
									Distinct: true,
									new Number("2")
								)
							),
							BinaryOperator.Multiplicative,
							new Parenthesis(
								new Number("3")
							)
						),
						BinaryOperator.Multiplicative,
						new Parenthesis(
							new Select(
								new Number("4"),
								Distinct: false,
								Having: null
							)
						)
					),
					BinaryOperator.Relational,
					new Parenthesis(
						new Select(
							new Number("5"),
							Distinct: true,
							Having: null
						)
					)
				),
				BinaryOperator.Relational,
				new Number("6")
			),
			Distinct: false,
			new Number("7")
		);
		Console.WriteLine(tree.ToFormattedString());
		var parsedTree = Parser.Parse(tokens);
		Console.WriteLine(parsedTree.ToFormattedString());
		new DebugPrintingVisitor(Console.Out, 2).WriteLine(parsedTree);
	}
}
