using System;
using System.Collections.Generic;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Exceptions
{
    public static class ExceptionsTask1
    {
        public static void Task1GenerateMenu()
        {
            Console.WriteLine("Бажаєте згенерувати тестові файли? (y/n)");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                //Generate method
            }
            Console.WriteLine("\nПочинаємо обробку файлів...\n");
            Task1();
        }
        private static void Task1()
        {
        }
    }
}