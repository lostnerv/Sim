using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;

namespace 建模数模.tools
{
    class RWFile
    {
        public DataTable Read(string path,string flag)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            DataTable dt = new DataTable();
            bool triger = false;
            string trim;
            string[] array;

            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Contains(flag))
                {
                    trim = Regex.Replace(line, "\\s{2,}", ",");
                    array = trim.Split(',');
                    foreach (string name in array)
                    {
                        DataColumn column = new DataColumn();
                        column.DataType = System.Type.GetType("System.String");//该列的数据类型 
                        column.ColumnName = name;
                        dt.Columns.Add(column);
                    }
                    triger = true;
                    continue;
                }
                if (triger)
                {
                    trim = Regex.Replace(line, "\\s{2,}", ",");
                    array = trim.Split(',');
                    DataRow row = dt.NewRow();
                    for (int i = 0; i < array.Length;i++ )
                    {
                        row[i] = array[i];
                    }
                    dt.Rows.Add(row);
                }
                
            }
            return dt;
        }

        public void Write(string path,String content)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(content);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        public void Append(string path, String content)
        {
            FileStream fs = null;
            string filePath = path;
            //将待写的入数据从字符串转换为字节数组  
            Encoding encoder = Encoding.UTF8;
            byte[] bytes = encoder.GetBytes(content);
            try
            {
                fs = File.OpenWrite(filePath);
                //设定书写的開始位置为文件的末尾  
                fs.Position = fs.Length;
                //将待写入内容追加到文件末尾  
                fs.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("文件打开失败{0}", ex.ToString());
            }
            finally
            {
                fs.Close();
            }
            Console.ReadLine();  
        }

    }
}
