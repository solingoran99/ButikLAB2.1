using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButikLAB2._1
{
	internal class Product
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
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("Lush Locks\nOur products:");
			Console.ResetColor();
			foreach (var product in products)
			{
				Console.WriteLine($"-{product.Name}: {product.Description} (Price: {product.Price}kr)");
			}
		}
	}
}
