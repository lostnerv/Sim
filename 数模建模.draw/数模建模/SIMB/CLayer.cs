using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using 建模数模.tools;
using System.IO;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows;

namespace 数模建模.SIMB
{
    struct ImageArray
    {
        //ijk总数
        public int total_i;
        public int total_j;
        public int total_k;
        //所有体的容器
        public List<Point_data> m_values ;//List 结构体不能有初始值设定
    };

    class Point_data//struct无法修改“xxx”的返回值，因为它不是变量;方法2 每次赋值都实例化
    {
	    //体的ijk值
	    public int i;
        public int j;
        public int k;
	    //fgrid值
        public double[] Egrid_values;
	    //prt的值
        public double Prt_value;
    };
     //点
     struct FLOAT_POINT
     {
         public double x;
         public double y;
     };
     //沉积相类型设置
     struct Facies_Type
     {
         public int facies_type;
         public string name;
         public COLORREF color;//Color是byte
     };
     struct Oil_Type
    {
	    public int id;
	    public string name;
	    public COLORREF color;
    };
     struct COLORREF //自建
     {
         public int R;
         public int G;
         public int B;
     }; 
    struct CSize //自建
    {
        public double cx;
        public double cy;
    };
    enum myEnum : int
    {
        layertype_model = 0,
        layertype_facies = 1,
        layertype_fault = 2,
        layertype_boundary = 3,
        layertype_well = 4,
        layertype_oilboundary = 5
    }; 
    class CLayer
    {
        private double M_PI = 3.14159265358979323846; 
        private double m_d_Facies_Grid_Size;
        private ImageArray image_array= new ImageArray();
        private Dictionary<string, ImageArray> Image = new Dictionary<string, ImageArray>();
        //沉积相数据
        private List<FLOAT_POINT> m_vFacies = new List<FLOAT_POINT>();
        private List<Facies_Type> m_vFaciesTypeTable = new List<Facies_Type>();
        private List<int> m_vFaciesType = new List<int>();
        //断层数据
        private List<List<FLOAT_POINT>> m_vFault = new List<List<FLOAT_POINT>>();
        //边界数据
        private List<List<FLOAT_POINT>> m_vBoundary = new List<List<FLOAT_POINT>>();
        private double m_d_xmin;
        private double m_d_xmax;
        private double m_d_ymin;
        private double m_d_ymax;
        private bool m_b_need_recalc;
        private int m_k;//当前层
        private int m_i_seloilboundary;
        private FLOAT_POINT m_origin =new FLOAT_POINT();
        private CSize m_size=new CSize();
        private double m_d_zoomfactor;
        private List<TRIVERTEX> v_model_tri = new List<TRIVERTEX>();//<TRIVERTEX>
        private List<TRIVERTEX> v_facies_tri = new List<TRIVERTEX>();//<TRIVERTEX>
        private List<GRADIENT_TRIANGLE> v_model_triangle= new  List<GRADIENT_TRIANGLE>();
        private List<GRADIENT_RECT>  v_facies_rect= new  List<GRADIENT_RECT>();
        private List<List<Point> > m_vFault_draw=new List<List<Point> >() ;
        private List<List<Point> > m_vBoundary_draw =new List<List<Point> > ();
        //井位数据
	    private List<FLOAT_POINT> m_vWell=new List<FLOAT_POINT> ();
	    private List<Point> m_vWell_draw=new List<Point> ();
	    double m_d_wellsize;
        //剩余油区域
	    private List<List<FLOAT_POINT> > m_vOilBoundary=new List<List<FLOAT_POINT> >();
	  //  CPen m_OilPen;
	    private List<List<Point> > m_vOilBoundary_draw=new List<List<Point> > ();
	    private List<int> m_vOilType=new List<int>();
	    private List<Oil_Type> m_vOilTypeTable=new List<Oil_Type>();
       	// 图层类型，数模0，沉积相1，断层2，边界3，井位4，剩余油边界5
	   
        public CLayer()
        {
            m_b_need_recalc = true;
            m_d_xmin = float.MaxValue; ;
            m_d_xmax = -float.MaxValue; ;
            m_d_ymin = float.MaxValue; ;
            m_d_ymax = -float.MaxValue; ;
            m_k = 0;
            m_i_seloilboundary = -1;
            //m_pView1 = NULL;
            //m_pView2 = NULL;

            read_color_table("");
            read_oiltype_table("");
        }

        public bool read_grid(string str_file)
        {
            StreamReader filesr = new StreamReader(str_file, Encoding.Default);
            if (filesr==null)
	        {
		        //Basic_Func::ok_messbox("打开文件失败:B53-1_E100.FGRID--"+m_path_in, this);
		        return false;
	        }

	        string strLine;
	        //Progress_Mess progress("读取网格文件", 1024);
	        //string strOut;
	        //string strTemp;
	        //Point_data temp_data;
            string[]  parser;
	        //List<double> temp;
	        //int counter = 0;
	        int current_point = 0;

            while ((strLine = filesr.ReadLine()) != null)
	        {
		       strLine= strLine.Trim();

		        if(strLine.IndexOf("'DIMENS  '",0,9)>0)
		        {
			        strLine = filesr.ReadLine();
			        //TRACE("'DIMENS  '"+strLine+"\n");
                    strLine = strLine.Trim();
                    parser = strLine.Split(' ');
                    image_array.total_i = int.Parse(parser[0]);
                    image_array.total_j = int.Parse(parser[1]);
                    image_array.total_k = int.Parse(parser[2]);
			        //image_array.m_values.resize(image_array.total_i * image_array.total_j * image_array.total_k);
			        continue;
		        }

                if (strLine.IndexOf("'COORDS  '", 0, 9) > 0) 
		        {
                    strLine = filesr.ReadLine();
			        //TRACE("'COORDS  '"+strLine+"\n");
                    strLine = strLine.Trim();
                    parser = strLine.Split(' ');
                    //System.Console.WriteLine(image_array.m_values[current_point].i);
                    image_array.m_values[current_point].i = int.Parse(parser[0]);
                    image_array.m_values[current_point].j = int.Parse(parser[1]);
                    image_array.m_values[current_point].k = int.Parse(parser[2]);
			
			        continue;
		        }

                if (strLine.IndexOf("'CORNERS  '", 0, 9) > 0) 
		        {
			        int index = 0;
			        for(int i = 0;i < 6;i++)
			        {
                        strLine = filesr.ReadLine();
                        strLine = strLine.Trim();
                        parser = strLine.Split(' ');
				        for(int k = 0;k < parser.Length; k++)
				        {
                            image_array.m_values[current_point].Egrid_values[index] = Convert.ToDouble(parser[k]);
					        index++;
				        }
			        }
			        current_point++;
			        continue;
		        }

		        //progress.step();
	        }
            filesr.Close();
	        //file.Close();
	        //progress.finish();
	        return true;
        }
        public bool putValueInStruct(string str_file)
        {
            StreamReader filesr = new StreamReader(str_file, Encoding.Default);
            if (filesr == null)
            {
                //Basic_Func::ok_messbox("打开文件失败:B53-1_E100.FGRID--"+m_path_in, this);
                return false;
            }

            string strLine;
	        //Progress_Mess progress("填入数据", 1024);
            string date_temp;
	        //string strTemp;
	        //Point_data temp_data;
	        string[] parser;
	        //List<double> temp;
            string date="";
	        int count = 0;
	        int k = 0;

            while ((strLine = filesr.ReadLine()) != null)
	        {
                strLine = strLine.Trim();

                if (strLine.IndexOf("PRESSURE AT", 0, 10) > 0) 
		        {
                    strLine = filesr.ReadLine();
                    strLine = strLine.Trim();
                    parser = strLine.Split(' ');
			        date_temp = parser[3];
			
			        if(date_temp == "JAN")
			        {
				        date=parser[4]+"-01-"+parser[2];
			        }
			        if(date_temp == "FEB")
			        {
				        date=parser[4]+"-02-"+parser[2];
			        }
			        if(date_temp == "MAR")
			        {
				        date=parser[4]+"-03-"+parser[2];
			        }
			        if(date_temp == "APR")
			        {
				        date=parser[4]+"-04-"+parser[2];
			        }
			        if(date_temp == "MAY")
			        {
				        date=parser[4]+"-05-"+parser[2];
			        }
			        if(date_temp == "JUN")
			        {
				        date=parser[4]+"-06-"+parser[2];
			        }
			        if(date_temp == "JUL")
			        {
				        date=parser[4]+"-07-"+parser[2];
			        }
			        if(date_temp == "AUG")
			        {
				        date=parser[4]+"-08-"+parser[2];
			        }
			        if(date_temp == "SEP")
			        {
				        date=parser[4]+"-09-"+parser[2];
			        }
			        if(date_temp == "OCT")
			        {
				        date=parser[4]+"-10-"+parser[2];
			        }
			        if(date_temp == "NOV")
			        {
				        date=parser[4]+"-11-"+parser[2];
			        }
			        if(date_temp == "DEC")
			        {
				        date=parser[4]+"-12-"+parser[2];
			        }
			        continue;
		        }

                if (strLine.IndexOf("(I", 0, 1) > 0) //if (strLine.Left(2) == "(I")
		        {
                    strLine = filesr.ReadLine();
                    strLine = strLine.Trim();

			        //步长
			        int step = image_array.total_i * image_array.total_j;
		
			        //一个日期需要读取的分段数量
			        int blocktimes = image_array.total_k * (image_array.total_i/15 + 1);
			
			        //残余的列数
			        int remainCols = image_array.total_i % 15;
		
			        for(int j = 0 ;j < image_array.total_j;j++)
			        {
                        strLine = filesr.ReadLine();
                        strLine = strLine.Trim();
                        parser = strLine.Split(')');
				        string temp = parser[1];
                        parser = temp.Split(' ');
				        if(parser.Length<15)
				        {
					        for(int i = 0;i < remainCols;i++)
					        {
						        if(parser[i] == "-----")
						        {
                                    image_array.m_values[image_array.total_i * j + i + step * k + count * 15].Prt_value = Convert.ToDouble("-9999");
						        }
						        else
						        {
                                    image_array.m_values[image_array.total_i * j + i + step * k + count * 15].Prt_value = Convert.ToDouble(parser[i]);
						        }
					        }
				        }
				        else
				        {
					        for(int i = 0;i < 15; i++)
					        {
						        if(parser[i] == "-----")
						        {
                                    image_array.m_values[image_array.total_i * j + i + step * k + count * 15].Prt_value = Convert.ToDouble("-9999");
							
						        }
						        else
						        {
                                    image_array.m_values[image_array.total_i * j + i + step * k + count * 15].Prt_value = Convert.ToDouble(parser[i]);
						        }
					        }
				        }
			        }
			
			        k++;
			        if(k == image_array.total_k)
			        {
				        k = 0;
			        }
			        count++;

			        if(count == blocktimes)
			        //if(count == 1)
			        {
				        Image[date] = image_array;
				        count = 0;
				        /*TRACE("result1 IS %lf",image_array.m_values[1].Prt_value);
				        TRACE("result8849 IS %lf",image_array.m_values[8849].Prt_value);
				        TRACE("result8850 IS %lf",image_array.m_values[8850].Prt_value);
				        TRACE("result8851 IS %lf",image_array.m_values[8851].Prt_value);*/
			        }
			        continue;
		        }

		        //progress.step();
	        }
	        filesr.Close();
	        //progress.finish();
	        return true;
        }

        public bool read_model_data(string str_file)
        {
            string strFGrid = str_file;
            string strPrt = str_file.Substring(0,str_file.Length - 5) + "PRT";
	        if (read_grid(strFGrid))
	        {
		        return putValueInStruct(strPrt);
	        }
	        return false;
        }

        public bool read_facies_data(string str_file)
        {
	        StreamReader filesr = new StreamReader(str_file, Encoding.Default);
	        if (filesr==null)
	        {
		        //Basic_Func::ok_messbox("打开文件失败:" + m_file_ydepth2, this);
		        return false;
	        }

	        m_vFacies.Clear();
	        m_vFaciesTypeTable.Clear();

	        string strLine;
	        string[] parser;
	        while ((strLine = filesr.ReadLine()) != null)
	        {
		        strLine= strLine.Trim();
		        parser = Regex.Split(strLine," \t",RegexOptions.IgnoreCase);
		        if (parser.Length!=4)
		        {
			        continue;
		        }
		        FLOAT_POINT pt;
		        pt.x = Convert.ToDouble(parser[0]);
		        pt.y = Convert.ToDouble(parser[1]);
		        string strType = parser[3];

		        int index = -1;
		        for (int i = 0; i<m_vFaciesTypeTable.Count; i++)
		        {
			        if(m_vFaciesTypeTable[i].name == strType)
			        {
				        index = i;
				        break;
			        }
		        }
		        if (index==-1)
		        {
			        index = m_vFaciesTypeTable.Count;
			        Facies_Type item;
			        item.facies_type = index;
			        item.name = strType;
                    Random ran=new Random();
                    item.color.R = ran.Next(0, 255);
                    item.color.G = ran.Next(0, 255);
                    item.color.B = ran.Next(0, 255);
			        m_vFaciesTypeTable.Add(item);
		        }

		        m_vFacies.Add(pt);
		        m_vFaciesType.Add(index);
	        }
            filesr.Close();

	        if (m_vFacies.Count>2)
	        {
		        m_d_Facies_Grid_Size = m_vFacies[1].x - m_vFacies[0].x;
	        }
	        return true;
        }

        public bool read_fault_data(string str_file)
        {
            StreamReader filesr = new StreamReader(str_file, Encoding.Default);
            if (filesr == null)
            {
		        //Basic_Func::ok_messbox("打开文件失败:" + m_file_ydepth2, this);
		        return false;
	        }

	        m_vFault.Clear();

	        List<FLOAT_POINT> line=new List<FLOAT_POINT>();
	        int id = -1;
	        string strLine;
	        string[] parser;
            while ((strLine = filesr.ReadLine()) != null)
	        {
                strLine = strLine.Trim();
                parser = strLine.Split(',');
		        if (parser.Length!=4)
		        {
			        continue;
		        }
		        FLOAT_POINT pt;
		        pt.x = Convert.ToDouble(parser[0]);
                pt.y = Convert.ToDouble(parser[1]);
		        int i = int.Parse(parser[2]);

                if (i != id && line != null)//!line.empty()!!!!!!!!!!
		        {
			        m_vFault.Add(line);
			        line.Clear();
		        }
		        id = i;
		        line.Add(pt);
	        }
            if (line!=null)//!line.empty()
	        {
		        m_vFault.Add(line);
	        }
	        filesr.Close();
	        return true;
        }

        public bool read_boundary_data(string str_file)
        {
	        StreamReader filesr =new StreamReader(str_file,Encoding.Default);
            if (filesr==null)
	        {
		        //Basic_Func::ok_messbox("打开文件失败:" + m_file_ydepth2, this);
		        return false;
	        }

	        m_vBoundary.Clear();

	        List<FLOAT_POINT> line= new List<FLOAT_POINT>();
	        int id = -1;
	        string strLine;
	        string[] parser;
	        while((strLine=filesr.ReadLine())!=null)
	        {
		        strLine.Trim();
		        parser = strLine.Split(',');
		        if (parser.Length!=4)
		        {
			        continue;
		        }
		        FLOAT_POINT pt;
		        pt.x = Convert.ToDouble(parser[0]);
                pt.y = Convert.ToDouble(parser[1]);
		        int i = int.Parse(parser[2]);

		        if(i!=id && line!=null)//!line.empty()
		        {
			        m_vBoundary.Add(line);
			        line.Clear();
		        }
		        id = i;
		        line.Add(pt);
	        }
            if (line != null)//!line.empty()
	        {
                m_vBoundary.Add(line);
	        }
	        filesr.Close();

	        return true;
        }

        public bool calc_rect(double xmin, double xmax,double ymin, double ymax)
        {
	        xmin = ymin = float.MaxValue;
            xmax = ymax = -float.MaxValue; 

	        if (Image!=null)//!Image.empty()
	        {
		        ImageArray imag = image_array;
		        for (int i=0; i<imag.m_values.Count;++i)
		        {
			        xmin = Math.Min(imag.m_values[i].Egrid_values[0], xmin);
			        xmax = Math.Max(imag.m_values[i].Egrid_values[3], xmax);
			        ymin = Math.Min(imag.m_values[i].Egrid_values[1], ymin);
			        ymax = Math.Max(imag.m_values[i].Egrid_values[4], ymax);
		        }
	        }

	        for (int i=0; i<m_vFacies.Count; ++i)
	        {
		        xmin = Math.Min(m_vFacies[i].x, xmin);
		        xmax = Math.Max(m_vFacies[i].x, xmax);
		        ymin = Math.Min(m_vFacies[i].y, ymin);
		        ymax = Math.Max(m_vFacies[i].y, ymax);
	        }

	        for (int i=0; i<m_vFault.Count; ++i)
	        {
		        List<FLOAT_POINT> line = m_vFault[i];
		        for (int j=0; j<line.Count; ++j)
		        {
			        xmin = Math.Min(line[j].x, xmin);
			        xmax = Math.Max(line[j].x, xmax);
			        ymin = Math.Min(line[j].y, ymin);
			        ymax = Math.Max(line[j].y, ymax);
		        }
	        }

	        for (int i=0; i<m_vBoundary.Count; ++i)
	        {
		        List<FLOAT_POINT> line = m_vBoundary[i];
		        for (int j=0; j<line.Count; ++j)
		        {
			        xmin = Math.Min(line[j].x, xmin);
			        xmax = Math.Max(line[j].x, xmax);
			        ymin = Math.Min(line[j].y, ymin);
			        ymax = Math.Max(line[j].y, ymax);
		        }
	        }
	        return true;
        }

        public void show_full()
        {
	        double xmin=0,xmax=0,ymin=0,ymax=0;//?
	        calc_rect(xmin,xmax,ymin,ymax);
	        m_d_xmin = xmin;
	        m_d_xmax = xmax;
	        m_d_ymin = ymin;
	        m_d_ymax = ymax;

	        set_origin(xmin,ymax);

	        double dzoomx = m_size.cx / (m_d_xmax-m_d_xmin);
	        double dzoomy = m_size.cy / (m_d_ymax-m_d_ymin);
	        m_d_zoomfactor = Math.Min(dzoomx, dzoomy);

	        calc_draw_object();
	        //m_pView1->Invalidate();
	        //m_pView2->Invalidate();
        }
        public void  move(CSize size)
        {
	        double dx = size.cx / m_d_zoomfactor;
	        double dy = -size.cy / m_d_zoomfactor;
	        m_origin.x -= dx;
	        m_origin.y -= dy;
	        calc_draw_object();
	        //m_pView1->Invalidate();
	        //m_pView2->Invalidate();
        }
        public void pan()
        {
	        //
        }
        public void zoom_in()
        {
	        m_d_zoomfactor = m_d_zoomfactor*1.5;
	        calc_draw_object();
        }

        public void zoom_out()
        {
	        m_d_zoomfactor = m_d_zoomfactor/1.5;
	        calc_draw_object();
        }
        //CColorLookUpTable m_ColorTable
       public bool read_color_table(string str_file)
        {
	        m_ColorTable.SetValueRange(0,1);
	        m_ColorTable.SetColor(0, RGB(255,0,0));
	        m_ColorTable.SetColor(0.1,RGB(200,150,100));
	        m_ColorTable.SetColor(0.5, RGB(0,255,0));
	        m_ColorTable.SetColor(1, RGB(0,0,255));
	        return true;
        }

        public bool read_faciestype_table(string str_file)
        {
	        return true;
        }

        public bool read_oiltype_table(string str_file)
        {
	        for (int i=0; i<10; ++i)
	        {
		        Oil_Type ot;
		        ot.id = i;
		        ot.name=string.Format("类型%d", i+1);//??未测试
		        //ot.color = Basic_Func::rand_color();
                Random ran=new Random();
                ot.color.R = ran.Next(0, 255);
                ot.color.G = ran.Next(0, 255);
                ot.color.B = ran.Next(0, 255);
		        m_vOilTypeTable.Add(ot);
	        }
	        return true;
        }
        public void set_origin(double x, double y)
        {
	        m_origin.x = x;
	        m_origin.y = y;
        }
       public  bool calc_draw_object()
        {
	        calc_draw_model();
	        calc_draw_facies();
	        calc_draw_fault();
	        calc_draw_boundary();
	        calc_draw_well();
	        calc_draw_oilboundary();
	        return true;
        }
       public bool calc_draw_model()
        {
	        if (Image==null)//empty?
	        {
		        return false;
	        }
	        ImageArray imag = image_array;
	        int nsize = imag.m_values.Count();
	        //v_model_tri.resize(4*nsize);
	        //v_model_triangle.resize(2*nsize);

	        for (int i=0; i<nsize; ++i)
	        {
		        double v_f =imag.m_values[i].Prt_value;
		        v_f = (v_f-113)/5;//测试数据
		       // COLORREF color = RGB(255,255,255);
                COLORREF color;
                color.R=255;
                color.G=255;
                color.B=255;
		        if (v_f>=0 && v_f<=1)
		        {
			        color = m_ColorTable.GetColor(v_f);//CColorLookUpTable m_ColorTable;
		        }

		        Color tri = v_model_tri[i*4];//TRIVERTEX
		        tri.A=0;
		        tri.B=(GetBValue(color))<<8;
		        tri.G=(GetGValue(color))<<8;
		        tri.R=(GetRValue(color))<<8;
		        double x = imag.m_values[i].Egrid_values[0];
		        double y = imag.m_values[i].Egrid_values[1];
		        WorldPointToLogicPoint(x, y);
		        tri.x = x;
		        tri.y = y;

		        Color tri1 = v_model_tri[i*4+1];
		        tri1.A=0;
		        tri1.B=(GetBValue(color))<<8;
		        tri1.G=(GetGValue(color))<<8;
		        tri1.R=(GetRValue(color))<<8;
		        x = imag.m_values[i].Egrid_values[3];
		        y = imag.m_values[i].Egrid_values[4];
		        WorldPointToLogicPoint(x, y);
		        tri1.x = x;
		        tri1.y = y;

		        TRIVERTEX& tri2 = v_model_tri[i*4+2];
		        tri2.Alpha=0;
		        tri2.Blue=(GetBValue(color))<<8;
		        tri2.Green=(GetGValue(color))<<8;
		        tri2.Red=(GetRValue(color))<<8;
		        x = imag.m_values[i].Egrid_values[6];
		        y = imag.m_values[i].Egrid_values[7];
		        WorldPointToLogicPoint(x, y);
		        tri2.x = x;
		        tri2.y = y;

		        TRIVERTEX& tri3 = v_model_tri[i*4+3];
		        tri3.Alpha=0;
		        tri3.Blue=(GetBValue(color))<<8;
		        tri3.Green=(GetGValue(color))<<8;
		        tri3.Red=(GetRValue(color))<<8;
		        x = imag.m_values[i].Egrid_values[9];
		        y = imag.m_values[i].Egrid_values[10];
		        WorldPointToLogicPoint(x, y);
		        tri3.x = x;
		        tri3.y = y;

		        v_model_triangle[2*i].Vertex1 = i*4;
		        v_model_triangle[2*i].Vertex2 = i*4+1;
		        v_model_triangle[2*i].Vertex3 = i*4+2;
		        v_model_triangle[2*i+1].Vertex1 = i*4+1;
		        v_model_triangle[2*i+1].Vertex2 = i*4+2;
		        v_model_triangle[2*i+1].Vertex3 = i*4+3;
	        }

	        return true;
        }

        public bool calc_draw_facies()
        {
	        int nsize = m_vFacies.Count;
	        //v_facies_tri.resize(nsize*2);
	        //v_facies_rect.resize(nsize);
	        for(int i = 0;i<nsize;++i)
	        {
		        int itype = m_vFaciesType[i];
		        COLORREF color;
		        for (int j=0;j<m_vFaciesTypeTable.Count; ++j)
		        {
			        if (itype==m_vFaciesTypeTable[j].facies_type)
			        {
				        color = m_vFaciesTypeTable[j].color;
				        break;
			        }
		        }
		        int index = i;
		        index=2;
		        TRIVERTEX& tri = v_facies_tri[index];
		        tri.Alpha=0;
		        tri.Blue=(GetBValue(color))<<8;
		        tri.Green=(GetGValue(color))<<8;
		        tri.Red=(GetRValue(color))<<8;

		        double x = m_vFacies[i].x-m_d_Facies_Grid_Size/2;
		        double y = m_vFacies[i].y-m_d_Facies_Grid_Size/2;

		        WorldPointToLogicPoint(x, y);////world坐标转化为逻辑坐标
		        tri.x = x;
		        tri.y = y;

		        TRIVERTEX& tri2 = v_facies_tri[index+1];
		        tri2.Alpha=0;
		        tri2.Blue=(GetBValue(color))<<8;
		        tri2.Green=(GetGValue(color))<<8;
		        tri2.Red=(GetRValue(color))<<8;
		        double x2 = m_vFacies[i].x+m_d_Facies_Grid_Size/2;
		        double y2 = m_vFacies[i].y+m_d_Facies_Grid_Size/2;

		        WorldPointToLogicPoint(x2, y2);//world坐标转化为逻辑坐标
		        tri2.x = x2;
		        tri2.y = y2;

		        int index2 = i;
		        v_facies_rect[index2].LowerRight = index;
		        v_facies_rect[index2].UpperLeft = index+1;
	        }

	        return true;
        }

        public bool calc_draw_fault()
        {
	        m_vFault_draw.Clear();
	        int nsize = m_vFault.Count;
	        for (int i=0; i<nsize; ++i)
	        {
		        List<FLOAT_POINT> line = m_vFault[i];
		        List<Point> line_draw=new List<Point>();
		        for (int j=0; j<line.Count; ++j)
		        {
			        double x = line[j].x;
			        double y = line[j].y;
			        WorldPointToLogicPoint(x, y);
			        Point pt=new Point(x,y);
			        line_draw.Add(pt);
		        }
		        m_vFault_draw.Add(line_draw);
	        }
	        return true;
        }

        public bool calc_draw_boundary()
        {
	        m_vBoundary_draw.Clear();
	        int nsize = m_vBoundary.Count;
	        for (int i=0; i<nsize; ++i)
	        {
		        List<FLOAT_POINT> line = m_vBoundary[i];
		        List<Point> line_draw=new List<Point>();
		        for (int j=0; j<line.Count; ++j)
		        {
			        double x = line[j].x;
			        double y = line[j].y;
			        WorldPointToLogicPoint(x, y);
			        Point pt=new Point(x,y);
			        line_draw.Add(pt);
		        }
		        m_vBoundary_draw.Add(line_draw);
	        }
	        return true;
        }

        public bool calc_draw_well()
        {
	        int nsize = m_vWell.Count;
	        //m_vWell_draw.resize(nsize);
	        for (int i=0; i<nsize; ++i)
	        {
		        double x = m_vWell[i].x;
		        double y = m_vWell[i].y;
		        WorldPointToLogicPoint(x, y);
                		        
		       // m_vWell_draw[i].X = x;
		       // m_vWell_draw[i].Y = y;
                m_vWell_draw[i]=new Point(x,y);
	        }
	        return true;
        }

        public bool calc_draw_oilboundary()
        {
	        m_vOilBoundary_draw.Clear();
	        int nsize = m_vOilBoundary.Count;
	        for (int i=0; i<nsize; ++i)
	        {
		        List<FLOAT_POINT> line = m_vOilBoundary[i];
		        List<Point> line_draw=new List<Point>();
		        for (int j=0; j<line.Count; ++j)
		        {
			        double x = line[j].x;
			        double y = line[j].y;
			        WorldPointToLogicPoint(x, y);
			        Point pt=new Point(x,y);
			        line_draw.Add(pt);
		        }
		        m_vOilBoundary_draw.Add(line_draw);
	        }
	        //m_vOilType.resize(m_vOilBoundary_draw.Count);
	        return true;
        }


        public void set_size(CSize size)
        {
	        m_size = size;
        }

        public void WorldPointToLogicPoint(double x, double y)
        {
	        x = (x-m_origin.x)*m_d_zoomfactor;
	        y = -(y-m_origin.y)*m_d_zoomfactor;
        }

        public void LogicPointToWorldPoint(double x, double y)
        {
	        x = x/m_d_zoomfactor+m_origin.x;
	        y = -y/m_d_zoomfactor+m_origin.y;
        }

        public void add_boundary(List<Point> v_pt)
        {
	        List<FLOAT_POINT> v_fpt =new List<FLOAT_POINT>();
	        //v_fpt.resize(v_pt.Count);
	        for (int i=0; i<v_pt.Count; ++i)
	        {
		        double x = v_pt[i].X;
		        double y = v_pt[i].Y;
		        LogicPointToWorldPoint(x,y);
                //v_fpt[i].x = x;
		        //v_fpt[i].y = y;
                FLOAT_POINT fpt=new FLOAT_POINT();
                fpt.x=x;
                fpt.y=y;
                v_fpt[i]=fpt;
		        
	        }
	        m_vBoundary.Add(v_fpt);
	        calc_draw_boundary();
        }

        public void add_oilboundary(List<Point> v_pt)
        {
	        List<FLOAT_POINT> v_fpt=new List<FLOAT_POINT> ();
	       // v_fpt.resize(v_pt.Count);
	        for (int i=0; i<v_pt.Count; ++i)
	        {
		        double x = v_pt[i].X;
		        double y = v_pt[i].Y;
		        LogicPointToWorldPoint(x,y);
		        //v_fpt[i].x = x;
		       // v_fpt[i].y = y;
                FLOAT_POINT fpt=new FLOAT_POINT();
                fpt.x=x;
                fpt.y=y;
                v_fpt[i]=fpt;
	        }
	        m_vOilBoundary.Add(v_fpt);
	        calc_draw_oilboundary();
        }

        public void draw(CDC* pDC, List<int> order)
        {
	        for (int i=order.Count-1; i>=0; i--)
	        {
		        if (order[i]==(int)myEnum.layertype_model)
		        {
			        draw_model(pDC);
		        }
		        else if (order[i]==(int)myEnum.layertype_facies)
		        {
			        draw_facies(pDC);
		        }
		        else if (order[i]==(int)myEnum.layertype_fault)
		        {
			        draw_fault(pDC);
		        }
		        else if (order[i]==(int)myEnum.layertype_boundary)
		        {
			        draw_boundary(pDC);
		        }
		        else if (order[i]==(int)myEnum.layertype_well)
		        {
			        draw_well(pDC);
		        }
		        else if (order[i]==(int)myEnum.layertype_oilboundary)
		        {
			        draw_oilboundary(pDC);
		        }
	        }

        }

        public void draw_model(CDC* pDC)
        {
	        if (v_model_tri==null || v_model_triangle==null)//empty()
	        {
		        return;
	        }
	        GradientFill(pDC->GetSafeHdc(),&v_model_tri[0],v_model_tri.Count,&v_model_triangle[0],v_model_triangle.Count,GRADIENT_FILL_TRIANGLE);
        }

        public void draw_facies(CDC* pDC)
        {
	        if (v_facies_tri==null || v_facies_rect==null)//empty()
	        {
		        return;
	        }
	        GradientFill(pDC->GetSafeHdc(),&v_facies_tri[0],v_facies_tri.Count,&v_facies_rect[0],v_facies_rect.Count,GRADIENT_FILL_RECT_H);
        }

        public void draw_fault(CDC* pDC)
        {
	        if (m_vFault_draw==null)//.empty()
	        {
		        return;
	        }
	        for (int i=0; i<m_vFault_draw.Count; ++i)
	        {
		        List<Point> line = m_vFault_draw[i];
		        for (int j=0; j<line.Count-1; ++j)
		        {
			        pDC->MoveTo(line[j]);
			        pDC->LineTo(line[j+1]);
		        }
	        }
        }

       public  void draw_boundary(CDC* pDC)
        {
	        if (m_vBoundary_draw==null)//.empty()
	        {
		        return;
	        }
	        for (int i=0; i<m_vBoundary_draw.Count; ++i)
	        {
		        List<Point> line = m_vBoundary_draw[i];
		        for (int j=0; j<line.Count; ++j)
		        {
			        pDC->MoveTo(line[j]);
			        int k = (j+1==line.Count) ? 0 : j+1;
			        pDC->LineTo(line[k]);
		        }
	        }
        }

       public  void draw_well(CDC* pDC)
        {
        }

        public void draw_oilboundary(CDC* pDC)
        {
            if (m_vOilBoundary_draw == null)//.empty()
	        {
		        return;
	        }
	        for (int i=0; i<m_vOilBoundary_draw.Count; ++i)
	        {
		        List<Point> line = m_vOilBoundary_draw[i];
		        for (int j=0; j<line.Count; ++j)
		        {
			        pDC->MoveTo(line[j]);
			        int k = (j+1==line.Count) ? 0 : j+1;
			        pDC->LineTo(line[k]);
		        }

		        COLORREF color = RGB(40,130,170);
		        for (int j=0; j<m_vOilTypeTable.Count; ++j)
		        {
			        if (m_vOilTypeTable[j].id == m_vOilType[i])
			        {
				        color = m_vOilTypeTable[j].color;
				        break;
			        }
		        }

		        CBrush br(color);
		        CRgn rgn;  

		        if( rgn.CreatePolygonRgn(&line[0], line.Count, ALTERNATE) && line[0].x > 0)  
		        {  
			        pDC->FillRgn(&rgn, &br);  
		        }

		        br.DeleteObject();  
		        rgn.DeleteObject(); 
	        }
        }

       public void draw_sel_oilboundary(CDC* pDC)
        {
	        if (m_i_seloilboundary<0 || m_i_seloilboundary>=m_vOilBoundary_draw.Count)
	        {
		        return;
	        }

	        List<CPoint> &line = m_vOilBoundary_draw[m_i_seloilboundary];

	        CBrush br(RGB(40,130,170));  
	        CRgn rgn;  

	        if( rgn.CreatePolygonRgn(&line[0], line.Count, ALTERNATE) && line[0].x > 0)  
	        {  
		        pDC->FillRgn(&rgn, &br);  
	        }

	        br.DeleteObject();  
	        rgn.DeleteObject(); 
        }

        public double det(double x1,double y1,double x2,double y2) 
        { 
	        return x1*y2-x2*y1; 
        } 

        public double area(List<FLOAT_POINT> line) 
        { 
	        int i; 
	        float temp=0; 
	        for(i=0;i<line.Count-1;i++) 
	        { 
		        temp+=det(line[i].x,line[i].y,line[i+1].x,line[i+1].y); 
	        } 
	        temp+=det(line[i].x,line[i].y,line[0].x,line[0].y); 
	        return temp/2; 
        } 

        public double get_area()
        {
	        double d_area = 0;

	        for (int i=0; i<m_vBoundary.Count; ++i)
	        {
		        List<FLOAT_POINT> line = m_vBoundary[i];
		        d_area += area(line);
	        }

	        return d_area;
        }

        public bool PtInPolygon(Point p, List<Point> line) 
        { 
	        int nCross = 0;
	        int nCount = line.Count;
	        for (int i=0; i<nCount; i++) 
	        { 
		        Point p1 = line[i]; 
		        Point p2 = line[(i + 1) % nCount];

		        // 求解 y=p.y 与 p1p2 的交点
		        if ( p1.y == p2.y ) // p1p2 与 y=p0.y平行 
			        continue;
		        if ( p.y < min(p1.y, p2.y) ) // 交点在p1p2延长线上 
			        continue; 
		        if ( p.y >= max(p1.y, p2.y) ) // 交点在p1p2延长线上 
			        continue;

		        // 求交点的 X 坐标 -------------------------------------------------------------- 
		        double x = (double)(p.Y - p1.Y) * (double)(p2.X - p1.X) / (double)(p2.Y - p1.Y) + p1.X;

		        if ( x > p.X ) 
			        nCross++; // 只统计单边交点 
	        }

	        // 单边交点为偶数，点在多边形之外 --- 
	        return (nCross % 2 == 1); 
        }

        public bool PtInPolygon2(FLOAT_POINT p, List<FLOAT_POINT> line) 
        { 
	        int nCross = 0;
	        int nCount = line.Count;
	        for (int i=0; i<nCount; i++) 
	        { 
		        FLOAT_POINT p1 = line[i]; 
		        FLOAT_POINT p2 = line[(i + 1) % nCount];

		        // 求解 y=p.y 与 p1p2 的交点
		        if ( p1.y == p2.y ) // p1p2 与 y=p0.y平行 
			        continue;
		        if ( p.y < min(p1.y, p2.y) ) // 交点在p1p2延长线上 
			        continue; 
		        if ( p.y >= max(p1.y, p2.y) ) // 交点在p1p2延长线上 
			        continue;

		        // 求交点的 X 坐标 -------------------------------------------------------------- 
		        double x = (double)(p.y - p1.y) * (double)(p2.x - p1.x) / (double)(p2.y - p1.y) + p1.x;

		        if ( x > p.x ) 
			        nCross++; // 只统计单边交点 
	        }

	        // 单边交点为偶数，点在多边形之外 --- 
	        return (nCross % 2 == 1); 
        }

        public int hit_test_oilboundary(Point pt)
        {
	        int isel = -1;
	        for (int i=0; i<m_vOilBoundary_draw.Count; ++i)
	        {
		        List<Point> line = m_vOilBoundary_draw[i];
		        if (PtInPolygon(pt, line))
		        {
			        return i;
		        }
	        }
	        return isel;
        }

        public void set_sel_oilboundary_type(int id)
        {
	        if (m_i_seloilboundary<0)
	        {
		        return;
	        }
	        m_vOilType[m_i_seloilboundary] = id;
        }

       public void stat_oil()
        {
           Dictionary<int, double> v_oil =new Dictionary<int, double>();
	       // std::map<int, double> v_oil;

	        for (int i=0; i<m_vOilTypeTable.Count; ++i)
	        {
		        int id = m_vOilTypeTable[i].id;
		        v_oil[id] = 0;
	        }

	        for (int i=0; i<m_vOilBoundary.Count; ++i)
	        {
		        List<FLOAT_POINT> line = m_vOilBoundary[i];

		        int id = m_vOilType[i];
		        double d_oil = v_oil[id];
		        for (int j=0; j<image_array.m_values.Count; ++j)
		        {
			        FLOAT_POINT pt;
			        pt.x = (image_array.m_values[j].Egrid_values[0] + image_array.m_values[j].Egrid_values[9])/2;
			        pt.y = (image_array.m_values[j].Egrid_values[1] + image_array.m_values[j].Egrid_values[10])/2;
			        if (PtInPolygon2(pt,line))
			        {
				        d_oil += image_array.m_values[j].Prt_value;
			        }
		        }
		        v_oil[id] = d_oil;
	        }
	        string strResult;
	        //for (std::map<int, double>::iterator it=v_oil.begin(); it!=v_oil.end(); ++it)
            foreach (int key in v_oil.Keys)
	        {
		        string str;
		        str=string.Format("%d : %f; ", key, v_oil[key]);//str.Format("%d : %f; ", it->first, it->second);
		        strResult += str;
	        }
	        Basic_Func::ok_messbox(strResult, NULL);
        }
    }
}
