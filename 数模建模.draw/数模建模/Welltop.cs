using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using 建模数模.tools;

namespace 数模建模
{
    class Welltop
    {
        private DataTable data;
        private DataTable wellnum;
        private string wellSet = null;


        public Welltop(DataTable dt)
        {
            wellnum = dt;
            foreach (DataRow tempRow in wellnum.Rows)
            {
                wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
            }
            wellSet = wellSet.Substring(0, wellSet.Length - 1);
        }

        //将远程数据与本地数据合并
        public DataTable getData(string localPath = null)
        {
            string sql = "select jh,xcfzmc,xcfzds,hd from daa071 where jh in (" + wellSet + ") order by xcxh";

            data = GetDataAsDataTable.GetDataReasult(sql);

            //创建table的第一列 
            DataColumn typeColumn = new DataColumn();
            typeColumn.DataType = System.Type.GetType("System.String");//该列的数据类型 
            typeColumn.ColumnName = "type";//该列得名称 
            typeColumn.DefaultValue = "horizon";//该列得默认值 

            data.Columns.Add(typeColumn);

            foreach (DataRow tempRow in wellnum.Rows)
            {
                string jh = tempRow[0].ToString().Trim();
                DataRow[] matches = data.Select(" jh= '" + jh + "'");

                int count = matches.Length;

                if (count != 0)
                {
                    DataRow row = matches[count - 1];
                    double result = Convert.ToDouble(row[2]) + Convert.ToDouble(row[3]);

                    DataRow newrow = data.NewRow();
                    newrow[0] = row[0];
                    newrow[1] = "P1D";
                    newrow[2] = result.ToString();
                    newrow[3] = row[3];
                    newrow[4] = "horizon";
                    data.Rows.Add(newrow);
                }
                
            }
            return data;
        }

        //输出结果
        public void WriteResult(String path)
        {
            RWFile file = new RWFile();
            file.Append(path, "jh        xcfzmc      xcfzds        type\r\n");
            foreach (DataRow tempRow in wellnum.Rows)
            {
                string jh = tempRow[0].ToString().Trim();
                DataRow[] matches = data.Select(" jh= '" + jh + "'");

                foreach (DataRow row in matches)
                {
                    string jhout = row[0].ToString();
                    string xcfzmc = row[1].ToString();
                    string xcfzds = row[2].ToString();
                    string type = row[4].ToString();

                    jhout = jhout.PadRight(15);
                    xcfzmc = xcfzmc.PadRight(5);
                    xcfzds = xcfzds.PadRight(10);
                    type = type.PadRight(10);

                    string result = jhout + "   " + xcfzmc + " " + xcfzds + "  " + type + "\r\n";
                    file.Append(path, result);
                }
            }
        }
    }
}
