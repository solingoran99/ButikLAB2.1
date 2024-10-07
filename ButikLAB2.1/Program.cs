using System.Security.Cryptography.X509Certificates;

namespace ButikLAB2._1
{
	public class Program
	{
		private static Currency currentCurrency = Currency.SEK;
		static void Main(string[] args)
		{

			const string customersFilePath = "customers.txt";
			//List of products

			List<Product> products = new List<Product>
			{
				new Product("Lush Shampoo","Repairs and adds volume for soft, bouncy hair.", 119.00 ),
				new Product("Lush Conditioner", "Nourishes and hydrates for smooth, manageable hair.", 112.00),
				new Product("Lush Hair Mask", "Overnight hair treatment for frizz control and shine", 219.20),
				new Product("Lush Scalp Serum", "Repair hair serum", 99.00),
				new Product("Lush Hair Oil", "Strengthens and adds shine for quick growth", 269.00)
			};

			List<Customer> customers = Customer.LoadCustomers(customersFilePath);

			if (customers.Count == 0)
			{
				customers = new List<Customer>
				{
					new Customer("Knatte", "123", points: 150),
					new Customer("Fnatte", "321", points: 260),
					new Customer("Tjatte", "213", points: 540)
				};
			}

			bool running = true;
			Customer loggedInCustomer = null;
			while (running)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Magenta;
				Console.WriteLine("Welcome to Lush Locks! To see our newest releases, hurry up and join!");
				Console.ResetColor();

				Console.WriteLine("1.Log in\n2.Become a member\n3.Exit\nEnter the corresponding number:");
				string userInput = Console.ReadLine();
				int userChoice;


				if (int.TryParse(userInput, out userChoice))
				{    
					switch (userChoice)
					{
						case 1:
							Console.Clear();
							loggedInCustomer = Customer.Login(customers);
							if (loggedInCustomer != null)
							{
								ShoppingMenu(loggedInCustomer, products, customers, customersFilePath);
							}
							break;
						case 2:
							Console.Clear();
							Customer.RegisterCustomer(customers, products, customersFilePath);
							break;
						case 3:
							running = false;
							break;
						default:
							Console.WriteLine("Invalid choice. Choose 1, 2 or 3.");
							break;
					}					
				}
				else
				{
					Console.WriteLine("OOPS error! Please enter a number.");
				}
			}	
		}

		//Shopping menu method
		public static void ShoppingMenu(Customer customer, List<Product> products, List<Customer> customers, string customersFilePath)
		{
			bool shopping = true;

			while (shopping)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Magenta;
				Console.WriteLine("Lush Locks");
				Console.ResetColor();
				Console.WriteLine("1.Profile\n2.Shop\n3.View Cart\n4.Check Out\n5.Change Currency\n6.Log Out");


				if (int.TryParse(Console.ReadLine(), out int shoppingChoice))
				{
					switch (shoppingChoice)
					{
						case 1:
							Console.Clear();
							Console.ForegroundColor = ConsoleColor.Magenta;
							Console.WriteLine("Lush Locks - Profile");
							Console.ResetColor();
                            Console.WriteLine(customer.ToString(currentCurrency));
                            Console.WriteLine("Press enter to go back to the menu.");
							Console.ReadKey();
                            break;
						case 2:
							ShoppingLoop(customer, products);
							break;

						case 3:
							customer.ViewCart(currentCurrency);
							break;

						case 4:
							customer.CheckOut(customersFilePath, customers, currentCurrency);
							break;
						case 5:
							ChangeCurrency();
							break;

						case 6:
							shopping = false;
							break;

						default:
							Console.WriteLine("Invalid choice");
							break;

					}
				}

				else
				{
					Console.WriteLine("Invalid, enter a number 1-4. ");
				}
			}
		}

		//Method to change to currency

		public static void ChangeCurrency()
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Lush Locks - Select Currency");
			Console.ResetColor();
			Console.WriteLine("1. SEK (Swedish Krona)\n2. EUR (EURO)\n3. USD (US Dollar)\nEnter your choice:");

			string input = Console.ReadLine();
			int userChoice;
			if (int.TryParse(input, out userChoice))
			{
				switch (userChoice)
				{
					case 1:
						currentCurrency = Currency.SEK;
						break;
					case 2:
						currentCurrency = Currency.EUR;
						break;
					case 3:
						currentCurrency = Currency.USD;
						break;
					default:
						Console.WriteLine("Invalid choice. Set to SEK.");
						currentCurrency = Currency.SEK;
						break;
				}
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine($"Currency changed to: {currentCurrency}");
				Console.ResetColor();
				System.Threading.Thread.Sleep(1500);
			}
		}

		//LOOP for shoppinng
		public static void ShoppingLoop(Customer customer, List<Product> products)
		{
			bool continueShopping = true;

			while (continueShopping)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Magenta;
				Console.WriteLine("Lush Locks");
				Console.ResetColor();
				Product.DisplayProducts(products, currentCurrency);

				Console.WriteLine("\nEnter the number of the product to add to your cart:\nType 'exit' to stop shopping.");
				string productInput = Console.ReadLine();
				int productNumber;

				if (productInput.Trim().ToLower().Equals("exit", StringComparison.OrdinalIgnoreCase))
				{
					continueShopping = false; 
				}

				else if (int.TryParse(productInput, out productNumber) && productNumber > 0 && productNumber <= products.Count)
				{
					Product selectedProduct = products[productNumber - 1];
					customer.UpdateCart(selectedProduct); 
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"\n{selectedProduct.Name} has been added to your cart.");
					Console.ResetColor();

					System.Threading.Thread.Sleep(1500);
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Please enter a valid number from the list or type 'exit' to go back.");
					Console.ResetColor();
					System.Threading.Thread.Sleep(1500);
				}
			}
		}
	}

	public enum Currency
	{
		SEK,
		USD,
		EUR
	}
}


