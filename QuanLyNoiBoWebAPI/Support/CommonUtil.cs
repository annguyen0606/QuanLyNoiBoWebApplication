using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNoiBoWebAPI.Support
{
    public class CommonUtil
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string[] VietnameseSigns = new string[] { "aAeEoOuUiIdDyY", "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ", "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ", "íìịỉĩ", "ÍÌỊỈĨ", "đ", "Đ", "ýỳỵỷỹ", "ÝỲỴỶỸ" };

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

        /// <summary>
        /// Return request theo template quy định
        /// </summary>
        /// <param name="err_code"></param>
        /// <param name="err_message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ServiceReturn(int err_code, string err_message, object data)
        {
            IDictionary<string, object> v_Dict = new Dictionary<string, object>();
            v_Dict.Add(AppConst.ERR_CODE, err_code);
            v_Dict.Add(AppConst.ERR_MESSAGE, err_message);
            v_Dict.Add(AppConst.DATA, data);

            return v_Dict;
        }

        public static short? ConvertStringToShort(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            try
            {
                short res = Convert.ToInt16(value);
                return res;
            }
            catch (Exception ex)
            {
                logger.Error(A("ConvertStringToShort", value, ex));
                return null;
            }
        }

        public static int? ConvertStringToInt(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            try
            {
                int res = Convert.ToInt32(value);
                return res;
            }
            catch (Exception ex)
            {
                logger.Error(A("ConvertStringToInt", value, ex));
                return null;
            }
        }
        public static long? ConvertStringToLong(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            try
            {
                long res = Convert.ToInt64(value);
                return res;
            }
            catch (Exception ex)
            {
                logger.Error(A("ConvertStringToLong", value, ex));
                return null;
            }
        }

        public static decimal? ConvertStringToDecimal(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            try
            {
                decimal res = Convert.ToDecimal(value);
                return res;
            }
            catch (Exception ex)
            {
                logger.Error(A("ConvertStringToDecimal", value, ex));
                return null;
            }
        }
        public static double? ConvertStringToDouble(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            try
            {
                double res = Convert.ToDouble(value);
                return res;
            }
            catch (Exception ex)
            {
                logger.Error(A("ConvertStringToDouble", value, ex));
                return null;
            }
        }

    }
}
