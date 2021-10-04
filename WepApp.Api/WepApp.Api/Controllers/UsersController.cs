using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using WepApp.Api.DataAccess.Repositories;
using WepApp.Api.Entities;

namespace WepApp.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController:ControllerBase
    {
        private int UserId => Int32.Parse(this.User.Claims.Single(c=> c.Type==ClaimTypes.NameIdentifier).Value);
        private IUserRepository<User> userRep;
        public UsersController(IUserRepository<User> rep)
        {
            this.userRep = rep;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(this.userRep.GetAll());
        }

        [Authorize]
        [HttpGet("account")]
        public IActionResult GetUser()
        {
            return new JsonResult(this.userRep.Get(this.UserId));
        }
    }
}
