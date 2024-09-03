using System;
using System.Collections.Generic;
using System.Linq;
using MortezaeeShop.Models;
using System.Threading.Tasks;

namespace MortezaeeShop.Data.Repositories
{
    public interface IUserRepository
    {
        bool IsExistUserByEmail(string email);
        void AddUser(Users users);
        Users GetUserForLogin(string email,string password);
    }

    public class UserRepository : IUserRepository
    {
        MortezaeeShopContext _context;
        public UserRepository(MortezaeeShopContext context)
        {
            _context = context;
        }
        public void AddUser(Users users)
        {
            _context.Add(users);
            _context.SaveChanges();
        }

        public Users GetUserForLogin(string email, string password)
        {
            return _context.Users
                .SingleOrDefault(u => u.Email == email && u.Password == password);
        }

        public bool IsExistUserByEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}
