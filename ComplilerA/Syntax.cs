using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComplilerA
{
    internal class Syntax
    {

        private List<string> lexerRes = Lexer.GetResult();
        private static Dictionary<string, int> variableNameAndType = new Dictionary<string, int>() { };

        private static void HelperCheck(string line, int lineIndex) 
            // this is working. If not, check hard numbers in pattern.
            // They aint matching the word yuo are expecting
        {
            string pattern;
            int typeIndex = 0;
            if (line.Contains("39 ")) //var
            {
                line = line.Remove(0, 3);
                if (line.Length == 0)
                    return;
            }

            if (line.Contains("126")) //array
            {
                pattern = @"(\w+) 10 126 11 (\d+)d 20 (\d+)d 12 32 (\d{3}) 9";
                typeIndex = 4;
                //varaibleName : array [StartIndex .. EndIndex] of SelectedType;
            }
            else
            {
                pattern = @"(\w+) 10 (\d{3}) 9";
                typeIndex = 2;
                //varaibleName : SelectedType;
            }
            if (line.Contains(" 2 "))
            {
                //remove var from equation
                string[] tmp = line.Split(" 2 ");
                string ending = tmp[tmp.Length - 1].Remove(0, tmp[tmp.Length - 1].IndexOf(" 10 "));
                for (int i = 0; i < tmp.Length; i++)
                {
                    if (!tmp[i].Contains(ending))
                        tmp[i] += ending;
                    //Console.WriteLine(Regex.IsMatch(tmp[i], pattern)); //Check for every separate variable declaration
                    Match matchMultiplevariables = Regex.Match(tmp[i], pattern);
                    //Console.WriteLine(matchMultiplevariables.Groups[1].Value + " " + matchMultiplevariables.Groups[typeIndex].Value);
                    if (!variableNameAndType.Keys.Contains(matchMultiplevariables.Groups[1].Value))
                    {
                        variableNameAndType[matchMultiplevariables.Groups[1].Value] = Convert.ToInt32(matchMultiplevariables.Groups[typeIndex].Value);
                    }
                    else
                    {
                        //THIS WILL NOT DIFFERENCIATE BETWEEN LOCAL VARAIBLES AND GLOBAL ONES
                        //generate error. Variable already set
                        Errors.GenerateError(6, lineIndex, 0, $"{matchMultiplevariables.Groups[1].Value}");
                    }
                }
            }
            else
            {
                //Console.WriteLine(Regex.IsMatch(line, pattern)); //Check separate variable declaration
                Match matchNoVar = Regex.Match(line, pattern);
                string numberType = matchNoVar.Groups[typeIndex].Value;
                string newVariable = matchNoVar.Groups[1].Value;
                //Console.WriteLine(matchNoVar.Groups[1].Value + " " + matchNoVar.Groups[typeIndex].Value);
                if (!variableNameAndType.Keys.Contains(matchNoVar.Groups[1].Value))
                {
                    variableNameAndType[matchNoVar.Groups[1].Value] = Convert.ToInt32(matchNoVar.Groups[typeIndex].Value);
                    //If this encounter something NOT from this block it crashes. USE TRY HERE
                }
                else
                {
                    // THIS WILL NOT DIFFERENCIATE BETWEEN LOCAL VARAIBLES AND GLOBAL ONES
                    //generate error. Variable already set
                    Errors.GenerateError(6, lineIndex, 0, $"{matchNoVar.Groups[1].Value}");
                }
            }
        }

        private static string BasicCalculation(string line)
        {
            string[] parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            int typeN1 = Convert.ToInt32(parts[0]);
            int sighOper = Convert.ToInt32(parts[1]);
            int typeN2 = Convert.ToInt32(parts[2]);
            int[] compareOperators =
            {
                Lexer.lesserSymbol, Lexer.lesserEqualSymbol, Lexer.equalSymbol,
                Lexer.greaterEqualSymbol, Lexer.greaterSymbol, Lexer.nonEqualSymbol
            };
            int[] simpleOperators = {Lexer.slashSymbol, Lexer.starSymbol, Lexer.minusSymbol, Lexer.plusSymbol };
            //boolean operations
            if (compareOperators.Contains(sighOper) && typeN1 == typeN2)
            {
                return Lexer.booleanKeyword.ToString();
            }
            else if (typeN1==typeN2 && (sighOper==Lexer.orKeyword || sighOper == Lexer.xorKeyword || sighOper == Lexer.notKeyword))
            {
                return Lexer.booleanKeyword.ToString();
            }
            //Arithmetic operations
            else if ((Lexer.shortintKeyword <= typeN1 && typeN1 <= Lexer.decimalKeyword) &&
                (Lexer.shortintKeyword <= typeN2 && typeN2 <= Lexer.decimalKeyword) && simpleOperators.Contains(sighOper))
            {
                if (sighOper==Lexer.slashSymbol)
                    return Lexer.doubleKeyword.ToString(); // a/b is always real
                if (typeN1 >= typeN2) // operations like a+b, a*b, a-b
                    return typeN1.ToString();
                return typeN2.ToString();
            }
            //mod and div operations
            else if ((Lexer.shortintKeyword <= typeN1 && typeN1 <= Lexer.uint64Keyword) &&
                (Lexer.shortintKeyword <= typeN2 && typeN2 <= Lexer.uint64Keyword) && 
                (sighOper == Lexer.divKeyword || sighOper == Lexer.modKeyword))
            {
                return Lexer.integerKeyword.ToString();
            }
            // THis complicated expression is string operations
            else if (((sighOper == Lexer.plusSymbol && typeN1 == typeN2 && typeN1 == Lexer.stringKeyword) || 
                (sighOper == Lexer.starSymbol && Lexer.shortintKeyword <= typeN2 && typeN2 <= Lexer.decimalKeyword && typeN1 == Lexer.stringKeyword) ||
                sighOper == Lexer.starSymbol && Lexer.shortintKeyword <= typeN1 && typeN1 <= Lexer.decimalKeyword && typeN2 == Lexer.stringKeyword))
            {
                return Lexer.stringKeyword.ToString();
            }
            else
            {
                Errors.GenerateError(8, 0, 0, $"Invalid expression {line}");
                return "-1";
            }            
        }
        private bool CheckEquation(int expectedRes, string line) //TODO
        {
            string newLine = "";
            string[] split = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return true;
        }
        private void CheckVariableDeclaration() // this is not handling declarations of constants
        {
            //As I understand, variables can be declared anywhere BEFORE main programm block. Those places include functions/procedures
            //So my thought proces is. Variables can be declared anywhere when we are not in a block (begin/end)
            bool allowDeclaration = false;
            int amountOfBlocks = 0;
            foreach (var line in lexerRes)
            {
                if (line.Contains(Lexer.beginKeyword.ToString()))
                {
                    allowDeclaration = false;
                    amountOfBlocks++;
                    continue;
                }
                else if (line.Contains(Lexer.endKeyword.ToString()))
                {
                    amountOfBlocks--;
                    continue;
                }
                if ((line.Contains(Lexer.varKeyword.ToString()) || allowDeclaration) && amountOfBlocks==0)
                {
                    allowDeclaration = true;
                    HelperCheck(line, lexerRes.IndexOf(line));
                }
                else
                    continue;
            }
        }

        private void CheckVariablesOperations()
        {
            bool allowOperations = false;
            int amountOfBrackets = 0;
            string[] operators = new string[18] 
            {
                Lexer.plusSymbol.ToString(), Lexer.minusSymbol.ToString(), Lexer.starSymbol.ToString(), Lexer.slashSymbol.ToString(),
                Lexer.arrowSymbol.ToString(),  Lexer.divKeyword.ToString(), Lexer.modKeyword.ToString(), Lexer.andKeyword.ToString(), 
                Lexer.orKeyword.ToString(), Lexer.xorKeyword.ToString(),Lexer.lesserSymbol.ToString(), Lexer.lesserEqualSymbol.ToString(),
                Lexer.equalSymbol.ToString(), Lexer.greaterEqualSymbol.ToString(), Lexer.greaterSymbol.ToString(), 
                Lexer.nonEqualSymbol.ToString(), Lexer.shlKeyword.ToString(), Lexer.shrKeyword.ToString()
            };
            foreach (var line in lexerRes)
            {
                if (amountOfBrackets>0)
                {
                    allowOperations = true;
                }
                else
                {
                    allowOperations = false;
                }
                if (line.Contains(Lexer.beginKeyword.ToString())) //only begin counts here. Rework later.
                {
                    amountOfBrackets++; 
                    continue; //?
                }
                else if (line.Contains(Lexer.endKeyword.ToString()))
                {
                    amountOfBrackets--;
                    continue;
                }
                if (allowOperations)
                {
                    //USE REGEX HERE. "(\w+) assign (\.?*)"
                    Match match = Regex.Match(line, @"(\w+) (\d{2}) (\*?)");
                    if (match.Groups[2].Value != "21")
                    {
                        Errors.GenerateError(7, lexerRes.IndexOf(line), 0); //invalid operations  (without assigning)
                        return;
                    }
                    else //TODO
                    {
                        int expectedResult = variableNameAndType[match.Groups[1].Value];
                        if (CheckEquation(expectedResult, match.Groups[3].Value) == false)
                        {
                            /*
                             * Нужна элементарная функция, в которой будет рассматриваться возможность выполнения операции для данной тройки (переменная/оператор/переменная)
                             * Эта функция будет вызываться сначала для скобок (если таковые присутствуют). Потом для операций с высоким приоритетом. 
                             * Операции сложения/вычитания тип не меняют. А т.к. я тут не считаю то мне похуй на переполнения/выход за границу.
                             */
                            Errors.GenerateError(8, lexerRes.IndexOf(line), 0); //Types doesnt match in equation
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }

        public Syntax() 
        {
            CheckVariableDeclaration();
            CheckVariablesOperations();
            if (Errors.GetErrorList().Count > 0)
            {
                Errors.ShowErrorList();
            }

            //for variable operations will be required recursion (for every two separate variables) resulted type goes further until the end.
        }
    }
}
