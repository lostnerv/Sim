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
        public DataTable readFile(string filepath,String ch)
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
                            nextlineHead=""+Convert.ToInt32(nextlineHead);
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
                            else if (arrlist.Count>0)//层号不匹配
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
    }
}
