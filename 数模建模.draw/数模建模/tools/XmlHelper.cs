using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace 数模建模.tools
{
    class XmlHelper
    {
        public void CreateXMLDocument()
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,<?xml version="1.0" encoding="gb2312"?>
            XmlDeclaration xmlDeclar;
            xmlDeclar = xmlDoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmlDoc.AppendChild(xmlDeclar);
            //加入Employees根元素
            XmlElement xmlElement = xmlDoc.CreateElement("", "DataPath", "");
            xmlDoc.AppendChild(xmlElement);
            //添加节点
            //XmlNode root = xmlDoc.SelectSingleNode("DataPath");
            //XmlElement xe1 = xmlDoc.CreateElement("Node");
            //xe1.SetAttribute("Name", "Wellhead");
            //xe1.SetAttribute("Path", "C:\\");
            //添加子节点
            //XmlElement xeSub1 = xmlDoc.CreateElement("title");
            //xeSub1.InnerText = "学习VS";
            //xe1.AppendChild(xeSub1);

            //XmlElement xeSub2 = xmlDoc.CreateElement("price");
            //xe1.AppendChild(xeSub2);
            //XmlElement xeSub3 = xmlDoc.CreateElement("weight");
            //xeSub3.InnerText = "20";
            //xeSub2.AppendChild(xeSub3);

            //root.AppendChild(xe1);
            xmlDoc.Save("DataConfig.xml");//保存的路径
        }

        public void AddXMLDocument(string name, string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("DataConfig.xml");
            XmlNode root = xmlDoc.SelectSingleNode("DataPath");//查找<Employees> 
            XmlElement xe1 = xmlDoc.CreateElement("Node");//创建一个<Node>节点 
            xe1.SetAttribute("name", name);//设置该节点genre属性 
            xe1.SetAttribute("path", path);//设置该节点ISBN属性 

            //XmlElement xesub1 = xmlDoc.CreateElement("title");
            //xesub1.InnerText = "C#入门帮助";//设置文本节点 
            //xe1.AppendChild(xesub1);//添加到<Node>节点中 
            //XmlElement xesub2 = xmlDoc.CreateElement("author");
            //xesub2.InnerText = "高手";
            //xe1.AppendChild(xesub2);
            //XmlElement xesub3 = xmlDoc.CreateElement("price");
            //xesub3.InnerText = "158.3";
            //xe1.AppendChild(xesub3);

            root.AppendChild(xe1);//添加到<Employees>节点中 
            xmlDoc.Save("DataConfig.xml");
        }

        public void EditXMLDocument(string name, string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("DataConfig.xml");

            XmlNodeList nodeList = xmlDoc.SelectSingleNode("DataPath").ChildNodes;//获取Employees节点的所有子节点 

            foreach (XmlNode xn in nodeList)//遍历所有子节点 
            {
                XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型 
                if (xe.GetAttribute("name") == name)//如果genre属性值为“张三” 
                {
                    xe.SetAttribute("path", path);//则修改该属性为“update张三” 

                    //XmlNodeList nls = xe.ChildNodes;//继续获取xe子节点的所有子节点 
                    //foreach (XmlNode xn1 in nls)//遍历 
                    //{
                    //    XmlElement xe2 = (XmlElement)xn1;//转换类型 
                    //    if (xe2.Name == "author")//如果找到 
                    //    {
                    //        xe2.InnerText = "亚胜";//则修改
                    //    }
                    //}
                }
            }
            xmlDoc.Save("DataConfig.xml");//保存。
        }


        public string GetXMLDocument(string name)
        {
            string filepath = null;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("DataConfig.xml");

            XmlNodeList nodeList = xmlDoc.SelectSingleNode("DataPath").ChildNodes;//获取Employees节点的所有子节点 

            foreach (XmlNode xn in nodeList)//遍历所有子节点 
            {
                XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型 
                if (xe.GetAttribute("name") == name)//如果genre属性值为“张三” 
                {
                    filepath = xe.GetAttribute("path");//则修改该属性为“update张三” 
                }
            }
            return filepath;
        }
        /*
         * 创建指定名称的xml
         * @DSnow
         */
        public void CreateXMLDocument(String xmlname)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,<?xml version="1.0" encoding="gb2312"?>
            XmlDeclaration xmlDeclar;
            xmlDeclar = xmlDoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmlDoc.AppendChild(xmlDeclar);
            //加入Employees根元素
            XmlElement xmlElement = xmlDoc.CreateElement("", "Data", "");
            xmlDoc.AppendChild(xmlElement);
            xmlDoc.Save(xmlname);//保存的路径
        }
        /*
         * Data元素里添加Node节点，数组内的第一个值视为主键
         * xmlname：xml名称
         * vallist：同一行的数据数组比如 {井号，虚拟井号，日期，……}
         * @DSnow
         */
        public void AddXMLDocument(String xmlname, List<String> vallist)
        {
            if (vallist != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlname);
                XmlNode root = xmlDoc.SelectSingleNode("Data");//查找<Employees> 
                XmlElement xe1 = xmlDoc.CreateElement("Node");//创建一个<Node>节点 
                for (int i = 0; i < vallist.Count; i++)
                {
                    xe1.SetAttribute("val" + i, vallist[i]);//设置该节点genre属性 
                }
                root.AppendChild(xe1);//添加到<Employees>节点中 
                xmlDoc.Save(xmlname);
            }
        }
        /*
         * Data元素里修改Node节点，数组内的第一个值视为主键
         * xmlname：xml名称
         * vallist：同一行的数据数组比如 {井号，虚拟井号，日期，……}
         * @DSnow
         */
        public void EditXMLDocument(String xmlname, List<String> vallist)
        {
            if (vallist != null && vallist.Count > 1)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlname);

                XmlNodeList nodeList = xmlDoc.SelectSingleNode("Data").ChildNodes;//获取Employees节点的所有子节点 

                foreach (XmlNode xn in nodeList)//遍历所有子节点 
                {
                    XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型 
                    if (xe.GetAttribute("val0") == vallist[0])//如果genre属性值为“张三” 
                    {
                        for (int i = 1; i < vallist.Count; i++)
                        {
                            xe.SetAttribute("val" + i, vallist[i]);//设置该节点genre属性 
                        }
                    }
                }
                xmlDoc.Save(xmlname);//保存。
            }
        }
        /*
         * 获取Data元素里Node节点的第二个值
         * xmlname：xml名称
         * val0：同一行的数据数组的第一个值，如井号
         * @DSnow
         */
        public String GetXMLDocument2(String xmlname, String val0)
        {
            string val1 = null;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlname);

            XmlNodeList nodeList = xmlDoc.SelectSingleNode("Data").ChildNodes;//获取Employees节点的所有子节点 

            foreach (XmlNode xn in nodeList)//遍历所有子节点 
            {
                XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型 
                if (xe.GetAttribute("val0") == val0)//如果genre属性值为“张三” 
                {
                    val1 = xe.GetAttribute("val1");//则修改该属性为“update张三” 
                }
            }
            return val1;
        }
    }

}
