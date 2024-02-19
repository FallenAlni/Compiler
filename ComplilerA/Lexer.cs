using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplilerA
{
    internal class Lexer
    {
        //Constants below
        #region 
        public const int
            pointSymbol = 1,
            komaSymbol = 2,
            slashSymbol = 3,
            starSymbol = 4,
            plusSymbol = 5,
            minusSymbol = 6,
            equalSymbol = 7, // =
            arrowSymbol = 8, // ^
            semicolonSymbol = 9, // ,
            colonSymbol = 10, // : 
            lBraketSymbol = 11, // [
            rBraketSymbol = 12, // ]
            lParSymbol = 13, // (
            rParSymbol = 14, // )
            lesserSymbol = 15, // <
            greaterSymbol = 16, // >
            lesserEqualSymbol = 17, // <=
            greaterEqualSymbol = 18, //>=
            nonEqualSymbol = 19, // <>
            twoPointsSymbol = 20, // ..
            assignSymbol = 21, // :=
            bottomLineSymbol = 22, // _
            apostropheSYmbol = 23, // `

            doKeyword = 29,
            ifKeyword = 30,
            inKeyword = 31,
            ofKeyword = 32,
            toKeyword = 33,
            orKeyword = 34,
            asKeyword = 35,
            isKeyword = 36,

            endKeyword = 37,
            tryKeyword = 38,
            varKeyword = 39,
            divKeyword = 40,
            andKeyword = 41,
            notKeyword = 42,
            forKeyword = 43,
            modKeyword = 44,
            nilKeyword = 45,
            setKeyword = 46,
            shlKeyword = 47,
            xorKeyword = 48,
            shrKeyword = 49,

            thenKeyword = 51,
            elseKeyword = 52,
            caseKeyword = 53,
            fileKeyword = 54,
            gotoKeyword = 55,
            typeKeyword = 56,
            withKeyword = 57,
            autoKeyword = 58,
            lockKeyword = 59,
            loopKeyword = 60,
            usesKeyword = 61,

            beginKeyword = 62,
            whileKeyword = 63,
            constKeyword = 64,
            labelKeyword = 65,
            untilKeyword = 66,
            writeKeyword = 67,
            classKeyword = 68,
            eventKeyword = 69,
            raiseKeyword = 70,
            usingKeyword = 71,

            downtoKeyword = 72,
            exceptKeyword = 73,
            recordKeyword = 74, // type
            repeatKeyword = 75,
            sealedKeyword = 76,
            typeofKeyword = 77,
            randomKeyword = 78,
            packedKeyword = 79,

            programKeyword = 80,
            finallyKeyword = 81,
            foreachKeyword = 82,

            functionKeyword = 83,
            operatorKeyword = 84,
            propertyKeyword = 85,
            sequenceKeyword = 86,
            templateKeyword = 87,

            procedureKeyword = 88,
            randomizeKeyword = 89,
            interfaceKeyword = 90,

        //integer
            shortintKeyword = 100,
            smallintKeyword = 101,
            integerKeyword = 102,
            longintKeyword = 103,
            int64Keyword = 104,
            byteKeyword = 105,
            wordKeyword = 106,
            longwordKeyword = 107,
            cardinalKeyword = 108,
            uint64Keyword = 109,
        //real
            realKeyword = 110,
            doubleKeyword = 111,
            singleKeyword = 112,
            decimalKeyword = 113,
        //boolean
            booleanKeyword = 114,
        //char
           charKeyword = 125,
        //array
            arrayKeyword = 126,
        //string
            stringKeyword = 127;

        #endregion

        public readonly Dictionary<string, int> symbols = new Dictionary<string, int>()
        {
            {";", semicolonSymbol}, {":=", assignSymbol}, {"*", starSymbol}, {"+", plusSymbol},
            {"_", bottomLineSymbol}, {"-", minusSymbol}, {"/", slashSymbol}, {",", komaSymbol},
            {"..", twoPointsSymbol}, {".",pointSymbol}, {"^" , arrowSymbol}, {"<", lesserSymbol},
            {"<=", lesserEqualSymbol}, {">", greaterSymbol}, {">=", greaterEqualSymbol},
            {":", colonSymbol}, {"=", equalSymbol}, {"'", apostropheSYmbol},
            {"[", lBraketSymbol}, {"]", rBraketSymbol}, {"(", lParSymbol}, {")", rParSymbol},
        };

        private List<string> variableNames = new List<string>();
        private static List<string> newResult = new List<string>();


        public static List<string> GetResult()
        {
            return newResult;
        }

        public void RemoveSimpleComments(string[] lines) //this is working correctly
        {
            bool commentStarted = false;
            for (int i=0; i<lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '{' && !commentStarted)
                    {
                        commentStarted = true;
                    }
                    else if (lines[i][j] == '}' && !commentStarted)
                    {
                        Errors.GenerateError(4, i, j);
                    }
                    if (commentStarted)
                    {
                        int ind = lines[i].IndexOf('{');
                        int end = lines[i].IndexOf('}', lines[i].IndexOf('{'));
                        if (end == -1)
                        {
                            Errors.GenerateError(3, i, lines[i].Length);
                            break;
                        }
                        lines[i] = lines[i].Remove(ind, end - ind + 1);
                        break;
                    }
                }
                commentStarted = false;
            }
        }

        public void RemoveHardComments(string[] lines) //this is also working
        {
            bool commentStarted = false;
            bool openComment = false;
            int startPos=0;
            int line = 0;
            for (int i=0; i<lines.Length; i++)
            {

                if (lines[i].Contains("(*") && !commentStarted)
                {
                    openComment = true;
                    commentStarted = true;
                    startPos = lines[i].IndexOf("(*");
                    line = i;
                }
                else if (lines[i].Contains("*)") && !commentStarted)
                {
                    Errors.GenerateError(1, i, lines[i].IndexOf("*)"));
                    break;
                }
                if (commentStarted)
                {
                    if (lines[i].IndexOf("*)", startPos) == -1)
                    {
                        lines[i] = lines[i].Remove(startPos, lines[i].Length-startPos);
                        startPos = 0;
                    }
                    else
                    {
                        lines[i] = lines[i].Remove(startPos, lines[i].IndexOf("*)")+2);
                        commentStarted = false;
                        openComment = false;
                    }
                }
            }
            if (openComment)
            {
                Errors.GenerateError(0, line, startPos);
            }
        }
        public void ShowNewResult()
        {
            Console.WriteLine("\nNew Result\n");
            foreach (var line in newResult)
            {
                Console.WriteLine(line);
            }
        }

        public Lexer(string filename) //this is actually working
        {
            string[] lines = File.ReadAllLines(filename);
            bool isDigit = false;
            bool isVariable = false;
            string currentString = "";

            Console.WriteLine("Initial code: \n");

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
            Keyword kword = new Keyword();

            RemoveSimpleComments(lines);
            RemoveHardComments(lines);

            if (Errors.GetErrorList().Count == 0)
            {
                Console.WriteLine("\nResult of Lexer\n");
                foreach (string line in lines)
                {
                    currentString = "";
                    string tmpStringResult = "";

                    for (int j = 0; j < line.Length; j++)
                    {                        
                        if ((!isVariable && Char.IsDigit(line[j])) || (j < line.Length - 1 && line[j] == '.' && Char.IsDigit(line[j + 1])))
                        {
                            isDigit = true;
                            currentString += line[j];
                            //HERE WILL BE ONLY NUMBERS: EITHER 8 or 9.1 ...
                            continue;
                        }

                        else if ((!isDigit && Char.IsLetterOrDigit(line[j])) || line[j] == '_') //check letterOrDigit later
                        {
                            currentString += line[j];
                            isVariable = true;
                            //HERE WILL BE ANY POSSIBLE WORDS ( they dont have symbols and 1st letter IS NOT A DIGIT)
                            //If word is ending at the end of the line it will not be saved (Example is const/end/begin). Fix for it
                            if (currentString==line)
                            {
                                isVariable = false;
                                tmpStringResult += kword.Keywords()[currentString.Length][currentString].ToString() + " ";;
                                currentString = "";
                            }
                            else
                                continue;
                        }

                        else
                        {
                            //For some god forsaken reasons tmpResult.Add(" "); after this block gave double emptyspaces so it works now  
                            if (isDigit)
                            {
                                currentString += "d"; //helps identify if this is a digit or postlexer code
                                tmpStringResult += currentString + " ";;
                                currentString = "";
                                isDigit = false;
                            }

                            else if (isVariable)
                            {
                                isVariable = false;

                                if (kword.Keywords()[currentString.Length].ContainsKey(currentString))
                                {
                                    tmpStringResult += kword.Keywords()[currentString.Length][currentString].ToString() + " ";
                                    currentString = "";
                                }

                                else
                                {
                                    if (line[j].ToString() != "'"  && !variableNames.Contains(currentString))
                                        variableNames.Add(currentString);
                                    tmpStringResult += currentString + " ";
                                    currentString = "";
                                }
                            }
                            if (line[j] == ' ')
                                continue;
                            if (j < line.Length - 1 && symbols.ContainsKey(line[j].ToString() + line[j + 1].ToString()))
                            {
                                tmpStringResult += symbols[line[j].ToString() + line[j + 1].ToString()].ToString() + " ";
                                j++;
                            }

                            else
                            {
                                var k = line[j].ToString();
                                if (symbols.ContainsKey(line[j].ToString()))
                                {
                                    tmpStringResult += symbols[line[j].ToString()].ToString() + " ";
                                }
                                else
                                {
                                    Errors.GenerateError(5, Array.IndexOf(lines, line)+1, line.IndexOf(line[j]));
                                }
                            }
                        }
                    }

                    if (tmpStringResult.Length == 0 || (tmpStringResult.Length == 1 && tmpStringResult[0] == ' ')) // this will remove empty lines (where comments was)
                        continue;
                    newResult.Add(tmpStringResult);
                }
                ShowNewResult();
            }
            if (Errors.GetErrorList().Count > 0)
            {
                Errors.ShowErrorList();
            }
        }
    }
}
