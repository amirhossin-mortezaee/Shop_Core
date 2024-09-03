using Microsoft.AspNetCore.Mvc;
using MortezaeeShop.Data;
using MortezaeeShop.Data.Repositories;
using MortezaeeShop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MortezaeeShop.Components
{
    public class ProductGroupsComponent : ViewComponent
    {
        private IGroupRepository _GroupRepository;
        public ProductGroupsComponent(IGroupRepository groupRepository)
        {
            _GroupRepository = groupRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            
                    
            return View("/Views/Components/ProductGroupsComponent.cshtml", _GroupRepository.GetGroupForShow());
        } 
    }
}
