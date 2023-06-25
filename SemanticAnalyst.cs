using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PHPtoC_
{
    class SemanticAnalyst
    {
        List<String> VariableUsed = new List<String>();
        List<String> LocalVariables = new List<String>();
        List<String> GlobalVariables = new List<String>();

        public void Start(Parser.Grammar grammar)
        {
            List<String> UnusedVar = new List<String>();
            List<String> InexistVar = new List<String>();
            Analyze(grammar);
            UnusedVar = UnusedVariables();
            InexistVar = InexistVariables();

            Console.WriteLine("\nGlobal Variables : ");
            ShowList(GlobalVariables);
            Console.WriteLine("\n\nLocal Variables : ");
            ShowList(LocalVariables);
            Console.WriteLine("\n\nUsed Variables : ");
            ShowList(VariableUsed);
            Console.WriteLine("\n\nVariables that stated but not used : ");
            ShowList(UnusedVar);
            Console.WriteLine("\n\nVariables that used but not stated : ");
            ShowList(InexistVar);
        }

        public static void ShowList(List<String> list)
        {
            List<String> noDupes = list.Distinct().ToList();
            foreach (string content in noDupes)
                Console.Write(content + ", ");
        }

        public void Analyze(Parser.Grammar grammar) 
        {
            string prefix = "#";
            int suffix = 1;
            int suffixInc = suffix + 1;
            string GrammarNumberStart;
            string GrammarNumberEnd;
            int currentLocation = 0;

            int totalGrammar = grammar.GetGrammarAmount();
            int lengthGrammar = grammar.GetGrammarLength();
            Parser.Node startGrammar = grammar.returnGrammar();
            Parser.Node endGrammar = grammar.returnGrammar();
            

            while (suffixInc <= totalGrammar  && currentLocation < lengthGrammar)
            {
                GrammarNumberStart = prefix + suffix;
                GrammarNumberEnd = prefix + suffixInc;
                while (startGrammar.Pattern != GrammarNumberStart)
                    startGrammar = startGrammar.Next;
                while (endGrammar.Pattern != GrammarNumberEnd)
                    endGrammar = endGrammar.Next;

                while (startGrammar != endGrammar)
                {
                    startGrammar = startGrammar.Next;
                    switch (startGrammar.Pattern)
                    {
                        case "STATEMENT":
                            StatementCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "CALL":
                            CallCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        //Conditional
                        case "IFCONSTRUCTION":
                            ConditionalCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "ELSECONSTRUCTION":
                            ConditionalCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "ELSEIFCONSTRUCTION":
                            ConditionalCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        //Loop
                        case "WHILECONSTRUCTION":
                            LoopCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "DOCONSTRUCTION":
                            LoopCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "FORCONSTRUCTION":
                            ForCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "FOREACHCONSTRUCTION":
                            ForCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        //variable increment/decrement
                        case "INCREMENT":
                            UsedVarAdd(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;

                        case "DECREMENT":
                            UsedVarAdd(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                            startGrammar = endGrammar;
                            break;
                    }
                }
                suffix++;
                suffixInc++;
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
                        StatementCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "CALL":
                        CallCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    //Conditional
                    case "IFCONSTRUCTION":
                        ConditionalCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "ELSECONSTRUCTION":
                        ConditionalCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "ELSEIFCONSTRUCTION":
                        ConditionalCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    //Loop
                    case "WHILECONSTRUCTION":
                        LoopCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "DOCONSTRUCTION":
                        LoopCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "FORCONSTRUCTION":
                        ForCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "FOREACHCONSTRUCTION":
                        ForCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "INCREMENT":
                        UsedVarAdd(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "DECREMENT":
                        UsedVarAdd(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;
                }
            }
            
        }

        public static void ForCase(Parser.Grammar grammar, List<string> VariableUsed, List<string> GlobalVariables, List<string> LocalVariables, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            while(startGrammar.Pattern != "VARIABLE")
                startGrammar = startGrammar.Next;
            LocalVariables.Add(startGrammar.Next.Pattern);
            startGrammar = startGrammar.Next;
            UsedVarAdd(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
        }

        public static void ConditionalCase(Parser.Grammar grammar, List<string> VariableUsed, List<string> GlobalVariables, List<string> LocalVariables, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            while (startGrammar != endGrammar)
            {
                switch (startGrammar.Pattern)
                {
                    case "NAME":
                        startGrammar = startGrammar.Next;
                        VariableUsed.Add(startGrammar.Pattern);
                        break;

                    case "IDENTIFIER":
                        startGrammar = startGrammar.Next.Next;
                        LocalVariables.Add(startGrammar.Pattern);
                        break;

                    case "CALL":
                        startGrammar = startGrammar.Next.Next;
                        GlobalVariables.Add(startGrammar.Pattern);
                        break;

                    case "VARIABLE":
                        startGrammar = startGrammar.Next;
                        VariableUsed.Add(startGrammar.Pattern);
                        break;

                }
                startGrammar = startGrammar.Next;
            }
        }

        public static void LoopCase(Parser.Grammar grammar, List<string> VariableUsed, List<string> GlobalVariables, List<string> LocalVariables, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "CONDITIONS":
                        ConditionalCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                    case "CONTENT":
                        StatementCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                }
            }
        }

        public static void UsedVarAdd(Parser.Grammar grammar, List<string> VariableUsed, List<string> GlobalVariables, List<string> LocalVariables, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            while (startGrammar != endGrammar)
            {
                switch (startGrammar.Pattern)
                {
                    case "VARIABLE":
                        startGrammar = startGrammar.Next;
                        VariableUsed.Add(startGrammar.Pattern);
                        break;

                }
                startGrammar = startGrammar.Next;
            }
        }

        public static void StatementCase(Parser.Grammar grammar, List<string> VariableUsed, List<string> GlobalVariables, List<string> LocalVariables, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "IDENTIFIER":
                        startGrammar = startGrammar.Next.Next;
                        GlobalVariables.Add(startGrammar.Pattern);
                        break;

                    case "FUNCTION":
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
                        break;
                }
            }
        }

        public static void FunctionCase(Parser.Grammar grammar, List<string> VariableUsed, List<string> GlobalVariables, List<string> LocalVariables, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "CALL":
                        startGrammar = startGrammar.Next.Next;
                        GlobalVariables.Add(startGrammar.Pattern);
                        break;

                    case "FORMALPARAMETERS":
                        startGrammar = startGrammar.Next;
                        LocalVariables.Add(startGrammar.Pattern);
                        break;

                    case "NAME":
                        startGrammar = startGrammar.Next;
                        GlobalVariables.Add(startGrammar.Pattern);
                        break;

                    case "IDENTIFIER":
                        startGrammar = startGrammar.Next.Next;
                        LocalVariables.Add(startGrammar.Pattern);
                        break;

                    case "VARIABLE":
                        startGrammar = startGrammar.Next;
                        VariableUsed.Add(startGrammar.Pattern);
                        break;

                    case "CONDITIONS":
                        ConditionalCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;

                }
            }
        }

        public static void CallCase(Parser.Grammar grammar, List<string> VariableUsed, List<string> GlobalVariables, List<string> LocalVariables, Parser.Node startGrammar, Parser.Node endGrammar)
        {
            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "CALL":
                        startGrammar = startGrammar.Next.Next;
                        VariableUsed.Add(startGrammar.Pattern);
                        break;

                    case "NAME":
                        startGrammar = startGrammar.Next;
                        VariableUsed.Add(startGrammar.Pattern);
                        break;

                    case "VARIABLE":
                        startGrammar = startGrammar.Next;
                        VariableUsed.Add(startGrammar.Pattern);
                        break;

                    case "CONDITIONS":
                        ConditionalCase(grammar, VariableUsed, GlobalVariables, LocalVariables, startGrammar, endGrammar);
                        startGrammar = endGrammar;
                        break;
                }
                
            }
        }

        public List<string> UnusedVariables()
        {
            List<String> UnusedVar = new List<String>();
            UnusedVar = GlobalVariables.Except(VariableUsed).ToList();
            return UnusedVar;
        }

        public List<string> InexistVariables()
        {
            List<String> InexistVar = new List<String>();
            InexistVar = VariableUsed.Except(GlobalVariables).ToList();
            return InexistVar;
        }
    }
}
