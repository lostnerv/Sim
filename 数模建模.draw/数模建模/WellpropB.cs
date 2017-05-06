using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using 建模数模.tools;

namespace 数模建模
{
    class WellpropB
    {
        DataTable wellnum;
        public DataTable data = new DataTable();
        DataTable Outputresult = new DataTable();
        double kxd, stl,bhd;
        

        public WellpropB(/*DataTable dt*/)
        {
            //wellnum = dt;
            //foreach (DataRow tempRow in wellnum.Rows)
            //{
            //    wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
            //}
            //wellSet = wellSet.Substring(0, wellSet.Length - 1);


            DataColumn depcolumn = new DataColumn();
            depcolumn.ColumnName = "深度";
            depcolumn.DefaultValue = 0;
            Outputresult.Columns.Add(depcolumn);

            DataColumn kxdcolumn = new DataColumn();
            kxdcolumn.ColumnName = "孔隙度";
            kxdcolumn.DefaultValue = 0;
            Outputresult.Columns.Add(kxdcolumn);

            DataColumn stlcolumn = new DataColumn();
            stlcolumn.ColumnName = "渗透率";
            stlcolumn.DefaultValue = 0;
            Outputresult.Columns.Add(stlcolumn);

            DataColumn bhdcolumn = new DataColumn();
            bhdcolumn.ColumnName = "含油饱和度";
            bhdcolumn.DefaultValue = 0;
            Outputresult.Columns.Add(bhdcolumn);
        }

        

        public double[] MaxAndMinValue(string path)
        {
            string colname = "";
            string line;
            StreamReader sr = new StreamReader(path);
            bool triger = false;
            string trim;
            string[] array;
            double[] result = new double[2];

            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("测井系列"))
                {
                    trim = Regex.Replace(line, "\\s{1,}", ",");
                    array = trim.Split(',');

                    if (array[1].ToString() == "JD581")
                    {
                        colname = "SP";
                    } 
                    else
                    {
                        colname = "GR";
                    }
                }
                if (line.Contains("DEP"))
                {
                    trim = Regex.Replace(line, "\\s{2,}", ",");
                    array = trim.Split(',');
                    foreach(string title in array)
                    {
                        DataColumn column = new DataColumn();
                        column.ColumnName = title;
                        column.DataType = System.Type.GetType("System.Double");
                        data.Columns.Add(column);
                    }
                    triger = true;
                    continue;
                }
                if (triger)
                {
                    
                    trim = Regex.Replace(line, "\\s{2,}", ",");
                    array = trim.Split(',');
                    DataRow row = data.NewRow();
                    for (int i = 0; i < array.Length;i++ )
                    {
                        row[i] = Convert.ToDouble(array[i].ToString().Trim());
                    }
                    data.Rows.Add(row);
                }
            }

            string temp = "Max(" + colname + ")";

            result[0] = Convert.ToDouble(data.Compute(temp, ""));
            result[1] = Convert.ToDouble(data.Compute("Min(" + colname + ")", ""));

            return result;
        }

        public double GR(double max,double min,double gr)
        {
           
           return (gr - min)/(max - min);
        }

        public double SP(double max, double min, double sp)
        {
            
            return (sp - min) / (max - min);
        }

        //计算孔隙度和渗透率
        public void KScal(string block,string sequnce,double rtgr,double spgr,double t,bool flag=true )
        {
            double[] result = new double[2];

           

            if (sequnce == "JD581")
            {
                switch(block)
                {
                    case "宋芳屯宋北区块":

                        

                        kxd = -34.395 + 0.186*t + 2.485*spgr;
                        stl = -4.29 + 0.189*kxd + 1.59*Math.Log(rtgr)-0.214*spgr;
                        //System.Console.WriteLine(kxd);
                        //System.Console.WriteLine(stl);
                        break;
                    case "宋芳屯宋南区块":
                        kxd = -18.379 + 0.128*t + 4.727*spgr;
                        stl = -2.678 + 0.178*kxd + 0.222*Math.Log(rtgr)-0.51*spgr;
                        break;
                    case "宋芳屯宋西区块":
                        kxd = -27.684 + 0.156*t + 4.620*spgr;
                        stl = -3.030 + 0.191*kxd + 0.413*Math.Log(rtgr)-0.646*spgr;
                        break;
                    case "宋芳屯油田":
                        kxd = -16.461 + 0.120*t + 4.861*spgr;
                        stl = -11.023 + 8.698 * Math.Log(kxd) + 0.413 * Math.Log(rtgr) - 0.149 * spgr;
                        break;
                    case "升平油田":
                        kxd = -10.598 + 0.115 * t + 0.416 * spgr;
                        stl = -8.498 + 7.649 * Math.Log(kxd) + 0.111 * Math.Log(rtgr) - 0.293 * spgr;
                        break;
                    case "徐家围子油田":
                        kxd = -18.802 + 0.144 * t + 1.224 * Math.Log(spgr);
                        stl = -2.915 + 0.197 * Math.Log(kxd) + 0.392 * Math.Log(rtgr) - 0.26 * spgr;
                        break;
                    case "永乐油田":
                        kxd = -32.702 + 0.171*t + 5.767*spgr;
                        stl = -2.999 + 0.132*kxd + 1.186*Math.Log(rtgr)+0.445*spgr;
                        break;
                }
            } 
            else
            {
                switch (block)
                {
                    case "宋芳屯宋北区块":
                        kxd = -25.993 + 0.164*t - 5.841*spgr;
                        stl = 10.063 + 0.09912*kxd -5.856*Math.Log(rtgr)-0.133*spgr;
                        break;
                    case "宋芳屯宋西区块":
                        kxd = -33.203 + 0.192*t - 8.581*spgr;
                        stl = 0.491 + 0.190*kxd -0.01414*Math.Log(rtgr)-1.966*Math.Log(spgr);
                        break;
                    case "宋芳屯油田":
                        kxd = -26.864 + 0.152*t + 5.670*spgr;
                        stl = 5.311 + 0.119*kxd + 0.671*Math.Log(rtgr)-3.906*Math.Log(spgr);
                        break;
                    case "升平油田":
                        if (flag)
                        {
                            kxd = -18.819 + 0.133 * t + 3.836 * spgr;
                            stl = -2.561 + 0.187 * kxd + 0.692 * Math.Log(rtgr) - 0.00592 * Math.Log(spgr);
                        }
                        else
                        {
                            //kxd = -2.647 + 0.128 * t - 6.151 + 2.633 * spgr;
                            //stl = 0.619 + 0.238 * kxd - 1.636  - 0.00292 * spgr;
                        }
                        break;
                    case "徐家围子油田":

                        kxd = -37.395 + 0.205 * t + 6.182 * Math.Log(spgr);
                        stl = -1.703 + 0.163 * kxd + 0.616 * Math.Log(rtgr) - 0.0141 *spgr;
                        break;
                    case "肇州油田葡萄花油层":
                        if (flag)
                        {
                            kxd = -18.819 + 0.133 * t + 3.836 * spgr;
                            stl = -2.561 + 0.187 * kxd + 0.692 * Math.Log(rtgr) - 0.00592 * Math.Log(spgr);
                        } 
                        else
                        {
                        }
                        break;
                    case "":
                        if (flag)
                        {
                            kxd = -18.819 + 0.133 * t + 3.836 * spgr;
                            stl = -2.561 + 0.187 * kxd + 0.692 * Math.Log(rtgr) - 0.00592 * Math.Log(spgr);
                        } 
                        else
                        {
                        }
                        break;
                    case "永乐油田":
                        kxd = -31.912 + 0.168*t + 5.613*spgr;
                        stl = -8.144 + 6.349*Math.Log(kxd) - 0.00408*Math.Log(rtgr)+1.218*Math.Log(spgr);
                        break;
                    case "卫星油田":
                        kxd = -14.806 + 0.133*t + 5.79*spgr;
                        stl = -9.152 + 7.822*kxd + 0.238*Math.Log(rtgr)-0.0138*spgr;
                        break;
                }
            }  
        }

        public void Calbhd(double vcl,string block,double rt,double kxd,double rcl = 2.5)
        {         
            double rw = 0;
            switch (block)
            {
                case "宋芳屯宋北区块":
                    rw = 0.36;
                    break;
                case "宋芳屯南部":
                    rw = 0.35;
                    break;
                case "宋芳屯西部":
                    rw = 0.37;
                    break;
                case "升平":
                    rw = 0.37;
                    break;
                case "徐家围子":
                    rw = 0.36;
                    break;
                case "肇州":
                    rw = 0.29;
                    break;
                case "永乐":
                    rw = 0.30;
                    break;
                case "卫星":
                    rw = 0.39;
                    break;
            }

            double temp = Math.Pow(vcl,1-vcl)/Math.Pow(rcl,1/2);
            double temp1 = Math.Pow(kxd,1.25)/Math.Pow(2.5*rw,1/2);
            double temp2 = 1/Math.Pow(rt,1/2);
            double temp3 = temp/(temp1 + temp2);
            bhd = Math.Pow(temp3, 4 / 5);
        }

        public void getData(string path,string block, string sequnce)
        {
            double[] MaxAndMin = MaxAndMinValue(path);

            foreach(DataRow row in data.Rows)
            {
                DataRow newRow = Outputresult.NewRow();
                newRow[0] = row["DEP"];
                if (sequnce == "JD581")
                {
                    double rtgr = Convert.ToDouble(row["RL3D"]);
                    double spgr = Convert.ToDouble(row["SP"]);
                    double t = Convert.ToDouble(row["HAC"]);                   
                    KSBcal(block, sequnce, rtgr, GR(MaxAndMin[0], MaxAndMin[1],spgr), t);
                    newRow[1] = kxd;
                    newRow[2] = stl;
                    newRow[3] = bhd;
                } 
                else
                {
                    double rtgr = Convert.ToDouble(row["LLD"]);
                    double spgr = Convert.ToDouble(row["GR"]);
                    double t = Convert.ToDouble(row["HAC"]);
                    KSBcal(block, sequnce, rtgr, GR(MaxAndMin[0], MaxAndMin[1], spgr), t);
                    
                    newRow[1] = kxd;
                    newRow[2] = stl;
                    newRow[3] = bhd;
                }

                Outputresult.Rows.Add(newRow);
                
            }
        }

        public void writeResult()
        {
            foreach (DataRow row in Outputresult.Rows)
            {
                System.Console.WriteLine(row[0].ToString() + "   " + row[1].ToString() + "   " + row[2].ToString() + "   " + row[3].ToString());
            }
            //RWFile file = new RWFile();

            //foreach (DataRow tempRow in wellnum.Rows)
            //{
            //    string jh = tempRow[0].ToString().Trim();
            //    DataRow[] matches = data.Select(" jh= '" + jh + "'");
            //    if (!Directory.Exists(path))
            //    {
            //        Directory.CreateDirectory(path);
            //        file.Append(path + "\\" + jh, "深度      孔隙度    渗透率    含油饱和度\r\n");
            //        foreach (DataRow row in matches)
            //        {
            //            string depth = row[1].ToString();
            //            string kxd = row[2].ToString();
            //            string stl = row[3].ToString();
            //            string bhd = row[4].ToString();

            //            string resultLine = depth + "   " + kxd + "   " + stl + "  " + bhd + "\r\n";
            //            file.Append(path + "\\" + jh, resultLine);
            //        }
            //    }
            //}
        }

        //计算孔隙度和渗透率
        public void KSBcal(string block, string sequnce, double rt, double spgr, double t, bool flag = true)
        {
            double[] result = new double[2];
            
            double a = 0;
            double b = 0;
            double m = 0;
            double n = 0;
            double vcl = 0;
            double bhd = 0;
            double bhd_temp = 0;
            double rw = 0;

            if (sequnce == "JD581")
            {
                switch (block)
                {
                    case "宋芳屯宋北区块":
                        a = 1.1691;
                        b = 1.0813;
                        m = 1.8188;
                        n = 1.586;
                        rw = 0.36;
                        vcl = Math.Pow(10,(2.421-0.0221*spgr));
                        kxd = -34.395 + 0.186 * t + 2.485 * spgr;
                        stl = -4.29 + 0.189 * kxd + 1.59 * Math.Log(rt) - 0.214 * spgr;
                        bhd_temp = (1/rt*(Math.Pow(vcl,1-vcl)/Math.Pow(2.5,0.5) + Math.Pow(kxd,m/2)/Math.Pow(a*rw,0.5)));
                        bhd = Math.Pow(bhd_temp, n);
                        //System.Console.WriteLine(t.ToString()+" "+bhd.ToString());
                        break;
                    case "宋芳屯宋南区块":
                        a = 0.9766;
                        b = 1.9325;
                        m = 1.0431;
                        n = 1.537;
                        rw = 0.35;
                        vcl = Math.Pow(10,(2.421-0.0221*spgr));
                        kxd = -18.379 + 0.128 * t + 4.727 * spgr;
                        stl = -2.678 + 0.178 * kxd + 0.222 * Math.Log(rt) - 0.51 * spgr;
                        bhd_temp = (1/rt*(Math.Pow(vcl,1-vcl)/Math.Pow(2.5,0.5) + Math.Pow(kxd,m/2)/Math.Pow(a*rw,0.5)));
                        bhd = Math.Pow(bhd_temp, n);
                        break;
                    case "宋芳屯宋西区块":
                        a = 1.0215;
                        b = 1.9078;
                        m = 1.0933;
                        n = 1.4625;
                        rw = 0.37;
                        vcl = Math.Pow(10,(2.421-0.0221*spgr));
                        kxd = -27.684 + 0.156 * t + 4.620 * spgr;
                        stl = -3.030 + 0.191 * kxd + 0.413 * Math.Log(rt) - 0.646 * spgr;
                        bhd_temp = (1/rt*(Math.Pow(vcl,1-vcl)/Math.Pow(2.5,0.5) + Math.Pow(kxd,m/2)/Math.Pow(a*rw,0.5)));
                        bhd = Math.Pow(bhd_temp, n);
                        break;
                    case "宋芳屯油田":
                        kxd = -16.461 + 0.120 * t + 4.861 * spgr;
                        stl = -11.023 + 8.698 * Math.Log(kxd) + 0.413 * Math.Log(rt) - 0.149 * spgr;
                        break;
                    case "升平油田":
                        a = 1.0181;
                        b = 1.8476;
                        m = 1.0997;
                        n = 1.4969;
                        rw = 0.37;
                        vcl = Math.Pow(10,(2.421-0.0221*spgr));
                        kxd = -10.598 + 0.115 * t + 0.416 * spgr;
                        stl = -8.498 + 7.649 * Math.Log(kxd) + 0.111 * Math.Log(rt) - 0.293 * spgr;
                        bhd_temp = (1/rt*(Math.Pow(vcl,1-vcl)/Math.Pow(2.5,0.5) + Math.Pow(kxd,m/2)/Math.Pow(a*rw,0.5)));
                        bhd = Math.Pow(bhd_temp, n);
                        break;
                    case "徐家围子油田":
                        a = 1.1341;
                        b = 1.7964;
                        m = 1.1014;
                        n = 1.4742;
                        rw = 0.37;
                        vcl = Math.Pow(10,(2.421-0.0221*spgr));
                        kxd = -18.802 + 0.144 * t + 1.224 * Math.Log(spgr);
                        stl = -2.915 + 0.197 * Math.Log(kxd) + 0.392 * Math.Log(rt) - 0.26 * spgr;
                        break;
                    case "永乐油田":
                        a = 1.1341;
                        b = 1.7964;
                        m = 1.1014;
                        n = 1.4742;
                        rw = 0.30;
                        vcl = Math.Pow(10,(2.421-0.0221*spgr));
                        kxd = -32.702 + 0.171 * t + 5.767 * spgr;
                        stl = -2.999 + 0.132 * kxd + 1.186 * Math.Log(rt) + 0.445 * spgr;
                        break;
                }
            }
            else
            {
                switch (block)
                {
                    case "宋芳屯宋北区块":
                        a = 1.1691;
                        b = 1.0813;
                        m = 1.8188;
                        n = 1.586;
                        rw = 0.36;
                        vcl = Math.Pow(10,(1.285+0.01242*spgr));
                        kxd = -25.993 + 0.164 * t - 5.841 * spgr;
                        stl = 10.063 + 0.09912 * kxd - 5.856 * Math.Log(rt) - 0.133 * spgr;
                        bhd_temp = (1/rt*(Math.Pow(vcl,1-vcl)/Math.Pow(2.5,0.5) + Math.Pow(kxd,m/2)/Math.Pow(a*rw,0.5)));
                        bhd = Math.Pow(bhd_temp, n);
                        break;
                    case "宋芳屯宋西区块":
                        a = 1.1691;
                        b = 1.0813;
                        m = 1.8188;
                        n = 1.586;
                        rw = 0.36;
                        vcl = Math.Pow(10,(1.285+0.01242*spgr));
                        kxd = -33.203 + 0.192 * t - 8.581 * spgr;
                        stl = 0.491 + 0.190 * kxd - 0.01414 * Math.Log(rt) - 1.966 * Math.Log(spgr);
                        bhd_temp = (1/rt*(Math.Pow(vcl,1-vcl)/Math.Pow(2.5,0.5) + Math.Pow(kxd,m/2)/Math.Pow(a*rw,0.5)));
                        bhd = Math.Pow(bhd_temp, n);
                        break;
                    case "宋芳屯油田":
                        a = 1.1691;
                        b = 1.0813;
                        m = 1.8188;
                        n = 1.586;
                        rw = 0.36;
                        vcl = Math.Pow(10,(1.285+0.01242*spgr));
                        kxd = -26.864 + 0.152 * t + 5.670 * spgr;
                        stl = 5.311 + 0.119 * kxd + 0.671 * Math.Log(rt) - 3.906 * Math.Log(spgr);
                        bhd_temp = (1/rt*(Math.Pow(vcl,1-vcl)/Math.Pow(2.5,0.5) + Math.Pow(kxd,m/2)/Math.Pow(a*rw,0.5)));
                        bhd = Math.Pow(bhd_temp, n);
                        break;
                    case "升平油田":
                        if (flag)
                        {
                            a = 1.0181;
                            b = 1.8476;
                            m = 1.0997;
                            n = 1.4969;
                            rw = 0.37;
                            vcl = Math.Pow(10, (1.845 + 0.747 * spgr));
                            kxd = -18.819 + 0.133 * t + 3.836 * spgr;
                            stl = -2.561 + 0.187 * kxd + 0.692 * Math.Log(rt) - 0.00592 * Math.Log(spgr);
                            bhd_temp = (1 / rt * (Math.Pow(vcl, 1 - vcl) / Math.Pow(2.5, 0.5) + Math.Pow(kxd, m / 2) / Math.Pow(a * rw, 0.5)));
                            bhd = Math.Pow(bhd_temp, n);
                        }
                        else
                        {
                            //kxd = -2.647 + 0.128 * t - 6.151 + 2.633 * spgr;
                            //stl = 0.619 + 0.238 * kxd - 1.636  - 0.00292 * spgr;
                        }
                        break;
                    case "徐家围子油田":

                        a = 1.1341;
                        b = 1.7964;
                        m = 1.1014;
                        n = 1.4742;
                        rw = 0.37;
                        vcl = Math.Pow(10,(2.421-0.0221*spgr));

                        kxd = -37.395 + 0.205 * t + 6.182 * Math.Log(spgr);
                        stl = -1.703 + 0.163 * kxd + 0.616 * Math.Log(rt) - 0.0141 * spgr;
                        bhd_temp = (1 / rt * (Math.Pow(vcl, 1 - vcl) / Math.Pow(2.5, 0.5) + Math.Pow(kxd, m / 2) / Math.Pow(a * rw, 0.5)));
                        bhd = Math.Pow(bhd_temp, n);
                        break;
                    case "肇州油田葡萄花油层":
                        if (flag)
                        {
                            a = 1.0825;
                            b = 1.8528;
                            m = 1.0835;
                            n = 1.4311;
                            rw = 0.29;
                            vcl = Math.Pow(10, (1.463 + 0.01652 * spgr));
                            
                            kxd = -18.819 + 0.133 * t + 3.836 * spgr;
                            stl = -2.561 + 0.187 * kxd + 0.692 * Math.Log(rt) - 0.00592 * Math.Log(spgr);
                            bhd_temp = (1 / rt * (Math.Pow(vcl, 1 - vcl) / Math.Pow(2.5, 0.5) + Math.Pow(kxd, m / 2) / Math.Pow(a * rw, 0.5)));
                            bhd = Math.Pow(bhd_temp, n);
                        }
                        else
                        {
                        }
                        break;
                    case "":
                        if (flag)
                        {
                            kxd = -18.819 + 0.133 * t + 3.836 * spgr;
                            stl = -2.561 + 0.187 * kxd + 0.692 * Math.Log(rt) - 0.00592 * Math.Log(spgr);
                        }
                        else
                        {
                        }
                        break;
                    case "永乐油田":
                        a = 1.1341;
                        b = 1.7964;
                        m = 1.1014;
                        n = 1.4742;
                        rw = 0.30;
                        vcl = Math.Pow(10,(2.397+1.547*spgr));
                        kxd = -31.912 + 0.168 * t + 5.613 * spgr;
                        stl = -8.144 + 6.349 * Math.Log(kxd) - 0.00408 * Math.Log(rt) + 1.218 * Math.Log(spgr);
                        bhd_temp = (1 / rt * (Math.Pow(vcl, 1 - vcl) / Math.Pow(2.5, 0.5) + Math.Pow(kxd, m / 2) / Math.Pow(a * rw, 0.5)));
                            bhd = Math.Pow(bhd_temp, n);
                        break;
                    case "卫星油田":
                        a = 1.5951;
                        b = 1.5023;
                        m = 1.1303;
                        n = 1.6323;
                        rw = 0.30;
                        vcl = Math.Pow(10,(1.810+1.113*spgr));
                        kxd = -14.806 + 0.133 * t + 5.79 * spgr;
                        stl = -9.152 + 7.822 * kxd + 0.238 * Math.Log(rt) - 0.0138 * spgr;
                        break;
                }
            }
        }
    }
}
