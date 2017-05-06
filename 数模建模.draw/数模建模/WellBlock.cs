using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using 建模数模.tools;

namespace 数模建模
{
    class WellBlock
    {
        public DataTable getWellBlock()
        {
            string sql = "select distinct qkdy from daa01";
            DataTable dt = GetDataAsDataTable.GetDataReasult(sql);
            return dt;
        }
    }
}
