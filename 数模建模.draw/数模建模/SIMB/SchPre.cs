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
    class SchPre
    {
        private DataTable wellnum;
        private string wellSet = null;
        XmlHelper xmlsns = new XmlHelper();
        public DataTable resultData = new DataTable();
        //结果就是井号X1,X2,X3...,Xn
        public SchPre(DataTable dt)
        {
            if (!File.Exists("virtualWell.xml"))// 不存在xml文件
            {
                xmlsns.CreateXMLDocument("virtualWell.xml");
            }
            if (dt != null)
            {
                wellnum = dt;
                foreach (DataRow tempRow in wellnum.Rows)
                {
                    wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
                }
                wellSet = wellSet.Substring(0, wellSet.Length - 1);
            }

        }

        /*
         * 提取并生成油井、水井井史文件history.vol
         * pathNoName：文件目标路径（不含文件名）
         * @DSnow
         */
        public void writeHistory(String pathNoName,string yt,string cw)
        {
            DataTable data = new DataTable();
            String sql = "select jh WELL, substr(ny, 1, 4) YEAR, substr(ny, 5, 2) MONTH, '01' DAY, ycyl OIL, ycsl WATER, nvl(ycql,0) GAS, 0 WINJ, 0 GINJ, round(scts,1) DAYS from dba04 where jh in (" + wellSet + ") ";
            sql += "union all select jh WELL, substr(ny, 1, 4) YEAR, substr(ny, 5, 2) MONTH, '01' DAY, 0 OIL, 0 WATER, 0 GAS, yzsl WINJ, 0 GINJ, round(scts,1) DAYS from dba05 where jh in (" + wellSet + ")";

            data = GetDataAsDataTable.GetDataReasult(sql);
            resultData = data.Copy();
            RWFile file = new RWFile();
            file.Append(pathNoName + "\\history.vol", "*METRIC" + "\r\n");
            file.Append(pathNoName + "\\history.vol", "*MONTHlY" + "\r\n");
            file.Append(pathNoName + "\\history.vol", "*IGNORE_MISSING" + "\r\n");
            // 没有空格，用的TAB
            file.Append(pathNoName + "\\history.vol", "*WEll	*YEAR	*MONTH	*DAY	*OIl	*WATER	*GAS	*WINJ	*GINJ	*DAYS" + "\r\n");
            foreach (DataRow row in data.Rows)
            {
                double density = 0.867;// 密度初始值
                String wellnumTmp = row["WELL"].ToString();
                foreach (DataRow wrow in wellnum.Rows)// 密度判断
                {
                    if (wellnumTmp == wrow[0].ToString().Trim())
                    {
                        //string yt = wrow[1].ToString();
                        //string cw = wrow[2].ToString();
                        switch (yt)
                        {
                            // case "卫星":
                            //    if ("cw"=="P") 
                            //     break;//0.867
                            case "升平":
                                if ("F" == cw) density = 0.868;
                                break;
                            case "宋芳屯": if ("F" == cw) density = 0.872;
                                break;
                            case "徐家围子": density = 0.864;
                                break;
                            case "永乐": if ("P" == cw) density = 0.865; else density = 0.866;
                                break;
                            case "肇州": if ("P" == cw) density = 0.863;
                                break;
                            default:
                                break;
                        }
                        break;
                    };
                }
                if (wellnumTmp.Length > 8)//井号名称长度大于8的，取虚拟井号xml文件
                {
                    String tmpVal2 = xmlsns.GetXMLDocument2("virtualWell.xml", wellnumTmp);
                    if (tmpVal2 != null && tmpVal2 != "")
                    {
                        wellnumTmp = tmpVal2;
                    }
                }
                String result = wellnumTmp + "	" + row["YEAR"].ToString() + "	" + row["MONTH"].ToString() + "	" + row["DAY"].ToString()
                    + "	" + (System.Convert.ToDouble(row["OIl"].ToString()) / density).ToString("0.00") + "	" + row["WATER"].ToString() + "	" + row["GAS"].ToString()
                    + "	" + row["WINJ"].ToString() + "	" + row["GINJ"].ToString() + "	" + row["DAYS"].ToString() + "\r\n";
                file.Append(pathNoName + "\\history.vol", result);
            }

        }

        /*
         * 添加或修改，以井号为主键，删除的话可以写成{“井号”，“”}
         * @DSnow
         */
        public void saveorUpdateWellXML(String val1, String val2)
        {
            //val1井号 val2虚拟井号
            String tmpVal2 = xmlsns.GetXMLDocument2("virtualWell.xml", val1);

            List<String> vallist = new List<String>();
            vallist.Add(val1.Trim().ToUpper());
            vallist.Add(val2.Trim().ToUpper());
            if (tmpVal2 != null)
            {
                xmlsns.EditXMLDocument("virtualWell.xml", vallist);
            }
            else
            {
                xmlsns.AddXMLDocument("virtualWell.xml", vallist);
            }
        }
        // 射孔
        // unknowNum默认0.1397
        public void writeSk(String pathNoName, String unknowNum = "0.1397")
        {
            DataTable data = new DataTable();
            String sql = "select a.jh, to_char( case when tcrq>sgrq then sgrq else trunc(tcrq,'month') end,'dd.mm.yyyy') rq,jdds1,jdds2 from daa091 d left join daa01 a on trim(d.jh)=a.jh ";
            sql += " where a.jh  in (" + wellSet + ") order by jh,jdds1";
            String lastWell = null;
            data = GetDataAsDataTable.GetDataReasult(sql);
            resultData = data.Copy();
            RWFile file = new RWFile();
            file.Append(pathNoName + "\\sk.ev", "--	DATE	EVENT	LAYER	TOP	BOTTOM	WELLDIA	SKIN	kh	TABLE_NO	BHP" + "\r\n");
            file.Append(pathNoName + "\\sk.ev", "UNITS METRIC" + "\r\n");
            foreach (DataRow row in data.Rows)
            {
                if (row["jh"].ToString() != lastWell)
                {
                    file.Append(pathNoName + "\\sk.ev", "WELLNAME	" + row["jh"].ToString() + "\r\n");
                    lastWell = row["jh"].ToString();
                }
                String result = "	" + row["rq"].ToString() + "	perforation	" + row["jdds1"].ToString() + "	" + row["jdds2"].ToString()
                    + "	" + unknowNum + "	" + "0" + "\r\n";
                file.Append(pathNoName + "\\sk.ev", result);
            }
        }
        // 措施文件 需求有问题 
        // 待定!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // 2016-7-28新需求修改后
        public void writeCs(String pathNoName)
        {
            DataTable data = new DataTable();
            // +"  csmc,"
            // +"  sgfs,"
            // +"  cscd"
            String sql = "with w_manyds as(select distinct bccs.jh,"
                        + "wgrq, rq,"
                        +"sgjdds1,"
                        +"sgjdds2,"
                        +"case"
                        +" when csmc = '压裂' or (csmc like '其它%' and"
                        +"  sgfs in ('水力喷射径向水平打孔',"
                        +"    '超短半径侧钻水平井',"
                        +"   '水力喷射',"
                        +"   '水力喷射深度打',"
                        +"    '超短半径',"
                        +"   '水力喷射打孔')) then"
                        +"'frac'"
                        + "when  sgfs in( '机械堵水','化学堵水') or"
                        +"   (csmc like '其它%' and sgfs in ('化学堵水', '炮眼封堵')) "
                        +"  then"
                        +"  'squeeze'"
                        +" when csmc = '酸化' then"
                        +"   'acidise'"
                        +"  when csmc = '补孔' or csmc = '射孔' then"
                        +"    'perforaton'"
                        +"   else"
                        +"    csmc"
                        + " end csmc, d91.jdds1,d91.jdds2 , "
                        + " case when csmc = '补孔' then 0 "
                        + " when csmc = '压裂' or csmc = '酸化' then -2 "
                        + " end S "
                        + "from BC_CSCGJL@Dzcxyh.regress.rdbms.dev.us.oracle.com bccs "
                        + " left join daa091@Dzcxyh.regress.rdbms.dev.us.oracle.com d91 on d91.jh=bccs.jh "
                        +" where csmc || cscd <> '卡堵水全井'"
                        +" and sgfs not in ('自生气吞吐', '自生气解堵', '二氧化碳吞吐增油量少')"
                        // + " and csmc not in ('调参', '换泵')   "
                        + "and ( csmc in ('卡堵水','补孔','压裂','酸化')  "
                        +"or sgfs in('水力喷射径向水平打孔','超短半径侧钻水平井','水力喷射','水力喷射深度打','超短半径','水力喷射打孔','化学堵水','炮眼封堵'))"
                        + " and bccs.jh  in (" + wellSet + ")) "
                        + "  select t2.JH,"
                        + "case when wgrq<RQ then  to_char(trunc(tcrq,'month') , 'dd.mm.yyyy')"
                        + " else  to_char(wgrq, 'dd.mm.yyyy') end rq,"
                        + "   RQ, nvl(sgjdds1,nulljdds1)sgjdds1, "
                        + "  nvl(sgjdds2,nulljdds2)sgjdds2, CSMC, JDDS1, JDDS2 , "
                        + "  case when csmc='acidise' or (jdds1<sgjdds1 and sgjdds1<jdds2) "
                        + "     then null   else 1 end wrong,S "
                        + "   from (  select t.*, row_number()over(partition by t.jh,t.sgjdds1 order by abs(t.jdds1-t.sgjdds1),jdds1) row_rank   "
                        + "    from w_manyds t  ) t2 "
                        + "left join(select jh,min(jdds1)nulljdds1,max(jdds2)nulljdds2 from daa091@Dzcxyh.regress.rdbms.dev.us.oracle.com "
                        + " where jh in (" + wellSet + ") "  
                        + "  group by jh) g91 on g91.jh=t2.jh "
                        + "left join daa01@Dzcxyh.regress.rdbms.dev.us.oracle.com  d1 on t2.jh=d1.jh "
                        + " where row_rank = 1  "
                        +"order by jh, sgjdds1 ";
            String lastWell = null;
            //Console.Write(sql);
            data = GetDataAsDataTable.GetDataReasult(sql);
            resultData = data.Copy();
            RWFile file = new RWFile();
            //file.Append(pathNoName + "\\cs.ev", "--	DATE	EVENT	LAYER	TOP	BOTTOM	WELLDIA	SKIN	kh	TABLE_NO	BHP" + "\r\n");
            file.Append(pathNoName + "\\cs.ev", "--	DATE	EVENT	TOP	BOTTOM	-2	JDDS1	JDDS2	WRONG	S" + "\r\n");//s表皮系数
            file.Append(pathNoName + "\\cs.ev", "UNITS METRIC" + "\r\n");
            foreach (DataRow row in data.Rows)
            {
                if (row["jh"].ToString() != lastWell)
                {
                    file.Append(pathNoName + "\\cs.ev", "WELLNAME	" + row["jh"].ToString() + "\r\n");
                    lastWell = row["jh"].ToString();
                }
                String result = "	" + row["rq"].ToString() + "	" + row["csmc"].ToString() + "	" + row["sgjdds1"].ToString() + "	" + row["sgjdds2"].ToString()
                + "	" + "-2" + "	" + row["jdds1"].ToString() + "	" + row["jdds2"].ToString() + "	" + row["wrong"].ToString() + "	" + row["s"].ToString() + "\r\n";
                file.Append(pathNoName + "\\cs.ev", result);
            }
        }
        // daa01没有cyfs 无法分辨提捞井,所以使用了A2_105.REGRESS.RDBMS.DEV.US.ORACLE.COM
        // dblink中，只有A2_105.REGRESS.RDBMS.DEV.US.ORACLE.COM的井号数量与A2一致，本地daa01少了1000+
        public void writeWellnet(String pathNoName)
        {
            DataTable data = new DataTable();
            String sql = "select well_desc, case when oil_production_method='1G' then 'BAILING' when well_purpose like '1%' then 'PRODUCER' when well_purpose like '3%' then 'INJECTOR' else to_char(well_purpose) end net ";
            sql += "from cd_well_source@A2_105.REGRESS.RDBMS.DEV.US.ORACLE.COM s left join pc_dev_well_attr@A2_105.REGRESS.RDBMS.DEV.US.ORACLE.COM a on trim(a.well_id)=trim(s.well_id) where well_desc  in (" + wellSet + ")";

            data = GetDataAsDataTable.GetDataReasult(sql);
            resultData = data.Copy();
            RWFile file = new RWFile();
            file.Append(pathNoName + "\\well.net", "*DATE SOS" + "\r\n");
            file.Append(pathNoName + "\\well.net", "*GROUPNODE" + "\r\n");
            file.Append(pathNoName + "\\well.net", "'PRODUCER'	'FIELD'" + "\r\n");
            file.Append(pathNoName + "\\well.net", "'INJECTOR'	'FIELD'" + "\r\n");
            file.Append(pathNoName + "\\well.net", "'BAILING'	'FIELD'" + "\r\n");
            file.Append(pathNoName + "\\well.net", "*LEAFNODE" + "\r\n");
            foreach (DataRow row in data.Rows)
            {
                String result = "'" + row["well_desc"].ToString() + "'" + "	" + "'" + row["net"].ToString() + "'" + "\r\n";
                file.Append(pathNoName + "\\well.net", result);
            }
        }
        // 是井底流压限制文件
        // 给我个bhpNum
        public void writeLimit(String pathNoName, String bhpNum)
        {
            DataTable data = new DataTable();
            String sql = "select to_char(to_date(ny,'yyyymm'),'dd.mm.yyyy')rq,jh,ny from dba04 where jh  in (" + wellSet + ") ";
            sql += "union all select to_char(to_date(ny,'yyyymm'),'dd.mm.yyyy')rq,jh,ny from dba05 where jh  in (" + wellSet + ") order by jh,ny";
            String lastWell = null;
            data = GetDataAsDataTable.GetDataReasult(sql);
            resultData = data.Copy();
            RWFile file = new RWFile();
            file.Append(pathNoName + "\\bhp.ev", "UNITS METRIC" + "\r\n");
            file.Append(pathNoName + "\\bhp.ev", "单位 bar" + "\r\n");
            foreach (DataRow row in data.Rows)
            {
                if (row["jh"].ToString() != lastWell)
                {
                    file.Append(pathNoName + "\\bhp.ev", "WELLNAME	" + row["jh"].ToString() + "\r\n");
                    lastWell = row["jh"].ToString();
                }
                String result = row["rq"].ToString() + "	" + "BHP" + "	" + bhpNum + "\r\n";
                file.Append(pathNoName + "\\bhp.ev", result);
            }
        }


    }
}
