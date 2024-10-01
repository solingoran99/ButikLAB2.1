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

		public static void DisplayProducts(List<Product> products)
		{
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("Our products:");
			Console.ResetColor();
			
			for (int i  = 0; i < products.Count; i++)
			{
                Console.WriteLine($"{i+1}. {products[i].Name}: {products[i].Description} (Price:){products[i].Price}Kr");
            }
		}
	}
}
