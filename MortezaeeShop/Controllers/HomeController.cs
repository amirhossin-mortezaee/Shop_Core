using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MortezaeeShop.Data;
using MortezaeeShop.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace MortezaeeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MortezaeeShopContext _Context;

        public HomeController(ILogger<HomeController> logger, MortezaeeShopContext context)
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
            if (product == null)
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
            if (product != null)
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
                var order = _Context.Order.FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);
                if (order != null)
                {
                    var orderDetails = _Context.OrderDetails.FirstOrDefault(d => d.OrderId == order.OrderId &&
                    d.ProductId == product.Id);
                    if(orderDetails != null)
                    {
                        orderDetails.Count += 1;
                    }
                    else
                    {
                        _Context.OrderDetails.Add(new OrderDetail()
                        {
                            OrderId = order.OrderId,
                            ProductId = product.Id,
                            Price = product.Item.Price,
                            Count = 1
                        });
                    }
                }
                else
                {
                    order = new Order()
                    {
                        IsFinaly = false,
                        CreateTime = DateTime.Now,
                        UserId = userId
                    };
                    _Context.Order.Add(order);
                    _Context.SaveChanges();
                    _Context.OrderDetails.Add(new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        ProductId = product.Id,
                        Price = product.Item.Price,
                        Count = 1
                    });

                }

                _Context.SaveChanges();
            }
            return RedirectToAction("ShowCart");
        }
        [Authorize]
        public IActionResult ShowCart()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            var order = _Context.Order.Where(o => o.UserId == userId && !o.IsFinaly)
                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product).FirstOrDefault();
            return View(order);
        }
        [Authorize]
        public IActionResult RemoveCart(int DetailId)
        {
            var orderDetails = _Context.OrderDetails.Find(DetailId);
            _Context.Remove(orderDetails);
            _Context.SaveChanges();
            return RedirectToAction("ShowCart");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
