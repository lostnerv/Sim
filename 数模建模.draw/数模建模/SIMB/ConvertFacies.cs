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
    class ConvertFacies
    {
        public int x = -1;
        public int y = -1;
        public int z = -1;
        public double minv = -1;
        public double maxv = -1;
        public double[] permx;//渗透率
        public double[] poro;//孔隙度
        public int[] fipnum, facies;
        public DataTable wellCoord = new DataTable();//井位网格坐标
        public DataTable wellStat = new DataTable();//井别记录
        public DataTable faultDt = new DataTable();
        public DataTable dzDt = new DataTable();//厚度

        public ConvertFacies()
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
        //提取数据
        //网格
        public DataTable readPRT(string filepath, String ch, String soiltime)
        {
            permx = new double[x * y * z];
            poro = new double[x * y * z];
            DataTable dtresult = new DataTable();
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
                column.DataType = System.Type.GetType("System.Double");//该列的数据类型 
                column.ColumnName = "v" + i;
                dzDt.Columns.Add(column);
            }
            for (int i = 1; i <= y; i++)
            {
                DataRow row = dzDt.NewRow();
                dzDt.Rows.Add(row);

            }
            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;

            while ((line = sr.ReadLine()) != null)
            {
                if (line.Trim() != "")
                {
                    if (line.Contains("1: PORO"))//空隙度
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
                    }
                    else if (line.Contains("1: PERMX"))//渗透率
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
                    } 
                    else if (line.Contains("DZ ") && line.Contains(" AT"))//z厚度
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
                    }
                    else if (line.Contains("SOIL ") && line.Contains(" AT"))//饱和度信息 UNITS BARSA是压力单位
                    {
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
                            string[] sArray = line.Substring(12).Split(' ');
                            if (sArray.Length > 2)
                            {
                                foreach (string i in sArray)
                                {
                                    if (i != "" && "-----" != i)
                                    {
                                        //System.Console.WriteLine("++:"+i);
                                        double val = Convert.ToDouble(i.Replace("*", "."));
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
                            if ("1*".Equals(compdatArray[6]))
                                row["地层系数"] = Convert.ToDouble(compdatArray[7]);
                            else
                                row["地层系数"] = Convert.ToDouble(compdatArray[8]);// 计算注采完善明显 2016-11-7 17:02:06 
                            wellCoord.Rows.Add(row);
                        }
                    }
                    else if ("WELSPECS".Equals(line.Trim()))//油井井别wellStat
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

        public int[] readRegInc(string filepath)
        {
            fipnum = new int[x * y * z];
            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Trim() != "")
                {
                    if ("FIPNUM".Equals(line.Trim()))//相图
                    {
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
                                        for (int i = 0; i < Convert.ToInt32(manys[0]); i++)
                                        {
                                            facies[fipnumCount] = Convert.ToInt32(manys[1]);
                                            fipnumCount++;
                                        }
                                    }
                                    else if (onefipnum != null && !"".Equals(onefipnum))
                                    {
                                        facies[fipnumCount] = Convert.ToInt32(onefipnum);
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

        public void convertFacies(string filepath,string outPath)
        {
            int[] faciesIntArr = readFacies(filepath);
            Console.WriteLine("changdu" + faciesIntArr.Count());
            int faciesCount = 0;
            int headFacies = 0;
            outPath = outPath + "\\facies.txt";
            Console.WriteLine("outPath" + outPath);
            RWFile file = new RWFile();//RWFile频繁open close对效率有影响？
            file.Append(outPath, "FACIES");
            foreach (int oneFacies in faciesIntArr)
            {
                int tmpTxt = 0;
                if (0 == faciesCount % (x * y))
                {
                    headFacies = headFacies + 100;
                    file.Append(outPath, "\r\n");
                }
                tmpTxt = oneFacies + headFacies;
                file.Append(outPath, tmpTxt + " ");
                //Console.WriteLine("tmpTxt" + tmpTxt);
                faciesCount++;
            }
        }
        //分析时间
        public List<String> readSoilTime(string filepath)
        {
            List<String> timeList = new List<String>();
            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("SOIL     AT"))
                {
                    line = sr.ReadLine();
                    line = line.Substring(15, 15).Trim();
                    timeList.Add(line);
                }
            }
            return timeList;
        }
    }
}
