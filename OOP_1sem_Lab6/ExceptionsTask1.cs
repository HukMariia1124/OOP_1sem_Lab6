using System;
using System.Collections.Generic;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OOP_1sem_Lab6
{
    public static class ExceptionsTask1
    {
        public static void Task1GenerateMenu()
        {
            string key;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Do you want to generate test files? (y/n)");
                Console.ResetColor();
                key = Console.ReadLine()!;
                if (key == "y")
                {
                    ExceptionsGenerateTestFiles.GenerateTestFiles();
                    break;
                }
                if (key != "n")
                {
                    Console.WriteLine("Invalid Input.\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
            while (key != "n");
            Console.WriteLine("\nStarting file processing...\n");
            Task1();
        }
        private static void Task1()
        {
            StreamWriter writerNoFile;
            StreamWriter writerBadData;
            StreamWriter writerOverflow;

            try
            {
                writerNoFile = new StreamWriter("no_file.txt", false);
                writerBadData = new StreamWriter("bad_data.txt", false);
                writerOverflow = new StreamWriter("overflow.txt", false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Critical error: Unable to create report files.");
                Console.WriteLine(ex.Message);
                return;
            }

            long validSum = 0;
            int validCount = 0;

            for (int i = 10; i <= 29; i++)
            {
                string filename = $"{i}.txt";

                try
                {
                    string[] lines = File.ReadAllLines(filename);

                    checked
                    {
                        int num1 = int.Parse(lines[0]);
                        int num2 = int.Parse(lines[1]);

                        int multiplication = num1 * num2;

                        validSum += multiplication;
                        validCount++;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{filename}: Success! {num1} * {num2} = {multiplication}");
                        Console.ResetColor();
                    }
                }
                catch (FileNotFoundException)
                {
                    Error(filename);
                    writerNoFile.WriteLine($"{filename} \t [Time: {DateTime.Now.ToLongTimeString()}]");
                }
                catch (IndexOutOfRangeException)
                {
                    Error(filename);
                    writerBadData.WriteLine($"{filename} \t [Time: {DateTime.Now.ToLongTimeString()}]");
                }
                catch (FormatException)
                {
                    Error(filename);
                    writerBadData.WriteLine($"{filename} \t [Time: {DateTime.Now.ToLongTimeString()}]");
                }
                catch (OverflowException)
                {
                    Error(filename);
                    writerOverflow.WriteLine($"{filename} \t [Time: {DateTime.Now.ToLongTimeString()}]");
                }
                catch (Exception)
                {
                    Error(filename);
                    writerBadData.WriteLine($"{filename} \t [Time: {DateTime.Now.ToLongTimeString()}]");
                }
            }
            static void Error(string filename)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{filename}: Error! \t [Time: {DateTime.Now.ToLongTimeString()}]");
                Console.ResetColor();
            }

            try
            {
                writerNoFile.Close();
                writerBadData.Close();
                writerOverflow.Close();
            }
            catch
            {
            }

            try
            {
                double average = (double)validSum / validCount;

                Console.WriteLine("\n--- Result ---");
                Console.WriteLine($"Number of valid multiplications: {validCount}");
                Console.WriteLine($"Arithmetic mean: {average}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to calculate average (possibly no valid data).");
            }

            Console.WriteLine("Work completed. Check the .txt files.");
        }
    }
}