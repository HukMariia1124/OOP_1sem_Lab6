    using System;
    using System.IO;
    using System.Collections.Generic;

    namespace OOP_1sem_Lab6
{
    public static class ExceptionsGenerateTestFiles
    {
        public static void GenerateTestFiles()
        {
            Random rnd = new Random();

            var errorTypes = new List<string> { "NoFile", "Overflow", "FormatText", "FormatDouble", "Empty", "SingleLine", "None" };

            for (int i = 10; i <= 29; i++)
            {
                string fileName = $"{i}.txt";

                if (File.Exists(fileName)) File.Delete(fileName);

                try
                {

                    string errorType = errorTypes[rnd.Next(errorTypes.Count)];

                    if (errorType == "NoFile")
                    {
                        Console.WriteLine($"[GEN] {fileName}: \tWill be skipped (simulating missing file)");
                        continue;
                    }

                    using (StreamWriter sw = new StreamWriter(fileName))
                    {
                        switch (errorType)
                        {
                            case "Overflow":
                                sw.WriteLine("2000000000");
                                sw.WriteLine("2");
                                Console.WriteLine($"[GEN] {fileName}: \tData for OverflowException");
                                break;
                            case "FormatText":
                                sw.WriteLine("Ten");
                                sw.WriteLine("5");
                                Console.WriteLine($"[GEN] {fileName}: \tData for FormatException (text)");
                                break;
                            case "FormatDouble":
                                sw.WriteLine("12.5");
                                sw.WriteLine("2");
                                Console.WriteLine($"[GEN] {fileName}: \tData for FormatException (fractional)");
                                break;
                            case "Empty":
                                Console.WriteLine($"[GEN] {fileName}: \tData for IndexOutOfRangeException (empty)");
                                break;
                            case "SingleLine":
                                sw.WriteLine("50");
                                Console.WriteLine($"[GEN] {fileName}: \tData for IndexOutOfRangeException (1 line)");
                                break;
                            default:
                                int n1 = rnd.Next(1, 1000);
                                int n2 = rnd.Next(1, 1000);
                                sw.WriteLine(n1);
                                sw.WriteLine(n2);
                                Console.WriteLine($"[GEN] {fileName}: \tValid data ({n1} * {n2})");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating file {fileName}: {ex.Message}");
                }
            }
            Console.WriteLine("--------------------------------------------------");
        }
    }
    }