using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MortezaeeShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MortezaeeShop.Controllers
{
    public class ProductController : Controller
    {
        MortezaeeShopContext _context;
        public ProductController(MortezaeeShopContext context)
        {
            _context = context;
        }
        [Route("Group/{id}/{name}")]
        public IActionResult ShowProductByGroupId(int id,string name)
        {
            ViewData["GroupName"] = name;
            var Products = _context.categoryToProducts
                .Where(c => c.CategoryId == id)
                .Include(c => c.Product)
                .Select(c => c.Product)
                .ToList();

            return View(Products);
        }
    }
}
