using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNoiBoWebAPI.Support
{
    public class ArgumentModel
    {
        public string ARGUMENT_NAME;
        public string POSITION;
        public OracleDbType DATA_TYPE;
        public ParameterDirection IN_OUT;
    }
}
