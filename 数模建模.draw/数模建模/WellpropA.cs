using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using 建模数模.tools;
using System.IO;

namespace 数模建模
{
    //数据库属性直接输出
    class WellpropA
    {
        private DataTable data;
        private DataTable wellnum;
        private DataTable result = new DataTable();
        private string wellSet = null;

        public WellpropA(DataTable dt)
        {
            wellnum = dt;
            foreach (DataRow tempRow in wellnum.Rows)
            {
                wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
            }
            wellSet = wellSet.Substring(0, wellSet.Length - 1);

            DataColumn jhcol = new DataColumn();
            jhcol.DataType = System.Type.GetType("System.String");//该列的数据类型 
            jhcol.ColumnName = "井号";//该列得名称 
            result.Columns.Add(jhcol);

            DataColumn depcol = new DataColumn();
            depcol.DataType = System.Type.GetType("System.Double");//该列的数据类型 
            depcol.ColumnName = "深度";//该列得名称 
            result.Columns.Add(depcol);

            DataColumn kxdcol = new DataColumn();
            kxdcol.DataType = System.Type.GetType("System.Double");//该列的数据类型 
            kxdcol.ColumnName = "孔隙度";//该列得名称 
            result.Columns.Add(kxdcol);

            DataColumn stlcol = new DataColumn();
            stlcol.DataType = System.Type.GetType("System.String");//该列的数据类型 
            stlcol.ColumnName = "渗透率";//该列得名称 
            result.Columns.Add(stlcol);

            DataColumn bhdcol = new DataColumn();
            bhdcol.DataType = System.Type.GetType("System.String");//该列的数据类型 
            bhdcol.ColumnName = "饱和度";//该列得名称 
            result.Columns.Add(bhdcol);
        }

        //将远程数据与本地数据合并
        public DataTable getData(string localPath = null)
        {
            string sql = "select jh,syds+syhd as depth,kxd,stl,hqbhd as bhd from daa074 where jh in (" + wellSet + ")";
            data = GetDataAsDataTable.GetDataReasult(sql);

            foreach (DataRow row in data.Rows)
            {
                if (row[4].ToString() == "1099")
                {
                    row[4] = "0";
                }
            }

            foreach (DataRow tempRow in wellnum.Rows)
            {
                string jh = tempRow[0].ToString().Trim();

                DataRow[] matches = data.Select(" jh= '" + jh + "'", "depth asc");

                if (matches.Length > 0)
                {
                    double start = Convert.ToDouble(matches[0][1]);
                    double depth = start;
                    string depthString = null;
                    string kxd = null;
                    string stl = null;
                    string bhd = null;

                    int j = 0;
                    int i = 0;
                    while (depth < Convert.ToDouble(matches[matches.Length - 1][1]))
                    {
                        j++;
                        depth = start + 0.02 * j;
                        if (i < matches.Length && depth < Convert.ToDouble(matches[matches.Length - 1][1]))
                        {
                            //如果大于下一层的值，ksb转为下层
                            if (depth >= Convert.ToDouble(matches[i + 1][1]))
                            {
                                kxd = matches[i + 1][2].ToString();
                                stl = matches[i + 1][3].ToString();
                                bhd = matches[i + 1][4].ToString();

                                i++;
                            }
                            else//如果不大于下层则为当前值
                            {
                                kxd = matches[i + 1][2].ToString();
                                stl = matches[i + 1][3].ToString();
                                bhd = matches[i + 1][4].ToString();
                            }
                        }
                        else
                        {
                            depth = Convert.ToDouble(matches[matches.Length - 1][1]);
                            kxd = matches[matches.Length - 1][2].ToString();
                            stl = matches[matches.Length - 1][3].ToString();
                            bhd = matches[matches.Length - 1][4].ToString();
                        }

                        depthString = depth.ToString();

                        DataRow row1 = result.NewRow();
                        row1[0] = jh;
                        row1[1] = depthString;
                        row1[2] = kxd;
                        row1[3] = stl;
                        row1[4] = bhd;
                        result.Rows.Add(row1);

                    }


                }
            }
            return result;

        }

        //输出结果
        public void WriteResult(String path)
        {
            RWFile file = new RWFile();

            foreach (DataRow tempRow in wellnum.Rows)
            {
                string jh = tempRow[0].ToString().Trim();
                DataRow[] matches = data.Select(" jh= '" + jh + "'");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    file.Append(path + "\\" + jh + ".txt", "深度      孔隙度    渗透率    含油饱和度\r\n");
                    foreach (DataRow row in matches)
                    {
                        string depth = row[1].ToString().PadRight(10);
                        string kxd = row[2].ToString().PadRight(10);
                        string stl = row[3].ToString().PadRight(10);
                        string bhd = row[4].ToString().PadRight(10);

                        string resultLine = depth + "   " + kxd + "   " + stl + "  " + bhd + "\r\n";
                        file.Append(path + "\\" + jh+".txt", resultLine);
                    }
                }
                else
                {
                    file.Append(path + "\\" + jh + ".txt", "深度      孔隙度    渗透率    含油饱和度\r\n");
                    foreach (DataRow row in matches)
                    {
                        string depth = row[1].ToString().PadRight(10);
                        string kxd = row[2].ToString().PadRight(10);
                        string stl = row[3].ToString().PadRight(10);
                        string bhd = row[4].ToString().PadRight(10);

                        string resultLine = depth + "   " + kxd + "   " + stl + "  " + bhd + "\r\n";
                        file.Append(path + "\\" + jh + ".txt", resultLine);
                    }
                }
            }
        }
    }
}
