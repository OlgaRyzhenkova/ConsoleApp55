using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Block2
{
    public class Block
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            DateTime checkDate;
            var students = ReadStudentsFromFile("input.txt", out checkDate);

            int count = 0;
            Console.WriteLine($"Студенти молодші за 17 років на {checkDate:dd.MM.yyyy}:\n");

            for (int i = 0; i < students.Count; i++)
            {
                var s = students[i];
                int age = CalculateAge(s.birthDate, checkDate);

                if (age < 17)
                {
                    count++;
                    Console.WriteLine($"Прізвище: {s.lastName}");
                    Console.WriteLine($"Ім'я: {s.firstName}");
                    Console.WriteLine($"По батькові: {s.patronymic}");
                    Console.WriteLine($"Стать: {s.gender}");
                    Console.WriteLine($"Дата народження: {s.birthDate:dd.MM.yyyy}");
                    Console.WriteLine($"Оцінка з математики: {(s.math.HasValue ? s.math.ToString() : "Н")}");
                    Console.WriteLine($"Оцінка з фізики: {(s.physics.HasValue ? s.physics.ToString() : "Н")}");
                    Console.WriteLine($"Оцінка з інформатики: {(s.informatic.HasValue ? s.informatic.ToString() : "Н")}");
                    Console.WriteLine($"Стипендія: {s.scholarship}");
                    Console.WriteLine(new string('-', 40));
                }
            }

            Console.WriteLine($"\nЗагалом студентів молодших за 17: {count}");
        }

        static List<(string lastName, string firstName, string patronymic, char gender, DateTime birthDate, int? math, int? physics, int? informatic, int scholarship)>
            ReadStudentsFromFile(string filePath, out DateTime checkDate)
        {
            var list = new List<(string, string, string, char, DateTime, int?, int?, int?, int)>();
            checkDate = DateTime.MinValue;

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
            {
                Console.WriteLine("!!! Файл порожній.");
                return list;
            }

            if (!DateTime.TryParseExact(lines[0], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out checkDate))
            {
                Console.WriteLine("!!! Некоректна дата у першому рядку: " + lines[0]);
                return list;
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 9)
                {
                    Console.WriteLine("!!! Неправильна кількість полів у рядку: " + line);
                    continue;
                }

                bool isValid = true;

                string lastName = parts[0];
                string firstName = parts[1];
                string patronymic = parts[2];

                char gender = ' ';
                if (parts[3].Length == 1)
                    gender = parts[3][0];
                else
                {
                    Console.WriteLine("!!! Некоректне поле стать: " + parts[3]);
                    isValid = false;
                }

                DateTime birthDate = new DateTime();
                if (!DateTime.TryParseExact(parts[4], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate))
                {
                    Console.WriteLine("!!! Некоректна дата: " + parts[4]);
                    isValid = false;
                }

                int? math = ParseGrade(parts[5]);
                int? physics = ParseGrade(parts[6]);
                int? informatic = ParseGrade(parts[7]);

                int scholarship = 0;
                bool parsed = int.TryParse(parts[8], out scholarship);
                if (!parsed || (scholarship != 0 && (scholarship < 1167 || scholarship > 4321)))
                {
                    Console.WriteLine("!!! Некоректна стипендія: " + parts[8]);
                    isValid = false;
                }

                if (isValid)
                {
                    list.Add((lastName, firstName, patronymic, gender, birthDate, math, physics, informatic, scholarship));
                }
            }

            return list;
        }

        static int? ParseGrade(string grade)
        {
            if (grade == "-")
                return null;

            if (int.TryParse(grade, out int result))
                return result;
            else
                return null;
        }

        static int CalculateAge(DateTime birthDate, DateTime checkDate)
        {
            int age = checkDate.Year - birthDate.Year;
            if (birthDate > checkDate.AddYears(-age))
                age--;
            return age;
        }
    }
}
