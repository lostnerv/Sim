using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using 建模数模.tools;

namespace 数模建模
{
    class DataCheck
    {
        private DataTable wellnum;
        private string wellSet = null;

        public DataCheck(DataTable dt)
        {
            wellnum = dt;
            foreach (DataRow tempRow in wellnum.Rows)
            {
                wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
            }
            wellSet = wellSet.Substring(0, wellSet.Length - 1);
        }

        public DataTable CheckData()
        {
            System.Console.WriteLine(wellnum.Rows.Count);
            DataTable dt4 = new DataTable();

            DataColumn daa02Column = new DataColumn();
            daa02Column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            daa02Column.ColumnName = "Daa02不全";//该列得名称 
            dt4.Columns.Add(daa02Column);

            DataColumn daa074Column = new DataColumn();
            daa074Column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            daa074Column.ColumnName = "Daa074不全";//该列得名称 
            dt4.Columns.Add(daa074Column);

            DataColumn daa073Column = new DataColumn();
            daa073Column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            daa073Column.ColumnName = "Daa073不全";//该列得名称 
            dt4.Columns.Add(daa073Column);

            DataColumn daa071Column = new DataColumn();
            daa071Column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            daa071Column.ColumnName = "Daa071不全";//该列得名称 
            dt4.Columns.Add(daa071Column);

            DataColumn daa01Column = new DataColumn();
            daa01Column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            daa01Column.ColumnName = "Daa01不全";//该列得名称 
            dt4.Columns.Add(daa01Column);

            DataColumn dba04Column = new DataColumn();
            dba04Column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            dba04Column.ColumnName = "Dba04不全";//该列得名称 
            dt4.Columns.Add(dba04Column);

            DataColumn dba05Column = new DataColumn();
            dba05Column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            dba05Column.ColumnName = "Dba05不全";//该列得名称 
            dt4.Columns.Add(dba05Column);

            string sqldaa074 = "select distinct(jh) as wellnum from daa074 where jh in (" + wellSet + ")";
            string sqldaa054 = "select distinct(jh) as wellnum from daa054 where jh in (" + wellSet + ")";
            string sqldaa02 = "select distinct(jh) as wellnum from daa02 where jh in (" + wellSet + ")";
            string sqldaa073 = "select distinct(jh) as wellnum from daa073 where jh in (" + wellSet + ")";
            string sqldaa071 = "select distinct(jh) as wellnum from daa071 where jh in (" + wellSet + ")";
            string sqldaa091 = "select distinct(jh) as wellnum from daa091 where jh in (" + wellSet + ")";
            string sqldba05 = "select distinct(jh) as wellnum from dba05 where jh in (" + wellSet + ")";
            string sqldba04 = "select distinct(jh) as wellnum from dba04 where jh in (" + wellSet + ")";
            string sqldaa01 = "select jh as wellnum from daa01 where jh in (" + wellSet + ")";
            string sql04 = "select jh from daa01 where substr(mqjb,0,1) = '1' and jh in (" + wellSet + ")";
            string sql05 = "select jh from daa01 where substr(mqjb,0,1) = '3' and jh in (" + wellSet + ")";

            DataTable result04 = GetDataAsDataTable.GetDataReasult(sql04);
            DataTable result05 = GetDataAsDataTable.GetDataReasult(sql05);
            DataTable resultdba04 = GetDataAsDataTable.GetDataReasult(sqldba04);
            DataTable resultdba05 = GetDataAsDataTable.GetDataReasult(sqldba05);

            DataTable resultdaa02 = GetDataAsDataTable.GetDataReasult(sqldaa02);
            DataTable resultdaa074 = GetDataAsDataTable.GetDataReasult(sqldaa074);
            DataTable resultdaa054 = GetDataAsDataTable.GetDataReasult(sqldaa054);
            DataTable resultdaa073 = GetDataAsDataTable.GetDataReasult(sqldaa073);
            DataTable resultdaa071 = GetDataAsDataTable.GetDataReasult(sqldaa071);
            DataTable resultdaa01 = GetDataAsDataTable.GetDataReasult(sqldaa01);
           
            var nodaa02 = wellnum.AsEnumerable().Except(resultdaa02.AsEnumerable(), DataRowComparer.Default);
            var nodaa074 = wellnum.AsEnumerable().Except(resultdaa074.AsEnumerable(), DataRowComparer.Default);
            var nodaa054 = wellnum.AsEnumerable().Except(resultdaa054.AsEnumerable(), DataRowComparer.Default);
            var nodaa073 = wellnum.AsEnumerable().Except(resultdaa073.AsEnumerable(), DataRowComparer.Default);
            var nodaa071 = wellnum.AsEnumerable().Except(resultdaa071.AsEnumerable(), DataRowComparer.Default);
            var nodaa01 = wellnum.AsEnumerable().Except(resultdaa01.AsEnumerable(), DataRowComparer.Default);

            var nodaa04 = result04.AsEnumerable().Except(resultdba04.AsEnumerable(), DataRowComparer.Default);
            var nodaa05 = result05.AsEnumerable().Except(resultdba05.AsEnumerable(), DataRowComparer.Default);

            foreach (var temp in nodaa02)
            {
                DataRow newrow = dt4.NewRow();
                newrow[0] = temp["wellnum"];
                dt4.Rows.Add(newrow);
            }
            foreach (var temp in nodaa074)
            {
                DataRow newrow = dt4.NewRow();
                newrow[1] = temp["wellnum"];
                dt4.Rows.Add(newrow);
            }
            foreach (var temp in nodaa073)
            {
                DataRow newrow = dt4.NewRow();
                newrow[2] = temp["wellnum"];
                dt4.Rows.Add(newrow);
            }
            foreach (var temp in nodaa071)
            {
                DataRow newrow = dt4.NewRow();
                newrow[3] = temp["wellnum"];
                dt4.Rows.Add(newrow);
            }
            foreach (var temp in nodaa01)
            {
                DataRow newrow = dt4.NewRow();
                newrow[4] = temp["wellnum"];
                dt4.Rows.Add(newrow);
            }
            foreach (var temp in nodaa04)
            {
                DataRow newrow = dt4.NewRow();
                newrow[5] = temp["wellnum"];
                dt4.Rows.Add(newrow);
            }
            foreach (var temp in nodaa05)
            {
                DataRow newrow = dt4.NewRow();
                newrow[6] = temp["wellnum"];
                dt4.Rows.Add(newrow);
            }
           
            return dt4;
        }
    }



}
