using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ButikLAB2._1
{
	public class CartItem
	{
		public Product Product { get; set; }
		public int Quantity { get; set; }

		public CartItem(Product product, int quanity)
		{
			Product = product;
			Quantity = quanity;
		}

		public double TotalItemPrice()
		{
			return Product.Price * Quantity;
		}

		public override string ToString()
		{
			return $"{Product.Price}Kr (x{Quantity}) = {TotalItemPrice()}Kr";
		}
	}
}
