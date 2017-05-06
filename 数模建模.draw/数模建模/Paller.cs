using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using 建模数模.tools;

namespace 数模建模
{
    class Paller
    {
        DataTable dt = new DataTable();

        public Paller()
        {
            DataColumn x = new DataColumn();
            x.ColumnName = "x";
            x.DefaultValue = 0;
            dt.Columns.Add(x);

            DataColumn kxdcolumn = new DataColumn();
            kxdcolumn.ColumnName = "y";
            kxdcolumn.DefaultValue = 0;
            dt.Columns.Add(kxdcolumn);

            DataColumn stlcolumn = new DataColumn();
            stlcolumn.ColumnName = "z";
            stlcolumn.DefaultValue = 0;
            dt.Columns.Add(stlcolumn);

            DataColumn bhdcolumn = new DataColumn();
            bhdcolumn.ColumnName = "排序列";
            bhdcolumn.DataType = System.Type.GetType("System.Double");
            bhdcolumn.DefaultValue = 0;
            dt.Columns.Add(bhdcolumn);

            DataColumn xdh = new DataColumn();
            xdh.ColumnName = "线代号";
            xdh.DefaultValue = 0;
            dt.Columns.Add(xdh);

            DataColumn wjm = new DataColumn();
            wjm.ColumnName = "文件名";
            wjm.DefaultValue = 0;
            dt.Columns.Add(wjm);
        }

        public DataTable getData(string path)
        {
            if (path == "")
            {
                return null;
            }
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string line,trim;
            string[] array;

            while ((line = sr.ReadLine()) != null)
            {
                    line = line.Trim();
                    trim = Regex.Replace(line, "\\s{1,}", ",");
                    array = trim.Split(',');
                    DataRow row = dt.NewRow();

                    for (int i = 0; i < 5; i++)
                    {
                        if (i == 0)
                        {
                            row[i] = "21"+array[i];
                        }
                        else if(i == 3)
                        {
                           
                            row[i] = Convert.ToDouble(array[i].ToString());
                        }
                        else
                        {
                            row[i] = array[i];
                        }
                    }
                    dt.Rows.Add(row);
             }
            return dt;
         }

        public void WriteResult(string path)
        {
            RWFile file = new RWFile();

            file.Append(path, "x  y  z 排序列 线代号 文件名\r\n");
            DataRow[] matches = dt.Select("", "排序列 asc");
            foreach (DataRow row in matches)
            {
                string x = row["x"].ToString().PadRight(15);
                string y = row["y"].ToString().PadRight(15);
                string z = row["z"].ToString().PadRight(15);
                string pxl = row["排序列"].ToString().PadRight(3);
                string xdh = row["线代号"].ToString().PadRight(3);
                string wjm = row["文件名"].ToString().PadRight(3);

                string result = x + "   " + y + " " + z + " " + pxl + " " + xdh + " " + wjm + "\r\n";
                file.Append(path, result);
            }

        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //        file.Append(path, "x  y  z 派序列 线代号 文件名\r\n");
        //        DataRow[] matches = dt.Select("", "派序列 asc");
        //        foreach (DataRow row in matches)
        //        {
        //            string x = row["x"].ToString();
        //            string y = row["y"].ToString();
        //            string z = row["z"].ToString();
        //            string pxl = row["排序列"].ToString();
        //            string xdh = row["线代号"].ToString();
        //            string wjm = row["文件名"].ToString();
        //            string result = x + "   " + y + " " + z + " " + pxl + " " + xdh + " " + wjm + "\r\n";
        //            file.Append(path, result);
        //        }
        //    }
        //    else
        //    {
        //        file.Append(path, "x  y  z 派序列 线代号 文件名\r\n");
        //        DataRow[] matches = dt.Select("", "派序列 asc");
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            string x = row["x"].ToString();
        //            string y = row["y"].ToString();
        //            string z = row["z"].ToString();
        //            string pxl = row["排序列"].ToString();
        //            string xdh = row["线代号"].ToString();
        //            string wjm = row["文件名"].ToString();
        //            string result = x + "   " + y + " " + z + " " + pxl + " " + xdh + " " + wjm + "\r\n";
        //            file.Append(path, result);
        //        }
        //    }
        }
    }
}
