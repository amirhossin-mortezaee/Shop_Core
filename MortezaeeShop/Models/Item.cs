﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MortezaeeShop.Models
{
    public class Item
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int MyProperty { get; set; }
        public int QuantityInStock { get; set; }
        //*******************************************************
        public Product product { get; set; }
    }
}
