using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MortezaeeShop.Data;
using MortezaeeShop.Models;

namespace MortezaeeShop.Pages.Admin
{
    public class DeleteModel : PageModel
    {
        private MortezaeeShopContext _context;

        public DeleteModel(MortezaeeShopContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Product Product { get; set; }
        public void OnGet(int id)
        {
            Product = _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public IActionResult OnPost()
        {
            var product = _context.Products.Find(Product.Id);
            var item = _context.Items.First(p => p.Id == product.ItemId);
            _context.Items.Remove(item);
            _context.Products.Remove(product);
            _context.SaveChanges();
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
            product.Id + ".jpg");

            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }

            return RedirectToPage("Index");
        }
    }
}
