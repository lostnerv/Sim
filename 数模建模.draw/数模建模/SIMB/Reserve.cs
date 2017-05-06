using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using 建模数模.tools;
using System.IO;
using 数模建模.tools;

namespace 数模建模.SIMB
{
    class Reserve
    {
        public DataTable nyNcyl = new DataTable();
        public DataTable djncyl(string jh)
        {
            jh = jh.Trim().ToUpper();
            // 使用max 避免报废井等问题
            DataTable data = new DataTable();
            String sql = "with w_ncyl as(select substr(ny,1,4) nf, max(ncyl) ncyl  from dba04 " 
                   +" where jh='"+jh+"' group by substr(ny,1,4) order by nf) "
                   +" select w_ncyl.*,sum(ncyl)over( order by nf)ljcyl from w_ncyl ";
            data = GetDataAsDataTable.GetDataReasult(sql);
            /*foreach (DataRow row in data.Rows)
            {
                double ncyl = Convert.ToDouble(row["ncyl"]);
                string year = row["nf"].ToString();
            }*/
            return data;
        }
        public DataTable qkljcyl(string qkdy)
        {
            DataTable data = new DataTable();
            String sql = "with w_jh as(select substr(ny,1,4) nf, b.jh,max(ncyl) ncyl from dba04 b "
                        +" left join daa01 a on a.jh=b.jh  "
                        + "  where  qkdy='" + qkdy + "' group by substr(ny,1,4),b.jh) " 
                        +" select a.* ,sum(ncyl) over( order by nf)ljcyl  "
                        + " from (  select nf,sum(ncyl)ncyl  from w_jh group by nf order by nf) a";
            data = GetDataAsDataTable.GetDataReasult(sql);
            nyNcyl = data;
            return data;
        }

        public double qkReserve(DataTable data)//=nyNcyl
        {
            int m=1;//X元数
            int n=0;//观测样本数
            double[] a = new double[m+1];  
            double[] v = new double[m];  
            double[] dt = new double[4];
            double minncyl = 0;
            int starti = 0;
            if (data.Rows.Count > 1)
            {
                for (int i = data.Rows.Count-2; i >= 0; i--)//除今年以外
                {
                    double tmpncyl=Convert.ToDouble(data.Rows[i]["ncyl"]);
                    if (tmpncyl > minncyl)
                    {
                        //System.Console.WriteLine(i+"+"+minncyl+"+"+tmpncyl);
                        minncyl=tmpncyl;
                        starti=i;
                        n++;
                    }
                    else
                        break;
                }
            }
            if (n >= 2)//至少2个数才线性回归
            {
                double[,] x = new double[1, n];
                double[] y = new double[n];
                int j = 0;

                for (int i = starti; i < data.Rows.Count - 1; i++)
                {
                    //System.Console.WriteLine(j+"++"+n);
                    double tmpncyl = Convert.ToDouble(data.Rows[i]["ncyl"]);
                    double tmpljcyl = Convert.ToDouble(data.Rows[i]["ljcyl"]);
                    x[0, j] = tmpljcyl;
                    y[j] = tmpncyl;
                    //System.Console.WriteLine("(" + x[0, j] + ")=" + y[j]);  
                    j++;

                }
                数模建模.SIMB.Linear.sqt2(x, y, m, n, a, dt, v);
                double result = Convert.ToDouble((-a[1] / a[0] - Convert.ToDouble(data.Rows[data.Rows.Count - 1]["ljcyl"])).ToString("0.0000"));
                return result;
            }
            else
                return -1;
        }

        public DataTable qkdy()
        {
            DataTable data = new DataTable();
            String sql = "select distinct qkdy from daa01 order by qkdy";
            data = GetDataAsDataTable.GetDataReasult(sql);
            return data;
        }
    }
}
