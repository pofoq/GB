using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace Interview.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            Thread thread = new Thread(new ThreadStart(SomeMethod));
            thread.Start();
            Thread.Sleep(9000);
            Console.WriteLine("Stop");
            SomeMethod(2);
        }

        private static void SomeMethod()
        {
            SomeMethod(1);
        }

        private static void SomeMethod(int threadNum)
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine($"{threadNum} - {i}");
                Thread.Sleep(1000);
            }
        }
    }
}