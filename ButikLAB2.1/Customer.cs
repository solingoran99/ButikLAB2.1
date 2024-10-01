using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButikLAB2._1
{
	internal class Customer
	{
		private string _name;
		private string _Password;

		//Cart property

		private List<Product> _cart;
		public List<Product> Cart { get { return _cart; } }


		public Customer(string name, string password)
		{
			Name = name;
			Password = password;
			_cart = new List<Product>();
		}

		public string Name
		{
			get => _name; set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new InvalidOperationException("Customer name cannot be empty.");
				}
				_name = value;
			}
		}
		public string Password
		{
			get => _Password; set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new InvalidOperationException("Password cannot be empty");
				}
				_Password = value;
			}
		}

		// Customer login method
		public static Customer Login(List<Customer> customers)
		{
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("Lush Locks");
			Console.ResetColor();
			Console.WriteLine("Please log in\nEnter your username:");
			string userName = Console.ReadLine();

			Console.WriteLine("Enter your password:");
			string userPassword = Console.ReadLine();

			Customer loggedInCustomer = customers.Find(c => c.Name == userName && c.Password == userPassword);

			if (loggedInCustomer != null)
			{
				Console.WriteLine($"Welcome, {loggedInCustomer.Name}");
				return loggedInCustomer;
			}
			else
			{
				Console.WriteLine("Invalid username or password. Please try again.");
				return null;
			}
		}

		public static void registerCustomer(List<Customer> customers)
		{
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("Lush Locks");
			Console.ResetColor();
			Console.WriteLine("To become a member create an account.\nEnter your username:");
			string userName = Console.ReadLine();

			Console.WriteLine("Enter your password:");
			string userPassword = Console.ReadLine();

			customers.Add(new Customer(userName, userPassword));
			Console.WriteLine("Successfully registered, log in to get started!");
		}

		//Cart view method

		public void ViewCart()
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("Your shopping cart:");
			Console.ResetColor();
			if (Cart.Count == 0)
			{
				Console.WriteLine("Your cart is empty");
			}
			else
			{
				foreach (var product in Cart)
				{
					Console.WriteLine($"-{product.Name}: {product.Price}kr");
				}
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"\nTotal Price: {TotalPrice()}kr");
				Console.ResetColor();
			}
			Console.WriteLine("Press enter to go back to the menu.");
			Console.ReadKey();
		}

		//Calculate total method

		public double TotalPrice()
		{
			double totalPrice = 0;

			foreach (var product in Cart)
			{
				totalPrice += product.Price;
			}
			return totalPrice;
		}


	}
}
