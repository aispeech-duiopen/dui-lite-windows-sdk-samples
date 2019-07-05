using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeneralSetting
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_GeneralSetting_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb_Show.Text))
            {
                string data = EnDeCryptHelper.Encrypt(tb_Show.Text);
                bool IsSuccess = BinaryHelper.WriteBinary(data);
                if (IsSuccess)
                {
                    System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory);
                }
                else
                {
                    MessageBox.Show("生成文件失败！");
                }
            }
            else
            {
                MessageBox.Show("内容不能为空！");
            }

        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb_Add.Text))
            {
                if (!string.IsNullOrEmpty(tb_Show.Text))
                {
                    tb_Show.Text += ";";
                }
                tb_Show.Text += tb_Add.Text;
                tb_Add.Text = string.Empty;
            }

        }
    }
}
