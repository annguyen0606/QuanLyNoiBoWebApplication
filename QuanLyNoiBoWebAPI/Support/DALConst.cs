using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNoiBoWebAPI.Support
{
    public class DALConst
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

        public readonly static string ORACLE_CONNECTION =
            String.Format("Connection Timeout=60;User Id={0};Password={1};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={4})))",
                GetKeySetting("ConnectionOracle:Id"),
                GetKeySetting("ConnectionOracle:Password"),
                GetKeySetting("ConnectionOracle:Host"),
                GetKeySetting("ConnectionOracle:Port"),
                GetKeySetting("ConnectionOracle:ServiceName")
            );

        public readonly static string REDIS_HOST = GetKeySetting("ConnectionRedis:Host");
        public readonly static int REDIS_PORT = Convert.ToInt32(GetKeySetting("ConnectionRedis:Port"));
        public readonly static string REDIS_PASS = GetKeySetting("ConnectionRedis:Password");

        public const string SYS_ERR_OK = "0";
        public const string SYS_ERR_UNKNOW = "-1";
        public const string SYS_ERR_EXCEPTION = "-2";
        public const string SYS_MGS_OK = "Success";
        public const string VALUE_NULL = "null";
        public const string P_ERR_CODE = "p_err_code";
        public const string P_ERR_MESSAGE = "p_err_message";
        public const string P_REFCURSOR = "p_refcursor";
        public const string DATA = "data";
        public const int LENGTH_ERR_CODE = 20;
        public const int LENGTH_ERR_MESSAGE = 225;

        public const string QUERY_SELECT_PACKAGE = "select distinct PACKAGE_NAME from USER_ARGUMENTS where PACKAGE_NAME is not null order by PACKAGE_NAME";
        public const string QUERY_SELECT_FUNCTION = "select distinct OBJECT_NAME from USER_ARGUMENTS where PACKAGE_NAME = '{0}'";
        public const string QUERY_SELECT_ARGUMENT = "select ARGUMENT_NAME, POSITION, DATA_TYPE, IN_OUT from USER_ARGUMENTS where OBJECT_NAME = '{0}' order by POSITION";

        public const string REDIS_KEY_ALL_PACKAGE = "ALL_PACKAGE_DATABASE:PACKAGE";

        public static string A(string logString, params object[] logParams)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(logString).Append(".:");
            foreach (object s in logParams)
            {
                sb.Append(" [").Append(s).Append("]");
            }
            return sb.ToString();
        }
    }
}
