using DUIDemo.Enums;
using DUIDemo.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DUIDemo.Windows
{
    /// <summary>
    /// WinSetting.xaml 的交互逻辑
    /// </summary>
    public partial class WinSetting : Window
    {
        public WinSetting()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            BindingLanguage();
            InitASRModelComboBox();
        }

        private void InitASRModelComboBox()
        {
            cmb_ASRModel.SelectedValue = App.DUIManager.Config.CLOUD_ASR_RES_MODULE;
        }

        private void BindingLanguage()
        {
            string encryptdata = BinaryHelper.ReadBinary("setting");
            string data = EnDeCryptHelper.Decrypt(encryptdata);
            List<string> list = Regex.Split(data, ";").ToList();
            cmb_ASRModel.ItemsSource = list;

            //return;
            //string s = "aihome;airobot;aicar;comm;aienglish;aimedical";
            //string s2 = "qtwnUWHLUT2XcKk6LZKgjHgd5kfBoFDmMEZ7H1MVCjKOd2xWTF1Tr84gp6dMovM+aL3Dk9M6naw0D1JzGS8tOo6pYEiq6T6JEq//09yOlDw+w8jLHV9rljW9NbiLLhJr";

            //bool IsSuccess = BinaryHelper.WriteBinary(s2);
            //string temp = EnDeCryptHelper.Encrypt(s);
            //string temp2 = EnDeCryptHelper.Decrypt(s2);
            ////List<string> list = Regex.Split(s, ";").ToList();
            ////var list = EnumHelper.GetEnumList<EASRModel>();
            //cmb_ASRModel.ItemsSource = list;
        }



        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bd_Title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void cmb_ASRModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_ASRModel.SelectedValue == null)
            {
                return;
            }
            //App.DUIManager.ASRModel = (EASRModel)cmb_ASRModel.SelectedValue;
            App.DUIManager.Config.CLOUD_ASR_RES_MODULE = cmb_ASRModel.SelectedValue.ToString();
        }
    }
}
