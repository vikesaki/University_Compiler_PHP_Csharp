using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static PHPtoC_.Parser;

namespace PHPtoC_
{
    class Translator
    {
        List<String> TranslatorResult = new List<String>();

        int currentLocation = 0;


        public void PrintTransator(List<String> translated)
        {
            /*
             * so simply (i realized that this is a shitty variable assignment)
             * "tab" means adding tab in the beginning of line
             * "  " means next line
             * "~" means next line, next line
             */
            bool tabbedLines = false;
            for (int i = 0; i < translated.Count; i++)
            {
                switch (translated[i])
                {
                    case " ":
                        Console.WriteLine();
                        if (tabbedLines)
                            Console.Write("  ");
                        break;

                    case "~":
                        Console.Write("\n\n");
                        if (tabbedLines)
                            Console.Write("  ");
                        break;

                    case "{":
                        tabbedLines = true;
                        Console.Write(": \n\n  ");
                        break;

                    case "}":
                        tabbedLines = false;
                        break;

                    case "tab":
                        tabbedLines = true;
                        break;

                    case "notab":
                        tabbedLines = false;
                        break;


                    default:
                        Console.Write(translated[i] + " ");
                        break;
                }
            }
        }

        public void TranslatorStart(Parser.Grammar grammar, List<String> translated)
        {
            Translate(grammar, translated);
        }

        public void Translate(Parser.Grammar grammar, List<String> translated)
        {
            string prefix = "#";
            int suffix = 1;
            int suffixInc = suffix + 1;
            string GrammarNumberStart;
            string GrammarNumberEnd;
            

            int totalGrammar = grammar.GetGrammarAmount();
            int lengthGrammar = grammar.GetGrammarLength();
            Parser.Node startGrammar = grammar.returnGrammar();
            Parser.Node endGrammar = grammar.returnGrammar();


            while (suffixInc <= totalGrammar && currentLocation < lengthGrammar)
            {
                GrammarNumberStart = prefix + suffix;
                GrammarNumberEnd = prefix + suffixInc;
                while (startGrammar.Pattern != GrammarNumberStart)
                {
                    startGrammar = startGrammar.Next;
                    currentLocation++;
                }
                while (endGrammar.Pattern != GrammarNumberEnd)
                    endGrammar = endGrammar.Next;

                while (startGrammar != endGrammar)
                {
                    startGrammar = startGrammar.Next;
                    switch (startGrammar.Pattern)
                    {
                        case "STATEMENT":
                            StatementCase(grammar, translated, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "INCREMENT":
                            IncDecCase(grammar, translated, startGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "DECREMENT":
                            IncDecCase(grammar, translated, startGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "CALL":
                            CallCase(grammar, translated, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;


                        //Conditional
                        case "IFCONSTRUCTION":
                            ConditionalCase(grammar, translated, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "ELSECONSTRUCTION":
                            ConditionalCase(grammar, translated, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "ELSEIFCONSTRUCTION":
                            ConditionalCase(grammar, translated, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "WHILECONSTRUCTION":
                            ConditionalCase(grammar, translated, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "DOCONSTRUCTION":
                            DoWhileCase(grammar, translated, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "FORCONSTRUCTION":
                            ForCase(grammar, translated, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "FOREACHCONSTRUCTION":
                            ForCase(grammar, translated, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;
                    }
                }
                suffix++;
                suffixInc++;
                translated.Add("~");
            }

            GrammarNumberEnd = prefix;
            while (endGrammar.Pattern != GrammarNumberEnd)
                endGrammar = endGrammar.Next;
            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "STATEMENT":
                        StatementCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "INCREMENT":
                        IncDecCase(grammar, translated, startGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "DECREMENT":
                        IncDecCase(grammar, translated, startGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "CALL":
                        CallCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    //Conditional
                    case "IFCONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "ELSECONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "ELSEIFCONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "WHILECONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "DOCONSTRUCTION":
                        DoWhileCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "FORCONSTRUCTION":
                        ForCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "FOREACHCONSTRUCTION":
                        ForCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;
                }
            }
        }

        public static void EchoCase(Parser.Grammar grammar, List<String> translated, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            translated.Add("print(");
            startGrammar = startGrammar.Previous;
            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "VALUE":
                        startGrammar = startGrammar.Previous;
                        ValueCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;
                }
            }
            translated.Add(")");
        }

        public static void ForCase(Parser.Grammar grammar, List<String> translated, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            string condition = startGrammar.Pattern;

            translated.Add("For");
            Node conditionalGrammar = startGrammar;
            while (conditionalGrammar.Pattern != "CONTENT")
            {
                conditionalGrammar = conditionalGrammar.Next;
            }

            while (startGrammar != conditionalGrammar)
            {
                startGrammar = startGrammar.Next;

                switch (startGrammar.Pattern)
                {
                    case "VARIABLE":
                        startGrammar = startGrammar.Next;
                        string value = startGrammar.Pattern;
                        translated.Add(value.Remove(0, 1));
                        if (condition == "FORCONSTRUCTION")
                        {
                            while (startGrammar.Pattern != "VARIABLE")
                            {
                                startGrammar = startGrammar.Next;
                            }
                        }
                        else
                            translated.Add("in");
                        startGrammar = startGrammar.Next;
                        break;

                    case "INTNUMBER":
                        translated.Add("in range (");
                        translated.Add(startGrammar.Next.Pattern);
                        translated.Add(")");
                        startGrammar = conditionalGrammar;
                        break;
                }     
            }

            if (condition == "FOREACHCONSTRUCTION")
                deletePrevList(translated);
            translated.Add("tab");
            translated.Add(" ");
            startGrammar = startGrammar.Previous;
            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "STATEMENT":
                        StatementCase(grammar, translated, startGrammar, endGrammar);
                        translated.Add(" ");
                        break;

                    case "INCREMENT":
                        IncDecCase(grammar, translated, startGrammar);
                        while (startGrammar.Pattern != "VARIABLE")
                            startGrammar = startGrammar.Next;
                        translated.Add(" ");
                        break;

                    case "DECREMENT":
                        IncDecCase(grammar, translated, startGrammar);
                        while (startGrammar.Pattern != "VARIABLE")
                            startGrammar = startGrammar.Next;
                        translated.Add(" ");
                        break;

                    case "CALL":
                        CallCase(grammar, translated, startGrammar, endGrammar);
                        translated.Add(" ");
                        break;

                    case "ECHO":
                        EchoCase(grammar, translated, startGrammar, endGrammar);
                        translated.Add(" ");
                        break;

                    //Conditional
                    case "IFCONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        break;

                    case "ELSECONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        break;

                    case "ELSEIFCONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        break;
                }
            }
            deletePrevList(translated);
            translated.Add("notab");
        }

        public static void DoWhileCase(Parser.Grammar grammar, List<String> translated, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            string condition = startGrammar.Pattern;
            switch (condition)
            {
                case "DOCONSTRUCTION":
                    translated.Add("while True :");
                    break;
            }

            translated.Add("tab");
            translated.Add(" ");
            Node conditionalGrammar = startGrammar;
            while (conditionalGrammar.Pattern != "CONDITIONS")
            {
                conditionalGrammar = conditionalGrammar.Next;
            }

            while (startGrammar != conditionalGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "STATEMENT":
                        StatementCase(grammar, translated, startGrammar, endGrammar);
                        translated.Add(" ");
                        break;

                    case "INCREMENT":
                        IncDecCase(grammar, translated, startGrammar);
                        while (startGrammar.Pattern != "VARIABLE")
                            startGrammar = startGrammar.Next;
                        translated.Add(" ");
                        break;

                    case "DECREMENT":
                        IncDecCase(grammar, translated, startGrammar);
                        while (startGrammar.Pattern != "VARIABLE")
                            startGrammar = startGrammar.Next;
                        translated.Add(" ");
                        break;

                    case "CALL":
                        CallCase(grammar, translated, startGrammar, endGrammar);
                        translated.Add(" ");
                        break;

                    case "ECHO":
                        EchoCase(grammar, translated, startGrammar, endGrammar);
                        translated.Add(" ");
                        break;

                    //Conditional
                    case "IFCONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        break;

                    case "ELSECONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        break;

                    case "ELSEIFCONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        break;

                    case "VARIABLE":
                        startGrammar = startGrammar.Previous;
                        ValueCase(grammar, translated, startGrammar, conditionalGrammar);
                        startGrammar = startGrammar.Next;
                        break;

                    case "STRING":
                        startGrammar = startGrammar.Previous;
                        ValueCase(grammar, translated, startGrammar, conditionalGrammar);
                        startGrammar = startGrammar.Next;
                        break;

                    case "INTNUMBER":
                        startGrammar = startGrammar.Previous;
                        ValueCase(grammar, translated, startGrammar, conditionalGrammar);
                        startGrammar = startGrammar.Next;
                        break;

                    case "FLOATNUMBER":
                        startGrammar = startGrammar.Previous;
                        ValueCase(grammar, translated, startGrammar, conditionalGrammar);
                        startGrammar = startGrammar.Next;
                        break;
                }
            }

            startGrammar = startGrammar.Previous;
            translated.Add("if (");
            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "VARIABLE":
                        startGrammar = startGrammar.Previous;
                        ValueCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = startGrammar.Next;
                        break;

                    case "STRING":
                        startGrammar = startGrammar.Previous;
                        ValueCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = startGrammar.Next;
                        break;

                    case "INTNUMBER":
                        startGrammar = startGrammar.Previous;
                        ValueCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = startGrammar.Next;
                        break;

                    case "FLOATNUMBER":
                        startGrammar = startGrammar.Previous;
                        ValueCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = startGrammar.Next;
                        break;

                    case "CALL":
                        CallCase(grammar, translated, startGrammar, endGrammar);
                        break;

                    case "FUNCTION":
                        FunctionCase(grammar, translated, startGrammar, endGrammar);
                        break;

                    case "EQUALTO":
                        translated.Add("==");
                        break;

                    case "IDENTICALLYEQUALTO":
                        translated.Add("==");
                        break;

                    case "INEQUALTO":
                        translated.Add("!=");
                        break;

                    case "IDENTICALLYINEQUALTO":
                        translated.Add("!=");
                        break;

                    case "LESSERTHAN":
                        translated.Add("<");
                        break;

                    case "GREATERTHAN":
                        translated.Add(">");
                        break;

                    case "LESSEREQUALTO":
                        translated.Add("<=");
                        break;

                    case "GREATEREQUALTO":
                        translated.Add(">=");
                        break;

                    case "AND":
                        translated.Add("and");
                        break;

                    case "OR":
                        translated.Add("or");
                        break;

                    case "NOT":
                        translated.Add("not");
                        break;
                }
            }
            translated.Add(")");
            translated.Add(" ");
            translated.Add("  break");
            translated.Add("notab");
        }

        public static void ConditionalCase(Parser.Grammar grammar, List<String> translated, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            string condition = startGrammar.Pattern;
            switch (condition)
            {
                case "IFCONSTRUCTION":
                    translated.Add("if");
                    break;
                case "ELSEIFCONSTRUCTION":
                    translated.Add("elif");
                    break;
                case "ELSECONSTRUCTION":
                    translated.Add("else");
                    break;
                case "WHILECONSTRUCTION":
                    translated.Add("while");
                    break;
                case "DOCONSTRUCTION":
                    translated.Add("while");
                    break;

            }

            Node conditionalGrammar = startGrammar;
            while (conditionalGrammar.Pattern != "CONTENT")
            {
                conditionalGrammar = conditionalGrammar.Next;
            }
            conditionalGrammar = conditionalGrammar.Next;

            while (startGrammar != conditionalGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "VARIABLE":
                        startGrammar = startGrammar.Previous;
                        ValueCase(grammar, translated, startGrammar, conditionalGrammar);
                        startGrammar = startGrammar.Next;
                        break;

                    case "STRING":
                        startGrammar = startGrammar.Previous;
                        ValueCase(grammar, translated, startGrammar, conditionalGrammar);
                        startGrammar = startGrammar.Next;
                        break;

                    case "INTNUMBER":
                        startGrammar = startGrammar.Previous;
                        ValueCase(grammar, translated, startGrammar, conditionalGrammar);
                        startGrammar = startGrammar.Next;
                        break;

                    case "FLOATNUMBER":
                        startGrammar = startGrammar.Previous;
                        ValueCase(grammar, translated, startGrammar, conditionalGrammar);
                        startGrammar = startGrammar.Next;
                        break;

                    case "CALL":
                        CallCase(grammar, translated, startGrammar, conditionalGrammar);
                        break;

                    case "FUNCTION":
                        FunctionCase(grammar, translated, startGrammar, conditionalGrammar);
                        break;

                    case "EQUALTO":
                        translated.Add("==");
                        break;

                    case "IDENTICALLYEQUALTO":
                        translated.Add("==");
                        break;

                    case "INEQUALTO":
                        translated.Add("!=");
                        break;

                    case "IDENTICALLYINEQUALTO":
                        translated.Add("!=");
                        break;

                    case "LESSERTHAN":
                        translated.Add("<");
                        break;

                    case "GREATERTHAN":
                        translated.Add(">");
                        break;

                    case "LESSEREQUALTO":
                        translated.Add("<=");
                        break;

                    case "GREATEREQUALTO":
                        translated.Add(">=");
                        break;

                    case "AND":
                        translated.Add("and");
                        break;

                    case "OR":
                        translated.Add("or");
                        break;

                    case "NOT":
                        translated.Add("not");
                        break;
                }
            }

            if (condition == "WHILECONSTRUCTION")
                translated.Add(":");
            translated.Add("tab");
            translated.Add(" ");
            startGrammar = startGrammar.Previous;
            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "STATEMENT":
                        StatementCase(grammar, translated, startGrammar, endGrammar);
                        translated.Add(" ");
                        break;

                    case "INCREMENT":
                        IncDecCase(grammar, translated, startGrammar);
                        while (startGrammar.Pattern != "VARIABLE")
                            startGrammar = startGrammar.Next;
                        translated.Add(" ");
                        break;

                    case "DECREMENT":
                        IncDecCase(grammar, translated, startGrammar);
                        while (startGrammar.Pattern != "VARIABLE")
                            startGrammar = startGrammar.Next;
                        translated.Add(" ");
                        break;

                    case "CALL":
                        CallCase(grammar, translated, startGrammar, endGrammar);
                        translated.Add(" ");
                        break;

                    case "ECHO":
                        EchoCase(grammar, translated, startGrammar, endGrammar);
                        translated.Add(" ");
                        break;

                    //Conditional
                    case "IFCONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        break;

                    case "ELSECONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        break;

                    case "ELSEIFCONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        break;
                }
            }
            deletePrevList(translated);
            translated.Add("notab");
        }

        public static void IncDecCase(Parser.Grammar grammar, List<String> translated, Parser.Node startGrammar)
        {
            string operation = startGrammar.Pattern;
            startGrammar = startGrammar.Next.Next.Next; //var nam
            string value = startGrammar.Pattern;
            string phyValue = (value.Remove(0, 1));
            if (operation == "INCREMENT")
                phyValue += " += 1";
            if (operation == "DECREMENT")
                phyValue += " -= 1";
            translated.Add(phyValue);
        }

        public static void StatementCase(Parser.Grammar grammar, List<String> translated, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "IDENTIFIER":
                        IdentifierCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "CALL":
                        CallCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "FUNCTION":
                        FunctionCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    //Conditional
                    case "IFCONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "ELSECONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "ELSEIFCONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "WHILECONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "DOCONSTRUCTION":
                        DoWhileCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "FORCONSTRUCTION":
                        ForCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "FOREACHCONSTRUCTION":
                        ForCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;
                }
            }
        }

        public static void FunctionCase(Parser.Grammar grammar, List<String> translated, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            translated.Add("def");
            startGrammar = startGrammar.Next.Next;
            translated.Add(startGrammar.Pattern);
            string value;
            translated.Add("(");

            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "FORMALPARAMETERS":
                        startGrammar = startGrammar.Next;
                        value = startGrammar.Pattern;
                        translated.Add(value.Remove(0,1));

                        if (startGrammar.Next.Pattern == "DEFAULT")
                        {
                            startGrammar = startGrammar.Next;
                            translated.Add("=");
                            ValueCase(grammar, translated, startGrammar, startGrammar.Next.Next.Next);
                            translated.Add(",");
                            startGrammar = startGrammar.Next.Next.Next;
                            break;
                        }
                        translated.Add(",");
                        break;

                    case "CONTENT":
                        deletePrevList(translated);
                        translated.Add(")");
                        translated.Add("{");
                        break;

                    case "STATEMENT":
                        StatementCase(grammar, translated, startGrammar, endGrammar);
                        translated.Add("~");
                        break;

                    case "INCREMENT":
                        IncDecCase(grammar, translated, startGrammar);
                        while (startGrammar.Pattern != "VARIABLE")
                            startGrammar = startGrammar.Next;
                        translated.Add("~");
                        break;

                    case "DECREMENT":
                        IncDecCase(grammar, translated, startGrammar);
                        while (startGrammar.Pattern != "VARIABLE")
                            startGrammar = startGrammar.Next;
                        translated.Add("~");
                        break;

                    case "CALL":
                        CallCase(grammar, translated, startGrammar, endGrammar);
                        translated.Add("~");
                        break;

                    case "RETURN":
                        translated.Add("return");
                        ValueCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    //Conditional
                    case "IFCONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "ELSECONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "ELSEIFCONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "WHILECONSTRUCTION":
                        ConditionalCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "DOCONSTRUCTION":
                        DoWhileCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "FORCONSTRUCTION":
                        ForCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "FOREACHCONSTRUCTION":
                        ForCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "ECHO":
                        EchoCase(grammar, translated, startGrammar, endGrammar);
                        translated.Add("~");
                        break;
                }
            }
            translated.Add("}");
        }

        public static void CallCase(Parser.Grammar grammar, List<String> translated, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            bool arithmeticOperation = false;
            string value;
            startGrammar = startGrammar.Next.Next;
            translated.Add(startGrammar.Pattern);
            translated.Add("(");
            startGrammar = startGrammar.Next;
            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "INTNUMBER":
                        startGrammar = startGrammar.Next;
                        if (arithmeticOperation)
                        {
                            addBeforeList(translated, startGrammar.Pattern);
                            arithmeticOperation = false;
                            break;
                        }
                        translated.Add(startGrammar.Pattern);
                        translated.Add(",");
                        break;

                    case "STRING":
                        startGrammar = startGrammar.Next;
                        if (arithmeticOperation)
                        {
                            addBeforeList(translated, startGrammar.Pattern);
                            arithmeticOperation = false;
                            break;
                        }
                        translated.Add(startGrammar.Pattern);
                        translated.Add(",");
                        break;

                    case "FLOATNUMBER":
                        startGrammar = startGrammar.Next;
                        if (arithmeticOperation)
                        {
                            addBeforeList(translated, startGrammar.Pattern);
                            arithmeticOperation = false;
                            break;
                        }
                        translated.Add(startGrammar.Pattern);
                        translated.Add(",");
                        break;

                    case "VARIABLE":
                        startGrammar = startGrammar.Next; //var nam
                        value = startGrammar.Pattern;

                        if (arithmeticOperation)
                        {
                            addBeforeList(translated, value.Remove(0, 1));
                            arithmeticOperation = false;
                            break;
                        }
                        translated.Add(value.Remove(0, 1));
                        translated.Add(",");
                        break;

                    case "ADD":
                        arithmeticOperation = true;
                        translated.Add("+");
                        break;

                    case "SUB":
                        arithmeticOperation = true;
                        translated.Add("-");
                        break;

                    case "MUL":
                        arithmeticOperation = true;
                        translated.Add("*");
                        break;

                    case "DIV":
                        arithmeticOperation = true;
                        translated.Add("/");
                        break;

                    case "DEG":
                        arithmeticOperation = true;
                        translated.Add("^");
                        break;

                }
            }
            deletePrevList(translated);
            translated.Add(")");
        }

        public static void IdentifierCase(Parser.Grammar grammar, List<String> translated, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            //bool arithmeticOperation = false;

            while (startGrammar.Pattern != "VARIABLE")
                startGrammar = startGrammar.Next;
            startGrammar = startGrammar.Next; //var nam
            string value = startGrammar.Pattern;
            translated.Add(value.Remove(0,1));
            startGrammar = startGrammar.Next; //value
            translated.Add("=");
            ValueCase(grammar, translated, startGrammar, endGrammar);
            /*while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "INTNUMBER":
                        startGrammar = startGrammar.Next;
                        if (arithmeticOperation)
                        {
                            addBeforeList(translated, startGrammar.Pattern);
                            arithmeticOperation = false;
                            break;
                        }
                        translated.Add(startGrammar.Pattern);
                        break;

                    case "STRING":
                        startGrammar = startGrammar.Next;
                        if (arithmeticOperation)
                        {
                            addBeforeList(translated, startGrammar.Pattern);
                            arithmeticOperation = false;
                            break;
                        }
                        translated.Add(startGrammar.Pattern);
                        break;

                    case "FLOATNUMBER":
                        startGrammar = startGrammar.Next;
                        if (arithmeticOperation)
                        {
                            addBeforeList(translated, startGrammar.Pattern);
                            arithmeticOperation = false;
                            break;
                        }
                        translated.Add(startGrammar.Pattern);
                        break;

                    case "VARIABLE":
                        startGrammar = startGrammar.Next; //var nam
                        value = startGrammar.Pattern;
                        
                        if (arithmeticOperation)
                        {
                            addBeforeList(translated, value.Remove(0, 1));
                            arithmeticOperation = false;
                            break;
                        }
                        translated.Add(value.Remove(0, 1));
                        break;

                    case "ADD":
                        arithmeticOperation = true;
                        translated.Add("+");
                        break;

                    case "SUB":
                        arithmeticOperation = true;
                        translated.Add("-");
                        break;

                    case "MUL":
                        arithmeticOperation = true;
                        translated.Add("*");
                        break;

                    case "DIV":
                        arithmeticOperation = true;
                        translated.Add("/");
                        break;

                    case "DEG":
                        arithmeticOperation = true;
                        translated.Add("^");
                        break;

                    case "CALL":
                        CallCase(grammar, translated, startGrammar, endGrammar);
                        break;

                        /*case "FUNCTION":
                            FunctionCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "CALL":
                            CallCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "VARIABLE":
                            startGrammar = startGrammar.Next;
                            VariableUsed.Add(startGrammar.Pattern);
                            break;

                        case "NAME":
                            startGrammar = startGrammar.Next;
                            VariableUsed.Add(startGrammar.Pattern);
                            break;

                        case "CONDITIONS":
                            ConditionalCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;*
                }
            )*/
        }

        public static void ValueCase(Parser.Grammar grammar, List<String> translated, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            bool arithmeticOperation = false;
            string value;
            //translated.Add("=");
            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "INTNUMBER":
                        startGrammar = startGrammar.Next;
                        if (arithmeticOperation)
                        {
                            addBeforeList(translated, startGrammar.Pattern);
                            arithmeticOperation = false;
                            break;
                        }
                        translated.Add(startGrammar.Pattern);
                        break;

                    case "STRING":
                        startGrammar = startGrammar.Next;
                        if (arithmeticOperation)
                        {
                            addBeforeList(translated, startGrammar.Pattern);
                            arithmeticOperation = false;
                            break;
                        }
                        translated.Add(startGrammar.Pattern);
                        break;

                    case "FLOATNUMBER":
                        startGrammar = startGrammar.Next;
                        if (arithmeticOperation)
                        {
                            addBeforeList(translated, startGrammar.Pattern);
                            arithmeticOperation = false;
                            break;
                        }
                        translated.Add(startGrammar.Pattern);
                        break;

                    case "VARIABLE":
                        startGrammar = startGrammar.Next; //var nam
                        value = startGrammar.Pattern;

                        if (arithmeticOperation)
                        {
                            addBeforeList(translated, value.Remove(0, 1));
                            arithmeticOperation = false;
                            break;
                        }
                        translated.Add(value.Remove(0, 1));
                        break;

                    case "ADD":
                        arithmeticOperation = true;
                        translated.Add("+");
                        break;

                    case "SUB":
                        arithmeticOperation = true;
                        translated.Add("-");
                        break;

                    case "MUL":
                        arithmeticOperation = true;
                        translated.Add("*");
                        break;

                    case "DIV":
                        arithmeticOperation = true;
                        translated.Add("/");
                        break;

                    case "DEG":
                        arithmeticOperation = true;
                        translated.Add("^");
                        break;

                    case "CALL":
                        CallCase(grammar, translated, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "STATEMENT":
                        startGrammar = endGrammar;
                        break;

                    case "RETURN":
                        startGrammar = endGrammar;
                        break;

                    case "INCREMENT":
                        startGrammar = endGrammar;
                        break;

                    case "DECREMENT":
                        startGrammar = endGrammar;
                        break;

                    //conditional break down here
                    case "EQUALTO":
                        startGrammar = endGrammar;
                        break;

                    case "IDENTICALLYEQUALTO":
                        startGrammar = endGrammar;
                        break;

                    case "INEQUALTO":
                        startGrammar = endGrammar;
                        break;

                    case "IDENTICALLYINEQUALTO":
                        startGrammar = endGrammar;
                        break;

                    case "LESSERTHAN":
                        startGrammar = endGrammar;
                        break;

                    case "GREATERTHAN":
                        startGrammar = endGrammar;
                        break;

                    case "LESSEREQUALTO":
                        startGrammar = endGrammar;
                        break;

                    case "GREATEREQUALTO":
                        startGrammar = endGrammar;
                        break;

                    //logical break down
                    case "AND":
                        startGrammar = endGrammar;
                        break;

                    case "OR":
                        startGrammar = endGrammar;
                        break;

                    case "ELSE":
                        startGrammar = endGrammar;
                        break;

                }
            }
        }

        public static void addBeforeList(List<String> translated, string value)
        {
            int length = translated.Count;
            translated.Insert(length - 1, value);
        }

        public static void deletePrevList(List<String> translated)
        {
            int length = translated.Count;
            translated.RemoveAt(length - 1);
        }
    }
}
