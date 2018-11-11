using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Artem.Labs.ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var streetCount = GetInputInt("Приввет, сколько улиц в твоем городе");

            int sum = 0;
            var buildingCounts = new int[streetCount];
            for (int i = 0; i < streetCount; i++)
            {
                buildingCounts[i] = GetInputInt($"Сколько домов на {i+1} улице?");
                sum += buildingCounts[i];
            }

            Console.WriteLine($"Получилось: {sum}");
            Console.WriteLine("План города: ");
            foreach (var buildingCount in buildingCounts)
            {
                for (int i = 0; i < buildingCount; i++)
                {
                    
                }
            }
            Console.WriteLine();

            Console.ReadKey();
        }

        static int GetInputInt(string b)
        {
            int a;
            Console.WriteLine(b);
            while (!int.TryParse(Console.ReadLine(), out a))
                Console.WriteLine(b);
            return a;
        }
    }
}
