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
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            DateTime checkDate;
            List<(string LastName, string firstName, string patronymic, char gender, DateTime birthDate, int? math, int? physics, int? informatic, int scholaship)> students = ReadStudentFromFile("input.txt", out checkDate);
            int count = 0;
            Console.WriteLine($"Студенти молодші за 17 років на дату {checkDate:dd.MM.yyyy}:");
            foreach (var student in students)
            {
                int age = CalculateAge(student.birthDate, checkDate);
                if (age < 17)
                {
                    count++;
                    Console.WriteLine($"Прізвище: {student.LastName}");
                    Console.WriteLine($"Ім'я: {student.firstName}");
                    Console.WriteLine($"По батькові: {student.patronymic}");
                    Console.WriteLine($"Стать: {student.gender}");
                    Console.WriteLine($"День народження: {student.birthDate:dd.MM.yyyy}");
                    Console.WriteLine($"Оцінка з математики: {(student.math.HasValue ? student.math.Value.ToString() : "Н")}");
                    Console.WriteLine($"Оцінка з фізики: {(student.physics.HasValue ? student.physics.Value.ToString() : "Н")}");
                    Console.WriteLine($"Оцінка з інформатики: {(student.informatic.HasValue ? student.informatic.Value.ToString() : "Н")}");
                    Console.WriteLine($"Стипендія: {student.scholaship}");
                    Console.WriteLine("------------------------------------");
                }
            }
            Console.WriteLine($"Кількість студентів молодших за 17 років: {count}");
        }
        static List<(string, string, string, char, DateTime, int?, int?, int?, int)> ReadStudentFromFile(string filePath, out DateTime checkDate)
        {
            var students = new List<(string, string, string, char, DateTime, int?, int?, int?, int)>();
            var lines = File.ReadAllLines(filePath);
            checkDate = DateTime.MinValue;
            try
            {
                checkDate = DateTime.ParseExact(lines[0], "dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
                Console.WriteLine("!!! Помилка у першому рядку: неправильний формат дати (очікується dd.MM.yyyy)");
                return students; 
            }

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;
                string[] parts = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    if (parts.Length < 9)
                        throw new Exception("!!! Недостатньо полів (очікується 9)");

                    string lastName = parts[0];
                    string firstName = parts[1];
                    string patronymic = parts[2];

                    if (parts[3].Length != 1)
                        throw new Exception("!!! Стать має містити лише один символ");

                    char gender = parts[3][0];
                    DateTime birthDate = DateTime.ParseExact(parts[4], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    int? math = ParseGrade(parts[5]);
                    int? physics = ParseGrade(parts[6]);
                    int? informatic = ParseGrade(parts[7]);
                    int scholarship = int.Parse(parts[8]);
                    students.Add((lastName, firstName, patronymic, gender, birthDate, math, physics, informatic, scholarship));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"!!! Помилка в рядку {i + 1}: {ex.Message}");
                }
            }

            return students;
        }

        static int? ParseGrade(string gradeStr)
        {
            if (gradeStr == "-")
                return null;
            else
                return int.Parse(gradeStr);
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