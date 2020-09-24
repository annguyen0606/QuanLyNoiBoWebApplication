using QuanLyNoiBo.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNoiBoAdminApp.Services
{
    public interface IUserAPI
    {
        Task<string> Authenticate(LoginRequest request);
    }
}
