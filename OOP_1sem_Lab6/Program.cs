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
                        ExceptionsTask2.Task2ReadFiles();
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
