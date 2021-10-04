using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WepApp.Api.Entities;
using WepApp.Api.Models;

namespace WepApp.Api.DataAccess.Repositories
{
    public interface IUserRepository<T>
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        T LogIn(LoginModel credentials);
        T Register(LoginModel credentials);
    }
}
