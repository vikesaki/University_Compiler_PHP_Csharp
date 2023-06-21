using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PHPtoC_
{
    class Parser
    {
        public class Node
        {
            public Node Next;
            public Node Previous;
            public String Pattern;
            public int Level; //the level on the trees

            public Node(String pattern, int level, Node next, Node previous)
            {
                Pattern = pattern;
                Level = level;
                Next = next;
                Previous = previous;
            }

        }

        public class Grammar
        {
            public Node CurrentGrammar;

            public void AddGrammar(String pattern, int level)
            {
                Node newNode = new Node(pattern, level, null, null);
                Node last = CurrentGrammar;
                if (CurrentGrammar == null)
                {
                    CurrentGrammar = newNode;
                    return;
                }

                while (last.Next != null)
                    last = last.Next;

                last.Next = newNode;
                newNode.Previous = last;
            }

            public void AddBeforeGrammar(String pattern, int level)
            {
                while (CurrentGrammar.Next.Next != null)
                    CurrentGrammar = CurrentGrammar.Next;
                Node nextNode = CurrentGrammar;
                Node previousNode = CurrentGrammar.Previous;

                nextNode.Level = level + 1 ;
                nextNode.Next.Level = level + 2;
                if (nextNode == null)
                {
                    Console.WriteLine("Next node cant be null!");
                    return;
                }
                Node newNode = new Node(pattern, level, null, null);
                newNode.Previous = previousNode;
                newNode.Next = nextNode;
                previousNode.Next = newNode;
                nextNode.Previous = newNode;

                if (newNode.Previous == null)
                    CurrentGrammar = newNode;

                while (CurrentGrammar.Previous != null)
                    CurrentGrammar = CurrentGrammar.Previous;

            }

            public void ShowGrammar()
            {
                if (CurrentGrammar == null)
                {
                    Console.WriteLine("Current grammar doesn't exist!");
                    return;
                }

                Console.WriteLine("Current Grammar");
                while (CurrentGrammar != null)
                {
                    for (int i = 0; i < CurrentGrammar.Level; i++)
                    {
                        Console.Write("     ");
                    }
                    Console.Write(CurrentGrammar.Pattern + "\n");
                    CurrentGrammar = CurrentGrammar.Next;
                }
            }
        }

        
        int listNumber;
        Dictionary<string, string> presentation = new Dictionary<string, string>
        {
            {"Program","PROGRAM"},
            {"List","LIST" },
            {"Statement","STATEMENT"},
            {"Modification","MODIFICATION"} ,
            {"Formula","FORMULA"},
            {"Function","FUNCTION"},
            {"Identifier","IDENTIFIER"},
            {"Parameters","PARAMETERS"},
            {"FormalParameter","FORMALPARAMETERS"},
            {"ActParameter","ACTUALPARAMETER"},
            {"Variable","VARIABLE"},
            {"Value", "VALUE"},
            {"Name","NAME"},
            {"Add","ADD"},
            {"Sub","SUB"},
            {"Set","SET"},
            {"Mul","MUL"},
            {"Div","DIV"},
            {"Rem","REM"},
            {"Deg","DEG"},
            {"Call","CALL"},
            {"Content","CONTENT"},
            {"openBracket","OPENBRACKET"},
            {"closeBracket","CLOSEBRACKET"},
            {"Default","DEFAULT"},
            {"Less","LESS"},
            {"Greater","GREATER"},
            {"Begin","BEGIN"},
            {"IfConstruction","IFCONSTRUCTION"},
            {"ElifConstruction", "ELIFCONSTRUCTION"},
            {"WhileConstruction","WHILECONSTRUCTION"},
            {"ForConstruction","FORCONSTRUCTION"},
            {"DefConstruction","DEFCONSTRUCTION"},
            {"Block","BLOCK"},
            {"IntNumber","INTNUMBER"},
            {"FloatNumber","FLOATNUMBER"},
            {"String","STRING"},
            {"Return","RETURN"},
            {"ListElements","LISTELEMENTS"}
        };

        public void parserStart(List<string> lexeroutput, List<string> codeoutput)
        {
            Grammar grammar = new Grammar();
            int level = 0;
            if (lexeroutput == null || codeoutput == null)
            {
                Console.WriteLine("Input doesn't exist!");
                return;
            }
            grammar.AddGrammar(SearchOfDick("Program"), level);
            parserMain(grammar, lexeroutput, codeoutput, level);
            printParserResult(grammar);
        }

        public void printParserResult (Grammar grammar)
        {
            grammar.ShowGrammar();
        }

        //pulling dictionary reference
        public string SearchOfDick(string inp)
        {
            string result;
            //find key inp
            presentation.TryGetValue(inp, out result);
            return result;
        }

        //find the end of a single line code
        public int singleCodeLength(List<string> lexeroutput, int startPoint, string endMark)
        {
            for (int i = startPoint; i < lexeroutput.Count; i++)
            {
                if (lexeroutput[i] == endMark)
                    return i;
            }
            return 0;
        }

        public void parserMain(Grammar grammar, List<string> lexeroutput, List<string> codeoutput, int level)
        {
            int ASTNumber = 1;
            int i = 0;
            listNumber = i;
            while (i != lexeroutput.Count && listNumber != lexeroutput.Count && listNumber < lexeroutput.Count)
            {
                level = 1;
                grammar.AddGrammar("#" + ASTNumber, level);
                string inp = lexeroutput[listNumber];
                switch (inp)
                {
                    case "ID":
                        parserId(grammar, lexeroutput, codeoutput, level);
                        break;

                    case "FUNCTION":
                        parserFunction(grammar, lexeroutput, codeoutput, level);
                        break;

                    case "END_LINE":
                        level = 1;
                        break;

                    default:
                        Console.WriteLine("Unexpected tokens! - " + lexeroutput[listNumber]);
                        break;

                }
                ASTNumber++;
                listNumber++;
            }
        }

        public void parserFunction(Grammar grammar, List<string> lexeroutput, List<string> codeoutput, int level)
        {
            grammar.AddGrammar(SearchOfDick("Statement"), level);
            level++;
            grammar.AddGrammar(SearchOfDick("Function"), level);
            level++;
            listNumber++;
            grammar.AddGrammar(SearchOfDick("Name"), level);
            level++;
            grammar.AddGrammar(codeoutput[listNumber], level);
            level--;
            listNumber++;
            grammar.AddGrammar(SearchOfDick("Parameters"), level);
            listNumber++;
            int initialLevel = level;

            //loop for the parameters
            int EOParameters = singleCodeLength(lexeroutput, listNumber, "CLOSED_BRACKET");
            while (listNumber != EOParameters && listNumber <= EOParameters)
            {
                string inp = lexeroutput[listNumber];
                switch (inp)
                {
                    
                    //case of parameters
                    case "ID":
                        level++;
                        grammar.AddGrammar(SearchOfDick("FormalParameter"), level);
                        level++;
                        grammar.AddGrammar(codeoutput[listNumber], level);
                        level--;
                        if (lexeroutput[listNumber + 1] == "ASSIGNMENT_OPERATION__SET")
                        {
                            level++;
                            listNumber += 2; //skip the '=' sign
                            grammar.AddGrammar(SearchOfDick("Default"), level);
                            level++;
                            for (int i = listNumber; i != EOParameters; i++)
                            {
                                if (lexeroutput[i] == "COMMA")
                                {
                                    parserVariable(grammar, lexeroutput, codeoutput, level, "COMMA");
                                    listNumber = i;
                                }
                                if (lexeroutput[i] == "CLOSED_BRACKET")
                                {
                                    parserVariable(grammar, lexeroutput, codeoutput, level, "CLOSED_BRACKET");
                                    listNumber = i;
                                }
                            }
                        }
                        level = initialLevel;
                        break;
                    
                    //maybe this is not needed in PHP?
                    case "END_LINE":
                        level = initialLevel;
                        break;

                    default:
                        Console.WriteLine("Unexpected tokens! - " + lexeroutput[listNumber]);
                        break;
                }
                listNumber++;
            }

            //function content
            listNumber = EOParameters + 1;
            grammar.AddGrammar(SearchOfDick("Content"), level);
            level++;
            int EOContent = singleCodeLength(lexeroutput, listNumber, "END_VOID");
            initialLevel++;
            while (listNumber != EOContent && listNumber <= EOContent)
            {
                listNumber++;
                string inp = lexeroutput[listNumber];
                switch (inp)
                {
                    case "ID":
                        parserId(grammar, lexeroutput, codeoutput, level);
                        level = initialLevel;
                        break;

                    case "RETURN":
                        grammar.AddGrammar(SearchOfDick("Return"), level);
                        level++;
                        listNumber++;
                        parserVariable(grammar, lexeroutput, codeoutput, level, "END_LINE");
                        level = initialLevel;
                        break;

                    case "END_LINE":
                        level = initialLevel;
                        break;

                    case "END_VOID":
                        level = initialLevel;
                        break;

                    default:
                        Console.WriteLine("Unexpected tokens! - " + lexeroutput[listNumber]);
                        break;

                }     
            }
        }

        public void parserId(Grammar grammar, List<string> lexeroutput, List<string> codeoutput, int level)
        {
            int initialLevel = level;
            int EOL = singleCodeLength(lexeroutput, listNumber, "END_LINE");

            while (listNumber != EOL && listNumber != lexeroutput.Count)
            {
                listNumber++;
                string inp = lexeroutput[listNumber];
                switch (inp)
                {

                    //case of variable assignment
                    case "ASSIGNMENT_OPERATION__SET":
                        grammar.AddGrammar(SearchOfDick("Statement"), level);
                        level++;
                        grammar.AddGrammar(SearchOfDick("Identifier"), level);
                        level++;
                        grammar.AddGrammar(SearchOfDick("Variable"), level);
                        level++;
                        grammar.AddGrammar(codeoutput[listNumber-1], level);
                        level--;
                        listNumber++;
                        parserVariable(grammar, lexeroutput, codeoutput, level, "END_LINE");
                        break;

                    case "OPEN_BRACKET":
                        grammar.AddGrammar(SearchOfDick("Call"), level);
                        level++;
                        grammar.AddGrammar(SearchOfDick("Name"), level);
                        level++;
                        grammar.AddGrammar(codeoutput[listNumber - 1], level);
                        level--;
                        grammar.AddGrammar(SearchOfDick("ActParameter"), level);
                        level++;
                        listNumber++;
                        for (int i = listNumber; i != EOL; i++)
                        {
                            if (lexeroutput[i] == "COMMA")
                            {
                                parserVariable(grammar, lexeroutput, codeoutput, level, "COMMA");
                                listNumber = i;
                            }
                            if (lexeroutput[i] == "CLOSED_BRACKET")
                            {
                                parserVariable(grammar, lexeroutput, codeoutput, level, "CLOSED_BRACKET");
                                listNumber = i;
                            }
                        }
                        break;


                    case "END_LINE":
                        level = initialLevel;
                        break;
                }
            }
        }

        public void parserVariable(Grammar grammar, List<string> lexeroutput, List<string> codeoutput, int level, string endMark)
        {
            int initialLevel = level;
            grammar.AddGrammar(SearchOfDick("Value"), level);
            level++;
            bool arithmeticOperation = false;
            int levelAfterValue = level;
            int EOL = singleCodeLength(lexeroutput, listNumber, endMark);
            while (listNumber != EOL && listNumber != lexeroutput.Count)
            {
                string inp = lexeroutput[listNumber];
                switch (inp)
                {
                    case "NUM_INT":
                        if (arithmeticOperation)
                            level = levelAfterValue + 1;
                        //float check
                        bool itsFloat = false;
                        for (int i = listNumber; i < EOL; i++)
                        {
                            //handle the read so wont read more than int/float
                            if (lexeroutput[i] != "NUM_INT" && lexeroutput[i] != "DOT")
                                break;

                            if (lexeroutput[i] == "DOT")
                            {
                                grammar.AddGrammar(SearchOfDick("FloatNumber"), level);
                                level++;
                                string floatValue = codeoutput[i - 1] + codeoutput[i] + codeoutput[i + 1];
                                grammar.AddGrammar(floatValue, level);
                                level--;
                                itsFloat = true;
                                listNumber = i+1;
                                break;
                            }
                        }

                        //integer check
                        if (itsFloat != true)
                        {
                            grammar.AddGrammar(SearchOfDick("IntNumber"), level);
                            level++;
                            grammar.AddGrammar(codeoutput[listNumber], level);
                            level--;
                        }
                        break;

                    case "STRING":
                        if (arithmeticOperation)
                            level++;
                        grammar.AddGrammar(SearchOfDick("String"), level);
                        level++;
                        grammar.AddGrammar(codeoutput[listNumber], level);
                        level--;
                        break;

                    case "ARITHMETIC_OPERATION__ADD":
                        grammar.AddBeforeGrammar(SearchOfDick("Add"), levelAfterValue);
                        arithmeticOperation = true;
                        break;

                    case "ARITHMETIC_OPERATION__MULT":
                        grammar.AddBeforeGrammar(SearchOfDick("Mul"), levelAfterValue);
                        arithmeticOperation = true;
                        break;

                    case "ARITHMETIC_OPERATION__SUB":
                        grammar.AddBeforeGrammar(SearchOfDick("Sub"), levelAfterValue);
                        arithmeticOperation = true;
                        break;

                    case "ARITHMETIC_OPERATION__DIV":
                        grammar.AddBeforeGrammar(SearchOfDick("Div"), levelAfterValue);
                        arithmeticOperation = true;
                        break;

                    case "ARITHMETIC_OPERATION__REM_DIV":
                        grammar.AddBeforeGrammar(SearchOfDick("Rem"), levelAfterValue);
                        arithmeticOperation = true;
                        break;

                    case "ARITHMETIC_OPERATION__DEGREE":
                        grammar.AddBeforeGrammar(SearchOfDick("Deg"), levelAfterValue);
                        arithmeticOperation = true;
                        break;

                    case "ID":
                        grammar.AddGrammar(SearchOfDick("Variable"), level);
                        level++;
                        grammar.AddGrammar(codeoutput[listNumber], level);
                        break;

                    /* still have no idea how to implement the bracket
                     * so for now it is what it is
                   case "OPEN_BRACKET":
                       grammar.AddGrammar(SearchOfDick("openBracket"), levelAfterValue);
                       arithmeticOperation = true;
                       break;

                   case "CLOSED_BRACKET":
                       grammar.AddBeforeGrammar(SearchOfDick("closeBracket"), levelAfterValue);
                       arithmeticOperation = true;
                       break;*/

                    case "END_LINE":
                        level = initialLevel;
                        break;

                    default:
                        Console.WriteLine("Unexpected tokens! - " + lexeroutput[listNumber]);
                        break;
                }
                listNumber++;
            }
        }
    }
}
