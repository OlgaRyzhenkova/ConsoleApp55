using System;
using MyTime = (int hour, int min, int sec);
namespace Block1
{
    public class Block
    {
        public static void Main()
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Введіть перший час:");
            MyTime t1 = ReadTime();
            Console.WriteLine("Введіть другий час:");
            MyTime t2 = ReadTime();
            Console.WriteLine("Введені моменти часу:");
            Console.WriteLine("Час 1: " + MyTimeToString(t1));
            Console.WriteLine("Час 2: " + MyTimeToString(t2));
            MyTime t1Plus1Sec = AddOneSecond(t1);
            Console.WriteLine("Час 1 + 1 секунда: " + MyTimeToString(t1Plus1Sec));
            MyTime t1Plus1Min = AddOneMinute(t1);
            Console.WriteLine("Час 1 + 1 хвилина: " + MyTimeToString(t1Plus1Min));
            MyTime t1Plus1Hour = AddOneHour(t1);
            Console.WriteLine("Час 1 + 1 година: " + MyTimeToString(t1Plus1Hour));
            int diff = Difference(t1, t2);
            Console.WriteLine($"Різниця між часами: {diff}  секунд");
            Console.WriteLine("Яка під час 1: " + WhatLesson(t1));
            Console.WriteLine("Яка під час 2: " + WhatLesson(t2));
        }
        static MyTime ReadTime()
        {
            Console.Write("Години: ");
            int hour = int.Parse(Console.ReadLine());
            Console.Write("Хвилини: ");
            int min = int.Parse(Console.ReadLine());
            Console.Write("Секунди: ");
            int sec = int.Parse(Console.ReadLine());
            return (hour, min, sec);
        }
        static string MyTimeToString(MyTime t)
        {
            t = Normalize(t);
            return $"{t.hour:D2}:{t.min:D2}:{t.sec:D2}";

        }
        static MyTime Normalize(MyTime t)
        {
            int totalSeconds = t.hour * 3600 + t.min * 60 + t.sec;
            int secPerDay = 24 * 3600;
            totalSeconds %= secPerDay;
            if (totalSeconds < 0)
                totalSeconds += secPerDay;
            int hour = totalSeconds / 3600;
            int min = (totalSeconds / 60) % 60;
            int sec = totalSeconds % 60;
            return (hour, min, sec);
        }
        static int ToSecSinceMidnight(MyTime t)
        {
            t = Normalize(t);
            return t.hour * 3600 + t.min * 60 + t.sec;
        }
        static MyTime FromSecSinceMidnight(int t)
        {
            int secPerDay = 60 * 60 * 24;
            t %= secPerDay;
            if (t < 0)
                t += secPerDay;
            int hour = t / 3600;
            int min = (t / 60) % 60;
            int sec = t % 60;
            return (hour, min, sec);
        }
        static MyTime AddOneSecond(MyTime t)
        {
            return AddSeconds(t, 1);
        }
        static MyTime AddOneMinute(MyTime t)
        {
            return AddSeconds(t, 60);
        }
        static MyTime AddOneHour(MyTime t)
        {
            return AddSeconds(t, 3600);
        }
        static MyTime AddSeconds(MyTime t, int s)
        {
            int totalSec = ToSecSinceMidnight(t) + s;
            return FromSecSinceMidnight(totalSec);
        }
        static int Difference(MyTime t1, MyTime t2)
        {
            int sec1 = ToSecSinceMidnight(t1);
            int sec2 = ToSecSinceMidnight(t2);
            return sec1 - sec2;
        }
        static string WhatLesson(MyTime t)
        {
            int seconds = ToSecSinceMidnight(t);
            int[] startTime =
            {
            8 * 3600,
            9 * 3600 + 40 * 60,
            11 * 3600 + 20 * 60,
            13 * 3600,
            14 * 3600 + 40 * 60,
            16 * 3600 + 10 * 60
        };
            int pairDuration = 80 * 60;
            for (int i = 0; i < startTime.Length; i++)
            {
                int pairStart = startTime[i];
                int pairEnd = pairStart + pairDuration;
                if (seconds >= pairStart && seconds < pairEnd)
                    return $"{i + 1} - a пара";
                if (i < startTime.Length - 1)
                {
                    int nextPairStart = startTime[i + 1];
                    if (seconds >= pairEnd && seconds < nextPairStart)
                        return $"Перерва після {i + 1} - ї пари";
                }
            }
            if (seconds < startTime[0])
                return "Пари ще не почались";
            if (seconds >= startTime[startTime.Length - 1] + pairDuration)
                return "Пари закінчились";
            return "Перерва після останньої пари";
        }
    }
}