using System;
using System.Collections.Generic;
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

            var students = ReadStudentsFromFile(inputFile);
            double boysAverage = CalculateBoysAverage(students);

            if (boysAverage == -1)
            {
                Console.WriteLine("Немає студентів чоловічої статі для обчислення середнього балу.");
                return;
            }

            Console.WriteLine($"Середній бал студентів чоловічої статі: {boysAverage:F2}");
            PrintGirlsAboveAverage(students, boysAverage);
        }

        static List<(string surname, string name, string patronymic, string gender, string birthdate, int mark1, int mark2, int mark3, int scholarship)> ReadStudentsFromFile(string filePath)
        {
            var students = new List<(string, string, string, string, string, int, int, int, int)>();
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 9)
                {
                    PrintError($"Рядок {i + 1}: недостатньо даних (очікується 9 полів)");
                    continue;
                }

                string surname = parts[0];
                string name = parts[1];
                string patronymic = parts[2];
                string gender = parts[3];
                string birthdate = parts[4];

                if (gender != "Ч" && gender != "Ж")
                {
                    PrintError($"Рядок {i + 1}: некоректне значення статі: '{gender}'");
                    continue;
                }

                bool ok1 = TryParseMark(parts[5], out int mark1);
                bool ok2 = TryParseMark(parts[6], out int mark2);
                bool ok3 = TryParseMark(parts[7], out int mark3);
                bool okScholar = int.TryParse(parts[8], out int scholarship);

                if (!ok1 || !ok2 || !ok3)
                {
                    PrintError($"Рядок {i + 1}: помилка у записі оцінок (дозволено числа або '-')");
                    continue;
                }

                if (!okScholar)
                {
                    PrintError($"Рядок {i + 1}: помилка в полі стипендії");
                    continue;
                }

                students.Add((surname, name, patronymic, gender, birthdate, mark1, mark2, mark3, scholarship));
            }

            return students;
        }

        static double CalculateBoysAverage(List<(string surname, string name, string patronymic, string gender, string birthdate, int mark1, int mark2, int mark3, int scholarship)> students)
        {
            int sum = 0;
            int count = 0;

            foreach (var student in students)
            {
                if (student.gender == "Ч")
                {
                    int average = (student.mark1 + student.mark2 + student.mark3) / 3;
                    sum += average;
                    count++;
                }
            }

            if (count == 0)
                return -1;

            return (double)sum / count;
        }

        static void PrintGirlsAboveAverage(List<(string surname, string name, string patronymic, string gender, string birthdate, int mark1, int mark2, int mark3, int scholarship)> students, double boysAverage)
        {
            bool found = false;

            foreach (var student in students)
            {
                if (student.gender == "Ж")
                {
                    int average = (student.mark1 + student.mark2 + student.mark3) / 3;
                    if (average > boysAverage)
                    {
                        Console.WriteLine($"{student.surname} {student.name} {student.patronymic} - середній бал: {average}");
                        found = true;
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("Немає студенток, середній бал яких перевищує середній бал студентів чоловічої статі.");
            }
        }

        static bool TryParseMark(string input, out int mark)
        {
            if (input == "-")
            {
                mark = 0;
                return true;
            }

            return int.TryParse(input, out mark);
        }

        static void PrintError(string message)
        {
            Console.WriteLine(message);
        }
    }
}
