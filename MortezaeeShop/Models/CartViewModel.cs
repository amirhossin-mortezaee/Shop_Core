using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MortezaeeShop.Models
{
    public class CartViewModel
    {
        public CartViewModel()
        {
            cartItems = new List<CartItem>();
        }
        public List<CartItem> cartItems { get; set; }
        public decimal OrderTotal { get; set; }
    }
}
