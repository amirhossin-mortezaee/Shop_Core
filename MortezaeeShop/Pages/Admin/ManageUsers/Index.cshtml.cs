using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MortezaeeShop.Models;

namespace MyEshop.Pages.Admin.ManageUsers
{
    public class IndexModel : PageModel
    {
        private readonly MortezaeeShop.Data.MortezaeeShopContext _context;

        public IndexModel(MortezaeeShop.Data.MortezaeeShopContext context)
        {
            _context = context;
        }

        public IList<Users> Users { get;set; }

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();
        }
    }
}
