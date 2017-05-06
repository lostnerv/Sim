using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Data.OleDb;

namespace 建模数模.tools
{
    public static class GetDataAsDataTable
    {
        public static String connString = "User ID=djtsj;Password=djtsj;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 10.64.212.91)(PORT = 1521))) (CONNECT_DATA = (SERVICE_NAME = ora9)))";
        public static String connString1 = "User ID=guest;Password=guest;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 10.64.212.237)(PORT = 1521))) (CONNECT_DATA = (SERVICE_NAME = orcl)))";

        public static DataTable GetDataReasult(String sql)
        {
            DataTable result = new DataTable();
            using (OracleConnection connection = new OracleConnection(connString))
            {
                OracleCommand command = connection.CreateCommand();
                command.CommandText = sql;
                connection.Open();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    result = dt;
                }
            }
            return result;
        }


        public static DataTable GetDataReasult1(String sql)
        {
            DataTable result = new DataTable();
            using (OracleConnection connection = new OracleConnection(connString1))
            {
                OracleCommand command = connection.CreateCommand();
                command.CommandText = sql;
                connection.Open();

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    result = dt;
                }
            }
            return result;
        }

        //读取excel信息
        public static DataSet readExcelData(string path, string sheet)
        {
            String connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + "Extended Properties=Excel 8.0;";
            OleDbConnection objcon = new OleDbConnection(connStr);

            objcon.Open();
            OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM ["+sheet+"$]", objcon);
            OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();
            objAdapter1.SelectCommand = objCmdSelect;
            DataSet objDataset = new DataSet();

            objAdapter1.Fill(objDataset, "XLData");
            objcon.Close();
            return objDataset;
        }

        public static DataTable LoadDataFromExcel(string filePath, string sheet)
        {
            try
            {
                string strConn;
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
                OleDbConnection OleConn = new OleDbConnection(strConn);
                OleConn.Open();
                String sql = "SELECT * FROM  ["+sheet+"$]";//可是更改Sheet名称，比如sheet2，等等   

                OleDbDataAdapter OleDaExcel = new OleDbDataAdapter(sql, OleConn);
                DataSet OleDsExcle = new DataSet();
                OleDaExcel.Fill(OleDsExcle, "Sheet1");
                OleConn.Close();
                return OleDsExcle.Tables[0];
            }
            catch (Exception err)
            {
                System.Console.WriteLine(err);
                return null;
            }
        }

        public static DataTable LoadDataFromExcel(string filePath)
        {
            try
            {
                string strConn;
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
                OleDbConnection OleConn = new OleDbConnection(strConn);
                OleConn.Open();
                String sql = "SELECT * FROM  [$A1:R65536]";//可是更改Sheet名称，比如sheet2，等等   

                OleDbDataAdapter OleDaExcel = new OleDbDataAdapter(sql, OleConn);
                DataSet OleDsExcle = new DataSet();
                OleDaExcel.Fill(OleDsExcle, "Sheet1");
                OleConn.Close();
                return OleDsExcle.Tables[0];
            }
            catch (Exception err)
            {
                System.Console.WriteLine(err);
                return null;
            }
        }  
    }
}
