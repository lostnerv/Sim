﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 数模建模
{
    /// <summary>
    /// SaveConfig.xaml 的交互逻辑
    /// </summary>
    public partial class SaveConfig : UserControl
    {
        public SaveConfig()
        {
            InitializeComponent();
        }

        private String path = null;
        private String file_Path = null;
        public String dataPath
        {
            get { return path; }
            set
            {
                path = value;
                this.filename.Text = dataPath;
            }
        }
    }
}
