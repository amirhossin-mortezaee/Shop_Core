using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MortezaeeShop.Data;
using MortezaeeShop.Models;

namespace MortezaeeShop.Pages.Admin
{
    public class AddModel : PageModel
    {
        private MortezaeeShopContext _context;

        public AddModel(MortezaeeShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AddEditProductViewModel Product { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var item = new Item()
            {
                Price = Product.Price,
                QuantityInStock = Product.QuantityInStock
            };
            _context.Add(item);
            _context.SaveChanges();

            var pro = new Product()
            {
                Name = Product.Name,
                Item = item,
                Discription = Product.Description
            };
            _context.Add(pro);
            _context.SaveChanges();

            pro.ItemId = pro.Id;
            _context.SaveChanges();

            if(Product.Picture?.Length > 0)
            {
                string FilePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    pro.Id + Path.GetExtension(Product.Picture.FileName));
                using (var stream = new FileStream(FilePath,FileMode.Create))
                {
                    Product.Picture.CopyTo(stream);
                }
            }
            return RedirectToPage("Index");
        }
    }
}
