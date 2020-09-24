using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyNoiBoAdminApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public Task<IActionResult> Login()
        {
            //Hhaha
            return View();
        }
    }
}
