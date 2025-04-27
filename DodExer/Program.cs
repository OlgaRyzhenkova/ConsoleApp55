using System;
using System.IO;

namespace DodExer
{
    public class Exercise
    {
        public static void Main()
        {
            string inputFile = "input.txt";
            if (!File.Exists(inputFile))
            {
                Console.WriteLine("Файл не знайдено!");
                return;
            }

            string[] lines = File.ReadAllLines(inputFile);

            int boysCount = 0;
            int boysSum = 0;
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 8)
                    continue;

                string gender = parts[3];
                int mark1 = ParseMark(parts[5]);
                int mark2 = ParseMark(parts[6]);
                int mark3 = ParseMark(parts[7]);

                int average = (mark1 + mark2 + mark3) / 3;

                if (gender == "Ч")
                {
                    boysCount++;
                    boysSum += average;
                }
            }

            if (boysCount == 0)
            {
                Console.WriteLine("Немає студентів чоловічої статі для обчислення середнього балу.");
                return;
            }

            double boysAverage = (double)boysSum / boysCount;
            Console.WriteLine($"Середній бал студентів чоловічої статі: {boysAverage:F2}");

            bool foundGirls = false;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 8)
                    continue;

                string gender = parts[3];
                int mark1 = ParseMark(parts[5]);
                int mark2 = ParseMark(parts[6]);
                int mark3 = ParseMark(parts[7]);

                int average = (mark1 + mark2 + mark3) / 3;

                if (gender == "Ж" && average > boysAverage)
                {
                    string surname = parts[0];
                    string name = parts[1];
                    string patronymic = parts[2];

                    Console.WriteLine($"{surname} {name} {patronymic} - середній бал: {average}");
                    foundGirls = true;
                }
            }

            if (!foundGirls)
            {
                Console.WriteLine("Немає студенток, середній бал яких перевищує середній бал студентів чоловічої статі.");
            }
        }

        static int ParseMark(string mark)
        {
            return mark == "-" ? 0 : int.Parse(mark);
        }
    }
}
