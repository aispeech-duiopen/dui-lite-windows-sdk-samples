using ASRDemo.Enums;
using ASRDemo.Helper;
using ASRDemo.Model;
using ASRDemo.Model.DUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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
using static ASRDemo.Helper.DUILiteHelper;

namespace ASRDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Auth();
        }
        private static string OrginalString = string.Empty;
        private static MDUIManage manager = MDUIManage.GetInstance();
        private BackgroundWorker bw = new BackgroundWorker();

        private void Auth()
        {
            var callback = new DUILiteCallback(engine_callback);
            bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                manager.DUILiteLogInit();
                manager.DUILiteConfig();
                string authConfig = JsonHelper.ToJson(manager.AuthConfig);
                DUILiteAuth(authConfig, callback, null);
            };
            bw.RunWorkerAsync();
        }

        private void StartNativeASR()
        {
            string gramcfg = JsonHelper.ToJson(manager.NASRGramConfig);
            string gramparam = JsonHelper.ToJson(manager.NASRParam);
            string asrcfg = JsonHelper.ToJson(manager.NASRConfig);
            var result = DUILiteHelper.DUILiteNativeASRNew(gramcfg, gramparam, asrcfg);
            result = DUILiteHelper.DUILiteNativeASRStart(JsonHelper.ToJson(manager.EnNASRConfig));
        }

        private void StopNativeASR()
        {
            DUILiteHelper.DUILiteNativeASRStop();
            DUILiteHelper.DUILiteNativeASRRelease();
        }

        private int engine_callback(string userdata, int type, IntPtr msg, int len)
        {

            EEventReturn etype = (EEventReturn)type;
            if (etype == EEventReturn.DUILITE_EV_OUT_AUTH_COMPLETE)
            {
                //IsAuth = true;
                //auth_status = 1;
                //printf("%s, auth result: %.*s\n", tag, len, msg);
            }

            if (etype == EEventReturn.DUILITE_EV_OUT_NATIVE_VAD_BEGIN)
            {
                //printf("%s, native VAD begin: %.*s\n", tag, len, msg);
            }
            if (etype == EEventReturn.DUILITE_EV_OUT_NATIVE_VAD_END)
            {
                //printf("%s, native VAD end: %.*s\n", tag, len, msg);
            }
            if (etype == EEventReturn.DUILITE_EV_OUT_NATIVE_TTS_BEGIN)
            {
                //printf("%s, native TTS begin: %.*s\n", tag, len, msg);
            }

            if (etype == EEventReturn.DUILITE_EV_OUT_NATIVE_ASR_FINAL_RESULT)
            {
                byte[] managedArray = new byte[len];
                Marshal.Copy(msg, managedArray, 0, len);
                string result = Encoding.UTF8.GetString(managedArray);
                var data = JsonHelper.Parse<MNativeASRReturn>(result);
                manager.OutResult.ARSRealTimeOut = OrginalString + data.ngram.rec;

                string temp = data.ngram.rec.Replace(" ", "");
                if (temp.Contains("打开地盘"))
                {
                    temp = temp.Replace("打开地盘", "打开D盘");
                }
                if (temp.Contains("打开一盘"))
                {
                    temp = temp.Replace("打开一盘", "打开E盘");
                }
                this.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    tbl_Command.Text = temp;
                });
                DoCommand(temp);
                this.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    tbtn_ASR.IsChecked = false;
                });
            }

            if (etype == EEventReturn.DUILITE_EV_OUT_NATIVE_ASR_REALBACK_RESULT)
            {
                byte[] managedArray = new byte[len];
                Marshal.Copy(msg, managedArray, 0, len);
                string strInTime = Encoding.UTF8.GetString(managedArray);
                if (manager.OutResult == null)
                {
                    manager.OutResult = new Model.DUI.MOut();
                }
                var data = JsonHelper.Parse<MNativeASRReturn>(strInTime);
                if (data.ngram != null)
                {
                    manager.OutResult.ARSRealTimeOut = OrginalString + data.ngram.rec;
                }
                //manager.OutResult.ARSRealTimeOut = OrginalString + strInTime;
            }

            return 0;
        }


        private void DoCommand(string command)
        {
            if (command.Contains("打开") && command.Contains("我的电脑"))
            {
                System.Diagnostics.Process.Start("::{20D04FE0-3AEA-1069-A2D8-08002B30309D}");
            }
            if (command.Contains("打开") && (command.Contains("c盘") || command.Contains("C盘")))
            {
                System.Diagnostics.Process.Start("explorer.exe", @"C:\");
            }
            if (command.Contains("打开") && (command.Contains("d盘") || command.Contains("D盘")))
            {
                System.Diagnostics.Process.Start("explorer.exe", @"D:\");
            }
            if (command.Contains("打开") && (command.Contains("e盘") || command.Contains("E盘")))
            {
                System.Diagnostics.Process.Start("explorer.exe", @"E:\");
            }
            if (command.Contains("打开") && (command.Contains("文档")))
            {
                System.Diagnostics.Process.Start("explorer.exe", @"C:\Users\admin\Documents");
            }
        }

        private void tbtn_ASR_Checked(object sender, RoutedEventArgs e)
        {
            StartNativeASR();
        }

        private void tbtn_ASR_Unchecked(object sender, RoutedEventArgs e)
        {
            StopNativeASR();
        }

        private void bd_Title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
