using System;
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
using System.Windows.Forms;

namespace 数模建模
{
    /// <summary>
    /// SelectFolderPath.xaml 的交互逻辑
    /// </summary>
    public partial class SelectFolderPath : System.Windows.Controls.UserControl
    {
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

        public SelectFolderPath()
        {
            InitializeComponent();
        }

        private void fileSelect_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                file_Path = saveFileDialog.SelectedPath;
            this.filePath.Text = file_Path;
            
        }
    }
}
