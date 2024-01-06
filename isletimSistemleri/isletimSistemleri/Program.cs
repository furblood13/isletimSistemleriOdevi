using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Program
{
	private static List<int> numbers = Enumerable.Range(1, 1000000).ToList();
	private static List<int> primes = new List<int>();
	private static List<int> odds = new List<int>();
	private static List<int> evens = new List<int>();

	public static void Main()
	{
		var threads = new List<Thread>();

		for (int i = 0; i < 4; i++)
		{
			int start = i * 250000;
			int end = (i + 1) * 250000;
			var thread = new Thread(() => FindNumbers(start, end));
			threads.Add(thread);
			thread.Start();
		}

		foreach (var thread in threads)
		{
			thread.Join();
		}

		Console.WriteLine("Asallar: " + primes.Count);
		Console.WriteLine("Tek Sayilar: " + odds.Count);
		Console.WriteLine("Cift Sayilar: " + evens.Count);
	}

	private static void FindNumbers(int start, int end)
	{
		for (int i = start; i < end; i++)
		{
			if (IsPrime(numbers[i]))
			{
				lock (primes)
				{
					primes.Add(numbers[i]);
				}
			}

			if (numbers[i] % 2 == 0)
			{
				lock (evens)
				{
					evens.Add(numbers[i]);
				}
			}
			else
			{
				lock (odds)
				{
					odds.Add(numbers[i]);
				}
			}
		}
	}

	private static bool IsPrime(int number)
	{
		if (number < 2) return false;
		for (int i = 2; i <= Math.Sqrt(number); i++)
		{
			if (number % i == 0) return false;
		}
		return true;
	}
}

