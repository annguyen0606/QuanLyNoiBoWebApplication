using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyNoiBoWebAPI.AccessDataBase;
using QuanLyNoiBoWebAPI.Model;
using QuanLyNoiBoWebAPI.Support;

namespace QuanLyNoiBoWebAPI.Controllers
{
    [ApiController]
    public class EntitiesController : ControllerBase
    {
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [Route("api/GetEntities")]
        [HttpGet]
        public async Task<IActionResult> GetEntities(string function, string data1, string data2)
        {
            int v_ErrCode = AppConst.SYS_ERR_UNKNOW;
            string v_ErrMessage = String.Empty;
            if (function == "all")
            {
                DataSet dataSet = DatabaseHelper.GetData(AppConst.GET_ALL_VALUES_ENTITIES_TABLE, "all", data1, data2);
                return Ok(dataSet);
            }else if(function == "organization")
            {
                DataSet dataSet = DatabaseHelper.GetData(AppConst.GET_ALL_VALUES_ENTITIES_ORGANIZATION, "organization", data1, data2);
                return Ok(dataSet);
            }
            else
            {
                v_ErrCode = AppConst.SYS_ERR_EXCEPTION;
                v_ErrMessage = "Chua truyen vao function";
                return Ok(JsonConvert.SerializeObject(new RegisterResponseModel()
                {
                    err_code = v_ErrCode,
                    err_message = v_ErrMessage,
                    data = ""
                }));
            }
        }

        [Route("api/Zalo")]
        [HttpPost]
        public async Task<IDictionary<string, object>> Zalo([FromForm]Zalo zalo)
        {
            int v_ErrCode = AppConst.SYS_ERR_UNKNOW;
            string v_ErrMessage = String.Empty;
            IDictionary<string, object> v_Dict = new Dictionary<string, object>();

            try
            {
                object[] keyValues = { zalo.ZaloID, zalo.PhoneNumber, zalo.CompanyName, zalo.ServiceName};
                v_Dict = DatabaseHelper.ExecBOFunctionAdvance(AppConst.CHECK_ZALO_REGIST,keyValues);
                //v_Dict = await DapperHelper.CheckZalo(AppConst.CHECK_ZALO_REGIST, keyValues);
                v_ErrCode = Convert.ToInt32(v_Dict[AppConst.P_ERR_CODE]);
                v_ErrMessage = v_Dict[AppConst.P_ERR_MESSAGE].ToString();
            }
            catch (Exception ex)
            {
                v_ErrCode = AppConst.SYS_ERR_EXCEPTION;
                v_ErrMessage = ex.ToString();
            }

            return CommonUtil.ServiceReturn(v_ErrCode, v_ErrMessage, "");
        }
    }
}
