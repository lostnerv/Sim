using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using 建模数模.tools;
using System.IO;

namespace 数模建模
{
    class Wellprop
    {
        private DataTable data;
        private DataTable data1;
        private DataTable wellnum;
        private DataTable wellLithResult = new DataTable();
        private DataTable resultDatatable = new DataTable();
        private DataTable temp888 = new DataTable();
        private string wellSet = null;
        private DataTable result = new DataTable();
        private DataTable temp = new DataTable();

        public Wellprop(DataTable dt)
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
            lith.ColumnName = "岩性";//该列得名称 
            wellLithResult.Columns.Add(lith);
            
            resultDatatable = wellLithResult.Clone();
        }


        public DataTable getData()
        {
            //筛选daa074中的层属性
            string sql4 = "select jh as 井号,syds as 砂岩顶深,syds+syhd as 砂岩底深,yxhdds as 有效砂岩顶深,yxhdds+yxhd as 有效砂岩底深,ylsyhd as 一类砂岩厚度,yxhd as 有效厚度 from daa074 where jh in (" + wellSet + ")";
            data = GetDataAsDataTable.GetDataReasult(sql4);
            string sql5 = "select jh as 井号,jcds as 夹层顶深,jcds+jchd as 夹层底深 from daa054 where jh in(" + wellSet + ")";
            data1 = GetDataAsDataTable.GetDataReasult(sql5);

            foreach(DataRow row in wellnum.Rows)
            {
                string jh = row[0].ToString();
                DataRow[] matches = data.Select(" 井号= '" + jh + "'", "砂岩顶深 asc");
                DataRow[] matches1 = data1.Select(" 井号= '" + jh + "'", "夹层顶深 asc");

                for (int i = 0; i < matches.Length;i++ )
                {
                    //如果是未划砂岩
                    if (Convert.ToDouble(matches[i][5].ToString()) == 0 && Convert.ToDouble(matches[i][6].ToString()) == 0)
                    {
                        DataRow newrow = wellLithResult.NewRow();
                        newrow[0] = jh;
                        newrow[1] = matches[i][1];
                        newrow[2] = "3";
                        wellLithResult.Rows.Add(newrow);

                        DataRow newrow1 = wellLithResult.NewRow();
                        newrow1[0] = jh;
                        newrow1[1] = matches[i][2];
                        newrow1[2] = "0";
                        wellLithResult.Rows.Add(newrow1);
                    }
                    //如果有效砂岩和有效厚度为0，直接作为纯砂岩
                    else if (Convert.ToDouble(matches[i][3].ToString()) == 0 && Convert.ToDouble(matches[i][4].ToString()) == 0)
                    {
                        DataRow newrow = wellLithResult.NewRow();
                        newrow[0] = jh;
                        newrow[1] = matches[i][1];
                        newrow[2] = "2";
                        wellLithResult.Rows.Add(newrow);

                        DataRow newrow1 = wellLithResult.NewRow();
                        newrow1[0] = jh;
                        newrow1[1] = matches[i][2];
                        newrow1[2] = "0";
                        wellLithResult.Rows.Add(newrow1);
                    }
                    else//为纯砂岩、有效砂岩时
                    {
                        //System.Console.WriteLine("jh:" + matches[i][0].ToString());
                        //System.Console.WriteLine("1:" + matches[i][1].ToString());
                        //System.Console.WriteLine("2:" + matches[i][2].ToString());
                        //System.Console.WriteLine("3:" + matches[i][3].ToString());
                        //System.Console.WriteLine("4:" + matches[i][4].ToString());
                        //System.Console.WriteLine("5:" + matches[i][5].ToString());
                        //System.Console.WriteLine("6:" + matches[i][6].ToString());
                        //纯砂岩顶深
                        DataRow newrow = wellLithResult.NewRow();
                        newrow[0] = jh;
                        newrow[1] = Convert.ToDouble(matches[i][1].ToString());
                        newrow[2] = "2";
                        //wellLithResult.Rows.Add(newrow);

                        //纯砂岩底深
                        DataRow newrow1 = wellLithResult.NewRow();
                        newrow1[0] = jh;
                        newrow1[1] = Convert.ToDouble(matches[i][2].ToString());
                        newrow1[2] = "0";

                        //有效砂岩顶深
                        DataRow newrow2 = wellLithResult.NewRow();
                        newrow2[0] = jh;
                        newrow2[1] = Convert.ToDouble(matches[i][3].ToString());
                        newrow2[2] = "1";

                        //如果纯砂岩的顶深等于有效砂岩顶深，纯砂岩顶深岩性变为1
                        if (Convert.ToDouble(newrow[1]) == Convert.ToDouble(newrow2[1]))
                        {
                            newrow[0] = jh;
                            newrow[1] = Convert.ToDouble(matches[i][3].ToString());
                            newrow[2] = "1";
                        } 

                         //有效砂岩底深
                        DataRow newrow3 = wellLithResult.NewRow();
                        newrow3[0] = jh;
                        newrow3[1] = Convert.ToDouble(matches[i][4].ToString());
                        newrow3[2] = "2";

                        //如果纯砂岩底深等于有效砂岩底深，有效砂岩底深变为0
                        if (Convert.ToDouble(newrow3[1]) == Convert.ToDouble(newrow1[1]))
                        {
                            newrow3[0] = jh;
                            newrow3[1] = Convert.ToDouble(matches[i][4].ToString());
                            newrow3[2] = "0";
                        } 

                        //wellLithResult.Rows.Add(newrow);
                       
                        for (int j = 0; j < matches1.Length; j++)
                        {
                            //夹层顶深
                            DataRow newrow4 = wellLithResult.NewRow();
                            newrow4[0] = jh;
                            newrow4[1] = Convert.ToDouble(matches1[j][1].ToString());
                            newrow4[2] = "4";

                            //夹层底深
                            DataRow newrow5 = wellLithResult.NewRow();
                            newrow5[0] = jh;
                            newrow5[1] = Convert.ToDouble(matches1[j][2].ToString());
                            newrow5[2] = "4";

                            //如果夹层在砂岩内，废话夹层一定在砂岩内部
                            if (Convert.ToDouble(newrow4[1]) >= Convert.ToDouble(newrow[1]) && Convert.ToDouble(newrow5[1]) <= Convert.ToDouble(newrow1[1]))
                            {
                                //如果在有效砂岩内部
                                if (Convert.ToDouble(newrow4[1]) >= Convert.ToDouble(newrow2[1]) && Convert.ToDouble(newrow5[1]) <= Convert.ToDouble(newrow3[1]))
                                {
                                    //
                                    newrow4[2] = "4";
                                    newrow5[2] = "1";
                                    //如果夹层顶深等于有效砂岩顶深
                                    if (Convert.ToDouble(newrow4[1]) == Convert.ToDouble(newrow2[1]))
                                    {
                                        newrow2[2] = "4";
                                        newrow4[2] = "4";
                                    }
                                    //如果夹层底深等于有效砂岩底深
                                    if (Convert.ToDouble(newrow5[1]) == Convert.ToDouble(newrow3[1]))
                                    {
                                        newrow5[2] = "2";
                                        newrow3[2] = "2";
                                    }

                                    wellLithResult.Rows.Add(newrow4);
                                    wellLithResult.Rows.Add(newrow5);
                                }
                                else//如果不在有效砂岩内部
                                {
                                    //如果夹层顶深等于有效砂岩底深
                                    if (Convert.ToDouble(newrow4[1]) == Convert.ToDouble(newrow3[1]))
                                    {
                                        newrow3[2] = "4";
                                        newrow4[2] = "4";
                                    }
                                    //如果夹层底深等于有效砂岩顶深
                                    if (Convert.ToDouble(newrow5[1]) == Convert.ToDouble(newrow2[1]))
                                    {
                                        newrow5[2] = "1";
                                        newrow2[2] = "1";
                                    }

                                    wellLithResult.Rows.Add(newrow4);
                                    wellLithResult.Rows.Add(newrow5);
                                }
                            }

                        }

                        if (Convert.ToDouble(newrow[1]) == Convert.ToDouble(newrow2[1]))
                        {
                            wellLithResult.Rows.Add(newrow2);
                        }
                        else
                        {
                            wellLithResult.Rows.Add(newrow);
                            wellLithResult.Rows.Add(newrow2);
                        }
                        if (Convert.ToDouble(newrow1[1]) == Convert.ToDouble(newrow3[1]))
                        {
                            wellLithResult.Rows.Add(newrow3);
                        }
                        else
                        {
                            wellLithResult.Rows.Add(newrow1);
                            wellLithResult.Rows.Add(newrow3);
                        }
                        //wellLithResult.Rows.Add(newrow);
                        //wellLithResult.Rows.Add(newrow1);
                        //wellLithResult.Rows.Add(newrow2);
                        //wellLithResult.Rows.Add(newrow3);
                    }         
                }
            }
            
            temp888 = wellLithResult.Clone();
          
           
            //string wellnum = "8F21-S30";
            foreach (DataRow tempRow in wellnum.Rows)
            {
                double temp_v = -1;
                List<double> hahah = new List<double>();

                DataRow[] temp1 = wellLithResult.Select(" 井号= '" + tempRow[0] + "'", "深度 asc");
                foreach (DataRow row in temp1)
                {
                    DataRow rowhaha = temp888.NewRow();

                    if (temp_v != Convert.ToDouble(row[1].ToString()))
                    {
                        rowhaha[0] = row[0];
                        rowhaha[1] = row[1];
                        rowhaha[2] = row[2];

                        temp_v = Convert.ToDouble(row[1].ToString());
                        temp888.Rows.Add(rowhaha);
                    }
                    else
                    {
                        hahah.Add(temp_v);
                    }
                }
                
                for (int i = 0; i < temp888.Rows.Count; i++)
                {
                    for (int j = 0; j < hahah.Count; j++)
                    {
                        if (Convert.ToDouble(temp888.Rows[i][1].ToString()) == hahah[j])
                        {
                            temp888.Rows.RemoveAt(i);
                            temp888.AcceptChanges();
                        }
                    }
                }
            }
            return temp888;
        }

        //public void getData()
        //{
        //    RWFile file = new RWFile();

        //    DataColumn wellnumColumn = new DataColumn();
        //    wellnumColumn.DataType = System.Type.GetType("System.String");//该列的数据类型 
        //    wellnumColumn.ColumnName = "井号";//该列得名称 
        //    result.Columns.Add(wellnumColumn);

        //    DataColumn depColumn = new DataColumn();
        //    depColumn.DataType = System.Type.GetType("System.Double");//该列的数据类型 
        //    depColumn.ColumnName = "深度";//该列得名称 
        //    result.Columns.Add(depColumn);

        //    DataColumn hdColumn = new DataColumn();
        //    hdColumn.DataType = System.Type.GetType("System.Double");//该列的数据类型 
        //    hdColumn.ColumnName = "厚度";//该列得名称 
        //    result.Columns.Add(hdColumn);

        //    DataColumn valColumn = new DataColumn();
        //    valColumn.DataType = System.Type.GetType("System.String");//该列的数据类型 
        //    valColumn.ColumnName = "岩性值";
        //    result.Columns.Add(valColumn);

        //    DataColumn val1Column = new DataColumn();
        //    val1Column.DataType = System.Type.GetType("System.String");//该列的数据类型 
        //    val1Column.ColumnName = "岩性值1";
        //    result.Columns.Add(val1Column);

        //    DataTable tempresult = result.Clone();
        //    //筛选daa074中的层属性
        //    string sql4 = "SELECT jh,syds,syhd,ylsyds,yxhd,syds+syhd as hd FROM daa074 where jh in(" + wellSet + ")";
        //    data = GetDataAsDataTable.GetDataReasult(sql4);

        //    //增加岩性列
        //    DataColumn yxColumn1 = new DataColumn();
        //    yxColumn1.DataType = System.Type.GetType("System.String");//该列的数据类型 
        //    yxColumn1.ColumnName = "yx";//该列得名称 
        //    data.Columns.Add(yxColumn1);


        //    foreach (DataRow row in data.Rows)
        //    {
        //        //1:有效砂岩，有效厚度不为0
        //        //2:纯砂岩（有效厚度为0，一类砂岩厚度不为0）
        //        //3:划砂岩（有效厚度为0，一类砂岩厚度为0)
        //        //0:泥岩
        //        //一类砂岩厚度索引是3，有效砂岩厚度是4
        //        if (Convert.ToDouble(row[4]) != 0)
        //        {
        //            row[6] = "1";
        //        }
        //        else if (Convert.ToDouble(row[3]) == 0 && Convert.ToDouble(row[4]) == 0)
        //        {
        //            row[6] = "3";
        //        }
        //        else if (Convert.ToDouble(row[3]) != 0 && Convert.ToDouble(row[4]) == 0)
        //        {
        //            row[6] = "2";
        //        }
        //        else
        //        {
        //            row[6] = "0";
        //        }
        //    }

        //    foreach (DataRow row in data.Rows)
        //    {
        //        DataRow addRow = result.NewRow();
        //        addRow[0] = row[0];//井号
        //        addRow[1] = row[1];//深度
        //        addRow[2] = row[5];//厚度
        //        addRow[3] = row[6];//岩性
        //        addRow[4] = row[6];//岩性1
        //        result.Rows.Add(addRow);
        //    }


        //    //筛选夹层的属性
        //    string sql5 = "select jh,jcds as syds,jchd as yxhd,jcds+jchd as hd from daa054 where jh in(" + wellSet + ")";
        //    data1 = GetDataAsDataTable.GetDataReasult(sql5);

        //    //添加岩性列
        //    DataColumn yxColumn = new DataColumn();
        //    yxColumn.DataType = System.Type.GetType("System.String");//该列的数据类型 
        //    yxColumn.ColumnName = "yx";//该列得名称 
        //    yxColumn.DefaultValue = "4";
        //    data1.Columns.Add(yxColumn);

        //    foreach (DataRow row in data1.Rows)
        //    {
        //        DataRow addRow = result.NewRow();
        //        addRow[0] = row[0];//井号
        //        addRow[1] = row[1];//深度
        //        addRow[2] = row[3];//厚度
        //        addRow[3] = row[4];//岩性
        //        addRow[4] = row[4];//岩性1
        //        result.Rows.Add(addRow);
        //    }

        //    foreach (DataRow tempRow in wellnum.Rows)
        //    {
        //        string jh = tempRow[0].ToString().Trim();
        //        DataRow[] matches = result.Select(" 井号= '" + jh + "'", "深度 asc");
        //        for (int i = 0; i < matches.Length; i++)
        //        {
        //           if (i != matches.Length -1)
        //           {
        //               //如果是非夹层
        //               if (matches[i][3] != "4")
        //               {
        //                   //非夹层的尾端就是0
        //                   matches[i][4] = "0";
        //                   if (Convert.ToDouble(matches[i][2]) == Convert.ToDouble(matches[i + 1][1]))
        //                   {
        //                       //System.Console.WriteLine("in" + matches[i][2].ToString());
        //                       matches[i][4] = matches[i+1][3];
        //                   }
        //                   //如果下层不是夹层
        //                   if (matches[i+1][3] != "4")
        //                   {
        //                       //直接将尾端设置为0
        //                       //matches[i][4] = "0";
        //                   }
        //                   else//如果下层是夹层
        //                   {
        //                       int j = i+1;
        //                       //将所有的非夹层的底深设置为非夹层的岩性
        //                       while (matches[j][3] == "4")
        //                       {
        //                           matches[j][4] = matches[i][3];
        //                           if (j + 1 != matches.Length)
        //                           {
        //                               j++;
        //                           }
        //                           else
        //                           {
        //                               break;
        //                           }
        //                       }
        //                   }
        //               }
        //               else
        //               {
        //                   continue;
        //               }
        //           }
        //        }

        //        if (matches.Length >0)
        //        {
        //            double tp = Convert.ToDouble(matches[0][2]);
        //            foreach (DataRow wellLithRow in matches)
        //            {
        //                DataRow newlith = wellLithResult.NewRow();
        //                DataRow newlith1 = wellLithResult.NewRow();



        //                if (tp != Convert.ToDouble(wellLithRow[1]))
        //                {
        //                    newlith[0] = wellLithRow[0];
        //                    newlith[1] = wellLithRow[1];
        //                    newlith[2] = wellLithRow[3];
        //                    wellLithResult.Rows.Add(newlith);
        //                }


        //                newlith1[0] = wellLithRow[0];
        //                newlith1[1] = wellLithRow[2];
        //                newlith1[2] = wellLithRow[4];
        //                tp = Convert.ToDouble(wellLithRow[2]);
        //                wellLithResult.Rows.Add(newlith1);
        //            }
        //        }
                
        //        //foreach (DataRow row in wellLithResult.Rows)
        //        //{
        //        //    System.Console.WriteLine(row[0] + " " + row[1] + " " + row[2]);
        //        //}
        //    }
        //}

        public DataTable WriteResultData()
        {
            foreach (DataRow tempRow in wellnum.Rows)
            {
                string jh = tempRow[0].ToString().Trim();

                DataRow[] matches = temp888.Select(" 井号= '" + jh + "'", "深度 asc");

                if (matches.Length > 0)
                {
                    double start = Convert.ToDouble(matches[0][1]);
                    double depth = start;
                    string depthString = null;
                    string value = null;
                    int j = 0;
                    int i = 0;
                    while (depth < Convert.ToDouble(matches[matches.Length - 1][1]))
                    {
                        j++;
                        depth = start + 0.02 * j;
                        if (i < matches.Length && depth < Convert.ToDouble(matches[matches.Length - 1][1]))
                        {
                            //如果大于下一层的值，岩性转为下层
                            if (depth >= Convert.ToDouble(matches[i + 1][1]))
                            {
                                value = matches[i + 1][2].ToString();
                                i++;
                            }
                            else//如果不大于下层则为当前值
                            {
                                value = matches[i][2].ToString();
                            }
                        }
                        else
                        {
                            depth = Convert.ToDouble(matches[matches.Length - 1][1]);
                            value = matches[matches.Length - 1][2].ToString();
                        }

                        depthString = depth.ToString();

                        if (depthString.Length < 10)
                        {
                            depthString = depthString.PadRight(10);
                        }

                        if (value.Length < 2)
                        {
                            value = value.PadRight(2);
                        }

                        DataRow row1 = resultDatatable.NewRow();
                        row1[0] = jh;
                        row1[1] = depthString;
                        row1[2] = value;
                        resultDatatable.Rows.Add(row1);
                    }
                }
            }
            return resultDatatable;
        }

        public void WriteResult(String path)
        {
            RWFile file = new RWFile();
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                foreach (DataRow tempRow in wellnum.Rows)
                {
                    string jh = tempRow[0].ToString().Trim();
                    DataRow[] matches = resultDatatable.Select(" 井号= '" + jh + "'", "深度 asc");
                    file.Append(path + "\\" + jh + ".txt", "深度  岩性值\r\n");
                    foreach (DataRow dt in matches)
                    {
                        string dep = dt[1].ToString();
                        string val = dt[2].ToString();
                        if (dep.Length < 10)
                        {
                            dep = dep.PadRight(10);
                        }

                        if (val.Length < 2)
                        {
                            val = val.PadRight(2);
                        }
                        string resultline = dep + "   " + val + "\r\n";
                        file.Append(path + "\\" + jh + ".txt", resultline);
                    }
                }
            }
            else
            {
                foreach (DataRow tempRow in wellnum.Rows)
                {
                    string jh = tempRow[0].ToString().Trim();
                    DataRow[] matches = resultDatatable.Select(" 井号= '" + jh + "'", "深度 asc");
                    file.Append(path + "\\" + jh + ".txt", "深度  岩性值\r\n");
                    foreach (DataRow dt in matches)
                    {
                        string dep = dt[1].ToString();
                        string val = dt[2].ToString();
                        if (dep.Length < 10)
                        {
                            dep = dep.PadRight(10);
                        }

                        if (val.Length < 2)
                        {
                            val = val.PadRight(2);
                        }
                        string resultline = dep + "   " + val + "\r\n";
                        file.Append(path + "\\" + jh+".txt", resultline);
                    }
                }
            } 
        }
    }
}
