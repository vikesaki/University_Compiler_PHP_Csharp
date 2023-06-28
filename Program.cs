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
            List<string> translated = new List<string>();

            Stopwatch time = new Stopwatch();
            time.Start();

            {
                Lexer lexer = new Lexer();
                Parser parser = new Parser();
                SemanticAnalyst semanticAnalyst = new SemanticAnalyst();
                Translator translator = new Translator();
                

                Console.WriteLine("\n------------------------------------------------------------------------------------\nLexer Result (PHP)\n------------------------------------------------------------------------------------");
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

                Console.WriteLine("\n\n------------------------------------------------------------------------------------\nTranslator Result (Python)\n------------------------------------------------------------------------------------");
                translator.TranslatorStart(grammar, translated);
                translator.PrintTransator(translated);
            }

            time.Stop();
            Console.WriteLine("\n\n------------------------------------------------------------------------------------");
            Console.Write("Program runtime: {0}", time.Elapsed.TotalSeconds); 
            Console.Write(" seconds");
            Console.WriteLine("\n------------------------------------------------------------------------------------");

        }
    }
}