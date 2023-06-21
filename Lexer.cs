using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace PHPtoC_
{
    class Lexer
    {
        public static void print<Type>(Type Input)
        {
            Console.WriteLine(Input);
        }
        public static void println<Type>(Type Input)
        {
            Console.Write(Input);
        }

        public string InputNameFile = "Input.txt";
        public List<string> SaveLine = new List<string>();

        public List<string> outpTable = new List<string>();


        Dictionary<string, string> words1 = new Dictionary<string, string>
            {
                {"abstract", "ABSTRACT"},
                {"and", "AND"},
                {"as", "AS"},
                {"break", "BREAK"},
                {"callable", "CALLABLE"},
                {"case", "CASE"},
                {"catch", "CATCH"},
                {"class", "CLASS"},
                {"clone", "CLONE"},
                {"const", "CONST"},
                {"continue", "CONINUE"},
                {"declare", "DECLARE"},
                {"default", "DEFAULT"},
                {"die()", "DIE()"},
                {"do", "DO"},
                {"echo", "ECHO"},
                {"else", "ELSE"},
                {"elseif", "ELSEIF"},
                {"empty()", "EMPTY()"},
                {"enddeclare", "ENDDECLARE"},
                {"endfor", "ENDFOR"},
                {"endforach", "ENDFOREACH"},
                {"endif", "ENDIF"},
                {"endswitch", "ENDSWITCH"},
                {"endwhile", "ENDWHILE"},
                {"eval()", "EVAL()"},
                {"exit()", "EXIT()"},
                {"extends", "EXTENDS"},
                {"false", "FALSE"},
                {"final", "FINAL"},
                {"finally", "FINALLY"},
                {"fn", "FUNCTION"},
                {"for", "FOR"},
                {"foreach", "FOREACH"},
                {"function", "FUNCTION"},
                {"global", "GLOBAL"},
                {"goto", "GOTO"},
                {"if", "IF"},
                {"implements", "IMPLEMENTS"},
                {"include", "INCLUDE"},
                {"include_once", "INCLUDE_ONCE"},
                {"instanceof", "INSTANCEOF"},
                {"insteadof", "INSTEADOF"},
                {"interface", "INTERFACE"},
                {"isset()", "ISSET()"},
                {"list()", "LIST()"},
                {"match", "MATCH"},
                {"namespace", "NAMESPACE"},
                {"new", "NEW"},
                {"or", "OR"},
                {"print", "PRINT"},
                {"private", "PRIVATE"},
                {"protected", "PROTECTED"},
                {"public", "PUBLIC"},
                {"readonly", "READONLY"},
                {"require", "REQUIRE"},
                {"require_once", "REQUIRE_ONCE"},
                {"return", "RETURN"},
                {"static", "STATIC"},
                {"switch", "SWITCH"},
                {"throw", "THROW"},
                {"trait", "TRAIT"},
                {"true", "TRUE"},
                {"unset()", "UNSET()"},
                {"use", "USE"},
                {"var", "VAR"},
                {"while", "WHILE"},
                {"xor", "XOR"},
                {"yield", "YIELD"},
                {"yield from", "YIELD FROM"}
            };

        Dictionary<string, string> words2 = new Dictionary<string, string>
            {
                {"{", "START_VOID"},
                {"}", "END_VOID"},

                {"()", "NULL_ARGUMENT"},
                {"(", "OPEN_BRACKET"},
                {")", "CLOSED_BRACKET"},

                {"[", "OPEN_SQUEA_BRACKET"},
                {"]", "CLOSED_SQUEA_BRACKET"},

                {"\"", "DOUBLE_QUOTAT"},
                {"'", "ONCE_QUOTAT"},

                {".", "DOT"},
                {",", "COMMA"},
                {":", "COLON"},
                {";", "END_LINE"},

                {"+", "ARITHMETIC_OPERATION__ADD"},
                {"-", "ARITHMETIC_OPERATION__SUB"},
                {"*", "ARITHMETIC_OPERATION__MULT"},
                {"/", "ARITHMETIC_OPERATION__DIV"},
                {"%", "ARITHMETIC_OPERATION__REM_DIV"},
                {"**", "ARITHMETIC_OPERATION__DEGREE"},

                {"==", "COMPARISON_OPERATION__EQUAL"},
                {"===", "COMPARISON_OPERATION__IDENTICALLY_EQUAL"},
                {"!=", "COMPARISON_OPERATION__INEQUAL"},
                {"<>", "COMPARISON_OPERATION__INEQUAL"},
                {"!==", "COMPARISON_OPERATION__IDENTICALLY_INEQUAL"},
                {"<", "COMPARISON_OPERATION__LESS"},
                {">", "COMPARISON_OPERATION__GREAT"},
                {"<=", "COMPARISON_OPERATION__LESS_EQ"},
                {">=", "COMPARISON_OPERATION__GREAT_EQ"},
                {"<=>", "COMPARISON_OPERATION__SPACESHIP"},

                {"&&", "LOGICAL_OPERATION__AND"},
                {"||", "LOGICAL_OPERATION__OR"},
                {"!", "LOGICAL_OPERATION__NOT"},

                {"+=", "ASSIGNMENT_OPERATION__ADD_ASS"},
                {"-=", "ASSIGNMENT_OPERATION__SUB_ASS"},
                {"*=", "ASSIGNMENT_OPERATION__MUL_ASS"},
                {"/=", "ASSIGNMENT_OPERATION__DIV_ASS"},
                {"%=", "ASSIGNMENT_OPERATION__REM_ASS"},
                {"=", "ASSIGNMENT_OPERATION__SET"},

                {"&=", "BITWISE_ASSIGNMENT_OPERATION__AND"},
                {"|=", "BITWISE_ASSIGNMENT_OPERATION__OR"},
                {"^=", "BITWISE_ASSIGNMENT_OPERATION__XOR"},
                {"<<=", "BITWISE_ASSIGNMENT_OPERATION__SHL"},
                {">>=", "BITWISE_ASSIGNMENT_OPERATION__SHR"},

                {".=", "OTHER_ASSIGNMENT_OPERATION__STRING_CONCAT"},
                {"??=", "OTHER_ASSIGNMENT_OPERATION__NULL_CONCAT"},

                 {"?", "TERNAR_OPERATION"},

                {"&", "BITWISE_OPERATION__AND"},
                {"|", "BITWISE_OPERATION__OR"},
                {"^", "BITWISE_OPERATION__XOR"},
                {"<<", "BITWISE_OPERATION__SHL"},
                {">>", "BITWISE_OPERATION__SHR"},
            };

        Dictionary<string, string> words3 = new Dictionary<string, string>
            {
                {"{", "START_VOID"},
                {"}", "END_VOID"},

                {"(", "OPEN_BRACKET"},
                {")", "CLOSED_BRACKET"},

                {"[", "OPEN_SQUEA_BRACKET"},
                {"]", "CLOSED_SQUEA_BRACKET"},

                {".", "DOT"},
                {",", "COMMA"},
                {";", "END_LINE"},

                {"::", "INSIDE_LINK"},

                {"==", "COMPARISON_OPERATION__EQUAL"},
                {"===", "COMPARISON_OPERATION__IDENTICALLY_EQUAL"},
                {"!=", "COMPARISON_OPERATION__INEQUAL"},
                {"!==", "COMPARISON_OPERATION__IDENTICALLY_INEQUAL"},
                {"<=", "COMPARISON_OPERATION__LESS_EQ"},
                {">=", "COMPARISON_OPERATION__GREAT_EQ"},

                {"&&", "LOGICAL_OPERATION__AND"},
                {"||", "LOGICAL_OPERATION__OR"},

                {"+", "ARITHMETIC_OPERATION__ADD"},
                {"-", "ARITHMETIC_OPERATION__SUB"},
                {"*", "ARITHMETIC_OPERATION__MULT"},
                {"%", "ARITHMETIC_OPERATION__REM_DIV"},

                {".=", "OTHER_ASSIGNMENT_OPERATION__STRING_CONCAT"},
                {"??=", "OTHER_ASSIGNMENT_OPERATION__NULL_CONCAT"},

                {"^", "BITWISE_OPERATION__XOR"},
                {"<<", "BITWISE_OPERATION__SHL"},
                {">>", "BITWISE_OPERATION__SHR"},
            };

        public void OpenInputFiles(string nameInputFile)
        {
            if (nameInputFile == "") nameInputFile = InputNameFile;

            try
            {
                using (StreamReader fs = new StreamReader(nameInputFile))
                {
                    string currentLine = "";
                    while ((currentLine = fs.ReadLine()) != null)
                    {
                        SaveLine.Add(currentLine);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("This file can not be read:");
                Console.WriteLine(e.Message);
            }

            OutOfInputFile();
            improvingFileReadability_v2();
        }

        public List<int> indForQuoat = new List<int>();

        public void improvingFileReadability_v2()
        {
            //proxod po stroke vstavlyaya indexu
            for (int i = 0; i < SaveLine.Count; i++)
            {
                //flag changes
                bool changesMade = true;
                int countWh = 0;

                while (changesMade)
                {
                    changesMade = false;
                    countWh++;

                    foreach (KeyValuePair<string, string> entry in words2)
                    {
                        int indd = -1;
                        //ignore string
                        {
                            bool searchQuoatIsEneble = true;
                            indForQuoat = new List<int>();

                            while (searchQuoatIsEneble)
                            {
                                int firstIndex = -1;
                                int secondIndex = -1;
                                int trithIndex = -1;

                                if (indForQuoat.Count == 0) firstIndex = SaveLine[i].IndexOf("\"");
                                else firstIndex = SaveLine[i].IndexOf("\"", indForQuoat[indForQuoat.Count - 1]);

                                if (firstIndex != -1)
                                {
                                    secondIndex = SaveLine[i].IndexOf("\"", firstIndex + 1);
                                }

                                if (secondIndex != -1)
                                {
                                    trithIndex = SaveLine[i].IndexOf("\"", secondIndex + 1);

                                    indForQuoat.Add(firstIndex);
                                    indForQuoat.Add(secondIndex);
                                }

                                if (trithIndex == -1)
                                {
                                    searchQuoatIsEneble = false;
                                }
                            }
                        }


                        for (int u = 0; u < 1; u++)
                        {
                            string substring = entry.Key;
                            int index = SaveLine[i].IndexOf(substring, indd + 1);

                            if (index != -1)
                            {

                                bool isOk = true;

                                for (int p = 0; p < indForQuoat.Count; p++)
                                {
                                    if ((index >= indForQuoat[p]) && (index <= indForQuoat[p + 1]))
                                    {
                                        isOk = false;
                                    }

                                    p++;
                                }

                                if (isOk == true)
                                {
                                    //poverka pered i posle na space
                                    if ((index == 0 || SaveLine[i][index - 1] != ' ') &&
                                        (index + substring.Length == SaveLine[i].Length || SaveLine[i][index + substring.Length] != ' '))
                                    {
                                        //insert space
                                        SaveLine[i] = SaveLine[i].Insert(index, " ");
                                        SaveLine[i] = SaveLine[i].Insert(index + substring.Length + 1, " ");

                                        changesMade = true;
                                    }
                                    else if ((index != 0) && (SaveLine[i][index - 1] != ' ') &&
                                        (index + substring.Length < SaveLine[i].Length && SaveLine[i][index + substring.Length] == ' ')) // __:: _
                                    {
                                        SaveLine[i] = SaveLine[i].Insert(index, " ");
                                        changesMade = true;
                                    }
                                    else if ((index != 0) && (SaveLine[i][index - 1] == ' ') &&
                                        (index + substring.Length < SaveLine[i].Length && SaveLine[i][index + substring.Length] != ' ')) // _ ::__
                                    {
                                        SaveLine[i] = SaveLine[i].Insert(index + substring.Length, " ");
                                        changesMade = true;
                                    }

                                    int buf = index;
                                    if (indd != index)
                                    {
                                        index = indd;
                                        indd = buf;
                                        u--;
                                    }

                                }
                            }
                        }
                    }
                }

                //print("countWh = " + countWh);
                //print("");
            }
        }

        public void OutOfInputFile()
        {
            print("\nInput File:");

            if (SaveLine.Count != 0)
                SaveLine.ForEach(Console.WriteLine);
            else
                print("Input file cannot be read, or it's empty.");
        }
        public string SearchOfDick(string inp)
        {
            string res = null;
            //find key inp
            words2.TryGetValue(inp, out res);
            return res;
        }

        bool insertToEnd = false; // flag dlya byfera

        // rekursive find in massive
        public string SearchOfDickOnRecurs(string inp)
        {
            string res = null;
            words1.TryGetValue(inp, out res); // find by key in massive
            if (res == null) words2.TryGetValue(inp, out res);

            if (res == null) // Если не нашли
            {
                string newInp = "";


                {
                    //if lexem was find -> byfer, dalee stroka bez etoi lexemu, zapusk nerekursivnogo poiska na kajdom shage (sleva,sprava)
                }

                for (int i = 1; i < inp.Length; i++)
                {
                    newInp = new String(inp.ToCharArray(), 0, i);
                    string searchNewInp = SearchOfDick(newInp);

                    if (searchNewInp != null)
                    {
                        string finalInp = new String(inp.ToCharArray(), i, (inp.Length - i));
                        //print("finalInp = " + finalInp);

                        buferOfLexemsDetectionForDick.Add(newInp);
                        buferOfLexemsDetectionForDick.Add(searchNewInp);

                        insertToEnd = true;
                        SearchOfDickOnRecurs(finalInp);

                        return null;
                    }
                }

                for (int i = 1; i < inp.Length; i++)
                {
                    newInp = new String(inp.ToCharArray(), i, inp.Length - i);
                    string searchNewInp = SearchOfDick(newInp);

                    if (searchNewInp != null)
                    {
                        string finalInp = new String(inp.ToCharArray(), 0, i);

                        buferOfLexemsDetectionForDick.Add(newInp);
                        buferOfLexemsDetectionForDick.Add(searchNewInp);

                        insertToEnd = false;
                        SearchOfDickOnRecurs(finalInp);

                        return null;
                    }
                }
            }
            else // if find lexem in dictionary
            {
                if (buferOfLexemsDetectionForDick.Count != 0)
                {
                    buferOfLexemsDetectionForDick.Add(inp);
                    buferOfLexemsDetectionForDick.Add(res);

                    return res;
                }
                else
                    return res;
            }

            if (buferOfLexemsDetectionForDick.Count != 0)
            {
                int adrInp;
                if (int.TryParse(inp, out adrInp) == true) // if lexem is num
                {
                    tokenDetection(inp);
                    return res;
                }

                if (insertToEnd == false) // if second for -> v na4alo
                {
                    buferOfLexemsDetectionForDick.Insert(0, inp);
                    buferOfLexemsDetectionForDick.Insert(1, "ID");
                }
                else // if first for -> v konec
                {
                    buferOfLexemsDetectionForDick.Add(inp);
                    buferOfLexemsDetectionForDick.Add("ID");
                }
            }
            else
                return res;

            return res;
        }

        public List<string> buferOfLexemsDetectionForDick = new List<string>();
        // Буферный массив для распознанных лексем, которых было больше одной в строке, полученной процедурой SearchOfDickOnRecurs()

        // razdeleie lexem -> token
        public void processingLine()
        {
            for (int i = 0; i < SaveLine.Count; i++)
            {
                if (SaveLine[i] != "")
                {
                    char currentChar = SaveLine[i][0];
                    string bufer = "";

                    bool isString = false;

                    for (int j = 0; j < SaveLine[i].Length; j++)
                    {
                        int index = SaveLine[i].IndexOf("//"); //comment
                        if (index >= 0)
                        {
                            SaveLine[i] = SaveLine[i].Substring(0, index);
                            continue;
                        }

                        if (SaveLine[i][j] == '"')
                        {
                            if (isString == false)
                            {
                                if (bufer.Length > 0)
                                {
                                    tokenDetection(bufer);
                                    bufer = "";
                                }

                                isString = true;
                            }
                            else
                            {
                                isString = false;

                                outpTable.Add(bufer + "\"");
                                outpTable.Add("STRING");
                                //print("123");

                                bufer = "";
                                continue;
                            }
                        }

                        if (isString == false)
                        {
                            if ((SaveLine[i][j] != ' ') && (SaveLine[i][j] != '	'))
                            {
                                bufer += SaveLine[i][j];
                            }
                            else
                            {
                                if (bufer.Length > 0)
                                {
                                    tokenDetection(bufer);
                                    //print(bufer);
                                    bufer = "";
                                }
                            }
                        }
                        else
                        {
                            bufer += SaveLine[i][j];
                        }
                    }
                    if (bufer.Length > 0) tokenDetection(bufer);
                }
            }

            printFinalTable(); //table with lexems
        }

        public void tokenDetection(string inp) //inp - eto lexema ili neskolko lexem
        {

            int adrInp;
            if (int.TryParse(inp, out adrInp) == true) //if num
            {
                // То это число
                int indexOfDott = inp.IndexOf('.');
                if (indexOfDott != -1)
                {
                    string h = inp.Remove(indexOfDott);
                    if (h.IndexOf('.') != -1)
                    {

                        outpTable.Add(inp);
                        outpTable.Add("ERROR"); //. > 1 in line
                    }
                    else
                    {
                        outpTable.Add(inp);
                        outpTable.Add("NUM_FLOAT");
                    }
                }
                else
                {
                    outpTable.Add(inp);
                    outpTable.Add("NUM_INT");
                }
            }
            else // if not num
            {
                string res = SearchOfDickOnRecurs(inp); // Analyse lexem by dictionary

                if ((res == null) && (buferOfLexemsDetectionForDick.Count == 0)) // if lexema odna i ee net v slovare
                {
                    outpTable.Add(inp);
                    outpTable.Add("ID");
                }
                else if ((res != null) && (buferOfLexemsDetectionForDick.Count == 0)) // if lexem in dictionary
                {
                    outpTable.Add(inp);
                    outpTable.Add(res);
                }
                else
                {
                    detectingOfBuferLexems(); // if many lexems in line
                }
            }
        }

        public void detectingOfBuferLexems()
        {
            for (int i = 0; i < buferOfLexemsDetectionForDick.Count; i += 2)
            {
                outpTable.Add(buferOfLexemsDetectionForDick[i]);
                outpTable.Add(buferOfLexemsDetectionForDick[i + 1]);
            }

            buferOfLexemsDetectionForDick.Clear();
        }

        public int maxLengthFexem = 0;

        public void resultSplitter(List<string> lexemesOut, List<string> codeOut)
        {
            for (int i = 0; i < outpTable.Count; i += 2)
                codeOut.Add(outpTable[i]);

            for (int i = 1; i < outpTable.Count; i += 2)
                lexemesOut.Add(outpTable[i]);

            /*for (int i = 1; i < (outpTable.Count)/2; i++)
            {
                Console.Write(codeOut[i] + "  ");
                Console.Write(lexemesOut[i] + "  ");
                Console.WriteLine("");
            }*/


        }


        // Final table
        public void printFinalTable()
        {
            print("\nLexer output: \n");


            maxLengthFexem = 10;

            /*for (int i = 0; i < outpTable.Count; i ++)
                println("  " + outpTable[i]); */

            for (int i = 0; i < outpTable.Count; i += 2)
            {
                println(i / 2);

                if (outpTable.Count < 100)
                {
                    if ((i / 2 < 10)) println(" ");
                    else println("  ");
                }
                else
                {
                    if ((i / 2 < 10)) println("  ");
                    else if ((i / 2 < 100)) println(" ");
                }

                println("  " + outpTable[i]);

                int a_size = maxLengthFexem - outpTable[i].Length;

                if (a_size <= 0) a_size = 1;

                string newSpase = new String(' ', a_size);
                println(newSpase);

                print(" -  " + outpTable[i + 1]);
            }
        }
    }
}

