using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplilerA
{
    internal class Errors
    {
        static List<string> listErrors = new List<string>();

        public static List<string> GetErrorList() 
        {
            return listErrors; 
        }
        public static void ShowErrorList()
        {
            foreach (var item in Errors.GetErrorList())
            {
                Console.WriteLine(item);
            }
        }

        public static void GenerateError(int identificator, int lineIndex, int charIndex, string comment = "")
        {
            switch (identificator)
            {
                case 0: // long comment error
                    listErrors.Add($"Error in line {lineIndex} at character {charIndex}. Expected *).");
                    break;
                case 1:
                    listErrors.Add($"Error in line {lineIndex} at character {charIndex}. Got *). Comment is not open");
                    break;
                case 2:
                    listErrors.Add($"Error in line {lineIndex} at character {charIndex}. Got *). Comment is not closed");
                    break;
                case 3: // Short comment error
                    listErrors.Add($"Error in line {lineIndex} at character {charIndex}. Got {"{"}. Comment is not closed");
                    break;
                case 4:
                    listErrors.Add($"Error in line {lineIndex} at character {charIndex}. Got {"}"}. Comment is not open");
                    break;
                //Lexer errors ends here
                case 5:
                    listErrors.Add($"Error in line {lineIndex} at character {charIndex}. Invalid variable name");
                    break;
                case 6:
                    listErrors.Add($"Error in line {lineIndex}. Variable {comment} is already set");
                    break;
                case 7:
                    listErrors.Add($"Error in line {lineIndex}. Invalid operations");
                    break;

            }
        }
    }
}
