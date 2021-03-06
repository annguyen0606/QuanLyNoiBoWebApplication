﻿using Dapper;
using log4net;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using QuanLyNoiBoWebAPI.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNoiBoWebAPI.AccessDataBase
{
    public class DatabaseHelper
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Stopwatch stopWatch = new Stopwatch();

        /// <summary>
        /// Thực thi query không tham số
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataTable ExecuteReaderQuery(string commandText)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(DALConst.ORACLE_CONNECTION))
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = commandText;
                        OracleDataReader reader = cmd.ExecuteReader();
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }
        public static DataSet GetData(string storedProcedure, string function, string dataSearch, string status)
        {
            DataSet ds = new DataSet();
            ds.Dispose();
            try
            {
                using (OracleConnection con = new OracleConnection(DALConst.ORACLE_CONNECTION))
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = storedProcedure;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("p_refcursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        if (function == "all")
                        {
                            cmd.Parameters.Add(new OracleParameter("p_Company", OracleDbType.Varchar2)).Value = dataSearch;
                            cmd.Parameters.Add(new OracleParameter("p_Status", OracleDbType.Varchar2)).Value = status;
                        }
                        else if (function == "id")
                        {
                            cmd.Parameters.Add(new OracleParameter("p_ID", OracleDbType.Varchar2)).Value = "%" + dataSearch + "%";
                        }
                        else if (function == "cmnd")
                        {
                            cmd.Parameters.Add(new OracleParameter("p_identifycard", OracleDbType.Varchar2)).Value = dataSearch;
                        }
                        else if (function == "phone")
                        {
                            cmd.Parameters.Add(new OracleParameter("p_phonenumber", OracleDbType.Varchar2)).Value = "%" + dataSearch + "%";
                        }
                        else if (function == "ettypes")
                        {

                        }
                        else if (function == "organization")
                        {

                        }
                        else if (function == "zalo")
                        {
                            cmd.Parameters.Add(new OracleParameter("p_ZaloID", OracleDbType.Varchar2)).Value = dataSearch;
                            cmd.Parameters.Add(new OracleParameter("p_p_ServiceName", OracleDbType.Varchar2)).Value = status;
                        }
                        cmd.ExecuteNonQuery();
                        con.Close();

                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        da.Fill(ds);
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public static DataSet GetDataDiemDanh(string storedProcedure, string function, object[] keyValues)
        {
            //string function , string storedProcedure, string id, string company, string fromday, string today
            DataSet ds = new DataSet();
            try
            {
                using (OracleConnection con = new OracleConnection(DALConst.ORACLE_CONNECTION))
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = storedProcedure;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (function == "aperson")
                        {
                            cmd.Parameters.Add(new OracleParameter("p_IdEntity", OracleDbType.Varchar2)).Value = keyValues[0];
                        }
                        cmd.Parameters.Add(new OracleParameter("p_DayFrom", OracleDbType.Varchar2)).Value = keyValues[2];
                        cmd.Parameters.Add(new OracleParameter("p_DayTo", OracleDbType.Varchar2)).Value = keyValues[3];
                        cmd.Parameters.Add(new OracleParameter("p_Company", OracleDbType.Varchar2)).Value = keyValues[1];
                        if (function == "department")
                        {
                            cmd.Parameters.Add(new OracleParameter("p_Department", OracleDbType.Varchar2)).Value = keyValues[4];
                        }
                        cmd.Parameters.Add("p_refcursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        con.Close();
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        da.Fill(ds);
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ds;
            }
        }
        /// <summary>
        /// Thực thi query có tham số đầu vào
        /// </summary>
        /// <param name="commandText">query</param>
        /// <param name="keyFields">trường</param>
        /// <param name="keyValues">giá trị</param>
        /// <returns></returns>
        private DataTable ExecuteReaderQuery(string commandText, string[] keyFields, object[] keyValues)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(DALConst.ORACLE_CONNECTION))
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.BindByName = true;
                        cmd.CommandText = commandText;
                        foreach (var item in keyFields.Select((value, index) => new { Value = value, Index = index }))
                        {
                            OracleParameter param = new OracleParameter(item.Value, keyValues[item.Index]);
                            cmd.Parameters.Add(param);
                        }

                        OracleDataReader reader = cmd.ExecuteReader();
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        con.Close();
                        cmd.Dispose();
                        reader.Dispose();

                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("ExecuteReaderQuery", ex);
                return new DataTable();
            }
        }

        /// <summary>
        /// Call stored
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="keyFields"></param>
        /// <param name="keyValues"></param>
        /// <param name="keyTypes"></param>
        /// <param name="keyDirection"></param>
        /// <returns></returns>
        public IDictionary<string, object> ExecBOFunction(string storedName, string[] keyFields, object[] keyValues, int[] keyTypes, int[] keyDirection)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();

            string v_ErrCode = String.Empty;
            string v_ErrMessage = String.Empty;
            DataTable v_dt = new DataTable();

            try
            {
                using (OracleConnection connection = new OracleConnection(DALConst.ORACLE_CONNECTION))
                {
                    using (OracleCommand command = new OracleCommand(storedName, connection) { CommandType = CommandType.StoredProcedure, BindByName = true })
                    {
                        foreach (var item in keyFields.Select((value, index) => new { Value = value, Index = index }))
                        {
                            command.Parameters.Add(new OracleParameter()
                            {
                                IsNullable = true,
                                ParameterName = item.Value,
                                Value = keyValues[item.Index],
                                OracleDbType = (OracleDbType)keyTypes[item.Index],
                                Direction = (ParameterDirection)keyDirection[item.Index],
                                Size = (item.Value.Equals(DALConst.P_ERR_MESSAGE) || item.Value.Equals(DALConst.P_ERR_CODE) || (keyValues[item.Index] == null)) ? 100 : keyValues[item.Index].ToString().Length
                            });
                        }

                        connection.Open();
                        OracleDataAdapter adapter = new OracleDataAdapter(command);

                        if (command.Parameters[0].DbType == DbType.Object)
                        {
                            DataSet v_ds = new DataSet();
                            adapter.Fill(v_ds);
                            if (v_ds != null && v_ds.Tables.Count > 0)
                            {
                                v_dt = v_ds.Tables[0];
                            }
                        }

                        if (command.Parameters[keyFields.Length - 2].DbType == DbType.String)
                            v_ErrCode = (command.Parameters[keyFields.Length - 2].Value != null) ? ((OracleString)command.Parameters[keyFields.Length - 2].Value).ToString() : String.Empty;
                        else
                            v_ErrCode = (command.Parameters[keyFields.Length - 2].Value != null) ? ((OracleDecimal)command.Parameters[keyFields.Length - 2].Value).ToString() : String.Empty;

                        v_ErrMessage = (command.Parameters[keyFields.Length - 1].Value != null) ? ((OracleString)command.Parameters[keyFields.Length - 1].Value).ToString() : String.Empty;

                        connection.Close();
                        command.Dispose();
                        adapter.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                v_ErrCode = DALConst.SYS_ERR_UNKNOW;
                v_ErrMessage = ex.ToString();
                logger.Error("ExecBOFunction", ex);
            }

            result.Add(DALConst.P_ERR_CODE, v_ErrCode);
            result.Add(DALConst.P_ERR_MESSAGE, v_ErrMessage);
            result.Add(DALConst.DATA, v_dt);

            return result;
        }


        /// <summary>
        /// Execute Stored
        /// </summary>
        /// <param name="storedName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private async Task<IDictionary<string, object>> ExecStoredAsync(string storedName, OracleCommand command)
        {
            string v_ErrCode = DALConst.SYS_ERR_UNKNOW;
            string v_ErrMessage = String.Empty;
            DataTable v_dt = new DataTable();

            try
            {
                using (OracleConnection connection = new OracleConnection(DALConst.ORACLE_CONNECTION))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                    {
                        await Task.Run(() =>
                        {
                            command.Connection = connection;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = storedName;
                            command.BindByName = true;

                            foreach (OracleParameter parameter in command.Parameters)
                            {
                                if (parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.InputOutput)
                                {
                                    switch (parameter.ParameterName)
                                    {
                                        case DALConst.P_ERR_CODE:
                                            if (parameter.Value != DBNull.Value)
                                                if (parameter.Value.ToString() == DALConst.VALUE_NULL)
                                                    v_ErrCode = DALConst.SYS_ERR_EXCEPTION;
                                                else
                                                    v_ErrCode = parameter.Value.ToString();
                                            break;
                                        case DALConst.P_REFCURSOR:
                                            DataSet v_ds = new DataSet();
                                            adapter.Fill(v_ds);
                                            if (v_ds != null && v_ds.Tables.Count > 0)
                                                v_dt = v_ds.Tables[0];
                                            break;
                                        case DALConst.P_ERR_MESSAGE:
                                            v_ErrMessage = parameter.Value.ToString();
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                v_ErrCode = DALConst.SYS_ERR_UNKNOW;
                v_ErrMessage = ex.ToString();
                logger.Error(DALConst.A("ExecStoredAsync", storedName, ex));
            }

            return new Dictionary<string, object>() {
                    { DALConst.P_ERR_CODE, v_ErrCode },
                    { DALConst.P_ERR_MESSAGE, v_ErrMessage },
                    { DALConst.DATA, v_dt }
                };
        }

        public async Task<IDictionary<string, object>> InsertPhoneListDetailAsync(string storedName, object[] keyValues)
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
                dynamicParams.Add("p_NamePerson", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[1]);
                dynamicParams.Add("p_DateOfBirth", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[2]);
                dynamicParams.Add("p_Gender", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[3]);
                dynamicParams.Add("p_NativePlace", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[4]);
                dynamicParams.Add("p_PlaceOfPermanent", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[5]);
                dynamicParams.Add("p_PhoneNumber", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[6]);
                dynamicParams.Add("p_IdentifyCard", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[7]);
                dynamicParams.Add("p_SocialInsuranceNumber", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[8]);
                dynamicParams.Add("p_TaxIdentificationNumber", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[9]);
                dynamicParams.Add("p_PassportNumber", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[10]);
                dynamicParams.Add("p_Services", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[11]);
                dynamicParams.Add("p_Relationship", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[12]);
                dynamicParams.Add("p_Company", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[13]);
                dynamicParams.Add("p_Picture", OracleDbType.Varchar2, ParameterDirection.Input, keyValues[14]);
                dynamicParams.Add("p_refcursor", OracleDbType.RefCursor, ParameterDirection.Output, null);
                dynamicParams.Add("p_err_code", OracleDbType.Varchar2, ParameterDirection.Output, null, 10);
                dynamicParams.Add("p_err_message", OracleDbType.Varchar2, ParameterDirection.Output, null, 50);

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
                logger.Error(DALConst.A("InsertPhoneChangeTelcoAsync", storedName, ex));
            }

            if (logger.IsDebugEnabled)
            {
                stopWatch.Stop();
                logger.Info(DALConst.A("InsertPhoneChangeTelcoAsync.:Stopwatch", storedName, stopWatch.ElapsedMilliseconds));
            }

            return resultReturn;
        }
        public static void GetAllPackageDatabase()
        {
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Stopwatch st = new Stopwatch();

            if (logger.IsDebugEnabled)
            {
                st.Start();
            }

            try
            {
                IList<Task> tasks = new List<Task>();
                DataTable dtPackage = new DataTable();
                dtPackage = ExecuteReaderQuery(DALConst.QUERY_SELECT_PACKAGE);
                PackageModel[] arrPackageModel = new PackageModel[dtPackage.Rows.Count];

                foreach (var i in Enumerable.Range(0, dtPackage.Rows.Count))
                {
                    DataRow drPackage = dtPackage.Rows[i];

                    Task taskParent = Task.Factory.StartNew(() =>
                    {
                        string packageName = drPackage["PACKAGE_NAME"].ToString().ToUpper();

                        IList<Task> tasksChild = new List<Task>();
                        IList<FunctionModel> listFunction = new List<FunctionModel>();
                        DataTable dtFunction = new DataTable();
                        dtFunction = ExecuteReaderQuery(String.Format(DALConst.QUERY_SELECT_FUNCTION, packageName));

                        for (int j = 0; j < dtFunction.Rows.Count; j++)
                        {
                            DataRow drFunction = dtFunction.Rows[j];

                            string functionName = drFunction["OBJECT_NAME"].ToString();
                            IList<ArgumentModel> listArgument = new List<ArgumentModel>();
                            DataTable dtArgument = new DataTable();
                            dtArgument = ExecuteReaderQuery(String.Format(DALConst.QUERY_SELECT_ARGUMENT, functionName));

                            for (int k = 0; k < dtArgument.Rows.Count; k++)
                            {
                                DataRow drArgument = dtArgument.Rows[k];
                                OracleDbType oracleDbType = OracleDbType.Varchar2;
                                ParameterDirection direction = ParameterDirection.Input;

                                switch (drArgument["DATA_TYPE"].ToString().ToUpper().Replace(" ", ""))
                                {
                                    case "REFCURSOR":
                                        oracleDbType = OracleDbType.RefCursor;
                                        break;
                                    case "VARCHAR2":
                                        oracleDbType = OracleDbType.Varchar2;
                                        break;
                                    case "NUMBER":
                                        oracleDbType = OracleDbType.Int16;
                                        break;
                                    default:
                                        break;
                                }

                                switch (drArgument["IN_OUT"].ToString().ToUpper())
                                {
                                    case "IN":
                                        direction = ParameterDirection.Input;
                                        break;
                                    case "OUT":
                                        direction = ParameterDirection.Output;
                                        break;
                                    case "IN_OUT":
                                        direction = ParameterDirection.InputOutput;
                                        break;
                                    default:
                                        break;
                                }

                                listArgument.Add(new ArgumentModel()
                                {
                                    ARGUMENT_NAME = drArgument["ARGUMENT_NAME"].ToString(),
                                    POSITION = drArgument["POSITION"].ToString(),
                                    DATA_TYPE = oracleDbType,
                                    IN_OUT = direction
                                });
                            }

                            listFunction.Add(new FunctionModel()
                            {
                                FUNCTION_NAME = functionName,
                                LIST_ARGUMENT = listArgument
                            });
                        }

                        arrPackageModel[i] = (new PackageModel()
                        {
                            PACKAGE_NAME = packageName,
                            LIST_FUNCTION = listFunction
                        });
                    });

                    tasks.Add(taskParent);
                }

                Task.WhenAll(tasks).Wait();
                _listPackage = arrPackageModel.ToList<PackageModel>();

                RedisCache.Set(DALConst.REDIS_KEY_ALL_PACKAGE, JsonConvert.SerializeObject(_listPackage));
                logger.Info("GetAllPackageDatabase: " + _listPackage.Count + " Package");
            }
            catch (Exception ex)
            {
                logger.Error("GetAllPackageDatabase", ex);
            }

            if (logger.IsDebugEnabled)
            {
                st.Stop();
                logger.Info(DALConst.A("GetAllPackageDatabase.:Stopwatch", st.ElapsedMilliseconds));
            }
        }

        public static IList<PackageModel> _listPackage = new List<PackageModel>();
        public static IEnumerable<OracleParameter> GetParameterStored(string storedName, object[] keyValues)
        {
            string[] storedInfo = storedName.Split('.');
            string packageName = storedInfo[0];
            string functionName = storedInfo[1];

            OracleParameter command = new OracleParameter();
            IDictionary<string, object> result = new Dictionary<string, object>();

            string v_ErrCode = String.Empty;
            string v_ErrMessage = String.Empty;

            if (_listPackage.Count == 0)
            {
                _listPackage = JsonConvert.DeserializeObject<IList<PackageModel>>(RedisCache.Get(DALConst.REDIS_KEY_ALL_PACKAGE));
                if (_listPackage.Count == 0) GetAllPackageDatabase();
            }

            if (_listPackage.Count > 0)
            {
                IList<ArgumentModel> listArguments = _listPackage.FirstOrDefault(package => package.PACKAGE_NAME == packageName.ToUpper())?.LIST_FUNCTION
                                                                    .FirstOrDefault(function => function.FUNCTION_NAME == functionName.ToUpper())?.LIST_ARGUMENT;

                foreach (var item in listArguments.Select((value, index) => new { argumentModel = value, Index = index }))
                {
                    string fieldName = item.argumentModel.ARGUMENT_NAME.ToLower();
                    var value = keyValues[item.Index];
                    int sizeValue = 100;

                    switch (fieldName)
                    {
                        case DALConst.P_REFCURSOR:
                            sizeValue = 0;
                            break;
                        case DALConst.P_ERR_CODE:
                            sizeValue = DALConst.LENGTH_ERR_CODE;
                            break;
                        case DALConst.P_ERR_MESSAGE:
                            sizeValue = DALConst.LENGTH_ERR_MESSAGE;
                            break;
                        default:
                            if (keyValues[item.Index] == null) sizeValue = 0;
                            else sizeValue = value.ToString().Length;
                            break;
                    }

                    yield return new OracleParameter()
                    {
                        IsNullable = true,
                        ParameterName = fieldName,
                        Value = value,
                        OracleDbType = item.argumentModel.DATA_TYPE,
                        Direction = item.argumentModel.IN_OUT,
                        Size = sizeValue
                    };
                }
            }
            else
            {
                result.Add(DALConst.P_ERR_CODE, 106);
                result.Add(DALConst.P_ERR_MESSAGE, "Stored không tồn tại");
                result.Add(DALConst.DATA, new DataTable());
                logger.Error(DALConst.A("GetParameterStored", result[DALConst.P_ERR_MESSAGE].ToString()));
            }
        }
        public static IDictionary<string, object> ExecBOFunctionAdvance(string storedName, object[] keyValues)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            string v_ErrCode = String.Empty;
            string v_ErrMessage = String.Empty;
            DataTable v_dt = new DataTable();

            try
            {
                OracleCommand command = new OracleCommand();
                command.Parameters.AddRange(GetParameterStored(storedName, keyValues)?.ToArray());

                using (OracleConnection connection = new OracleConnection(DALConst.ORACLE_CONNECTION))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = storedName;
                        command.BindByName = true;

                        foreach (OracleParameter parameter in command.Parameters)
                        {
                            if (parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.InputOutput)
                            {
                                switch (parameter.ParameterName)
                                {
                                    case DALConst.P_ERR_CODE:
                                        if (parameter.Value != DBNull.Value)
                                            v_ErrCode = (parameter.Value.ToString() == DALConst.VALUE_NULL) ? DALConst.SYS_ERR_EXCEPTION : parameter.Value.ToString();
                                        break;
                                    case DALConst.P_REFCURSOR:
                                        DataSet v_ds = new DataSet();
                                        adapter.Fill(v_ds);
                                        if (v_ds != null && v_ds.Tables.Count > 0)
                                            v_dt = v_ds.Tables[0];
                                        break;
                                    case DALConst.P_ERR_MESSAGE:
                                        v_ErrMessage = parameter.Value.ToString();
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                v_ErrCode = DALConst.SYS_ERR_UNKNOW;
                v_ErrMessage = ex.ToString();
                logger.Error(DALConst.A("ExecBOFunctionAdvance", storedName, ex));
            }

            result.Add(DALConst.P_ERR_CODE, v_ErrCode);
            result.Add(DALConst.P_ERR_MESSAGE, v_ErrMessage);
            result.Add(DALConst.DATA, v_dt);

            return result;
        }
    }
}