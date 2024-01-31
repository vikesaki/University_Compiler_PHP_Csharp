using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static PHPtoC_.Parser;

namespace PHPtoC_
{
    class SemanticAnalyst
    {
        List<String> VariableUsed = new List<String>();
        List<String> LocalVariables = new List<String>();
        List<String> GlobalVariables = new List<String>();

        public void Start(Parser.Grammar grammar, bool Optimization)
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

            if (Optimization)
            {
                Console.WriteLine("\n\nRemoving variables stated but not used : ");
                RemoveUnused(grammar, UnusedVar);
            } 
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
                        VariableUsed.Add(startGrammar.Pattern);
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
        
        public void RemoveUnused (Parser.Grammar grammar, List<String> Unused)
        {
            string prefix = "#";
            int suffix = 1;
            int suffixInc = suffix + 1;
            string GrammarNumberStart;
            string GrammarNumberEnd;
            int currentLocation = 0;
            string currentPattern = "";

            int totalGrammar = grammar.GetGrammarAmount();
            int lengthGrammar = grammar.GetGrammarLength();
            Parser.Node startGrammar = grammar.returnGrammar();
            Parser.Node endGrammar = grammar.returnGrammar();


            while (suffixInc <= totalGrammar && currentLocation < lengthGrammar)
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
                            if (startGrammar.Next.Pattern == "IDENTIFIER" && startGrammar.Next.Next.Pattern == "VARIABLE")
                            {
                                startGrammar = startGrammar.Next.Next.Next;
                                currentPattern = startGrammar.Pattern;
                                var match = Unused.FirstOrDefault(stringToCheck => stringToCheck.Contains(currentPattern));
                                if (match != null)
                                {
                                    RemoveGrammar(grammar, currentPattern, currentLocation + 1);
                                }
                            }
                            if (startGrammar.Next.Pattern == "FUNCTION" && startGrammar.Next.Next.Pattern == "NAME")
                            {
                                startGrammar = startGrammar.Next.Next.Next;
                                currentPattern = startGrammar.Pattern;
                                var match = Unused.FirstOrDefault(stringToCheck => stringToCheck.Contains(currentPattern));
                                if (match != null)
                                {
                                    RemoveGrammar(grammar, currentPattern, currentLocation + 1);
                                }
                            }
                            startGrammar = startGrammar.Next.Next;
                            break;
                    }
                    break;
                }
                suffix++;
                suffixInc++;
                currentLocation++; 
            }

            GrammarNumberStart = prefix + suffix;
            while (startGrammar.Pattern != GrammarNumberStart)
                startGrammar = startGrammar.Next;
            while (endGrammar.Pattern != prefix)
                endGrammar = endGrammar.Next;

            while (startGrammar != endGrammar)
            {
                startGrammar = startGrammar.Next;
                switch (startGrammar.Pattern)
                {
                    case "STATEMENT":
                        if (startGrammar.Next.Pattern == "IDENTIFIER" && startGrammar.Next.Next.Pattern == "VARIABLE")
                        {
                            startGrammar = startGrammar.Next.Next.Next;
                            currentPattern = startGrammar.Pattern;
                            var match = Unused.FirstOrDefault(stringToCheck => stringToCheck.Contains(currentPattern));
                            if (match != null)
                            {
                                RemoveGrammar(grammar, currentPattern, currentLocation + 1);
                            }
                        }
                        if (startGrammar.Next.Pattern == "FUNCTION" && startGrammar.Next.Next.Pattern == "NAME")
                        {
                            startGrammar = startGrammar.Next.Next.Next;
                            currentPattern = startGrammar.Pattern;
                            var match = Unused.FirstOrDefault(stringToCheck => stringToCheck.Contains(currentPattern));
                            if (match != null)
                            {
                                RemoveGrammar(grammar, currentPattern, currentLocation + 1);
                            }
                        }
                        startGrammar = startGrammar.Next.Next;
                        break;
                }
                break;
            }
        }

        public void RemoveGrammar(Parser.Grammar grammar, string pattern, int location)
        {
            Parser.Node newStart = grammar.returnGrammar();
            Parser.Node newEnd = grammar.returnGrammar();

            string prefix = "#";
            int endLocation = location + 1;
            string start = prefix + location;
            string end = prefix + endLocation;
            int totalGrammar = grammar.GetGrammarAmount();
            int lengthGrammar = grammar.GetGrammarLength();
            while (endLocation <= totalGrammar && location < lengthGrammar)
            {
                while (newStart.Pattern != start)
                    newStart = newStart.Next;
                while (newEnd.Pattern != end)
                    newEnd = newEnd.Next;
                newStart = newStart.Next;

                if (newStart.Previous == null)
                {
                    while (newStart.Pattern != end)
                    {
                        grammar.CurrentGrammar = grammar.CurrentGrammar.Next;
                        grammar.CurrentGrammar.Previous = null;
                        newStart = newStart.Next;
                    }
                }
                else
                {
                    while (newStart.Pattern != end)
                    {
                        newStart.Previous.Next = newStart.Next;
                        newStart.Next.Previous = newStart.Previous;
                        newStart = newStart.Next;
                    }

                }
                return;
            }

            while (newStart.Pattern != start)
                newStart = newStart.Next;
            while (newEnd.Pattern != prefix)
                newEnd = newEnd.Next;
            newStart = newStart.Next;
            while (newStart.Pattern != prefix)
            {
                newStart.Previous.Next = newStart.Next;
                newStart.Next.Previous = newStart.Previous;
                newStart = newStart.Next;
            }

            //fixing numbering
            //shits doesnt work, dunno why
            /*
            while (newStart.Next != null)
            {
                if (newStart.Pattern == end)
                {
                    location++;
                    endLocation = location + 1;
                    start = "#" + location;
                    end = "#" + endLocation;
                    newStart.Pattern = start;
                }
                newStart = newStart.Next;
            }
            

            grammar.ShowGrammar();*/
        } 
    }
}

