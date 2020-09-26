using Dapper;
using log4net;
using Oracle.ManagedDataAccess.Client;
using QuanLyNoiBoWebAPI.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNoiBoWebAPI.AccessDataBase
{
    public class DapperHelper
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Stopwatch stopWatch = new Stopwatch();

        public static async Task<IDictionary<string, object>> InsertEntitiesAsync(string storedName, object[] keyValues, byte[] image)
        {
            if (logger.IsDebugEnabled)
            {
                stopWatch.Restart();
                stopWatch.Start();
            }

            IDictionary<string, object> resultReturn = new Dictionary<string, object>();

            try
            {
                OracleDynamicParameters dynamicParams = new OracleDynamicParameters();
                dynamicParams.Add("p_EntityID", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[0]);
                dynamicParams.Add("p_EntityName", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[1]);
                dynamicParams.Add("p_EntityPhoneNumber", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[2]);
                dynamicParams.Add("p_EntityType", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[3]);
                dynamicParams.Add("p_EntityAvatar", OracleDbType.Blob, ParameterDirection.Input, image);
                dynamicParams.Add("p_EntityDescrip", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[4]);
                dynamicParams.Add("p_EntityAddress", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[5]);
                dynamicParams.Add("p_EntityRelation", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[6]);
                dynamicParams.Add("p_EntityServices", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[7]);
                dynamicParams.Add("p_EntityNotes", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[8]);
                dynamicParams.Add("p_EntityWork", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[9]);
                dynamicParams.Add("p_EntitySchool", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[10]);
                dynamicParams.Add("p_refcursor", OracleDbType.RefCursor, ParameterDirection.Output, null);
                dynamicParams.Add("p_err_code", OracleDbType.Varchar2, ParameterDirection.Output, null, 10);
                dynamicParams.Add("p_err_message", OracleDbType.Varchar2, ParameterDirection.Output, null, 50);

                using (IDbConnection dbConnection = new OracleConnection(DALConst.ORACLE_CONNECTION))
                {
                    dbConnection.Open();
                    await SqlMapper.QueryAsync(dbConnection, storedName, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    resultReturn.Add(DALConst.P_ERR_CODE, dynamicParams.oracleParameters[dynamicParams.oracleParameters.Count - 2].Value.ToString());
                    resultReturn.Add(DALConst.P_ERR_MESSAGE, dynamicParams.oracleParameters[dynamicParams.oracleParameters.Count - 1].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                resultReturn.Add(DALConst.P_ERR_CODE, DALConst.SYS_ERR_EXCEPTION);
                resultReturn.Add(DALConst.P_ERR_MESSAGE, ex);
                logger.Error(DALConst.A("InsertEntitiesAsync", storedName, ex));
            }

            if (logger.IsDebugEnabled)
            {
                stopWatch.Stop();
                logger.Info(DALConst.A("InsertEntitiesAsync.:Stopwatch", storedName, stopWatch.ElapsedMilliseconds));
            }

            return resultReturn;
        }

        public static async Task<IDictionary<string, object>> Update(string storedName, object[] keyValues, string function)
        {
            if (logger.IsDebugEnabled)
            {
                stopWatch.Restart();
                stopWatch.Start();
            }

            IDictionary<string, object> resultReturn = new Dictionary<string, object>();

            try
            {
                OracleDynamicParameters dynamicParams = new OracleDynamicParameters();
                if (function == "RollCall")
                {
                    dynamicParams.Add("p_IdEntity", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[0].ToString());
                    dynamicParams.Add("p_DatePrevious", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[1].ToString());
                    dynamicParams.Add("p_DateNew", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[2].ToString());
                    dynamicParams.Add("p_TimePrevious", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[3].ToString());
                    dynamicParams.Add("p_TimeNew", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[4].ToString());
                    dynamicParams.Add("p_MinuteLate", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[5].ToString());
                    dynamicParams.Add("p_Company", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[6].ToString());
                    dynamicParams.Add("p_refcursor", OracleDbType.RefCursor, ParameterDirection.Output, null);
                    dynamicParams.Add("p_err_code", OracleDbType.Varchar2, ParameterDirection.Output, null, 10);
                    dynamicParams.Add("p_err_message", OracleDbType.Varchar2, ParameterDirection.Output, null, 50);
                }
                else if (function == "Service")
                {
                    dynamicParams.Add("p_ZaloID", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[0].ToString());
                    dynamicParams.Add("p_PhoneRegist", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[1].ToString());
                    dynamicParams.Add("p_Company", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[2].ToString());
                    dynamicParams.Add("p_EntityService", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[3].ToString());
                    dynamicParams.Add("p_err_code", OracleDbType.Varchar2, ParameterDirection.Output, null, 10);
                    dynamicParams.Add("p_err_message", OracleDbType.Varchar2, ParameterDirection.Output, null, 50);
                }
                using (IDbConnection dbConnection = new OracleConnection(DALConst.ORACLE_CONNECTION))
                {
                    await dbConnection.ExecuteAsync(storedName, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    resultReturn.Add(DALConst.P_ERR_CODE, dynamicParams.oracleParameters[dynamicParams.oracleParameters.Count - 2].Value.ToString());
                    resultReturn.Add(DALConst.P_ERR_MESSAGE, dynamicParams.oracleParameters[dynamicParams.oracleParameters.Count - 1].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                resultReturn.Add(DALConst.P_ERR_CODE, DALConst.SYS_ERR_EXCEPTION);
                resultReturn.Add(DALConst.P_ERR_MESSAGE, ex);
                logger.Error(DALConst.A("UpdateRollCallServiceUsing", storedName, ex));
            }

            if (logger.IsDebugEnabled)
            {
                stopWatch.Stop();
                logger.Info(DALConst.A("UpdateRollCallServiceUsing.:Stopwatch", storedName, stopWatch.ElapsedMilliseconds));
            }

            return resultReturn;
        }

        public static async Task<IDictionary<string, object>> InsertServicesUsingAsync(string storedName, object[] keyValues)
        {
            if (logger.IsDebugEnabled)
            {
                stopWatch.Restart();
                stopWatch.Start();
            }

            IDictionary<string, object> resultReturn = new Dictionary<string, object>();

            try
            {
                OracleDynamicParameters dynamicParams = new OracleDynamicParameters();
                dynamicParams.Add("p_ID", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[0]);
                dynamicParams.Add("p_NameService", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[1]);
                dynamicParams.Add("p_DateOfTime", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[2]);
                dynamicParams.Add("p_Address", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[3]);
                dynamicParams.Add("p_Notes", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[4]);
                dynamicParams.Add("p_refcursor", OracleDbType.RefCursor, ParameterDirection.Output, null);
                dynamicParams.Add("p_err_code", OracleDbType.Varchar2, ParameterDirection.Output, null, 10);
                dynamicParams.Add("p_err_message", OracleDbType.Varchar2, ParameterDirection.Output, null, 50);

                using (IDbConnection dbConnection = new OracleConnection(DALConst.ORACLE_CONNECTION))
                {
                    dbConnection.Open();
                    await SqlMapper.QueryAsync(dbConnection, storedName, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    resultReturn.Add(DALConst.P_ERR_CODE, dynamicParams.oracleParameters[dynamicParams.oracleParameters.Count - 2].Value.ToString());
                    resultReturn.Add(DALConst.P_ERR_MESSAGE, dynamicParams.oracleParameters[dynamicParams.oracleParameters.Count - 1].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                resultReturn.Add(DALConst.P_ERR_CODE, DALConst.SYS_ERR_EXCEPTION);
                resultReturn.Add(DALConst.P_ERR_MESSAGE, ex);
                logger.Error(DALConst.A("InsertServicesUsingAsync", storedName, ex));
            }

            if (logger.IsDebugEnabled)
            {
                stopWatch.Stop();
                logger.Info(DALConst.A("InsertServicesUsingAsync.:Stopwatch", storedName, stopWatch.ElapsedMilliseconds));
            }

            return resultReturn;
        }

        public static async Task<IDictionary<string, object>> CheckDataExist(string storedName, object[] keyValues, string function)
        {
            if (logger.IsDebugEnabled)
            {
                stopWatch.Restart();
                stopWatch.Start();
            }

            IDictionary<string, object> resultReturn = new Dictionary<string, object>();

            try
            {
                OracleDynamicParameters dynamicParams = new OracleDynamicParameters();
                if (function == "zalo")
                {
                    dynamicParams.Add("p_ZaloID", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[0]);
                    dynamicParams.Add("p_PhoneRegist", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[1]);
                    dynamicParams.Add("p_Company", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[2]);
                    dynamicParams.Add("p_ServiceName", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[3]);
                }
                else if (function == "id")
                {
                    dynamicParams.Add("p_EntityID", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[0]);
                }
                dynamicParams.Add("p_err_code", OracleDbType.Varchar2, ParameterDirection.Output, null, 10);
                dynamicParams.Add("p_err_message", OracleDbType.Varchar2, ParameterDirection.Output, null, 50);

                using (IDbConnection dbConnection = new OracleConnection(DALConst.ORACLE_CONNECTION))
                {
                    dbConnection.Open();
                    await SqlMapper.QueryAsync(dbConnection, storedName, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    resultReturn.Add(DALConst.P_ERR_CODE, dynamicParams.oracleParameters[dynamicParams.oracleParameters.Count - 2].Value.ToString());
                    resultReturn.Add(DALConst.P_ERR_MESSAGE, dynamicParams.oracleParameters[dynamicParams.oracleParameters.Count - 1].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                resultReturn.Add(DALConst.P_ERR_CODE, DALConst.SYS_ERR_EXCEPTION);
                resultReturn.Add(DALConst.P_ERR_MESSAGE, ex);
                logger.Error(DALConst.A("CheckDataExistsUsingAsync", storedName, ex));
            }

            if (logger.IsDebugEnabled)
            {
                stopWatch.Stop();
                logger.Info(DALConst.A("CheckDataExistsUsingAsync.:Stopwatch", storedName, stopWatch.ElapsedMilliseconds));
            }

            return resultReturn;
        }

        public static async Task<IDictionary<string, object>> CheckZalo(string storedName, object[] keyValues)
        {

            IDictionary<string, object> resultReturn = new Dictionary<string, object>();

            try
            {
                OracleDynamicParameters dynamicParams = new OracleDynamicParameters();
                dynamicParams.Add("p_ZaloID", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[0]?.ToString());
                dynamicParams.Add("p_PhoneRegist", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[1]?.ToString());
                dynamicParams.Add("p_Company", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[2]?.ToString());
                dynamicParams.Add("p_ServiceName", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[3]?.ToString());
                dynamicParams.Add("p_err_code", OracleDbType.Varchar2, ParameterDirection.Output, null, 10);
                dynamicParams.Add("p_err_message", OracleDbType.Varchar2, ParameterDirection.Output, null, 50);

                using (IDbConnection dbConnection = new OracleConnection(DALConst.ORACLE_CONNECTION))
                {
                    dbConnection.Open();
                    await SqlMapper.QueryAsync(dbConnection, storedName, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    resultReturn.Add(DALConst.P_ERR_CODE, dynamicParams.oracleParameters[dynamicParams.oracleParameters.Count - 2].Value.ToString());
                    resultReturn.Add(DALConst.P_ERR_MESSAGE, dynamicParams.oracleParameters[dynamicParams.oracleParameters.Count - 1].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                resultReturn.Add(DALConst.P_ERR_CODE, DALConst.SYS_ERR_EXCEPTION);
                resultReturn.Add(DALConst.P_ERR_MESSAGE, ex);
                logger.Error(DALConst.A("CheckZaloUsingAsync", storedName, ex));
            }


            return resultReturn;
        }
    }
}