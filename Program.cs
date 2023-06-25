using System.Diagnostics;

namespace PHPtoC_
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            List<string> lexemes = new List<string>();
            List<string> splittedCode = new List<string>();
            Parser.Grammar grammar = new Parser.Grammar();

            Stopwatch time = new Stopwatch();
            time.Start();

            {
                Lexer lexer = new Lexer();
                Parser parser = new Parser();
                SemanticAnalyst semanticAnalyst = new SemanticAnalyst();
                

                Console.WriteLine("\n------------------------------------------------------------------------------------\nLexer Result\n------------------------------------------------------------------------------------");
                lexer.OpenInputFiles("");
                lexer.processingLine();
                lexer.resultSplitter(lexemes, splittedCode);

                Console.WriteLine("\n------------------------------------------------------------------------------------\nParser Result\n------------------------------------------------------------------------------------");
                parser.parserStart(lexemes, splittedCode, grammar);
                grammar.ShowGrammar();
                Console.WriteLine("\nGrammar Length : " + grammar.GetGrammarLength());
                Console.WriteLine("\nGrammar Amount : " + grammar.GetGrammarAmount());

                Console.WriteLine("\n------------------------------------------------------------------------------------\nSemantic Analyst Result\n------------------------------------------------------------------------------------");
                semanticAnalyst.Start(grammar);
            }

            time.Stop();
            Console.WriteLine("\n------------------------------------------------------------------------------------");
            Console.Write("Program runtime: {0}", time.Elapsed.TotalSeconds); 
            Console.Write(" seconds");
            Console.WriteLine("\n------------------------------------------------------------------------------------");

        }
    }
}