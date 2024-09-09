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
    public class EditModel : PageModel
    {
        private MortezaeeShopContext _context;

        public EditModel(MortezaeeShopContext context)
        {
            _context = context;
        }
        [BindProperty]
        public AddEditProductViewModel Product { get; set; }
        [BindProperty]
        public List<int> selectedGroups { get; set; }
        public List<int> GroupsProduct { get; set; }
        public void OnGet(int id)
        {
            Product = _context.Products.Include(x => x.Item)
                .Where(x => x.Id == id)
                .Select(s => new AddEditProductViewModel()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Discription,
                    QuantityInStock = s.Item.QuantityInStock,
                    Price = s.Item.Price
                }).FirstOrDefault();

            Product.Categories = _context.Categorys.ToList();
            GroupsProduct = _context.categoryToProducts.Where(x => x.ProductId == Product.Id)
                .Select(x => x.CategoryId).ToList();
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var product = _context.Products.Find(Product.Id);
            var item = _context.Items.First(p => p.Id == product.ItemId);

            product.Name = Product.Name;
            product.Discription = Product.Description;
            item.Price = Product.Price;
            item.QuantityInStock = Product.QuantityInStock;

            _context.SaveChanges();
            if (Product.Picture?.Length > 0)
            {
                string FilePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    product.Id + Path.GetExtension(Product.Picture.FileName));
                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    Product.Picture.CopyTo(stream);
                }
            }
            _context.categoryToProducts.Where(c => c.ProductId == product.Id).ToList()
                .ForEach(g => _context.categoryToProducts.Remove(g));
            if (selectedGroups.Any() && selectedGroups.Count() > 0)
            {
                foreach (int gr in selectedGroups)
                {
                    _context.categoryToProducts.Add(new CategoryToProduct()
                    {
                        CategoryId = gr,
                        ProductId = product.Id
                    });
                    _context.SaveChanges();
                }
            }

            return RedirectToPage("Index");
        }
    }
}
