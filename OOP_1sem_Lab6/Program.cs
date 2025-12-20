using System.Diagnostics;
using System.Text;

namespace OOP_1sem_Lab6
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = UTF32Encoding.UTF8;
            string menu;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(
                """
                Choose one of the following options:
                1 First Task (work with .txt files).
                2 Second Task (work with images).
                0 Exit.
                """);
                Console.ResetColor();

                menu = Console.ReadLine()!;
                switch (menu)
                {
                    case "1":
                        ExceptionsTask1.Task1GenerateMenu();
                        break;
                    case "2":
                        string solutionDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\.."));
                        string task2Directory = Path.Combine(solutionDir, "WpfApp1", "bin", "Release", "net8.0-windows", "WpfApp1.exe");
                        Process.Start(task2Directory);
                        break;
                    case "0":
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid Input. Please try again.");
                        break;
                }
                if (menu != "0")
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (menu != "0");
        }
    }
}
