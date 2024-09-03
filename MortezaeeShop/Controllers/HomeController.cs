using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MortezaeeShop.Data;
using MortezaeeShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MortezaeeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MortezaeeShopContext _Context;
        private static Cart _Cart = new Cart();

        public HomeController(ILogger<HomeController> logger,MortezaeeShopContext context)
        {
            _logger = logger;
            _Context = context;
        }

        public IActionResult Index()
        {
            var products = _Context.Products.ToList();
            return View(products);
        }
        [Route("ContactUs")]
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Deatils(int id)
        {
            var product = _Context.Products
                .Include(p => p.Item)
                .SingleOrDefault(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }

            var Categores = _Context.Products
                .Where(p => p.Id == id)
                .SelectMany(c => c.categoryToProducts)
                .Select(ca => ca.Category)
                .ToList();

            var vm = new DetailsViewModel()
            {
                product = product,
                Categories = Categores
            };

            return View(vm);
        }
        [Authorize]
        public IActionResult AddToCart(int itemId)
        {
            var product = _Context.Products.Include(p => p.Item).SingleOrDefault(p => p.ItemId == itemId);
            if(product != null)
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).ToString());
                var order = _Context.Order.Where(o => o.UserId == userId && !o.IsFinaly);
            }
            return RedirectToAction("ShowCart");
        }

        public IActionResult ShowCart()
        {
            var Cartvm = new CartViewModel()
            {
                cartItems = _Cart.CartItems,
                OrderTotal = _Cart.CartItems.Sum(c => c.GetTotalPrice())
            };
            return View(Cartvm);
        }

        public IActionResult RemoveCart(int itemId)
        {
            _Cart.removeItem(itemId);
            return RedirectToAction("ShowCart");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
