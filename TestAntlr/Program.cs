﻿using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using System.Reflection.Emit;

namespace TestAntlr
{
    //编译成方法
    class Program
    {
        static void Main(string[] args)
        {
            var code = "3 - (4)";
            var antlrInputStream = new AntlrInputStream(code);
            var lexer = new Combined1Lexer(antlrInputStream);
            var cts = new CommonTokenStream(lexer);
            var parser = new Combined1Parser(cts);


            var dynamicMethod = new DynamicMethod("test", typeof(int), Type.EmptyTypes);
            var il = dynamicMethod.GetILGenerator();

            var visitor = new MyVisitor(il);
            visitor.Visit(parser.expression());
            il.Emit(OpCodes.Ret);

            var method = (Func<int>)dynamicMethod.CreateDelegate(typeof(Func<int>));
            Console.WriteLine(method());

            Console.ReadKey();
        }
    }

    public class MyVisitor : Combined1BaseVisitor<int>
    {
        private readonly ILGenerator _il;

        public MyVisitor(ILGenerator il)
        {
            _il = il;
        }

        public override int VisitBinaryExpression([NotNull] Combined1Parser.BinaryExpressionContext context)
        {
            var v1 = Visit(context.expression(0));
            var v2 = Visit(context.expression(1));
            var op = context.children[1].GetText();

            switch (op)
            {
                case "+":
                    _il.Emit(OpCodes.Add);
                    return 0;
                case "-":
                    _il.Emit(OpCodes.Sub);
                    return 0;
            }
            throw new InvalidOperationException();
        }

        public override int VisitPara([NotNull] Combined1Parser.ParaContext context)
        {
            return Visit(context.expression());
        }

        public override int VisitIntExpression([NotNull] Combined1Parser.IntExpressionContext context)
        {
            _il.Emit(OpCodes.Ldc_I4, int.Parse(context.GetText()));
            return 0;
        }
    }
}
