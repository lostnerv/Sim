using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using 建模数模.tools;

namespace 数模建模
{
    class Wellcruve
    {
        //井名  f128-150
        //单位  大庆
        //深度范围         1497.860       1591.260
        //空缺值  0
        //项目个数 13
        //项目名称  
        DataTable datatable = new DataTable();
        string wellname;
        string unit;
        string deprange;
        string defaltvalue;
        string projnum;
        string projname;
        string type;
        ArrayList list = new ArrayList();
        //DEP      DEPT      R025      R045        R1      RL3S      RL3D       HAC        RF       CAL       RMG       RMN       R25        R4        SP        GR
        //CI      LLD       LLS       CALC      MSFL      R400         AC      R250 
        public Wellcruve()
        {
            addColumn("DEP", datatable);//1
            addColumn("DEPT", datatable);//2
            addColumn("R025", datatable);//3
            addColumn("R045", datatable);//4
            addColumn("R1", datatable);//5
            addColumn("RL3S", datatable);//6
            addColumn("RL3D", datatable);//7
            addColumn("HAC", datatable);//8
            addColumn("RF", datatable);//9
            addColumn("CAL", datatable);//10
            addColumn("RMG", datatable);//11
            addColumn("RMN", datatable);//12
            addColumn("R25", datatable);//13
            addColumn("R4", datatable);//14
            addColumn("SP", datatable);//15
            addColumn("GR", datatable);//16
            addColumn("CI", datatable);//17
            addColumn("LLD", datatable);//18
            addColumn("LLS", datatable);//19
            addColumn("CALC", datatable);//20
            addColumn("MSFL", datatable);//21
            addColumn("R400", datatable);//22
            addColumn("AC", datatable);//23
            addColumn("R250", datatable);//24
        }

        public void addColumn(string name,DataTable dt)
        {
            DataColumn Column = new DataColumn();
            Column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            Column.ColumnName = name;//该列得名称 
            Column.DefaultValue = "0.000";
            dt.Columns.Add(Column);
        }

        public void getData(string path)
        {
            int[] index;
            string trim;
            string line;
            string[] array;
            StreamReader sr = new StreamReader(path, Encoding.Default);
            bool triger = false;

            if (Path.GetExtension(path) == ".las")
            {
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();
                    trim = Regex.Replace(line, "\\s{2,}", ",");
                    array = trim.Split(',');

                    if (line == "~Curve")
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Contains("~Parameter"))
                            {
                                break;
                            }
                            string[] key = line.Split(':');
                            switch (key[1])
                            {
                                case "MD":
                                    list.Add(0);
                                    break;
                                case "DEPT":
                                    list.Add(1);
                                    break;
                                case "R025":
                                    list.Add(2);
                                    break;
                                case "R045":
                                    list.Add(3);
                                    break;
                                case "R1":
                                    list.Add(4);
                                    break;
                                case "RL3S":
                                    list.Add(5);
                                    break;
                                case "RL3D":
                                    list.Add(6);
                                    break;
                                case "HAC":
                                    list.Add(7);
                                    break;
                                case "RF":
                                    list.Add(8);
                                    break;
                                case "CAL":
                                    list.Add(9);
                                    break;
                                case "RMG":
                                    list.Add(10);
                                    break;
                                case "RMN":
                                    list.Add(11);
                                    break;
                                case "R25":
                                    list.Add(12);
                                    break;
                                case "R4":
                                    list.Add(13);
                                    break;
                                case "SP":
                                    list.Add(14);
                                    break;
                                case "GR":
                                    list.Add(15);
                                    break;
                                case "CI":
                                    list.Add(16);
                                    break;
                                case "LLD":
                                    list.Add(17);
                                    break;
                                case "LLS":
                                    list.Add(18);
                                    break;
                                case "CALC":
                                    list.Add(19);
                                    break;
                                case "MSFL":
                                    list.Add(20);
                                    break;
                                case "R400":
                                    list.Add(21);
                                    break;
                                case "AC":
                                    list.Add(22);
                                    break;
                                case "R250":
                                    list.Add(23);
                                    break;
                                default:
                                    break;
                            }

                        }
                    }

                    if (line == "~Ascii")
                    {
                        line = sr.ReadLine();
                        triger = true;
                    }
                    if (triger)
                    {
                        trim = Regex.Replace(line, "\\s{2,}", ",");
                        array = trim.Split(',');
                        int count = 0;
                        DataRow row = datatable.NewRow();
                        foreach (int i in list)
                        {
                            row[i] = array[count];
                            count++;
                        }
                        datatable.Rows.Add(row);
                    }
                }
            }

            if (Path.GetExtension(path) == ".txt")
            {
                //井名  f128-150
                //单位  大庆
                //深度范围         1497.860       1591.260
                //空缺值  0
                //项目个数 13
                //项目名称  
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Trim();

                    if(line.Contains("井名"))
                    {
                        wellname = line;
                    }
                    else if(line.Contains("单位"))
                    {
                        unit = line;
                    }
                    else if(line.Contains("深度范围"))
                    {
                        deprange = line;
                    }
                    else if(line.Contains("空缺值"))
                    {
                        defaltvalue = line;
                    }
                    else if(line.Contains("项目个数"))
                    {
                        projnum = line;
                    }
                    else if(line.Contains("项目名称"))
                    {
                        projname = line;
                    }
                    else if(line.Contains("DEP"))
                    {
                        if (line.Contains("LLD"))
                        {
                            type = "DLS";
                        }
                        if (line.Contains("RL3D"))
                        {
                            type = "JD581";
                        }
                        trim = Regex.Replace(line, "\\s{2,}", ",");
                        array = trim.Split(',');
                       
                        foreach(string name in array)
                        {
                            switch(name)
                            {
                                case "DEP":
                                    list.Add(0);
                                    break;
                                case "DEPT":
                                    list.Add(1);
                                    break;
                                case "R025":
                                    list.Add(2);
                                    break;
                                case "R045":
                                    list.Add(3);
                                    break;
                                case "R1":
                                    list.Add(4);
                                    break;
                                case "RL3S":
                                    list.Add(5);
                                    break;
                                case "RL3D":
                                    list.Add(6);
                                    break;
                                case "HAC":
                                    list.Add(7);
                                    break;
                                case "RF":
                                    list.Add(8);
                                    break;
                                case "CAL":
                                    list.Add(9);
                                    break;
                                case "RMG":
                                    list.Add(10);
                                    break;
                                case "RMN":
                                    list.Add(11);
                                    break;
                                case "R25":
                                    list.Add(12);
                                    break;
                                case "R4":
                                    list.Add(13);
                                    break;
                                case "SP":
                                    list.Add(14);
                                    break;
                                case "GR":
                                    list.Add(15);
                                    break;
                                case "CI":
                                    list.Add(16);
                                    break;
                                case "LLD":
                                    list.Add(17);
                                    break;
                                case "LLS":
                                    list.Add(18);
                                    break;
                                case "CALC":
                                    list.Add(19);
                                    break;
                                case "MSFL":
                                    list.Add(20);
                                    break;
                                case "R400":
                                    list.Add(21);
                                    break;
                                case "AC":
                                    list.Add(22);
                                    break;
                                case "R250":
                                    list.Add(23);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        trim = Regex.Replace(line, "\\s{2,}", ",");
                        array = trim.Split(',');
                        int count = 0;
                        DataRow row = datatable.NewRow();
                        foreach(int i in list)
                        {    
                            row[i] = array[count];
                            count++;
                        }
                        datatable.Rows.Add(row);
                    }
                }
            }
            if (Path.GetExtension(path) == ".la")
            {

            }
            if (Path.GetExtension(path) == ".data")
            {

            }
        }

        public void WriteResult(String path)
        {
            RWFile file = new RWFile();
            file.Append(path, wellname + "\r\n");
            file.Append(path, unit + "\r\n");
            file.Append(path, deprange + "\r\n");
            file.Append(path, defaltvalue + "\r\n");
            file.Append(path, projnum + "\r\n");
            file.Append(path, "测井系列 " + type + "\r\n");
            file.Append(path, projname + "\r\n");
            file.Append(path, "DEP      DEPT      R025      R045        R1      RL3S      RL3D       HAC        RF       CAL       RMG       RMN       R25        R4        SP        GR    CI      LLD       LLS       CALC      MSFL      R400         AC      R250 \r\n");
            
          
            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                string result = datatable.Rows[i]["DEP"].ToString() + "   " + datatable.Rows[i]["DEPT"].ToString() + "   " + datatable.Rows[i]["R025"].ToString() + "   " + datatable.Rows[i]["R045"].ToString() + "   " + datatable.Rows[i]["R1"].ToString() + "   " + datatable.Rows[i]["RL3S"].ToString() + "   " + datatable.Rows[i]["RL3D"].ToString() + "   " + datatable.Rows[i]["HAC"].ToString() + "   " + datatable.Rows[i]["RF"].ToString() + "   " + datatable.Rows[i]["CAL"].ToString() + "   " + datatable.Rows[i]["RMG"].ToString() + "   " + datatable.Rows[i]["RMN"].ToString() + "   " + datatable.Rows[i]["R25"].ToString() + "   " + datatable.Rows[i]["R4"].ToString() + "   " + datatable.Rows[i]["SP"].ToString() + "   " + datatable.Rows[i]["GR"].ToString() + "   " + datatable.Rows[i]["LLD"].ToString() + "   " + datatable.Rows[i]["LLS"].ToString() + "   " + datatable.Rows[i]["CALC"].ToString() + "   " + datatable.Rows[i]["MSFL"].ToString() + "   " + datatable.Rows[i]["R400"].ToString() + "   " + datatable.Rows[i]["AC"].ToString() + "   " + datatable.Rows[i]["R250"].ToString() + "\r\n";
                file.Append(path, result);
            }
        }
    }
}
