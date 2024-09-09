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
        public AddEditProductViewModel product { get; set; }
        [BindProperty]
        public List<int> selectedGroups { get; set; }
        public void OnGet()
        {
            product = new AddEditProductViewModel()
            {
                Categories = _context.Categorys.ToList(),
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var item = new Item()
            {
                Price = product.Price,
                QuantityInStock = product.QuantityInStock
            };
            _context.Add(item);
            _context.SaveChanges();

            var pro = new Product()
            {
                Name = product.Name,
                Item = item,
                Discription = product.Description
            };
            _context.Add(pro);
            _context.SaveChanges();

            pro.ItemId = pro.Id;
            _context.SaveChanges();

            if(product.Picture?.Length > 0)
            {
                string FilePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    pro.Id + Path.GetExtension(product.Picture.FileName));
                using (var stream = new FileStream(FilePath,FileMode.Create))
                {
                    product.Picture.CopyTo(stream);
                }
            }

            if(selectedGroups.Any() && selectedGroups.Count() > 0)
            {
                foreach (int gr in selectedGroups)
                {
                    _context.categoryToProducts.Add(new CategoryToProduct()
                    {
                        CategoryId = gr,
                        ProductId = pro.Id
                    });
                    _context.SaveChanges();
                }
            }
            return RedirectToPage("Index");
        }
    }
}
