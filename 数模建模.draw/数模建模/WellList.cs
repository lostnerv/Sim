using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using 建模数模.tools;
using Microsoft.Win32;

namespace 数模建模
{
    class WellList
    {
        public DataTable getAllWellnum()
        {
            string sql = "select jh,cyjh from daa01";
            DataTable dt = GetDataAsDataTable.GetDataReasult(sql);
            return dt;
        }

        public DataTable getExcelData(string path)
        {
            DataTable dt = GetDataAsDataTable.LoadDataFromExcel(path, "Sheet1");
            return dt;
        }

        public DataTable getWellnumByArea(string Area)
        {
            string sql = "select jh,cyjh from daa01 where qkdy='"+Area+"'";
            DataTable dt = GetDataAsDataTable.GetDataReasult(sql);
            return dt;
        }
    }
}
