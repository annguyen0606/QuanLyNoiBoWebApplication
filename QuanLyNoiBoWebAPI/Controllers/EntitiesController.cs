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

       
    }
}
