using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using 建模数模.tools;
using System.IO;

namespace 数模建模
{
    class WellSand
    {
        private DataTable data;
        private DataTable wellnum;
        private string wellSet = null;
        private DataTable result = new DataTable();
        private DataTable wellLithResult = new DataTable();
        private DataTable resultDatatable = new DataTable();
        
        public WellSand(DataTable dt)
        {
            wellnum = dt;
            foreach (DataRow tempRow in wellnum.Rows)
            {
                wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
            }
            wellSet = wellSet.Substring(0, wellSet.Length - 1);

            DataColumn lithnum = new DataColumn();
            lithnum.DataType = System.Type.GetType("System.String");//该列的数据类型 
            lithnum.ColumnName = "井号";//该列得名称 
            wellLithResult.Columns.Add(lithnum);

            DataColumn lithdep = new DataColumn();
            lithdep.DataType = System.Type.GetType("System.Double");//该列的数据类型 
            lithdep.ColumnName = "深度";//该列得名称 
            wellLithResult.Columns.Add(lithdep);

            DataColumn lith = new DataColumn();
            lith.DataType = System.Type.GetType("System.String");//该列的数据类型 
            lith.ColumnName = "NTG";//该列得名称 
            wellLithResult.Columns.Add(lith);

            resultDatatable = wellLithResult.Clone();
        }

       

        public DataTable getData(string input = "0.1")
        {
            //DataColumn wellnumColumn = new DataColumn();
            //wellnumColumn.DataType = System.Type.GetType("System.String");//该列的数据类型 
            //wellnumColumn.ColumnName = "井号";//该列得名称 
            //result.Columns.Add(wellnumColumn);

            //DataColumn depColumn = new DataColumn();
            //depColumn.DataType = System.Type.GetType("System.Double");//该列的数据类型 
            //depColumn.ColumnName = "深度";//该列得名称 
            //result.Columns.Add(depColumn);

            //DataColumn hdColumn = new DataColumn();
            //hdColumn.DataType = System.Type.GetType("System.Double");//该列的数据类型 
            //hdColumn.ColumnName = "厚度";//该列得名称 
            //result.Columns.Add(hdColumn);

            //DataColumn valColumn = new DataColumn();
            //valColumn.DataType = System.Type.GetType("System.String");//该列的数据类型 
            //valColumn.ColumnName = "岩性值";
            //result.Columns.Add(valColumn);

            //DataColumn val1Column = new DataColumn();
            //val1Column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            //val1Column.ColumnName = "岩性值1";
            //result.Columns.Add(val1Column);

            DataTable tempresult = result.Clone();
            //筛选daa074中的层属性
            string sql4 = "SELECT jh as 井号,syds as 砂岩顶深,syhd+syds 砂岩底深,yxhdds as 有效厚度顶深,yxhdds+yxhd as 有效厚度底深,ylsyds as 一类砂岩顶深,yxhd 有效厚度 from daa074 where jh in(" + wellSet + ")";
            data = GetDataAsDataTable.GetDataReasult(sql4);

            foreach (DataRow tempRow in wellnum.Rows)
            {
                DataRow[] matches = data.Select(" 井号= '" + tempRow[0].ToString() + "'", "砂岩顶深 asc");

                for (int i = 0 ; i<matches.Length; i++)
                {
                    DataRow newrow = wellLithResult.NewRow();
                    DataRow newrow1 = wellLithResult.NewRow();
                    DataRow newrow2 = wellLithResult.NewRow();
                    DataRow newrow3 = wellLithResult.NewRow();

                    if (Convert.ToDouble(matches[i][3]) == 0 && Convert.ToDouble(matches[i][4]) == 0)
                    {
                        newrow[0] = tempRow[0].ToString();
                        newrow[1] = Convert.ToDouble(matches[i][1].ToString());
                        newrow[2] = input;

                        newrow1[0] = tempRow[0].ToString();
                        newrow1[1] = Convert.ToDouble(matches[i][2].ToString());
                        newrow1[2] = input;

                        wellLithResult.Rows.Add(newrow);
                        wellLithResult.Rows.Add(newrow1);
                    }

                    if (Convert.ToDouble(matches[i][1]) < Convert.ToDouble(matches[i][3]) && Convert.ToDouble(matches[i][3]) != 0)
                    {
                        newrow[0] = tempRow[0].ToString();
                        newrow[1] = Convert.ToDouble(matches[i][1].ToString());
                        newrow[2] = "0";

                        newrow1[0] = tempRow[0].ToString();
                        newrow1[1] = Convert.ToDouble(matches[i][3].ToString());
                        newrow1[2] = "1";

                        wellLithResult.Rows.Add(newrow);
                        wellLithResult.Rows.Add(newrow1);
                    }
                    if (Convert.ToDouble(matches[i][2]) > Convert.ToDouble(matches[i][4]) && Convert.ToDouble(matches[i][4]) != 0)
                    {
                        newrow2[0] = tempRow[0].ToString();
                        newrow2[1] = Convert.ToDouble(matches[i][2].ToString());
                        newrow2[2] = "0";

                        newrow3[0] = tempRow[0].ToString();
                        newrow3[1] = Convert.ToDouble(matches[i][3].ToString());
                        newrow3[2] = "1";

                        wellLithResult.Rows.Add(newrow2);
                        wellLithResult.Rows.Add(newrow3);
                    }
                }
                
            }

            //增加岩性列
            //DataColumn yxColumn1 = new DataColumn();
            //yxColumn1.DataType = System.Type.GetType("System.String");//该列的数据类型 
            //yxColumn1.ColumnName = "yx";//该列得名称 
            //data.Columns.Add(yxColumn1);


            //foreach (DataRow row in data.Rows)
            //{
            //    //1:有效砂岩，有效厚度不为0
            //    //2:纯砂岩（有效厚度为0，一类砂岩厚度不为0）
            //    //3:划砂岩（有效厚度为0，一类砂岩厚度为0)
            //    //0:泥岩
            //    //一类砂岩厚度索引是3，有效砂岩厚度是4
            //    if (Convert.ToDouble(row[4]) != 0)
            //    {
            //        row[6] = "1";
            //    }
            //    else if (Convert.ToDouble(row[3]) == 0 && Convert.ToDouble(row[4]) == 0)
            //    {
            //        row[6] = "3";
            //    }
            //    else if (Convert.ToDouble(row[3]) != 0 && Convert.ToDouble(row[4]) == 0)
            //    {
            //        row[6] = "2";
            //    }
            //    else
            //    {
            //        row[6] = "0";
            //    }
            //}

            //foreach (DataRow row in data.Rows)
            //{
            //    DataRow addRow = result.NewRow();
            //    addRow[0] = row[0];//井号
            //    addRow[1] = row[1];//深度
            //    addRow[2] = row[5];//厚度
            //    addRow[3] = row[6];//岩性
            //    addRow[4] = row[6];//岩性1
            //    result.Rows.Add(addRow);
            //}


            //foreach (DataRow tempRow in wellnum.Rows)
            //{
            //    string jh = tempRow[0].ToString().Trim();
            //    DataRow[] matches = result.Select(" 井号= '" + jh + "'", "深度 asc");
            //    for (int i = 0; i < matches.Length; i++)
            //    {
                   
            //            if (matches[i][3].ToString() == "1")
            //            {
            //                matches[i][4] = "1";
            //            }
            //            else if (matches[i][3].ToString() == "2")
            //            {
            //                //System.Console.WriteLine(matches[i][0].ToString() + " " + matches[i][1].ToString() + " " + matches[i][2].ToString() + " " + matches[i][4].ToString()+" "+i.ToString());
            //                if (i != 0 && Convert.ToDouble(matches[i - 1][1]) < Convert.ToDouble(matches[i - 1][1]) && Convert.ToDouble(matches[i - 1][2]) < Convert.ToDouble(matches[i - 1][2]))
            //                {
            //                    matches[i].Delete();
            //                    matches[i].AcceptChanges();
            //                }
            //                else
            //                {
            //                    matches[i][4] = "0.1";
            //                }
            //            }
            //            else
            //            {
            //                matches[i][4] = "0";
            //            }
                    
            //    }
            //    if (matches.Length > 0)
            //    {
            //        foreach (DataRow wellLithRow in matches)
            //        {
            //            DataRow newlith1 = wellLithResult.NewRow();
            //            newlith1[0] = wellLithRow[0];
            //            newlith1[1] = wellLithRow[1];
            //            newlith1[2] = wellLithRow[4];
            //            wellLithResult.Rows.Add(newlith1);
            //        }
            //    }
            //}
            //return wellLithResult;
            return wellLithResult;
        }

        public void WriteResult(string path)
        {
            RWFile file = new RWFile();

                foreach (DataRow tempRow in wellnum.Rows)
                {
                    string jh = tempRow[0].ToString().Trim();
                    DataRow[] matches = wellLithResult.Select("井号= '" + jh + "'");

                    file.Append(path + "\\" + jh, "DEP NTG\r\n");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                        foreach (DataRow row in matches)
                        {

                            string depth = row[1].ToString();
                            string ntg = row[2].ToString();

                            if (depth.Length < 15)
                            {
                                depth = depth.PadRight(15);
                            }
                            if (ntg.Length < 2)
                            {
                                depth = ntg.PadRight(2);
                            }

                            string resultLine = depth + "   " + ntg + "\r\n";
                            file.Append(path + "\\" + jh, resultLine);
                        }

                    }
                    else
                    {
                        foreach (DataRow row in matches)
                        {

                            string depth = row[1].ToString();
                            string ntg = row[2].ToString();

                            if (depth.Length < 15)
                            {
                                depth = depth.PadRight(15);
                            }
                            if (ntg.Length < 2)
                            {
                                depth = ntg.PadRight(2);
                            }

                            string resultLine = depth + "   " + ntg + "\r\n";
                            file.Append(path + "\\" + jh, resultLine);
                        }
                    }
                }
            }
        }
    }
