﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MortezaeeShop.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public Item item { get; set; }
        public int Quantity { get; set; }
        public decimal GetTotalPrice()
        {
            return item.Price * Quantity;
        }
    }
}
