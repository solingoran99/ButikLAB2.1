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

		//Cart property

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

		//Method to register a customer
		public static void RegisterCustomer(List<Customer> customers, List<Product>products)
		{
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("Lush Locks");
			Console.ResetColor();
			Console.WriteLine("To become a member create an account.\nEnter your username:");
			string userName = Console.ReadLine();

			Console.WriteLine("Enter your password:");
			string userPassword = Console.ReadLine();

			var newCustomer = new Customer(userName, userPassword, points: 0);
			customers.Add(new Customer(userName, userPassword));
			Console.WriteLine("Successfully registered! Press enter to go back to the main menu.");
			Console.ReadKey();

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
				foreach (var cartItem in Cart)
				{
					Console.WriteLine($"-{cartItem.Product.Name}: {cartItem.Quantity}x ({cartItem.Product.Price:F2}Kr per item) = {cartItem.TotalItemPrice():F2}Kr");
				}
				var (finalPrice, _) = TotalPrice();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"\nTotal Price: {finalPrice}kr");
				Console.ResetColor();
			}
			Console.WriteLine("Press enter to go back to the menu.");
			Console.ReadKey();
		}

		//Calculate total method

		public (double finalPrice, double discountAmount) TotalPrice()
		{
			double totalPrice = 0;

			foreach (var cartItem in Cart)
			{
				totalPrice += cartItem.TotalItemPrice();
			}

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

		public void CheckOut()
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

			var (finalPrice, discountAmount) = TotalPrice();
			double totalPrice = finalPrice + discountAmount;
			double discountPercentage = (discountAmount / totalPrice) * 100;
			
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"\nTotal Price: {finalPrice:F2}Kr");
			Console.WriteLine($"Discount: {discountAmount:F2} Kr ({discountPercentage}%)");
			Console.WriteLine($"Final Price: {finalPrice:F2} Kr");
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


	}
}
