using System.Text.RegularExpressions;

namespace ComplilerA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string filename1 = "Declarations of variables.txt";
            Lexer lexer = new Lexer(filename1);
            Syntax syntax = new Syntax();
        }
    }
}