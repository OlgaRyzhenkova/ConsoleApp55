using System;

class Program
{
    static void Main(string[] args)
    {
        System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Меню ===");
            Console.WriteLine("1. Запустити Block1");
            Console.WriteLine("2. Запустити Block2");
            Console.WriteLine("3. Запустити додаткові завдання");
            Console.WriteLine("0. Вийти");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Block1.Block.Main();
                    break;
                case "2":
                    Block2.Block.Main(); 
                    break;
                case "3":
                    DodExer.Exercise.Main();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }
            Console.WriteLine("Натисніть будь-яку клавішу, щоб повернутися до меню...");
            Console.ReadKey();
        }
    }
}