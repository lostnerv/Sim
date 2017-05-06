using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using 建模数模.tools;
using System.IO;
using System.Text.RegularExpressions;

namespace 数模建模
{
    class Wellskew
    {                                           
        DataTable dt = new DataTable();
        private DataTable wellnum;
        private string wellSet = null;
        DataTable skew = new DataTable();

        public Wellskew(DataTable dt1)
        {
            wellnum = dt1;
            foreach (DataRow tempRow in wellnum.Rows)
            {
                wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
            }
            wellSet = wellSet.Substring(0, wellSet.Length - 1);
            
        }

        public void WriteResult1(string path)
        {
            RWFile file = new RWFile();
            
            file.Append(path, "DEP      DEV      AZIM\r\n");
            foreach (DataRow row in dt.Rows)
            {
                string dep = row["测深"].ToString().PadRight(10);
                string dev = row["井斜角"].ToString().PadRight(10);
                string azim = row["方位角"].ToString().PadRight(10);
                string result = dep + "   " + dev + "  " + azim + "\r\n";
                file.Append(path, result);
            }
            
        }

        public DataTable getDate1(String path, string flag)
        {

            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;

            bool triger = false;
            string trim;
            string[] array;

            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Contains(flag))
                {
                    trim = Regex.Replace(line, "\\s{1,}", ",");
                    array = trim.Split(',');
                    
                    foreach (string name in array)
                    {
                        DataColumn column = new DataColumn();
                        column.DataType = System.Type.GetType("System.String");//该列的数据类型 
                        column.ColumnName = name;

                        dt.Columns.Add(column);
                    }
                    
                    triger = true;
                    continue;
                }
                if (triger)
                {
                    trim = Regex.Replace(line, "\\s{1,}", ",");
                    array = trim.Split(',');
                    DataRow row = dt.NewRow();

                    for (int i = 0; i < array.Length; i++)
                    {
                        row[i] = array[i];
                    }
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }

        public DataTable getDateFromDatabase()
        {
            string sql = "select cd_well_source.well_desc,";
            sql += "to_char(cd_survey_station_t.md,'9990.000') as 测深,";
            sql += "to_char(cd_survey_station_t.inclination,'9990.000') as 井斜角,";
            sql += "to_char(cd_survey_station_t.azimuth,'9990.000') as 方位角 ";
            sql += "from cd_well_source ";
            sql += "left join cd_survey_station_t ";
            sql += "on cd_well_source.well_id = cd_survey_station_t.well_id ";
            sql += "where well_desc in(" + wellSet + ")";
            
            skew = GetDataAsDataTable.GetDataReasult1(sql);
            
            return skew;
        }

        public DataTable getDate(String path,string flag)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
           
            bool triger = false;
            string trim;
            string[] array;
           
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Contains(flag))
                {
                    trim = Regex.Replace(line, "\\s{1,}", ",");
                    array = trim.Split(',');
                    foreach (string name in array)
                    {
                        DataColumn column = new DataColumn();
                        column.DataType = System.Type.GetType("System.String");//该列的数据类型 
                        column.ColumnName = name;

                        if (name == "INCL")
                        {
                            column.ColumnName = "DEV";
                        }

                        dt.Columns.Add(column);
                    }
                    triger = true;
                    continue;
                }

                if (triger)
                {
                    trim = Regex.Replace(line, "\\s{1,}", ",");
                    array = trim.Split(',');
                    DataRow row = dt.NewRow();
                   
                    for (int i = 0; i < array.Length; i++)
                    {
                        row[i] = array[i];
                    }
                    dt.Rows.Add(row);
                }  
            }
            return dt;
        }

        public DataTable getDate2(String path, string flag)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;

            bool triger = false;
            string trim;
            string[] array;

            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Contains(flag))
                {
                    trim = Regex.Replace(line, "\\s{1,}", ",");
                    array = trim.Split(',');
                    foreach (string name in array)
                    {
                        DataColumn column = new DataColumn();
                        column.DataType = System.Type.GetType("System.String");//该列的数据类型 
                        column.ColumnName = name;

                        if (name == "INCL")
                        {
                            column.ColumnName = "DEV";
                        }

                        dt.Columns.Add(column);
                    }
                    triger = true;
                    continue;
                }

                if (triger)
                {
                    trim = Regex.Replace(line, "\\s{1,}", ",");
                    array = trim.Split(',');
                    DataRow row = dt.NewRow();

                    for (int i = 0; i < array.Length; i++)
                    {
                        row[i] = array[i];
                    }
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }

        public void WriteResult(string path)
        {
            RWFile file = new RWFile();
            file.Append(path, "DEP  DEV   AZIM\r\n");

            foreach (DataRow row in dt.Rows)
            {
                string dep = row["DEP"].ToString().PadRight(10);
                string dev = row["DEV"].ToString().PadRight(10);
                string azim = row["AZIM"].ToString().PadRight(10);
                string result = dep + "   " + dev + "  " + azim + "\r\n";
                file.Append(path, result);
            }
        }

        public void WriteResult3(string path)
        {
            RWFile file = new RWFile();
            string resultline = "";
            foreach (DataRow tempRow in wellnum.Rows)
            {
                string jh = tempRow[0].ToString().Trim();
                DataRow[] matches = skew.Select(" well_desc= '" + jh + "'", "测深 asc");
                if (matches.Length <= 1)
                {
                    file.Append(path + "\\log.txt", jh+"\r\n");
                    continue;
                }
                file.Append(path + "\\" + jh+".txt", "DEP  DEV  AZIM\r\n");
                string dep = "";
                string val = "";
                string azim = "";
                foreach (DataRow dt in matches)
                {
                    dep = dt[1].ToString();
                    val = dt[2].ToString();
                    azim = dt[3].ToString();
                    
                    resultline = dep.PadRight(10) + " " + val.PadRight(10)+" "+azim.PadRight(10) + "\r\n";
                    file.Append(path + "\\" + jh + ".txt", resultline);
                }
                if (resultline.Trim() != "")
                {
                    resultline = (Convert.ToDouble(dep) + 300).ToString().PadRight(10) + " " + val.PadRight(10) + " " + azim.PadRight(10) + "\r\n";
                    file.Append(path + "\\" + jh + ".txt", resultline);
                }
            }
        }
    }
}
