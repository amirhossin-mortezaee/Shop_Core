using MortezaeeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MortezaeeShop.Data.Repositories
{
    public interface IGroupRepository
    {
        IEnumerable<Category> GetAllCategories();
        IEnumerable<ShowGroupViewModel> GetGroupForShow();
    }

    public class GroupRepository : IGroupRepository
    {
        private MortezaeeShopContext _context;
        public GroupRepository(MortezaeeShopContext context)
        {
            _context = context;
        }
        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categorys;
        }

        public IEnumerable<ShowGroupViewModel> GetGroupForShow()
        {
           return _context.Categorys
                .Select(c => new ShowGroupViewModel()
                {
                    GroupId = c.Id,
                    Name = c.Name,
                    ProductCount = _context.categoryToProducts.Count(g => g.CategoryId == c.Id)
                }).ToList();
        }
    }
}
