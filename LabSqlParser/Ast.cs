namespace LabSqlParser;
interface INode { }
interface IExpression : INode { }
interface ISelectOrExpression { }
record Select(IExpression SelectExpression, IExpression? Distinct, IExpression? Having) : IExpression { }
record Number(string Lexeme) : IExpression { }
record Parenthesis(ISelectOrExpression Child) : IExpression { }
record Binary(IExpression Left, BinaryOperator Operator, IExpression Right) : IExpression { }
enum BinaryOperator { Relational, Multiplicative }
