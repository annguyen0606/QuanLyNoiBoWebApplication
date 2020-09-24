using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuanLyNoiBo.ViewModels.System.Users;
using QuanLyNoiBoAdminApp.Services;

namespace QuanLyNoiBoAdminApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserAPI _userAPI;
        public UserController(IUserAPI userAPI)
        {
            _userAPI = userAPI;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return View(ModelState);
            var token = await _userAPI.Authenticate(loginRequest);
            return View();
        }
    }
}
