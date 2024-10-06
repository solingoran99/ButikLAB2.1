using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButikLAB2._1
{
	public enum MembershipLevel
	{
		Zero,
		Bronze,
		Silver,
		Gold
	}
	public class Customer
	{
		private string _name;
		private string _Password;
		private List<CartItem> _cart;
		public List<CartItem> Cart { get { return _cart; } }

		public MembershipLevel Level { get; private set; }
		public int Points { get; private set; }


		public Customer(string name, string password, int points = 0)
		{
			Name = name;
			Password = password;
			_cart = new List<CartItem>();
			Points = points;
			UpdateMembershipLevel();	
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
			Console.WriteLine("Enter your username:");
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

		//Method to register a customer
		public static void RegisterCustomer(List<Customer> customers, List<Product>products, string filePath )
		{
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("Lush Locks");
			Console.ResetColor();
			Console.WriteLine("To become a member create an account.");

			string userName;
			string userPassword;
			do
			{
				Console.WriteLine("Enter your username:");
				userName = Console.ReadLine();

				if (string.IsNullOrEmpty(userName))
				{
					Console.WriteLine("Username cannot be empty. Please enter a valid username");
				}
			} while (string.IsNullOrEmpty(userName));

			do
			{
                Console.WriteLine("Enter your password:");
				userPassword = Console.ReadLine();

				if (string.IsNullOrEmpty(userPassword))
				{
                    Console.WriteLine("Password cannot be empty. Please enter a valid password.");
                }
            }while(string.IsNullOrEmpty(userPassword));

			var newCustomer = new Customer(userName, userPassword, points: 0);
			customers.Add(newCustomer);
			SaveCustomers(filePath, customers);

			Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine("Successfully registered! Press enter to go back to the main menu.");
			Console.ResetColor();
			Console.ReadKey();
        }
		// load customers
		public static List<Customer> LoadCustomers(string filePath)
		{
			var customers = new List<Customer>();
			if (File.Exists(filePath))
			{
				foreach(var line in File.ReadAllLines(filePath))
				{
					var parts = line.Split(',');
					if(parts.Length == 3)
					{
						var name = parts[0];
						var password = parts[1];
						var points =int.Parse(parts[2]);

						customers.Add(new Customer(name, password, points));
					}
				}
			}
			return customers;
		}

		//save customers

		public static void SaveCustomers(string filePath, List<Customer> customers)
		{
			using (var writer = new StreamWriter(filePath))
			{
				foreach(var customer in customers)
				{
					writer.WriteLine($"{customer.Name},{customer.Password},{customer.Points}");
				}
			}
		}

		//Cart view method

		public void ViewCart(Currency currentCurrency)
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("Lush Locks - Your Cart");
			Console.ResetColor();
			if (Cart.Count == 0)
			{
				Console.WriteLine("Your cart is empty");
			}
			else
			{
				decimal finalTotal = 0;
				foreach (var cartItem in Cart)
				{
					decimal pricePerItem = ConvertPrice(cartItem.Product.Price, currentCurrency);
					decimal totalItemPrice = pricePerItem * cartItem.Quantity;
					Console.WriteLine($"-{cartItem.Product.Name}: {cartItem.Quantity}x ({pricePerItem:F2} {currentCurrency} per item) = {totalItemPrice:F2} {currentCurrency}");

					finalTotal += totalItemPrice;
				}
				var finalPrice = finalTotal;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"\nTotal Price: {finalPrice:F2}{currentCurrency}");
				Console.ResetColor();
			}
			Console.WriteLine("Press enter to go back to the menu.");
			Console.ReadKey();
		}

		//Calculate total method

		public (double finalPrice, double discountAmount) TotalPrice(Currency currentCurrency)
		{
			double conversionRate = Product.GetCurrencyConversionRate(currentCurrency);

			double totalPriceInSek = 0;

			foreach (var cartItem in Cart)
			{
				totalPriceInSek += cartItem.TotalItemPrice();
			}

			double totalPrice = totalPriceInSek * conversionRate;

			double discount = 0;

			switch (Level)
			{
				case MembershipLevel.Gold:
					discount = 0.15;
					break;
				case MembershipLevel.Silver:
					discount = 0.10;
				    break;
				case MembershipLevel.Bronze:
					discount = 0.05;
					break;
				default:
					discount = 0.0;
					break;
			}
			double discountAmount = totalPrice * discount;
			double finalPrice = totalPrice - discountAmount;

			return (finalPrice, discountAmount);
		}

		// Update cart method
		public void UpdateCart(Product product)
		{
			var existingItem = Cart.Find(item =>item.Product.Name == product.Name);

			if (existingItem != null)
			{
				existingItem.Quantity++;
			}
			else
			{
				Cart.Add(new CartItem(product, 1));
			}
		}

		private void AddPoints(int pointsToAdd)
		{
			Points += pointsToAdd;
			UpdateMembershipLevel();
		}

		//Method to adjust membership

		private void UpdateMembershipLevel()
		{
			
			if (Points >= 510)
			{
				Level = MembershipLevel.Gold;
			}
			else if (Points >= 250)
			{
				Level = MembershipLevel.Silver;
			}
			else if (Points >= 50)
			{
				Level = MembershipLevel.Bronze;
			}
			else
			{
				Level = MembershipLevel.Zero;
			}
		}


		//CheckOut method

		public void CheckOut(string customersFilePath, List<Customer> customers, Currency currenCurrency)
		{
			Console.Clear();
			Console.ForegroundColor= ConsoleColor.Magenta;
            Console.WriteLine("Lush Locks - Checkout");
			Console.ResetColor();

			

			if (Cart.Count == 0)
			{
                Console.WriteLine("Your cart is empty.\nPress enter to go back to the menu.");
				Console.ReadKey();
				return;
            }

			Console.WriteLine(this.ToString());

			var (finalPrice, discountAmount) = TotalPrice(currenCurrency);
			double totalPrice = finalPrice + discountAmount;
			double discountPercentage = (discountAmount / totalPrice) * 100;
			
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"\nTotal Price: {totalPrice:F2} {currenCurrency}");
			Console.WriteLine($"Discount: {discountAmount:F2} ({discountPercentage:F2}%)");
			Console.WriteLine($"Final Price: {finalPrice:F2} {currenCurrency}");
			Console.ResetColor();

			string confirmation;

			do
			{
				Console.WriteLine("Do you want to confirm the purchase? (yes/no)");
				confirmation = Console.ReadLine().ToLower().Trim();
			}
			while( confirmation != "yes" && confirmation != "no" );

           
			if(confirmation == "yes")
			{
				Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Thank you for shopping with Lush Locks!");
				Console.ResetColor();

				int pointEarned = (int)(finalPrice / 10);
				AddPoints(pointEarned);
				Cart.Clear();

				SaveCustomers(customersFilePath, customers);
            }
			else
			{
                Console.WriteLine("Your purchase was cancelled.");
            }

            Console.WriteLine("Press enter to go back to the menu.");
			Console.ReadKey();
        }

		//ToString() Method

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"Name: {Name}");
			sb.AppendLine($"Password: {Password}");
			sb.AppendLine($"Membership Level: {Level}");
			sb.AppendLine($"Points: {Points}");
			sb.AppendLine("Shopping Cart:");

			if (Cart.Count == 0)
			{
				sb.AppendLine("Your cart is empty.");
			}
			else
			{
				foreach (var cartItem in Cart)
				{
					sb.AppendLine($"- {cartItem.Product.Name}: {cartItem.Quantity}X ({cartItem.Product.Price:F2}Kr per item) = {cartItem.TotalItemPrice():F2}Kr");
				}
			}
			return sb.ToString();

            
        }

		private static decimal ConvertPrice(double priceInSEK, Currency currency)
		{
			double conversionRate = Product.GetCurrencyConversionRate(currency);
			return (decimal)(priceInSEK * conversionRate);
		}



	}
}
