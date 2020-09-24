using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNoiBoWebAPI.Support
{
    public class AppConst
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

        public const string ERR_CODE = "err_code";
        public const string ERR_MESSAGE = "err_message";
        public const string DATA = "data";

        public const int SYS_ERR_OK = 0;
        public const int SYS_ERR_UNKNOW = -1;
        public const int SYS_ERR_EXCEPTION = -2;
        public const string P_ERR_CODE = "p_err_code";
        public const string P_ERR_MESSAGE = "p_err_message";
        public const string P_REFCURSOR = "p_refcursor";
        public readonly static string INSERT_VALUE_ENTITIES_TABLE = GetKeySetting("InsertData:InsertEntities");
        public readonly static string INSERT_VALUE_SERVICESUSING_TABLE = GetKeySetting("InsertData:InsertServicesUsing");
        public readonly static string GET_ALL_VALUES_ENTITIES_TABLE = GetKeySetting("GetData:GetAllEntities");
        public readonly static string GET_VALUES_ENTITIES_TABLE_VIA_ID = GetKeySetting("GetData:GetEntitiesViaID");
        public readonly static string GET_VALUES_ENTITIES_TABLE_VIA_IDENTITY_CARD = GetKeySetting("GetData:GetEntitiesViaIdentityCard");
        public readonly static string GET_VALUES_ENTITIES_TABLE_VIA_PHONE_NUMBER = GetKeySetting("GetData:GetEntitiesViaPhoneNumber");
        public readonly static string GET_DATA_DIEMDANH = GetKeySetting("GetData:GetDataDiemDanh");
        public readonly static string GET_DATA_DIEMDANH_A_PERSON = GetKeySetting("GetData:GetDataDiemDanhAPerson");
        public readonly static string GET_DATA_ENTITYWORK_BY_ZALOID = GetKeySetting("GetData:GetEntityWorkByZaloID");
        public readonly static string GET_DATA_DIEMDANH_DEPARTMENT = GetKeySetting("GetData:GetDataDiemDanhDepartment");
        public readonly static string PUT_DATA_ROLL_CALL = GetKeySetting("PutData:PutDataRollCall");
        public readonly static string PUT_DATA_ZALO_REGIST = GetKeySetting("PutData:PutZaloRegist");
        public readonly static string CHECK_NFCID_EXISTS = GetKeySetting("Check:CheckNFCID");
        public readonly static string CHECK_ZALO_REGIST = GetKeySetting("Check:CheckZaloRegist");
        public readonly static string GET_ALL_VALUES_ENTITYTYPES = GetKeySetting("GetData:GetAllEntityTypes");
        public readonly static string GET_ALL_VALUES_ENTITIES_ORGANIZATION = GetKeySetting("GetData:GetAllEntitiesOrganization");
    }
}
