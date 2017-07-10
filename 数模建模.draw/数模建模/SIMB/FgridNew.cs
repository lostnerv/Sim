using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using 建模数模.tools;
using System.Text.RegularExpressions;
using System.Collections;

namespace 数模建模.SIMB
{
    /**
     * 饱和度图 
     * 读取fgrid文件
     *
     **/
    class FgridNew
    {
        public FgridNew()
        {

        }
        //提取数据
        public DataTable readFile(string filepath, String ch)
        {
            DataTable dtresult = new DataTable();
            Boolean startFlag = false;
            ArrayList arrlist = new ArrayList();
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "x";
            dtresult.Columns.Add(column);
            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.String");
            column2.ColumnName = "y";
            dtresult.Columns.Add(column2);

            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                // if (line.Trim() != "")
                //  {
                if (line.Contains("'COORDS  '"))
                {
                    string nextlineHead;
                    if (((nextlineHead = sr.ReadLine()) != null))
                    {
                        //System.Console.WriteLine(nextlineHead);
                        nextlineHead = nextlineHead.Substring(24, 12);//取出层号
                        nextlineHead = "" + Convert.ToInt32(nextlineHead);
                        //System.Console.WriteLine(nextlineHead);
                        if (ch.Equals(nextlineHead))
                        {
                            sr.ReadLine();//0
                            line = sr.ReadLine();//CORNERS
                            // if (line.Contains("'CORNERS '"))//下一行开始读取新空间8点
                            // {
                            int pointCount = 0;//第几行的点
                            startFlag = true;
                            while (startFlag)
                            {
                                string nextline;
                                if (((nextline = sr.ReadLine()) != null))
                                {
                                    // System.Console.WriteLine(nextline);
                                    pointCount++;//第几行的点
                                    if (!nextline.Contains("COORDS") && pointCount <= 3)//总共6行 6*3 底部坐标4点不要
                                    {
                                        nextline = nextline.Replace("   ", "*");
                                        nextline = nextline.Replace("  -", "*");//负值是错的都出国了
                                        nextline = nextline.Substring(1, nextline.Length - 1);
                                        string[] sArray = nextline.Split('*');//取出空间八点坐标
                                        foreach (string a in sArray)
                                        {
                                            arrlist.Add(a);
                                        }
                                    }

                                    else
                                        startFlag = false;
                                }
                                else
                                    break;
                            }
                            //  }
                        }
                        else if (arrlist.Count > 0)//层号不匹配
                        {
                            break;
                        }
                    }
                }
            }
            sr.Close();
            for (int startNum = 0; startNum < arrlist.Count - 2; startNum = startNum + 3)
            {

                DataRow row = dtresult.NewRow();
                row["x"] = Convert.ToDouble(arrlist[startNum]);
                row["y"] = Convert.ToDouble(arrlist[startNum + 1]);
                //System.Console.WriteLine("x," + Convert.ToDouble(arrlist[startNum]));
                //if (1 == (startNum+1) % 3)
                dtresult.Rows.Add(row);

            }
            /*
            foreach (DataRow rowaa in dtresult.Rows)
            {
                System.Console.WriteLine(rowaa[0].ToString());
                System.Console.WriteLine(rowaa[1].ToString());
                System.Console.WriteLine("-------------");
            }*/
            return dtresult;
        }
        /*取井错误
        public DataTable findinWell(double minX,double maxX,double minY,double maxY)
        {
            DataTable data = new DataTable();
            String sql = "select s.well_desc jh,geo_offset_east_bh jhx,geo_offset_north_bh jhy,"
                    + " case when well_purpose like '1%' then 'PROD' when well_purpose like '3%' then 'INJ' else  to_char(well_purpose) end jb"
                    + " from cd_wellbore_t b"
                    + " left join cd_well_source s on s.well_id=b.well_id"
                    + " where geo_offset_east_bh is not null and geo_offset_north_bh is not null"
                    + " and (well_purpose like '1%' or well_purpose like '3%' )"
                    + " and geo_offset_east_bh between " + minX + " and " + maxX + "  "
                    + " and geo_offset_north_bh between " + minY + " and " + maxY + "  ";
            //System.Console.WriteLine(sql);
          return GetDataAsDataTable.GetDataReasult1(sql); 
        }*/

        // 2017年7月10日 12:14:45
        // 计算所有层dz
        public double[] readDzFromFgrid(string filepath, int[] tablesize)
        {
            double[] dzs = new double[tablesize[0] * tablesize[1] * tablesize[2]];
            double[] grid24 = new double[24];
            Boolean startFlag = false;
            int dzCount = 0;
            //ArrayList arrlist = new ArrayList();
            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line="";
            while ((line = sr.ReadLine()) != null)
            {
                // if (line.Trim() != "")
                //  {
                if (line.Contains("'COORDS  '"))
                {
                    string nextlineHead;
                    if (((nextlineHead = sr.ReadLine()) != null))
                    {
                        //System.Console.WriteLine(nextlineHead);
                        nextlineHead = nextlineHead.Substring(24, 12);//取出层号
                        nextlineHead = "" + Convert.ToInt32(nextlineHead);

                        sr.ReadLine();//0
                        line = sr.ReadLine();//CORNERS   24 'REAL'
                        int pointCount = 0;//第几行的点
                        startFlag = true;
                        //Console.WriteLine(arrlist.Count);
                        while (startFlag)
                        {
                            string nextline;
                            if (((nextline = sr.ReadLine()) != null)) // 移动到八点行
                            {
                                pointCount++;//第几行的点
                                // Console.WriteLine(pointCount);
                                if (!nextline.Contains("COORDS") && pointCount <= 6)//总共6行 6*4
                                {
                                    //Console.WriteLine(nextline);
                                    nextline = nextline.Replace("   ", "*");
                                    nextline = nextline.Replace("  -", "*");//负值是错的都出国了
                                    nextline = nextline.Substring(1, nextline.Length - 1);
                                    string[] sArray = nextline.Split('*');//取出空间八点坐标
                                    int grid24Count = 0;
                                    foreach (string onePoint in sArray)
                                    {
                                        //Console.WriteLine(grid24Count + 4 * (pointCount - 1));
                                        grid24[grid24Count + 4*(pointCount-1)] = Convert.ToDouble(onePoint);
                                        //arrlist.Add(a);
                                        grid24Count++;
                                    }
                                }
                                else// 下一个方格
                                {
                                    startFlag = false;
                                }
                                if (6 == pointCount)//防止少读一行 COORDS
                                {
                                    double dz1 = grid24[14]- grid24[2];
                                    double dz2 = grid24[17] - grid24[5];
                                    double dz3 = grid24[20] - grid24[8];
                                    double dz4 = grid24[23] - grid24[11];
                                    // 2017年7月10日 11:33:23 dz
                                    dzs[dzCount] = (dz1 + dz2 + dz3 + dz4) / 4;
                                    dzCount++;
                                    break;
                                }
                            }
                            else
                                break;
                        }
                    }
                }
            }
            sr.Close();
            
           // for (int startNum = 0; startNum < arrlist.Count - 23; startNum = startNum + 24)
           // {
           // double dz1 = Convert.ToDouble(arrlist[startNum + 14]) - Convert.ToDouble(arrlist[startNum + 2]);
          //  double dz2 = Convert.ToDouble(arrlist[startNum + 17]) - Convert.ToDouble(arrlist[startNum + 5]);
          //  double dz3 = Convert.ToDouble(arrlist[startNum + 20]) - Convert.ToDouble(arrlist[startNum + 8]);
          //  double dz4 = Convert.ToDouble(arrlist[startNum + 23]) - Convert.ToDouble(arrlist[startNum + 11]);
            // 2017年7月10日 11:33:23 dz
          //  dzs[dzCount] = (dz1 + dz2 + dz3 + dz4) / 4;
          //  dzCount++;
            //}
            return dzs;
        }
    }
}
