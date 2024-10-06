using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButikLAB2._1
{
	public class Product
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }

		public Product(string name, string description, double price)
		{
			Name = name;
			Description = description;
			Price = price;
		}

		// Method to display products

		public static void DisplayProducts(List<Product> products, Currency currency)
		{

			double conversionRate = GetCurrencyConversionRate(currency);
			string currencySymbol = GetCurrencySymbol(currency);

			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("Our products:");
			Console.ResetColor();
			
			for (int i  = 0; i < products.Count; i++)
			{
				double convertedPrice = products[i].Price * conversionRate;
				Console.WriteLine($"{i+1}. {products[i].Name}: {products[i].Description} (Price: {convertedPrice:F2} {currencySymbol})");
            }
		}

		public static double GetCurrencyConversionRate(Currency currency)
		{
			switch (currency)
			{
				case Currency.EUR:
					return 0.085;
				case Currency.USD:
					return 0.090;
				case Currency.SEK:
				default:
					return 1.0;
			}
		}
		private static string GetCurrencySymbol (Currency currency)
		{
			switch (currency)
			{
				case Currency.USD:
					return "USD";
				case Currency.EUR:
					return "EUR";
				case Currency.SEK:
				default:
					return "SEK";

			}
		}
	}
}
