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
     * 沉积相带图
     * **/
    class DFDPoly
    {
        public ArrayList clist = new ArrayList();
        public ArrayList colorList = new ArrayList();       

        public DFDPoly()
        {

        }
        //提取数据
        public DataTable readPRT(string filepath)
        {
            DataTable dtresult = new DataTable();
            Boolean startFlag = false;
            Boolean hasPointFlag = false;
            clist.Add(0);//记录多边形起始点
           // int endNum = -1;
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");//该列的数据类型 
            column.ColumnName = "x";
            dtresult.Columns.Add(column);

            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.Double");//该列的数据类型 
            column2.ColumnName = "y";
            dtresult.Columns.Add(column2);

            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Trim() != "")
                {
                    if (line.Contains("COLOR"))//COLOR 下面有font 也有pline 0不填色
                    {
                        String nextline = sr.ReadLine();//下一行
                        if (nextline.Contains("Pline"))//下一行表头行
                        {
                            startFlag = true;
                            string[] sArray = line.Split(' ');//取color
                            if (2 == sArray.Length)
                            {
                                colorList.Add(sArray[1]);
                            }
                            continue;
                        }
                    }
                    else if (startFlag)
                    {
                        line = line.Trim();
                      // if ("S" == line || "" == line)
                      //  {
                      //      break;
                      //  }
                        string[] sArray=line.Split(',');
                        if (2==sArray.Length )
                        {
                            hasPointFlag = true;
                            DataRow row = dtresult.NewRow();
                            //System.Console.WriteLine(sArray[0] + "," );
                            row["x"] = Convert.ToDouble(sArray[0]);
                            row["y"] = Convert.ToDouble(sArray[1]);
                            dtresult.Rows.Add(row);
                        }
                        else//多边形结束点 或者 W 1.4280
                        {
                            if(hasPointFlag)
                            {
                                clist.Add(dtresult.Rows.Count);//新多边形从第count行开始 ，第一个list值是0，会多记录一个结尾
                                startFlag = false;
                            }
                            hasPointFlag = false;                             
                        }
                    }
                }
            }
            sr.Close();
            return dtresult;
        }
    }
}
