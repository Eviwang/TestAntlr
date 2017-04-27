using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using TestAntlr;

namespace TestAntlr2
{
    //解释型
    class Program
    {
        void Main(string[] args)
        {
            var code = "3 - (4)";
            var antlrInputStream = new AntlrInputStream(code);
            var lexer = new Combined1Lexer(antlrInputStream);
            var cts = new CommonTokenStream(lexer);
            var parser = new Combined1Parser(cts);



            var visitor = new MyVisitor();
            Console.WriteLine(visitor.Visit(parser.expression()));

            Console.ReadKey();
        }
    }

    public class MyVisitor : Combined1BaseVisitor<int>
    {
        public override int VisitBinaryExpression([NotNull] Combined1Parser.BinaryExpressionContext context)
        {
            var v1 = Visit(context.expression(0));
            var v2 = Visit(context.expression(1));
            var op = context.children[1].GetText();

            switch (op)
            {
                case "+":
                    return v1 + v2;
                case "-":
                    return v1 - v2;
            }
            throw new InvalidOperationException();
        }

        public override int VisitPara([NotNull] Combined1Parser.ParaContext context)
        {
            return Visit(context.expression());
        }

        public override int VisitIntExpression([NotNull] Combined1Parser.IntExpressionContext context)
        {
            return int.Parse(context.GetText());
        }
    }
}
