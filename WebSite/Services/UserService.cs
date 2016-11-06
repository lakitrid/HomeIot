using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Models;

namespace WebSite.Services
{
    public class UserService
    {
        private ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<User> GetUsers()
        {
            List<User> users =this._dbContext.Users.Select(e => new User { Login = e.UserName, Mail = e.Email }).ToList();
            return users;
        }
    }
}
