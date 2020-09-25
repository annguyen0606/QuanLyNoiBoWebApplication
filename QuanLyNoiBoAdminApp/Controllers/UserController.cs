using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyNoiBo.ViewModels.System.Users;
using QuanLyNoiBoAdminApp.Services;
using RestSharp;

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
            var client = new RestClient("http://cloudapi.conek.vn/api/GetDataDiemDanh?function=aperson&id=0601435066976126&company=Conek&fromday=2020-09-01&today=2020-09-15");
            client.Timeout = -1;
            var request1 = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request1);
            if(response.Content != null)
            {
                return View();
            }
            return View();
        }
    }
}
