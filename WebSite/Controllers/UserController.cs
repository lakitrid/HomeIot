using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using WebSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Authorization;
using System.Net;
using WebSite.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebSite.Controllers
{
    [Route("services/[controller]")]
    // Todo : [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [FromServices]
        public UserService UserService { get; set; }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return this.UserService.GetUsers();
        }

        [HttpGet, Route("{login}")]
        public User GetUser([FromRoute]string login)
        {
            return this.UserService.GetUsers().Where(e => e.Login.Equals(login)).FirstOrDefault();
        }
        
        [HttpPost]
        public async Task<string[]> Post([FromBody]User value)
        {
            var user = new ApplicationUser { UserName = value.Login, Email = value.Mail };
            var result = await _userManager.CreateAsync(user, value.Password);
            if (result.Succeeded)
            {
                this.Response.StatusCode = (int)HttpStatusCode.OK;
                return null;
            }

            this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return result.Errors.Select(e => e.Description).ToArray();
        }

        [HttpPut, Route("{oldLogin}")]
        public async Task<string[]> UpdateUser([FromRoute] string oldLogin, [FromBody]User value)
        {
            ApplicationUser user = await this._userManager.FindByNameAsync(oldLogin);
            user.UserName = value.Login;
            user.Email = value.Mail;

            var result = await this._userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                this.Response.StatusCode = (int)HttpStatusCode.OK;
                return null;
            }

            this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return result.Errors.Select(e => e.Description).ToArray();
        }

        [HttpDelete("{login}")]
        public void Delete([FromRoute]string login)
        {
        }
    }
}
