using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using 建模数模.tools;
using System.IO;
using System.Data.OleDb;

namespace 数模建模.SIMB
{
    class Fitting
    {
        private DataTable wellnum;
        private double myStand = 0;
        private int ngNum = 0;
        private double ngRate = 0;

        public DataTable dt = new DataTable();
        public string stageStr; //前中后期
        //拟合率
        public Fitting(string FilePath, double standard,string stage)//手输标准 1 >1<1不合格
        {
            stageStr = stage;
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            column.ColumnName = "DATE";
            dt.Columns.Add(column);
            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.String");//该列的数据类型 
            column2.ColumnName = "FWCT";
            dt.Columns.Add(column2);
            DataColumn column3 = new DataColumn();
            column3.DataType = System.Type.GetType("System.String");//该列的数据类型 
            column3.ColumnName = "FWCTH";
            dt.Columns.Add(column3);
            //读取excel
            importExcelToDataSet(FilePath);
            if (dt != null)
            {
                wellnum = dt;// 包含DATE FWCT FWCTH
                DataColumn column4 = new DataColumn();
                column4.DataType = System.Type.GetType("System.String");//该列的数据类型 
                column4.ColumnName = "val";
                dt.Columns.Add(column4);

                myStand = standard;
                int total = 0;
                foreach (DataRow row in wellnum.Rows)
                {
                    double fwct = double.Parse(row[1].ToString());
                    double fwcth = double.Parse(row[2].ToString());
                    double val = 0;
                    if (fwcth != 0)
                    {
                        val = Math.Abs(Math.Round(100 * (fwct - fwcth) / fwcth, 0));
                    }
                    row[3] = val;
                    if (val > standard)//不合格
                    {
                        ngNum++;
                    }
                    total++;
                }
                double rawNgRate = 100.0 * (double)ngNum / (double)total;
                ngRate = Math.Round(rawNgRate, 2);
             }          
        }

        public DataTable getTable()
        {
            return wellnum;
        }

        public int getNgNum()//不合格数
        {
            return ngNum;
        }

        public double getNgRate()//不合格率
        {
            return ngRate;
        }

        public DataSet importExcelToDataSet(string FilePath)
        {
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + FilePath + ";Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
            DataSet myDataSet = new DataSet();
            //默认第一行为表头索引
            try
            {
                myCommand.Fill(myDataSet);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("该Excel文件的工作表的名字不正确," + ex.Message);
            }
            conn.Close();
            //遍历一个表多行一列
            int rowCount = 0;
            int rowStage = 0;
            foreach (DataRow row in myDataSet.Tables[0].Rows)
            {
                if ("".Equals(row[0].ToString()))
                    break;
                //DataRow dtrow = dt.NewRow();
                //dtrow[0] = row[0].ToString();
                //dtrow[1] = row[1].ToString();
                //dtrow[2] = row[2].ToString();
                //dt.Rows.Add(dtrow);
                rowCount++;
            }
            rowStage = rowCount / 3;
            switch (stageStr)
            {
                case "前期":
                    for (int i = 0; i < rowStage;i++ )
                    {
                        DataRow dtrow = dt.NewRow();
                        dtrow[0] = myDataSet.Tables[0].Rows[i][0].ToString();
                        dtrow[1] = myDataSet.Tables[0].Rows[i][1].ToString();
                        dtrow[2] = myDataSet.Tables[0].Rows[i][2].ToString();
                        dt.Rows.Add(dtrow);
                    }
                    break;
                case "中期":
                    for (int i = rowStage; i < rowStage*2; i++)
                    {
                        DataRow dtrow = dt.NewRow();
                        dtrow[0] = myDataSet.Tables[0].Rows[i][0].ToString();
                        dtrow[1] = myDataSet.Tables[0].Rows[i][1].ToString();
                        dtrow[2] = myDataSet.Tables[0].Rows[i][2].ToString();
                        dt.Rows.Add(dtrow);
                    }
                    break;
                case "后期":
                    for (int i = rowStage * 2; i < rowCount; i++)
                    {
                        DataRow dtrow = dt.NewRow();
                        dtrow[0] = myDataSet.Tables[0].Rows[i][0].ToString();
                        dtrow[1] = myDataSet.Tables[0].Rows[i][1].ToString();
                        dtrow[2] = myDataSet.Tables[0].Rows[i][2].ToString();
                        dt.Rows.Add(dtrow);
                    }
                    break;
                default:
                    break;
            }
            return myDataSet;
        }

    }
}
