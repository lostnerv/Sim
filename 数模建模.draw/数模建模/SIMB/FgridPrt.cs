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
     * 饱和度  
     * 读取PRT文件
     **/
    class FgridPrt
    {
        public int x = -1;
        public int y = -1;
        public int z = -1;
        public double minv = -1;
        public double maxv = -1;
        public double[] permx;//渗透率
        public double[] poro;//孔隙度
        public double[] ntgs;//净毛比
        public int[] fipnum, facies;
        public DataTable wellCoord = new DataTable();//井位网格坐标
        public DataTable wellStat = new DataTable();//井别记录
        public DataTable faultDt = new DataTable();
        public DataTable dzDt = new DataTable();//厚度
        public DataTable welspecs = new DataTable();//井口网格坐标
        public DataTable welspecsFakerWellCoord = new DataTable();//井口网格坐标
        public double[] dzs;// 净毛比  2017年8月10日 20:02:18
        public DataTable dxDt = new DataTable();// 算面积 2017年8月11日 09:03:58
        public DataTable dyDt = new DataTable();// 算面积 2017年8月11日 09:03:58
        public FgridPrt()
        {
            //井位网格坐标
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "jh";
            wellCoord.Columns.Add(column);

            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.Int32");
            column2.ColumnName = "x";
            wellCoord.Columns.Add(column2);

            DataColumn column3 = new DataColumn();
            column3.DataType = System.Type.GetType("System.Int32");
            column3.ColumnName = "y";
            wellCoord.Columns.Add(column3);

            DataColumn column4 = new DataColumn();
            column4.DataType = System.Type.GetType("System.Int32");
            column4.ColumnName = "z";
            wellCoord.Columns.Add(column4);

            DataColumn column5 = new DataColumn();
            column5.DataType = System.Type.GetType("System.Double");
            column5.ColumnName = "地层系数";
            wellCoord.Columns.Add(column5);
            // 2017年7月31日 13:15:30
            DataColumn column6 = new DataColumn();
            column6.DataType = System.Type.GetType("System.Double");
            column6.ColumnName = "渗透率级差";
            wellCoord.Columns.Add(column6);
            //井
            DataColumn columnStat = new DataColumn();
            columnStat.DataType = System.Type.GetType("System.String");
            columnStat.ColumnName = "jh";
            wellStat.Columns.Add(columnStat);

            DataColumn columnStat2 = new DataColumn();
            columnStat2.DataType = System.Type.GetType("System.String");
            columnStat2.ColumnName = "stat";
            wellStat.Columns.Add(columnStat2);

            //断层
            DataColumn columni = new DataColumn();
            columni.DataType = System.Type.GetType("System.Int32");
            columni.ColumnName = "i";
            faultDt.Columns.Add(columni);

            DataColumn columnj = new DataColumn();
            columnj.DataType = System.Type.GetType("System.Int32");
            columnj.ColumnName = "j";
            faultDt.Columns.Add(columnj);

            DataColumn columnk = new DataColumn();
            columnk.DataType = System.Type.GetType("System.Int32");
            columnk.ColumnName = "k";
            faultDt.Columns.Add(columnk);

            DataColumn columnDirect = new DataColumn();
            columnDirect.DataType = System.Type.GetType("System.String");
            columnDirect.ColumnName = "direct";
            faultDt.Columns.Add(columnDirect);

            //井口网格坐标
            DataColumn welspecscolumn1 = new DataColumn();
            DataColumn welspecscolumn2 = new DataColumn();
            DataColumn welspecscolumn3 = new DataColumn();
            welspecscolumn1.DataType = System.Type.GetType("System.String");
            welspecscolumn2.DataType = System.Type.GetType("System.String");
            welspecscolumn3.DataType = System.Type.GetType("System.String");
            welspecscolumn1.ColumnName = "jh";
            welspecscolumn2.ColumnName = "x";
            welspecscolumn3.ColumnName = "y";
            welspecs.Columns.Add(welspecscolumn1);
            welspecs.Columns.Add(welspecscolumn2);
            welspecs.Columns.Add(welspecscolumn3);
            //井位网格坐标 伪装
            DataColumn columnFaker = new DataColumn();
            columnFaker.DataType = System.Type.GetType("System.String");
            columnFaker.ColumnName = "jh";
            welspecsFakerWellCoord.Columns.Add(columnFaker);

            DataColumn columnFaker2 = new DataColumn();
            columnFaker2.DataType = System.Type.GetType("System.Int32");
            columnFaker2.ColumnName = "x";
            welspecsFakerWellCoord.Columns.Add(columnFaker2);

            DataColumn columnFaker3 = new DataColumn();
            columnFaker3.DataType = System.Type.GetType("System.Int32");
            columnFaker3.ColumnName = "y";
            welspecsFakerWellCoord.Columns.Add(columnFaker3);

            DataColumn columnFaker4 = new DataColumn();
            columnFaker4.DataType = System.Type.GetType("System.Int32");
            columnFaker4.ColumnName = "z";
            welspecsFakerWellCoord.Columns.Add(columnFaker4);

        }
        //分析大小 FGRID
        public int[] readFGRID(string filepath)
        {
            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;
            x = -1;//列数112
            y = -1;//行数116
            z = -1;//层数
            sr.ReadLine();
            line = sr.ReadLine();
            string[] sArray = line.Split(' ');
            foreach (string i in sArray)
            {
                if (x < 0 && i.ToString() != "")
                    x = int.Parse(i);
                else if (y < 0 && i.ToString() != "")
                    y = int.Parse(i);
                else if (z < 0 && i.ToString() != "")
                    z = int.Parse(i);
            }
            sr.Close();
            int[] result = { x, y, z };
            return result;
        }
        ///  2017年5月10日 09:30:51 
        ///  2017年8月10日 17:17:49
        /// <summary>
        ///   提取数据  饱和度
        ///  孔隙度 
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="ch"></param>
        /// <param name="soiltime"></param>
        /// <returns></returns>
        public DataTable readPRT(string filepath, String ch, String soiltime)
        {
            permx = new double[x * y * z];
            poro = new double[x * y * z];
            dzs = new double[x * y * z];
            ntgs = new double[x * y * z];

            DataTable dtresult = new DataTable();
            DataTable poroDT = new DataTable();
            DataTable ntgDT = new DataTable();
            Boolean startFlag = false;
            int startNum = -1;//起始i值
            // int endNum = -1;
            //规划Datatable大小
            for (int i = 1; i <= x; i++)
            {
                DataColumn column = new DataColumn();
                column.DataType = System.Type.GetType("System.Double");//该列的数据类型 
                column.ColumnName = "v" + i;
                dtresult.Columns.Add(column);
            }
            for (int i = 1; i <= y; i++)
            {
                DataRow row = dtresult.NewRow();
                //row[0] = "-----";
                dtresult.Rows.Add(row);

            }
            //规划Datatable大小
            for (int i = 1; i <= x; i++)
            {
                DataColumn column = new DataColumn();
                DataColumn column2 = new DataColumn();
                DataColumn column3 = new DataColumn();
                column.DataType = System.Type.GetType("System.Double");//该列的数据类型 
                column2.DataType = System.Type.GetType("System.Double");//该列的数据类型 
                column2.DataType = System.Type.GetType("System.Double");//该列的数据类型 
                column.ColumnName = "v" + i;
                column2.ColumnName = "v" + i;
                column3.ColumnName = "v" + i;
                dzDt.Columns.Add(column);
                poroDT.Columns.Add(column2);
                ntgDT.Columns.Add(column3);
            }
            for (int i = 1; i <= y; i++)
            {
                DataRow row = dzDt.NewRow();
                DataRow poroRow = poroDT.NewRow();
                DataRow ntgRow = ntgDT.NewRow();
                dzDt.Rows.Add(row);
                poroDT.Rows.Add(poroRow);
                ntgDT.Rows.Add(ntgRow);

            }

            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;

            while ((line = sr.ReadLine()) != null)
            {
               //line= line.Trim();
                if (line.Trim() != "")
                {
                    /* if (line.Contains("1: PORO"))//空隙度
                     {
                         int poroCount = 0;
                         while ((line = sr.ReadLine()) != null)
                         {
                             if (line.Contains("/"))
                             { break; }
                             int sp = line.IndexOf(":");
                             line = line.Substring(sp + 1);
                             string[] poros = line.Split(' ');
                             foreach (string tobeporo in poros)
                             {
                                 // System.Console.WriteLine(tobeporo);
                                 if (tobeporo != null && tobeporo.Contains("*"))
                                 {//解压缩
                                     string[] manyporos = tobeporo.Split('*');
                                     for (int i = 0; i < Convert.ToInt32(manyporos[0]); i++)
                                     {
                                         poro[poroCount] = Convert.ToDouble(manyporos[1]);
                                         poroCount++;
                                     }

                                 }
                                 else if (tobeporo != null && !"".Equals(tobeporo))
                                 {
                                     poro[poroCount] = Convert.ToDouble(tobeporo);
                                     poroCount++;
                                 }
                             }
                         }
                         //System.Console.WriteLine(poro[x * y * z - 2]);
                         //System.Console.WriteLine(poro[x*y*z-1]);
                     }*/
                    /*else if (line.Contains("1: PERMX"))//渗透率
                    {
                        int permxCount = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Contains("/"))
                            { break; }
                            int sp = line.IndexOf(":");
                            line = line.Substring(sp + 1);
                            string[] permxs = line.Split(' ');
                            foreach (string tobepermx in permxs)
                            {
                                if (tobepermx != null && tobepermx.Contains("*"))
                                {//解压缩
                                    string[] manypermxs = tobepermx.Split('*');
                                    for (int i = 0; i < Convert.ToInt32(manypermxs[0]); i++)
                                    {
                                        permx[permxCount] = Convert.ToDouble(manypermxs[1]);
                                        permxCount++;
                                    }

                                }
                                else if (tobepermx != null && !"".Equals(tobepermx))
                                {
                                    permx[permxCount] = Convert.ToDouble(tobepermx);
                                    permxCount++;
                                }
                            }
                        }
                    }*/
                    /* else if (line.Contains("COMPDAT")) //井号的网格坐标wellCoord
                     {
                         line = sr.ReadLine();
                         //System.Console.WriteLine(line);
                         int endl = line.IndexOf(" '", 7);//开关井状态前一位
                         line = line.Substring(5, endl - 5);
                         //System.Console.WriteLine(line);
                         endl = line.IndexOf("'");
                         String jh = line.Substring(0, endl);
                         //System.Console.WriteLine("jh:" + jh);
                         line = line.Substring(endl + 2, line.Length - endl - 2);
                         //System.Console.WriteLine("line:" + line);
                         string[] coordArray = line.Split(' ');
                         DataRow row = wellCoord.NewRow();
                         row["jh"] = jh;
                         row["x"] = Convert.ToDouble(coordArray[0]);
                         row["y"] = Convert.ToDouble(coordArray[1]);
                         row["z"] = Convert.ToDouble(coordArray[2]);
                         wellCoord.Rows.Add(row);
                     }
                     else if (line.Contains("WELSPECS"))//油井井别wellStat
                     {
                         line = sr.ReadLine();
                         line = line.Substring(5);
                         int endl = line.IndexOf("'");
                         line = line.Substring(0, endl);
                         DataRow row = wellStat.NewRow();
                         row["jh"] = line;
                         row["stat"] = "OIL";
                         wellStat.Rows.Add(row);
                     }
                     else if (line.Contains("WCONINJE"))//水井井别wellStat
                     {
                         line = sr.ReadLine();
                         line = line.Substring(5);
                         int endl = line.IndexOf("'");
                         line = line.Substring(0, endl);
                         DataRow row = wellStat.NewRow();
                         row["jh"] = line;
                         row["stat"] = "WATER";
                         wellStat.Rows.Add(row);
                     }*/
                    //  
                    // 2017年8月11日 09:01:55 原来用网格算面积 现在用dx*dy
                   /* if (line.Contains("DX ") && line.Contains(" AT"))//z厚度
                    {
                        dxDt = readPRTijk(line, sr, ch);
                    }
                    else if (0==line.Trim().IndexOf("DY ") && line.Contains(" AT"))//z厚度
                    {
                        dyDt = readPRTijk(line, sr, ch);
                    }
                    else*/ if (line.Contains("DZ ") && line.Contains(" AT"))//z厚度
                    {
                        bool findDzFlag = false;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (findDzFlag && line.Contains("**"))
                            {
                                break;
                            }
                            if (line.Contains("MINIMUM VALUE"))
                            {
                                findDzFlag = true;
                            }
                            else if (line.Contains("I,  J,  K) I="))
                            {
                                bool dzstartFlag = false;
                                int startNumDz = int.Parse(line.Substring(15, 3).Trim());
                                while ((line = sr.ReadLine()) != null)
                                {
                                    if (("".Equals(line) || line.Contains("**")) && dzstartFlag)
                                    {
                                        break;
                                    }
                                    else if (line.Length > 0)
                                    {
                                        dzstartFlag = true;
                                        int k = 0;//一行的循环count i
                                        int rowNum = -1;
                                        int filech = -1;
                                        rowNum = int.Parse(line.Substring(4, 3).Trim());//行号j                                        
                                        filech = int.Parse(line.Substring(8, 3).Trim());//层号k
                                        if (ch.Equals("" + filech))
                                        {
                                            string[] sArray = line.Substring(12).Split(' ');
                                            if (sArray.Length > 2)
                                            {
                                                foreach (string i in sArray)
                                                {
                                                    if (i != "" && "-----" != i)
                                                    {
                                                        double val = Convert.ToDouble(i.Replace("*", "."));
                                                        dzDt.Rows[rowNum - 1][startNumDz + k - 1] = val;
                                                        k++;
                                                    }
                                                    else if ("-----" == i)
                                                    {
                                                        dzDt.Rows[rowNum - 1][startNumDz + k - 1] = 0;
                                                        k++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        // 2017年8月10日 20:03:09
                        int chInt = Convert.ToInt32(ch);
                        int POROcount = (chInt - 1) * x * y;
                        for (int j = 0; j < y; j++)
                        {
                            for (int i = 0; i < x; i++)
                            {
                                string dzStr = dzDt.Rows[j][i].ToString();
                                if (!"".Equals(dzStr))
                                    dzs[POROcount] = Convert.ToDouble(dzStr);
                                POROcount++;
                            }
                        }


                    }
                    // 2017年8月10日 16:45:58
                    // 孔隙度 从gpro 改为prt 单位%<1
                    else if (line.Contains("PORO ") && line.Contains(" AT"))
                    {
                        bool findPOROFlag = false;// 找到就开始准备结束
                        bool allBreak = false;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (findPOROFlag && line.Contains("***"))// 文末结束
                            {
                                break;
                            }
                            if (line.Contains("MINIMUM VALUE"))// 准备开始数据行
                            {
                                findPOROFlag = true;
                            }
                            else if (line.Contains("I,  J,  K) I="))// 开始数据行
                            {
                                bool POROdataStartFlag = false;
                                // dz
                                int startNumDz = int.Parse(line.Substring(15, 3).Trim());

                                while ((line = sr.ReadLine()) != null)
                                {
                                    if (("".Equals(line) || line.Contains("***")) && POROdataStartFlag)//"".Equals(line) ||
                                    {
                                        if (line.Contains("***")) allBreak = true;
                                        break;
                                    }
                                    else if (line.Length > 0 && !line.Contains("I,  J,  K) I="))
                                    {
                                        POROdataStartFlag = true;
                                        int k = 0;//一行的循环count i
                                        int rowNum = -1;
                                        int filech = -1;
                                        rowNum = int.Parse(line.Substring(4, 3).Trim());//行号j                                        
                                        filech = int.Parse(line.Substring(8, 3).Trim());//层号k
                                        if (ch.Equals("" + filech))//ch.Equals("" + filech))
                                        {
                                            string[] sArray = line.Substring(12).Split(' ');
                                            if (sArray.Length > 2)
                                            {
                                                foreach (string i in sArray)
                                                {
                                                    if (i != "" && "-----" != i)
                                                    {
                                                        double val = Convert.ToDouble(i.Replace("*", "."));
                                                        // poro[POROcount] = val;
                                                        poroDT.Rows[rowNum - 1][startNumDz + k - 1] = val;
                                                        k++;
                                                    }
                                                    else if ("-----" == i)
                                                    {
                                                        //poro[POROcount] = 0;
                                                        poroDT.Rows[rowNum - 1][startNumDz + k - 1] = 0;
                                                        k++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }// while2
                            }
                            if (allBreak) break;
                        }// while 1

                        int chInt = Convert.ToInt32(ch);
                        int POROcount = (chInt - 1) * x * y;
                        for (int j = 0; j < y; j++)
                        {
                            for (int i = 0; i < x; i++)
                            {
                                poro[POROcount] = (double)poroDT.Rows[j][i];
                                if (poro[POROcount] > 0)
                                {
                                    //Console.WriteLine(POROcount);
                                    //Console.WriteLine("cuk you kakdka  lkaaslmdl;asl;dkl;asdl;asl;d");
                                } 
                                POROcount++;
                            }
                        }

                    }
                    // 2017年8月10日 16:45:58
                    // 净毛比 从gpro 改为prt 单位%<1
                    else if (line.Contains("NTG ") && line.Contains(" AT"))
                    {
                        bool findPOROFlag = false;// 找到就开始准备结束

                        while ((line = sr.ReadLine()) != null)
                        {
                            if (findPOROFlag && line.Contains("***"))// 文末结束
                            {
                                break;
                            }
                            if (line.Contains("MINIMUM VALUE"))// 准备开始数据行
                            {
                                findPOROFlag = true;
                            }
                            else if (line.Contains("I,  J,  K) I="))// 开始数据行
                            {
                                bool POROdataStartFlag = false;
                                // dz
                                int startNumDz = int.Parse(line.Substring(15, 3).Trim());

                                while ((line = sr.ReadLine()) != null)
                                {
                                    if (("".Equals(line) || line.Contains("***")) && POROdataStartFlag)//"".Equals(line) ||
                                    {
                                        break;
                                    }
                                    else if (line.Length > 0 && !line.Contains("I,  J,  K) I="))
                                    {
                                        POROdataStartFlag = true;
                                        int k = 0;//一行的循环count i
                                        int rowNum = -1;
                                        int filech = -1;
                                        rowNum = int.Parse(line.Substring(4, 3).Trim());//行号j                                        
                                        filech = int.Parse(line.Substring(8, 3).Trim());//层号k
                                        if (ch.Equals("" + filech))//ch.Equals("" + filech))
                                        {
                                            string[] sArray = line.Substring(12).Split(' ');
                                            if (sArray.Length > 2)
                                            {
                                                foreach (string i in sArray)
                                                {
                                                    if (i != "" && "-----" != i)
                                                    {
                                                        double val = Convert.ToDouble(i.Replace("*", "."));
                                                        // poro[POROcount] = val;
                                                        ntgDT.Rows[rowNum - 1][startNumDz + k - 1] = val;
                                                        k++;
                                                    }
                                                    else if ("-----" == i)
                                                    {
                                                        //poro[POROcount] = 0;
                                                        ntgDT.Rows[rowNum - 1][startNumDz + k - 1] = 0;
                                                        k++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        int chInt = Convert.ToInt32(ch);
                        int POROcount = (chInt - 1) * x * y;
                        for (int j = 0; j < y; j++)
                        {
                            for (int i = 0; i < x; i++)
                            {
                                string tmpStr = ntgDT.Rows[j][i].ToString();
                                ntgs[POROcount] = Convert.ToDouble(tmpStr);
                                POROcount++;
                            }
                        }

                    }
                    else if (line.Contains("SOIL ") && line.Contains(" AT"))//饱和度信息 UNITS (无关：BARSA是压力单位
                    {
                        //Console.WriteLine(line);
                        line = sr.ReadLine();
                        if (line.Contains(soiltime))
                        {
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (line.Contains("MINIMUM VALUE"))
                                    break;
                            }
                            minv = Convert.ToDouble(line.Substring(16, 9).Trim());
                            maxv = Convert.ToDouble(line.Substring(63, 9).Trim());
                            startFlag = true;
                        }
                    }
                    else if (startFlag && line.Contains("I,  J,  K) I="))//表头行(
                    {
                        startFlag = true;
                        startNum = int.Parse(line.Substring(15, 3).Trim());
                    }
                    else if (startFlag && line.Contains("***********"))
                    {
                        break;
                    }
                    else if (startFlag)
                    {
                        int k = 0;//一行的循环count i
                        int rowNum = -1;
                        rowNum = int.Parse(line.Substring(4, 3).Trim());//行号j
                        int filech = -1;
                        filech = int.Parse(line.Substring(8, 3).Trim());//层号k
                        if (ch.Equals("" + filech))
                        {
                            line = line.Substring(12).Replace("-0", " 0");// 不规范数据
                            string[] sArray = line.Split(' ');
                            if (sArray.Length > 2)
                            {
                                foreach (string i in sArray)
                                {
                                    // System.Console.WriteLine("~~" + i);
                                    if (i != "" && "-----" != i)
                                    {
                                        //System.Console.WriteLine("++:"+i);
                                        double val = Convert.ToDouble(i.Replace("*", "."));// 不规范数据
                                        dtresult.Rows[rowNum - 1][startNum + k - 1] = val;
                                        k++;
                                    }
                                    else if ("-----" == i)
                                    {
                                        dtresult.Rows[rowNum - 1][startNum + k - 1] = 0;
                                        k++;
                                    }
                                    else
                                    {
                                        //System.Console.WriteLine(i);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            sr.Close();
            return dtresult;
        }

        //井点信息
        public void readSchInc(string filepath, String ch)
        {
            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Trim() != "")
                {
                    if (line.Contains("COMPDAT")) //井号的网格坐标wellCoord
                    {
                        line = sr.ReadLine();
                        string remark = line.Substring(0, 2);
                        if (!"--".Equals(remark))
                        {
                            string[] compdatArray = line.Split(' ');
                            int endl = line.IndexOf(" '", 7);//开关井状态前一位
                            int startJh = line.IndexOf("'");
                            line = line.Substring(startJh + 1, endl - (startJh + 1));// - 5
                            endl = line.IndexOf("'");//井号长度
                            String jh = line.Substring(0, endl);
                            line = line.Substring(endl + 2, line.Length - endl - 2);
                            string[] coordArray = line.Split(' ');
                            DataRow row = wellCoord.NewRow();
                            row["jh"] = jh;
                            row["x"] = Convert.ToDouble(coordArray[0]);
                            row["y"] = Convert.ToDouble(coordArray[1]);
                            row["z"] = Convert.ToDouble(coordArray[2]);

                            try
                            {
                                // if (compdatArray[6].ToString().Contains("*"))
                                if ("1*".Equals(compdatArray[6])) // 不对不对 地层系数不用了 1*是省略1
                                    row["地层系数"] = Convert.ToDouble(compdatArray[7]);
                                else if ("2*".Equals(compdatArray[6]))
                                    row["地层系数"] = Convert.ToDouble(compdatArray[7]);
                                else
                                    row["地层系数"] = Convert.ToDouble(compdatArray[8]);// 计算注采完善明显 2016-11-7 17:02:06 
                            }
                            catch
                            { //Console.WriteLine("没啥用的地层系数错误");
                            }
                            wellCoord.Rows.Add(row);
                        }
                    }
                    else if ("WELSPECS".Equals(line.Trim()))//油井井别wellStat
                    {
                        line = sr.ReadLine();
                        int startJh = line.IndexOf("'");
                        line = line.Substring(startJh + 1);
                        int endl = line.IndexOf("'");
                        String jh = line.Substring(0, endl);
                        line = line.Substring(endl + 1);
                        string[] welspecsArray = line.Split(' ');
                        DataRow row = welspecs.NewRow();
                        row["jh"] = jh;
                        row["x"] = Convert.ToDouble(welspecsArray[2]);
                        row["y"] = Convert.ToDouble(welspecsArray[3]);
                        welspecs.Rows.Add(row);
                    }
                    else if (line.Contains("WCONHIST"))//油井井别wellStat
                    {
                        line = sr.ReadLine();
                        int startJh = line.IndexOf("'");
                        line = line.Substring(startJh + 1);
                        int endl = line.IndexOf("'");
                        line = line.Substring(0, endl);
                        DataRow row = wellStat.NewRow();
                        row["jh"] = line;
                        row["stat"] = "OIL";
                        wellStat.Rows.Add(row);
                    }
                    else if (line.Contains("WCONINJE"))//水井井别wellStat
                    {
                        line = sr.ReadLine();
                        int startJh = line.IndexOf("'");
                        line = line.Substring(startJh + 1);
                        int endl = line.IndexOf("'");
                        line = line.Substring(0, endl);
                        DataRow row = wellStat.NewRow();
                        row["jh"] = line;
                        row["stat"] = "WATER";
                        wellStat.Rows.Add(row);
                    }
                }
            }
            sr.Close();
            for (int i = 1; i <= z; i++)
            {
                foreach (DataRow welspecsRow in welspecs.Rows)
                {
                    DataRow rowFaker = welspecsFakerWellCoord.NewRow();
                    rowFaker["jh"] = welspecsRow["jh"];
                    rowFaker["x"] = welspecsRow["x"];
                    rowFaker["y"] = welspecsRow["y"];
                    rowFaker["z"] = i;
                    welspecsFakerWellCoord.Rows.Add(rowFaker);
                }
            }
        }

        //断层信息
        public DataTable readGothInc(string filepath, String ch)
        {
            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Trim() != "")
                {
                    if (line.Contains("FAULTS")) //井号的网格坐标wellCoord
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = line.Trim();
                            if ("/".Equals(line))//断层结束点-1
                            {
                                DataRow row = faultDt.NewRow();
                                row["k"] = -1;
                                faultDt.Rows.Add(row);
                                break;
                            }
                            else if (!line.Contains("--"))
                            {
                                string[] lineStrs = line.Split(' ');
                                int saveCount = 0;
                                int[] ijk = new int[5];
                                string direct;
                                if (line.Contains("'Y'"))
                                {
                                    direct = "Y";
                                }
                                else
                                {
                                    direct = "X";
                                }
                                foreach (string oneStr in lineStrs)
                                {
                                    if (!"".Equals(oneStr) && !oneStr.Contains("'") && saveCount < 5)
                                    {
                                        ijk[saveCount] = Convert.ToInt32(oneStr);
                                        saveCount++;
                                    }
                                    if (5 == saveCount)
                                    {
                                        break;
                                    }

                                }
                                DataRow row = faultDt.NewRow();
                                row["i"] = ijk[0];
                                row["j"] = ijk[2];
                                row["k"] = ijk[4];
                                row["direct"] = direct;
                                faultDt.Rows.Add(row);
                            }
                        }
                    }
                }
            }
            sr.Close();
            return faultDt;
        }
        /**
         * 2017年5月8日 21:00:05 这方法以前特么写过 现在怎么又改回来读readREG了
         * 是不是
         * */
        public int[] readRegInc(string filepath)
        {
            fipnum = new int[x * y * z];
            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;
            Boolean checkComplete = false;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Trim() != "")
                {
                    String keyword = "";
                    if (line.Length > 5)
                    {
                        keyword = line.Substring(0, 6).Trim();
                    }
                    if (checkComplete)
                    { break; }
                    else if ("FIPNUM".Equals(keyword))// 2017年5月8日 20:57:12 line.Trim()))//
                    {
                        checkComplete = true;
                        int fipnumCount = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Contains("/"))
                            {
                                break;
                            }
                            string[] fipnumOneline = line.Split(' ');
                            foreach (string onefipnum in fipnumOneline)
                            {
                                if (onefipnum != null && !"".Equals(onefipnum))
                                {
                                    fipnum[fipnumCount] = Convert.ToInt32(onefipnum);
                                    fipnumCount++;
                                }
                            }
                        }
                    }
                }
            }
            sr.Close();
            return fipnum;
        }

        public int[] readFacies(string filepath)
        {
            facies = new int[x * y * z];
            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Trim() != "")
                {
                    if (line.Contains("FACIES"))//相图
                    {
                        int fipnumCount = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if ("".Equals(line))
                            {
                                break;
                            }
                            else if (line.Contains("--"))
                            {
                                continue;
                            }
                            else
                            {
                                string[] fipnumOneline = line.Split(' ');
                                foreach (string onefipnum in fipnumOneline)
                                {
                                    if ("/".Equals(onefipnum))
                                    {
                                        break;
                                    }
                                    else if (onefipnum != null && onefipnum.Contains("*"))
                                    {//解压缩
                                        string[] manys = onefipnum.Split('*');
                                        for (int i = 0; i < (int)Convert.ToDouble(manys[0]); i++)
                                        {
                                            facies[fipnumCount] = (int)Convert.ToDouble(manys[1]);
                                         // if (facies[fipnumCount] > 0)
                                          //  {
                                         //       Console.WriteLine("hahahahhahaha" + (facies[fipnumCount]));
                                         //   }
                                            fipnumCount++;
                                        }
                                    }
                                    else if (onefipnum != null && !"".Equals(onefipnum))
                                    {
                                        facies[fipnumCount] = (int)Convert.ToDouble(onefipnum);
                                        fipnumCount++;
                                    }
                                }
                            }
                        }
                        //System.Console.WriteLine("dd:" + fipnumCount);
                    }
                }
            }
            sr.Close();
            return facies;
        }

        //分析时间
        public List<String> readSoilTime(string filepath)
        {
            List<String> timeList = new List<String>();
            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                //if (line.Length > 10)
                //{ 
                if (line.Contains("SOIL     AT"))
                //   int tempIndex = line.IndexOf("SOIL     AT");
                //if (tempIndex != -1)
                {
                    line = sr.ReadLine();
                    line = line.Substring(15, 15).Trim();
                    timeList.Add(line);
                }
                // }
            }
            return timeList;
        }
        //  2017-1-9 20:00:40  2017年5月8日 20:28:58
        /// <summary>
        /// 没有poro
        ///    读取Facies文件中的NTG 1.txt 
        ///     改为GPRO 的NTG、PORO、PERMX 
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public double[] readNTG(string filepath)
        {
            //ntgs = new double[x * y * z];
            //  poro = new double[x * y * z];
            permx = new double[x * y * z];
            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;
            Boolean check1complete = false;
            Boolean check2complete = false;
            Boolean check3complete = false;
            while ((line = sr.ReadLine()) != null)
            {
                if (check1complete && check2complete && check3complete)
                {
                    break;
                }
                else if (line.Trim() != "")
                {
                    String keyword = line.Trim();
                    //if (line.Length > 2)
                    //{
                    //keyword = line.Substring(0, 3).Trim();
                    // }
                    // Console.WriteLine(keyword);
                    switch (keyword)
                    {
                        default:
                            // Console.WriteLine("de" + line);
                            break;
                        /* case "NTG":
                             Console.WriteLine("nt" + line);
                             int fipnumCount = 0;
                             while ((line = sr.ReadLine()) != null)
                             {

                                 check1complete = true;
                                 if ("".Equals(line))
                                 {
                                     break;
                                 }
                                 else if (line.Contains("--"))
                                 {
                                     continue;
                                 }
                                 else
                                 {

                                     string[] fipnumOneline = line.Split(' ');
                                     foreach (string onefipnum in fipnumOneline)
                                     {
                                         if ("/".Equals(onefipnum))
                                         {
                                             break;
                                         }
                                         else if (onefipnum != null && onefipnum.Contains("*"))
                                         {//解压缩
                                             string[] manys = onefipnum.Split('*');
                                             for (int i = 0; i < Convert.ToDouble(manys[0]); i++)
                                             {
                                                 ntgs[fipnumCount] = Convert.ToDouble(manys[1]);
                                                 fipnumCount++;
                                             }
                                         }
                                         else if (onefipnum != null && !"".Equals(onefipnum))
                                         {
                                             //Console.WriteLine(onefipnum+"ntcccc" + line);
                                             ntgs[fipnumCount] = Convert.ToDouble(onefipnum);
                                             fipnumCount++;
                                         }
                                     }
                                 }
                             }
                             break;
                         /* case "PORO":// 孔隙度
                              Console.WriteLine("poro" + line);
                              int poroCount = 0;
                              while ((line = sr.ReadLine()) != null)
                              {
                                  check2complete = true;
                                  if ("".Equals(line))
                                  {
                                      break;
                                  }
                                  else if (line.Contains("--"))
                                  {
                                      continue;
                                  }
                                  else
                                  {
                                      string[] fipnumOneline = line.Split(' ');
                                      foreach (string onefipnum in fipnumOneline)
                                      {
                                          if ("/".Equals(onefipnum))
                                          {
                                              break;
                                          }
                                          else if (onefipnum != null && onefipnum.Contains("*"))
                                          {//解压缩
                                              string[] manys = onefipnum.Split('*');
                                              for (int i = 0; i < Convert.ToDouble(manys[0]); i++)
                                              {
                                                  poro[poroCount] = Convert.ToDouble(manys[1]);
                                                  poroCount++;
                                              }
                                          }
                                          else if (onefipnum != null && !"".Equals(onefipnum))
                                          {
                                              poro[poroCount] = Convert.ToDouble(onefipnum);
                                              poroCount++;
                                          }
                                      }
                                  }
                              }
                              break;*/
                        case "PERMX":// 渗透率
                            Console.WriteLine("PERMX" + line);
                            int permxCount = 0;
                            while ((line = sr.ReadLine()) != null)
                            {
                                check3complete = true;
                                if ("".Equals(line))
                                {
                                    break;
                                }
                                else if (line.Contains("--"))
                                {
                                    continue;
                                }
                                else
                                {
                                   
                                    string[] fipnumOneline = line.Split(' ');
                                    foreach (string onefipnum in fipnumOneline)
                                    {
                                        if ("/".Equals(onefipnum))
                                        {
                                            break;
                                        }
                                        else if (onefipnum != null && onefipnum.Contains("*"))
                                        {//解压缩
                                            string[] manys = onefipnum.Split('*');
                                            for (int i = 0; i < Convert.ToDouble(manys[0]); i++)
                                            {
                                              
                                                permx[permxCount] = Convert.ToDouble(manys[1]);
                                                permxCount++;
                                            }
                                        }
                                        else if (onefipnum != null && !"".Equals(onefipnum))
                                        {
                                            //Console.WriteLine(line);
                                            permx[permxCount] = Convert.ToDouble(onefipnum);
                                            permxCount++;
                                        }
                                    }
                                }
                            }
                            break;
                    }

                }
            }
            sr.Close();
            return ntgs;
        }
        /**
         * 读取dz 从finit
         * 2017年5月23日 10:47:02         * 
         * */
        public double[] readDz(string filepath)
        {
            double[] dzs = new double[x * y * z];
            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;
            Boolean check1complete = false;
            Console.WriteLine(filepath);
            while ((line = sr.ReadLine()) != null)
            {
                if (check1complete)
                {
                    break;
                }
                else if (line.Trim() != "")
                {
                    String keyword = line.Trim();
                    if (keyword.Contains("DZ"))
                    {
                        int dzCount = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (check1complete)
                            {
                                break;
                            }

                            if ("".Equals(line))
                            {
                                break;
                            }
                            else if (line.Contains("--"))
                            {
                                continue;
                            }
                            else
                            {
                                string[] dzOneline = line.Split(' ');
                                foreach (string onedz in dzOneline)
                                {
                                    if ("/".Equals(onedz) || onedz.Contains("'"))
                                    {
                                        check1complete = true;
                                        break;
                                    }
                                    else if (onedz != null && onedz.Contains("*"))
                                    {//解压缩
                                        string[] manys = onedz.Split('*');
                                        for (int i = 0; i < Convert.ToDouble(manys[0]); i++)
                                        {
                                            dzs[dzCount] = Convert.ToDouble(manys[1]);
                                            dzCount++;
                                        }
                                    }
                                    else if (onedz != null && !"".Equals(onedz))
                                    {
                                        dzs[dzCount] = Convert.ToDouble(onedz);
                                        dzCount++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                    /*
                    else
                    {
                        break;
                    }*/
                }
            }
            sr.Close();
            return dzs;
        }
        /// <summary>
        /// 读取分区文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public int[] readPartFacies(string filepath)
        {
            if (filepath == null || "".Equals(filepath))
            {
                return null;
            }
            else
            {
                int[] partfacies = new int[x * y * z];
                StreamReader sr = new StreamReader(filepath, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Trim() != "")
                    {
                        if (line.Contains("FACIES"))//相图
                        {
                            int fipnumCount = 0;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if ("".Equals(line))
                                {
                                    break;
                                }
                                else if (line.Contains("--"))
                                {
                                    continue;
                                }
                                else
                                {
                                    string[] fipnumOneline = line.Split(' ');
                                    foreach (string onefipnum in fipnumOneline)
                                    {
                                        if ("/".Equals(onefipnum))
                                        {
                                            break;
                                        }
                                        else if (onefipnum != null && onefipnum.Contains("*"))
                                        {//解压缩
                                            string[] manys = onefipnum.Split('*');
                                            for (int i = 0; i < Convert.ToInt32(manys[0]); i++)
                                            {
                                                partfacies[fipnumCount] = Convert.ToInt32(manys[1]);
                                                fipnumCount++;
                                            }
                                        }
                                        else if (onefipnum != null && !"".Equals(onefipnum))
                                        {
                                            partfacies[fipnumCount] = (int)Convert.ToDouble(onefipnum);
                                            fipnumCount++;
                                        }
                                    }
                                }
                            }
                            //System.Console.WriteLine("dd:" + fipnumCount);
                        }
                    }
                }
                sr.Close();
                return partfacies;
            }

        }
        // 2017年8月11日 08:59:58
        /// <summary>
        /// 通用读取方法 比如类似prt.dx,prt.dy 等
        /// </summary>
        /// <returns></returns>
        public DataTable readPRTijk(string line, StreamReader sr, string ch)
        {
            DataTable dtresult = new DataTable();
            //规划Datatable大小
            for (int i = 1; i <= x; i++)
            {
                DataColumn column = new DataColumn();
                column.DataType = System.Type.GetType("System.Double");//该列的数据类型 
                column.ColumnName = "v" + i;
                dtresult.Columns.Add(column);
            }
            for (int i = 1; i <= y; i++)
            {
                DataRow row = dtresult.NewRow();
                dtresult.Rows.Add(row);
            }
            bool findDzFlag = false;
            bool allBreak = false;
            while ((line = sr.ReadLine()) != null)
            {
                if (findDzFlag && line.Contains("**"))
                {
                    break;
                }
                if (line.Contains("MINIMUM VALUE"))
                {
                    findDzFlag = true;
                }
                else if (line.Contains("I,  J,  K) I="))
                {
                    bool dzstartFlag = false;
                    int startNumDz = int.Parse(line.Substring(15, 3).Trim());
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (("".Equals(line) || line.Contains("**")) && dzstartFlag)
                        {
                            if (line.Contains("***")) allBreak = true;
                            break;
                        }
                        else if (line.Length > 0)
                        {
                            dzstartFlag = true;
                            int k = 0;//一行的循环count i
                            int rowNum = -1;
                            int filech = -1;
                            rowNum = int.Parse(line.Substring(4, 3).Trim());//行号j                                        
                            filech = int.Parse(line.Substring(8, 3).Trim());//层号k
                            if (ch.Equals("" + filech))
                            {
                                string[] sArray = line.Substring(12).Split(' ');
                                if (sArray.Length > 2)
                                {
                                    foreach (string i in sArray)
                                    {
                                        if (i != "" && "-----" != i)
                                        {
                                            double val = Convert.ToDouble(i.Replace("*", "."));
                                            dtresult.Rows[rowNum - 1][startNumDz + k - 1] = val;
                                            k++;
                                        }
                                        else if ("-----" == i)
                                        {
                                            dtresult.Rows[rowNum - 1][startNumDz + k - 1] = 0;
                                            k++;
                                        }
                                    }
                                }
                            }
                        }
                    } // while2
                }
                if (allBreak) break;
            } // while1
            return dtresult;
        }
    }
}
