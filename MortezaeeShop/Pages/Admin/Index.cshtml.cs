using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MortezaeeShop.Data;
using MortezaeeShop.Models;

namespace MortezaeeShop.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private MortezaeeShopContext _context;
        public IndexModel(MortezaeeShopContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> Products { get; set; }
        public void OnGet()
        {
            Products = _context.Products.Include(p => p.Item);
        }

        public void OnPost()
        {
        }
    }
}
