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

            Stopwatch time = new Stopwatch();
            time.Start();

            {
                Lexer lexer = new Lexer();
                Parser parser = new Parser();

                Console.WriteLine("\nLexer Result\n------------------------------------------------------------------------------------");
                lexer.OpenInputFiles("");
                lexer.processingLine();
                lexer.resultSplitter(lexemes, splittedCode);

                Console.WriteLine("\nParser Result\n------------------------------------------------------------------------------------");
                parser.parserStart(lexemes, splittedCode);
            }

            time.Stop();
            Console.Write("\nВремя выполнения программы: {0}", time.Elapsed.TotalSeconds); 
            Console.Write(" секунд");

        }
    }
}