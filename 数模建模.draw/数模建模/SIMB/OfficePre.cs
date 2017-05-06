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
    class OfficePre
    {
       // private DataTable wellnum;
        //private string wellSet = null;
        //private DataTable GgofileDT;
        //private SortedList GgofileSL ;
        //private String GgofileStr = null;
        private List<String> ggofileArr;
        private List<String> actnumfileArr;
        private List<String> permfileArr;
        private List<String> porofileArr;
        private List<String> ntgfileArr;
        private List<String> eqlnumfileArr;
        public DataTable z25sumDT;
        //结果就是井号X1,X2,X3...,Xn
        public OfficePre()//DataTable dt
        {
           //wellnum = dt;
           // foreach (DataRow tempRow in wellnum.Rows)
           // {
           //     wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
           // }
           // wellSet = wellSet.Substring(0, wellSet.Length - 1);
        }

       // public OfficePre(){}

        /**
         * 读取原始文件，分成几类List 
         * @DSnow
         **/
        public void getGridData(String path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            //GgofileDT = new DataTable();
            //GgofileStr=null;
            ggofileArr = new List<String>();
            actnumfileArr = new List<String>();
            permfileArr = new List<String>();
            porofileArr = new List<String>();
            ntgfileArr = new List<String>();
            eqlnumfileArr = new List<String>();
            String mark1 = "COORD";
            String mark2 = "ZCORN";
            String mark3 = "/";
            String markB1 = "ACTNUM";
            String markB2 = "PERMEABILITY";
            String markB3 = "PORO";
            String markB4 = "NTG";
            String markC1 = "FACIES";
            bool startTriger = false;
            //DataColumn column = new DataColumn();
            //column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            //column.ColumnName = "tmp";
            //GgofileDT.Columns.Add(column);
            String filename = null;

            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (!startTriger)//是否开始取数据
                {                    
                    if (mark1 == line || mark2 == line)
                    {
                        filename = "GGO";
                        startTriger = true;
                    }
                    else if (markB1 == line || markB2 == line || markB3 == line || markB4 == line || markC1 == line)
                    {
                        filename = line;
                        startTriger = true;
                    }
                }               
                if (startTriger)
                {
                    if ("GGO" == filename)
                        ggofileArr.Add(line + "\r\n");
                    else if (markB1 == filename)
                        actnumfileArr.Add(line + "\r\n");
                    else if (markB2 == filename)
                        permfileArr.Add(line + "\r\n");
                    else if (markB3 == filename)
                        porofileArr.Add(line + "\r\n");
                    else if (markB4 == filename)
                        ntgfileArr.Add(line + "\r\n");
                    else if (markC1 == filename)
                        eqlnumfileArr.Add(line + "\r\n");
                    //DataRow row = GgofileDT.NewRow();
                    //row[0] = line;
                    //GgofileDT.Rows.Add(row);
                   // GgofileStr = GgofileStr+line + "\r\n";
                }
                else
                {
                    continue;
                }
                if (mark3 == line) // 遇到结束标志
                {
                    //if (triger1 && triger2)//1,2 都完成后结束
                    //{
                    //    break;
                    //}
                    startTriger = false;
                }
            }
        }
        // 遍历的速度是瓶颈，后期可以尝试边读边写，不用存list
        //path结果输出路径
        public void writeGgoResult(String pathNoName)
        {
            RWFile file = new RWFile();
           // foreach (DataRow row in GgofileDT.Rows)
            //{
            //    String result = row["tmp"].ToString()
            //file.Append(path, GgofileStr);
            //GgofileStr = null;
           // }
           for (int i = 0; i < ggofileArr.Count; i++)
           {
               file.Append(pathNoName + "\\GGO.INC", ggofileArr[i]);
           }       
        }
        //path结果输出路径
        public void writeActnumResult(String pathNoName)
        {
            RWFile file = new RWFile();
            for (int i = 0; i < actnumfileArr.Count; i++)
            {
                file.Append(pathNoName + "\\actnum.dat", actnumfileArr[i]);
            }
        }
        //path结果输出路径
        public void writePermResult(String pathNoName)
        {
            RWFile file = new RWFile();
            for (int i = 0; i < permfileArr.Count; i++)
            {
                file.Append(pathNoName + "\\perm.dat", permfileArr[i]);
            }
        }
        //path结果输出路径
        public void writePoroResult(String pathNoName)
        {
            RWFile file = new RWFile();
            for (int i = 0; i < porofileArr.Count; i++)
            {
                file.Append(pathNoName + "\\poro.dat", porofileArr[i]);
            }
        }
        //path结果输出路径
        public void writeNtgResult(String pathNoName)
        {
            RWFile file = new RWFile();
            for (int i = 0; i < ntgfileArr.Count; i++)
            {
                file.Append(pathNoName + "\\ntg.dat", ntgfileArr[i]);
            }
        }

        //path结果输出路径
        public void writeCopyResult(String srcpath, String tarpath)
        {
            File.Copy(srcpath, tarpath);
        }

        //path结果输出路径
        public void writeEqlnumResult(String pathNoName)
        {
            RWFile file = new RWFile();
            for (int i = 0; i < eqlnumfileArr.Count; i++)
            {
                file.Append(pathNoName + "\\eqlnum.dat", eqlnumfileArr[i]);
            }
            File.Copy(pathNoName + "\\eqlnum.dat", pathNoName + "\\fipnum.dat");
            File.Copy(pathNoName + "\\eqlnum.dat", pathNoName + "\\satnum.dat");
        }

        public void getZ25sumData(String path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            z25sumDT = new DataTable();
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            column.ColumnName = "SUM";
            z25sumDT.Columns.Add(column);
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line != "" && line != "/" && !line.Contains("--"))
                {
                    DataRow row = z25sumDT.NewRow();
                    row[0] = line;
                    z25sumDT.Rows.Add(row);
                }               
            }
        }

        public void addZ25sumData(String newsum)
        {
            DataRow row = z25sumDT.NewRow();
            row[0] = newsum;
            z25sumDT.Rows.Add(row); 
        }

        public void delZ25sumData(String newsum)
        {
            foreach (DataRow row in z25sumDT.Rows)
            {
                string data = row["SUM"].ToString();
                if (newsum == data)
                {
                    row.Delete();
                    break;
                }
            }
        }

        public void updateZ25sumData(String oldsum,String newsum)
        {
            foreach (DataRow row in z25sumDT.Rows)
            {
                string data = row["SUM"].ToString();
                if (oldsum == data)
                {
                    row.BeginEdit();
                    row["SUM"] = newsum;
                    row.EndEdit();
                    break;
                }
            }           
        }
    }
}
