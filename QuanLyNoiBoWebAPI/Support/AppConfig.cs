using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNoiBoWebAPI.Support
{
    public class AppConfig
    {
        public static IConfigurationRoot _Configuration;

        public static string GetKeySetting(string key)
        {
            if (_Configuration == null)
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                _Configuration = builder.Build();
            }
            return _Configuration[key];
        }
    }
}
