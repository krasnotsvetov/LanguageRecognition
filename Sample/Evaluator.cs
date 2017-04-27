using LanguageRecognition.Runtime;
using Sample.Generated.Generated;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    public class Evaluator : IDisposable
    {

        /// <summary>
        /// TODO. 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private StreamReader StreamReaderFromString(string text)
        {
            var t = text;
            ///Stream writter is not dispose. Now, streamWritter only close inner stream in dispose, so it's not critical. But 
            /// it is potential bug
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.WriteLine(t);
            sw.Flush();
            ms.Position = 0;
            return new StreamReader(ms);
        }


        private Dictionary<string, int> variables;
        private StreamReader streamReader;
        public Evaluator(string text)
        {
            streamReader = StreamReaderFromString(text);
        }

        public Evaluator(Stream stream)
        {
            streamReader = new StreamReader(stream);
        }

        public void Evaluate()
        {
            if (variables != null)
            {
                return;
            }
            variables = new Dictionary<string, int>();
            var parser = new ExpressionParser(new LexicalAnalyzer(streamReader, new ExpressionVocabulary()));
            new ProgramVisitor(variables).VisitProgram(parser.Parse());
        }

        public void Dispose()
        {
            streamReader.Dispose();
        }

        public IReadOnlyDictionary<string, int> Variables
        {
            get
            {
                return (IReadOnlyDictionary<string, int>)variables;
            }
        }

        private class ProgramVisitor 
        {
            Dictionary<string, int> variables;

            static Dictionary<string, Func<int, int, int>> functions = new Dictionary<string, Func<int, int, int>>()
            {
                {"+", (a, b) => a + b },
                {"-", (a, b) => a - b },
                {"*", (a, b) => a * b },
                {"^", (a, b) => (int)Math.Pow(a, b) },
            };

            public ProgramVisitor(Dictionary<string, int> variables)
            {
                this.variables = variables;
            }

            public int VisitProgram(programNode program)
            {
                List<expressionNode> expressions = new List<expressionNode>();
                var cur = program.expressionStatement;
                while (cur != null)
                {
                    expressions.Add(cur.expression);
                    cur = cur.expressionStatement;
                }
                foreach (var e in expressions)
                {
                    VisitExpression(e);
                }

                return 0;
            }

            public int VisitExpression(expressionNode e)
            {
                if (e.sumExpression != null)
                {
                    return VisitSumExpression(e.sumExpression);
                }
                string variableName = e.mutable.variable.Id.Text;
                variables[variableName] = VisitExpression(e.expression);
                return variables[variableName];
            }

            public int VisitSumExpression(sumExpressionNode sumExpression)
            {
                int value = VisitTerm(sumExpression.term);
                if (sumExpression.sumExpression == null)
                {
                    return value;
                }
                string function = sumExpression.sum.SubOp == null ? "+" : "-";
                return functions[function](VisitSumExpression(sumExpression.sumExpression), value);
            }

            public int VisitTerm(termNode term)
            {
                if (term.term != null)
                {
                    return functions[term.MulOp.Text](VisitTerm(term.term), VisitPowExpression(term.powExpression));
                }
                else
                {
                    return VisitPowExpression(term.powExpression);
                }
            }

            public int VisitPowExpression(powExpressionNode powExpression)
            {
                if (powExpression.PowOp != null)
                {
                    return functions[powExpression.PowOp.Text](VisitUnaryExpression(powExpression.unaryExpression), VisitPowExpression(powExpression.powExpression));
                }
                return VisitUnaryExpression(powExpression.unaryExpression);
            }

            public int VisitUnaryExpression(unaryExpressionNode unaryExpression)
            {
                if (unaryExpression.unaryExpression != null)
                {
                    return functions[unaryExpression.SubOp.Text](0, VisitUnaryExpression(unaryExpression.unaryExpression));
                }
                if (unaryExpression.variable != null)
                {
                    return VisitVariable(unaryExpression.variable);
                }

                if (unaryExpression.sumExpression != null)
                {
                    return VisitSumExpression(unaryExpression.sumExpression);
                }
                return VisitConstant(unaryExpression.constant);
            }

            public int VisitConstant(constantNode constant)
            {
                return int.Parse(constant.Number.Text);
            }

            public int VisitVariable(variableNode variable)
            {
                string variableName = variable.Id.Text;
                if (!variables.ContainsKey(variableName))
                {
                    Console.WriteLine($"Warning :: variable {variableName} is not defined. Initialized with zero value");
                    variables[variableName] = 0;
                }
                return variables[variableName];
            }
        }

    }

}
