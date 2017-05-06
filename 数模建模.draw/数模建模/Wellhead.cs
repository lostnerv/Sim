using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using 建模数模.tools;
using 数模建模;

namespace 建模数模
{
    class Wellhead
    {
        private DataTable data;
        private DataTable localdata = new DataTable();
        private DataTable wellnum;
        private DataTable result = new DataTable();
        private string wellSet = null;
        DataTable haha = new DataTable();

        public Wellhead()
        {
           
        }

        public Wellhead(DataTable dt)
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
            string sql = "select daa01.jh as 井号, daa01.cyjh as 常用井号,";
            sql += "case when daa01.mqjb like '1%' then '3'";
            sql += "when daa01.mqjb like '3%' then '15' else daa01.mqjb end as 井别,";
            sql += "daa02.zzbx as x坐标,";
            sql += "daa02.hzby as y坐标,";
            sql += "daa02.zzbx as x本地坐标,";
            sql += "daa02.hzby as y本地坐标,";
            sql += "daa02.bxhb as 补心海拔,";
            sql += "max(daa073.dyds) + 30 as 底深,";
            sql += "'' as 一致性";
            sql += " from daa01";
            sql += " left join daa02";
            sql += " on trim(daa01.jh) = trim(daa02.jh)";
            sql += " left join daa073";
            sql += " on trim(daa01.jh) = trim(daa073.jh)";
            sql += " where jh in (" + wellSet + ")";
            sql += " group by daa01.jh,daa01.cyjh, daa01.mqjb, daa02.zzbx, daa02.hzby, daa02.bxhb";

            data = GetDataAsDataTable.GetDataReasult(sql);

            return data;
        }

        public DataTable getDataFromLocal(string localPath = null)
        {
            DataColumn wellnumColumn = new DataColumn();
            wellnumColumn.DataType = System.Type.GetType("System.String");//该列的数据类型 
            wellnumColumn.ColumnName = "井号";//该列得名称 
            localdata.Columns.Add(wellnumColumn);

            DataColumn hzbyColumn = new DataColumn();
            hzbyColumn.DataType = System.Type.GetType("System.String");//该列的数据类型 
            hzbyColumn.ColumnName = "y坐标（本地）";//该列得名称 
            localdata.Columns.Add(hzbyColumn);

            DataColumn zzbxColumn = new DataColumn();
            zzbxColumn.DataType = System.Type.GetType("System.String");//该列的数据类型 
            zzbxColumn.ColumnName = "x坐标（本地）";//该列得名称 
            localdata.Columns.Add(zzbxColumn);

            DataColumn dydsColumn = new DataColumn();
            dydsColumn.DataType = System.Type.GetType("System.String");//该列的数据类型 
            dydsColumn.ColumnName = "补心海拔（本地）";//该列得名称 
            localdata.Columns.Add(dydsColumn);

            DataTable dt = GetDataAsDataTable.LoadDataFromExcel(localPath, "Sheet1");
            if (!string.IsNullOrEmpty(localPath))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    
                    if (dt.Rows[i][1].ToString().Contains("-") && dt.Rows[i][1].ToString().Length < 10)
                    {
                        DataRow newrow = localdata.NewRow();
                       
                        //如果补心海拔和地面海拔为0
                        if (dt.Rows[i][2].ToString().Trim() == "" && dt.Rows[i][3].ToString().Trim() == "")
                        {
                            if (dt.Rows[i][7].ToString().Length < 7)
                            {
                                newrow[0] = dt.Rows[i][1].ToString();
                                if (dt.Rows[i][4].ToString().StartsWith("2"))
                                {
                                    newrow[1] = dt.Rows[i][5].ToString();
                                    newrow[2] = dt.Rows[i][4].ToString();
                                }
                                else
                                {
                                    newrow[1] = dt.Rows[i][4].ToString();
                                    newrow[2] = dt.Rows[i][5].ToString();
                                }
                                newrow[3] = dt.Rows[i][7].ToString();
                            }
                            else
                            {
                                newrow[0] = dt.Rows[i][1].ToString();
                                newrow[1] = dt.Rows[i + 1][4].ToString();
                                newrow[2] = dt.Rows[i + 1][5].ToString();
                                newrow[3] = dt.Rows[i + 1][3].ToString();
                            }
                        }
                        else
                        {
                            if (dt.Rows[i][4].ToString().StartsWith("2"))
                            {
                                newrow[1] = dt.Rows[i][5].ToString();
                                newrow[2] = dt.Rows[i][4].ToString();
                            }
                            else
                            {
                                newrow[1] = dt.Rows[i][4].ToString();
                                newrow[2] = dt.Rows[i][5].ToString();
                            }
                            newrow[0] = dt.Rows[i][1].ToString();
                            newrow[3] = dt.Rows[i][3].ToString();
                        }
                        localdata.Rows.Add(newrow);
                    }
                }
            }
            var enumTable = from bookTable in data.AsEnumerable()
                            join categoryTable in localdata.AsEnumerable()
                            on bookTable.Field<string>("常用井号") equals
                               categoryTable.Field<string>("井号")
                            select new
                            {
                                井号 = bookTable.Field<string>("井号"),
                                井别 = bookTable.Field<string>("井别"),
                                y坐标 = bookTable[4],
                                //y坐标 = bookTable.Field<decimal>("y坐标"),
                                x坐标 = bookTable[3],
                                //补心海拔 = bookTable.Field<decimal>("补心海拔"),
                                补心海拔 = bookTable[5],
                                y坐标本地 = categoryTable.Field<string>("x坐标（本地）"),
                                x坐标本地 = categoryTable.Field<string>("y坐标（本地）"),
                                补心海拔本地 = categoryTable.Field<string>("补心海拔（本地）"),
                                一致性 = ""
                            };
            haha = Data_Result.CopyToDataTable(enumTable);

            //foreach (DataRow hahah1 in haha.Rows)
            //{
            //    if (hahah1[2] != null && hahah1[5] != null)
            //    {
            //        if (Convert.ToDouble(hahah1[2]) != Convert.ToDouble(hahah1[5]))
            //        {
            //            hahah1[8] = "不一致";
            //        } 
            //    }
                

            //}
                            //where bookTable.Field<int>("Price") > 180
            return haha;
        }

        //输出结果
        public DataTable WriteResultData()
        {
            RWFile file = new RWFile();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string jh = data.Rows[i]["jh"].ToString();
                string y = data.Rows[i]["hzby"].ToString();
                string x = data.Rows[i]["zzbx"].ToString();
                string kb = data.Rows[i]["bxhb"].ToString();

                if (x == "" || y == "" || x == "0"||y=="0")
                {
                    string sql = "select cyjh from daa01 where jh = '" + jh + "'";
                    DataTable tyjh = GetDataAsDataTable.GetDataReasult(sql);

                    DataRow[] local = localdata.Select("井号 = '" + tyjh.Rows[0]["cyjh"].ToString().Trim() + "'");

                    if (local != null)
                    {
                        foreach (DataRow row in local)
                        {
                            x = row[1].ToString();
                            y = row[2].ToString();
                            kb = row[3].ToString();
                        }
                    }
                }
                string bottom = string.Format("{0:0000.00}", float.Parse(data.Rows[i]["dyds"].ToString()));
                
                string jb = data.Rows[i]["mqjb"].ToString();

                DataRow newrow = result.NewRow();
                newrow[0] = jh;
                newrow[1] = y;
                newrow[2] = x;
                newrow[3] = bottom;
                newrow[4] = kb;
                newrow[5] = jb;
                result.Rows.Add(newrow);
            }
            return result;
        }

        public void WriteResult_test(String path)
        {
            RWFile file = new RWFile();
            file.Append(path, "jh        Y        X        BOTTOM        KB        JB\r\n");
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string jh = data.Rows[i]["井号"].ToString();
                string y = "";
                string x = "";

                y = data.Rows[i]["y坐标"].ToString();
                x = data.Rows[i]["x坐标"].ToString();


                string bottom = data.Rows[i]["底深"].ToString();
                string kb = data.Rows[i]["补心海拔"].ToString();
                string jb = data.Rows[i]["井别"].ToString();

                if (jh.Length < 10)
                {
                    jh = jh.PadRight(10);
                }
                if (y.Length < 20)
                {
                    y = y.PadRight(20);
                }
                if (x.Length < 20)
                {
                    x = x.PadRight(20);
                }
                if (bottom.Length < 15)
                {
                    bottom = bottom.PadRight(15);
                }
                if (kb.Length < 15)
                {
                    kb = kb.PadRight(15);
                }
                if (jb.Length < 3)
                {
                    jb = jb.PadRight(3);
                }

                string resultOutput = jh + "  " + x + "   " + y + "   " + bottom + "  " + kb + "  " + jb + "\r\n";
                file.Append(path, resultOutput);
            }
        }
    

        public void WriteResult(String path)
        {
            RWFile file = new RWFile();
            file.Append(path, "jh        Y        X        BOTTOM        KB        JB\r\n");
            for (int i = 0; i < haha.Rows.Count; i++)
            {
                string jh = haha.Rows[i]["井号"].ToString();
                string y = "";
                string x = "";
                if (haha.Rows[i]["一致性"].ToString() == "不一致")
                {
                    y = haha.Rows[i]["y坐标本地"].ToString();
                    x = haha.Rows[i]["x坐标本地"].ToString();
                }
                else
                {
                    y = haha.Rows[i]["y坐标"].ToString();
                    x = haha.Rows[i]["x坐标"].ToString();
                }
                
                string bottom = haha.Rows[i]["深度"].ToString();
                string kb = haha.Rows[i]["补心海拔"].ToString();
                string jb = haha.Rows[i]["井别"].ToString();

                if (jh.Length < 10)
                {
                    jh = jh.PadRight(10);
                }
                if (y.Length < 20)
                {
                    y = y.PadRight(20);
                }
                if (x.Length < 20)
                {
                    x = x.PadRight(20);
                }
                if (bottom.Length < 15)
                {
                    bottom = bottom.PadRight(15);
                }
                if (kb.Length < 15)
                {
                    kb = kb.PadRight(15);
                }
                if (jb.Length < 3)
                {
                    jb = jb.PadRight(3);
                }

                string resultOutput = jh + "  " + y + "   " + x + "   " + kb + "  " + bottom + "  " + jb + "\r\n";
                file.Append(path, resultOutput);
            }
        }
    }
}
