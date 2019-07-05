using DUIDemo.Enums;
using DUIDemo.Helper;
using DUIDemo.Model;
using DUIDemo.Model.DUI;
using DUIDemo.Model.Player;
using DUIDemo.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using System.Windows.Threading;
using WMPLib;
using static DUIDemo.Helper.DUILiteHelper;

namespace DUIDemo.Pages
{
    /// <summary>
    /// PDUISelect.xaml 的交互逻辑
    /// </summary>
    public partial class PDUISelect : Page
    {
        private static MDUIManage manager = MDUIManage.GetInstance();
        private static bool IsASRStop = false;
        private static bool IsAuth = false;
        private static bool IsWakeup = false;
        private EGeneralSpeaker LastGeneralSpeaker;
        private List<string> ChoiceList = new List<string>();

        private static string OrginalString = string.Empty;

        private static PCMPlayer pcmPlayer = new PCMPlayer();
        private static WindowsMediaPlayer CloudPlayer = new WindowsMediaPlayer();
        BackgroundWorker bw = new BackgroundWorker();
        BackgroundWorker bwWakeup = new BackgroundWorker();
        private List<MVoice> VoiceList = new List<MVoice>();
        private string englishConfig = "{\"env\": \"use_xbnf_rec=1;use_frame_split=1;hold_conf=1;\"}";
        private string ARSCloudParam = "{\"enableRealTimeFeedback\":true, \"enableVAD\":false, \"enablePunctuation\":true, \"language\":\"zh-CN\", \"res\":\"aihome\"}";

        public PDUISelect()
        {
            InitializeComponent();
            InitChoiceList();
            //LoadEvent();
            InitNet();
            InitUI();
            manager.DUILiteLogInit();
            DataContext = App.DUIManager.OutResult;
            VoiceList = GetVoiceList();
            RadioButton rbt = this.FindName(ChoiceList[PublicFunction.intParse(cmb_GeneralSpeaker.SelectedValue)]) as RadioButton;
            rbt.IsChecked = true;
        }

        private void InitChoiceList()
        {
            ChoiceList = new List<string>();
            ChoiceList.Add("rbt_Women1");
            ChoiceList.Add("rbt_Boy1");
            ChoiceList.Add("rbt_Girl1");
            ChoiceList.Add("rbt_Men1");
        }

        private void InitNet()
        {
            if (NetHelper.IsConnectNetwork())
            {
                manager.ASRNet = ENet.Cloud;
                manager.TTSNet = ENet.Cloud;
                manager.Config.DUILITE_MODE = "both";
                manager.AuthConfig.localAuth = false;
            }
            else
            {
                manager.ASRNet = ENet.Native;
                manager.TTSNet = ENet.Native;
                manager.Config.DUILITE_MODE = "native";
                manager.AuthConfig.localAuth = true;
                rbt_ASRCloud.IsEnabled = false;
                tbl_Info.Visibility = Visibility.Visible;
                cmb_NetMode.IsEnabled = false;
            }
        }

        private void InitASRLanguageComboBox()
        {
            cmb_ASRLanguage.SelectedValue = PublicFunction.intParse(App.DUIManager.ASRLanguage);
        }

        private void BindingLanguage()
        {
            var list = EnumHelper.GetEnumList<EASRLanguage>();
            cmb_ASRLanguage.ItemsSource = list;
        }


        private void InitNetModeComboBox()
        {
            cmb_NetMode.SelectedValue = PublicFunction.intParse(App.DUIManager.TTSNet);
        }

        private void BindingNetMode()
        {
            var list = EnumHelper.GetEnumList<ENet>();
            cmb_NetMode.ItemsSource = list;
        }

        private void BindingGeneralSpeaker()
        {
            var list = EnumHelper.GetEnumList<EGeneralSpeaker>();
            cmb_GeneralSpeaker.ItemsSource = list;
        }

        private void InitUI()
        {
            tab_main.IsEnabled = false;
            BindingLanguage();
            BindingNetMode();
            BindingGeneralSpeaker();
            InitASRLanguageComboBox();
            InitNetModeComboBox();
            InitASRMode();
            InitASRNet();
            InitTTSNet();
        }

        private void InitASRNet()
        {
            if (App.DUIManager.ASRNet == ENet.Cloud)
            {
                rbt_ASRCloud.IsChecked = true;
            }
            else if (App.DUIManager.ASRNet == ENet.Native)
            {
                rbt_ASRNative.IsChecked = true;
            }
        }

        private void InitTTSNet()
        {
            if (App.DUIManager.TTSNet == ENet.Cloud)
            {
                //rbt_TTSCloud.IsChecked = true;
            }
            else if (App.DUIManager.TTSNet == ENet.Native)
            {
                //rbt_TTSNative.IsChecked = true;
            }
        }

        private void InitASRMode()
        {
            if (App.DUIManager.ASRMode == EASRMode.Wakeup)
            {
                rbt_Wakeup.IsChecked = true;
            }
            else if (App.DUIManager.ASRMode == EASRMode.UnWakeup)
            {
                rbt_UnWakeup.IsChecked = true;
            }
        }

        private void UpdateDUI(bool isAuth)
        {
            tab_main.IsEnabled = isAuth;
        }

        private void ReleaseDUILite()
        {
            //DUILiteHelper.DUILiteWakeupStop();
            //DUILiteHelper.DUILiteWakeupRelease();
            //DUILiteHelper.DUILiteCloudASRStop();
            //DUILiteHelper.DUILiteCloudASRRelease();
            //DUILiteHelper.DUILiteNativeTTSRelease();
            DUILiteHelper.DUILiteAuthRelease();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            (this.Parent as Window).Close();
        }

        private void btn_Auth_Click(object sender, RoutedEventArgs e)
        {
            var callback = new DUILiteCallback(engine_callback);
            bw = new BackgroundWorker();
            bw.DoWork += delegate
              {
                  manager.DUILiteConfig();
                  string authConfig = JsonHelper.ToJson(manager.AuthConfig);
                  DUILiteAuth(authConfig, callback, null);
              };
            bw.RunWorkerAsync();

            Thread thread = new Thread(() =>
            {
                while (!IsAuth)
                {
                    Thread.Sleep(30);
                }
                this.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    UpdateDUI(IsAuth);
                });
            });
            thread.Start();
        }

        private int engine_callback(string userdata, int type, IntPtr msg, int len)
        {

            EEventReturn etype = (EEventReturn)type;
            if (etype == EEventReturn.DUILITE_EV_OUT_AUTH_COMPLETE)
            {
                IsAuth = true;
                //auth_status = 1;
                //printf("%s, auth result: %.*s\n", tag, len, msg);
            }
            if (etype == EEventReturn.DUILITE_EV_OUT_CLOUD_ASR_FINAL_RESULT)
            {
                IsASRStop = true;

                byte[] managedArray = new byte[len];
                Marshal.Copy(msg, managedArray, 0, len);
                string result = Encoding.UTF8.GetString(managedArray);
                //OrginalString = manager.OutResult.ARSRealTimeOut;
                manager.OutResult.ARSRealTimeOut = OrginalString + result;
                this.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    if (App.DUIManager.ASRMode == EASRMode.UnWakeup)
                    {
                        tbtn_ASR.IsChecked = false;
                    }
                });

            }
            if (etype == EEventReturn.DUILITE_EV_OUT_CLOUD_ASR_REALBACK_RESULT)
            {
                byte[] managedArray = new byte[len];
                Marshal.Copy(msg, managedArray, 0, len);
                string strInTime = Encoding.UTF8.GetString(managedArray);
                if (manager.OutResult == null)
                {
                    manager.OutResult = new Model.DUI.MOut();
                }
                manager.OutResult.ARSRealTimeOut = OrginalString + strInTime;
                //manager.OutResult.ARSRealTimeOut = strInTime;
                //printf("%s, Cloud ASR result: %.*s\n", tag, len, msg);
                //cloud_asr_status = 1;
            }
            if (etype == EEventReturn.DUILITE_EV_OUT_NATIVE_VAD_AUDIO)
            {
                //    FILE* audio = NULL;
                //    int w_len = 0;

                //    int ret = 0;

                //    if (len == 0)
                //    {
                //        goto vadend;
                //    }

                //    audio = fopen("./vad.pcm", "ab+");
                //    if (NULL == audio)
                //    {
                //        printf("create file error!\n");
                //        goto vadend;
                //    }

                //    w_len = fwrite(msg, 1, len, audio);
                //    if (w_len != len)
                //    {
                //        printf("write audio file error!\n");
                //    }
                //    else
                //    {
                //        printf("write audio file!\n");
                //    }
                //    fclose(audio);
                //    audio = NULL;
                //vadend:
                //    return ret;
            }
            if (etype == EEventReturn.DUILITE_EV_OUT_NATIVE_VAD_BEGIN)
            {
                //printf("%s, native VAD begin: %.*s\n", tag, len, msg);
            }
            if (etype == EEventReturn.DUILITE_EV_OUT_NATIVE_VAD_END)
            {
                //printf("%s, native VAD end: %.*s\n", tag, len, msg);
            }

            if (etype == EEventReturn.DUILITE_EV_OUT_NATIVE_TTS_AUDIO)
            {
                using (FileStream fs = new FileStream("native_cntts.pcm", FileMode.OpenOrCreate))
                {

                }
                if (App.IsNewTTS)
                {
                    using (FileStream fs = new FileStream("native_cntts.pcm", FileMode.Truncate))
                    {
                        byte[] managedArray = new byte[len];
                        Marshal.Copy(msg, managedArray, 0, len);
                        foreach (byte b in managedArray)
                        {
                            fs.WriteByte(b);
                        }
                        App.IsNewTTS = false;
                        return 0;
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream("native_cntts.pcm", FileMode.Append))
                    {
                        byte[] managedArray = new byte[len];
                        Marshal.Copy(msg, managedArray, 0, len);
                        foreach (byte b in managedArray)
                        {
                            fs.WriteByte(b);
                        }
                        return 0;
                    }
                }
            }
            if (etype == EEventReturn.DUILITE_EV_OUT_NATIVE_TTS_BEGIN)
            {
                //printf("%s, native TTS begin: %.*s\n", tag, len, msg);
            }
            if (etype == EEventReturn.DUILITE_EV_OUT_NATIVE_TTS_END)
            {
                //printf("%s, native TTS end: %.*s\n", tag, len, msg);
                //native_tts_status = 1;
            }

            if (etype == EEventReturn.DUILITE_EV_OUT_CLOUD_TTS_RESULT)
            {
                byte[] managedArray = new byte[len];
                Marshal.Copy(msg, managedArray, 0, len);
                string url = Encoding.UTF8.GetString(managedArray);
                PlayCloudAudio(url);
            }

            if (etype == EEventReturn.DUILITE_EV_OUT_ERROR)
            {
                //printf("%s, error: %.*s\n", tag, len, msg);
                //error_status = 1;
            }

            if (etype == EEventReturn.DUILITE_EV_OUT_WAKEUP_RESULT)
            {
                if (!IsASRStop)
                {
                    return 0;
                }
                IsWakeup = true;
                byte[] managedArray = new byte[len];
                Marshal.Copy(msg, managedArray, 0, len);
                string strwakeup = Encoding.UTF8.GetString(managedArray);
                var returnParam = JsonHelper.Parse<MWakeReturn>(strwakeup);
                if (manager.OutResult == null)
                {
                    manager.OutResult = new Model.DUI.MOut();
                }
                if (string.IsNullOrEmpty(manager.OutResult.ARSRealTimeOut))
                {
                    manager.OutResult.ARSRealTimeOut += returnParam.wakeupWord + "\r\n";
                }
                else
                {
                    manager.OutResult.ARSRealTimeOut += "\r\n" + returnParam.wakeupWord + "\r\n";
                }
                OrginalString = manager.OutResult.ARSRealTimeOut;
                switch (App.DUIManager.ASRNet)
                {
                    case ENet.Cloud:
                        DUILiteHelper.DUILiteCloudASRStart(null);
                        break;
                    case ENet.Native:
                        if (App.DUIManager.ASRLanguage == EASRLanguage.English)
                        {
                            //string enConfig = JsonHelper.ToJson(manager.EnNASRConfig);
                            var result = DUILiteHelper.DUILiteNativeASRStart(englishConfig);
                        }
                        else
                        {
                            string enConfig = JsonHelper.ToJson(manager.EnNASRConfig);
                            var result = DUILiteHelper.DUILiteNativeASRStart(JsonHelper.ToJson(manager.EnNASRConfig));
                        }
                        break;
                }
                IsASRStop = false;
                //printf("%s, wakeup result: %.*s,\nuserdata:%s\n", tag, len, msg, (char*)userdata);
                //wakeup_status = 1;
            }

            if (etype == EEventReturn.DUILITE_EV_OUT_NATIVE_ASR_FINAL_RESULT)
            {
                IsASRStop = true;
                string wakeupParam = JsonHelper.ToJson(manager.WakeupParam);
                if (App.DUIManager.ASRMode == EASRMode.Wakeup)
                {
                    var sresult = DUILiteHelper.DUILiteWakeupStart(wakeupParam);
                }
                byte[] managedArray = new byte[len];
                Marshal.Copy(msg, managedArray, 0, len);
                string result = Encoding.UTF8.GetString(managedArray);
                var data = JsonHelper.Parse<MNativeASRReturn>(result);
                manager.OutResult.ARSRealTimeOut = OrginalString + data.ngram.rec;
                this.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    if (App.DUIManager.ASRMode == EASRMode.UnWakeup)
                    {
                        tbtn_ASR.IsChecked = false;
                    }
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


        private void cmb_ASRLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.DUIManager.ASRLanguage = (EASRLanguage)PublicFunction.intParse(cmb_ASRLanguage.SelectedValue);
            InitDifferentLanguageParams();
            //switch (App.DUIManager.ASRLanguage)
            //{
            //    case EASRLanguage.Simple_Chinese:
            //        App.DUIManager.Config.CLOUD_ASR_RES_MODULE = "comm";
            //        App.DUIManager.NASRConfig.resBinPath = "./res/nativeASR/gram/ngram.xinhangye.v0.1.bin";
            //        //App.DUIManager.Config.CLOUD_ASR_RES_MODULE = "comm";
            //        break;
            //    case EASRLanguage.English:
            //        App.DUIManager.Config.CLOUD_ASR_RES_MODULE = "aienglish";
            //        App.DUIManager.NASRConfig.resBinPath = "./res/nativeASR/gram/ngram.english.v0.1.bin";
            //        break;
            //}
        }

        private void InitDifferentLanguageParams()
        {
            switch (App.DUIManager.ASRLanguage)
            {
                case EASRLanguage.Simple_Chinese:
                    App.DUIManager.Config.CLOUD_ASR_RES_MODULE = "comm";
                    App.DUIManager.NASRConfig.resBinPath = "./res/nativeASR/gram/ngram.xinhangye.v0.1.bin";
                    App.DUIManager.NASRParam.ebnfFile = "./res/nativeASR/gram/gram.xbnf";
                    App.DUIManager.NASRGramConfig.resBinPath = "./res/nativeASR/gram/ebnfc.aicar.1.2.0.bin";
                    //App.DUIManager.Config.CLOUD_ASR_RES_MODULE = "comm";
                    break;
                case EASRLanguage.English:
                    App.DUIManager.Config.CLOUD_ASR_RES_MODULE = "aienglish";
                    App.DUIManager.NASRGramConfig.resBinPath = "./res/nativeASR/english_gram/ebnfc.aicar.english.0.0.2.bin";
                    App.DUIManager.NASRParam.ebnfFile = "./res/nativeASR/english_gram/gram.english.xbnf";
                    App.DUIManager.NASRConfig.resBinPath = "./res/nativeASR/english_gram/ngram.english.v0.1.bin";
                    break;
            }
        }

        private void LoadEvent()
        {
            //foreach (var item in sp_Speaker.Children)
            //{
            //    if (item is RadioButton)
            //    {
            //        RadioButton rbt = item as RadioButton;
            //        rbt.Checked += RadioButton_Checked;
            //    }
            //}

            //foreach (var item in sp_SpeakerStyle.Children)
            //{
            //    if (item is RadioButton)
            //    {
            //        RadioButton rbt = item as RadioButton;
            //        rbt.Checked += RadioButton_Checked;
            //    }
            //}

            foreach (var item in tab_Speaker.Items)
            {
                if (item is TabItem)
                {
                    TabItem tabitem = item as TabItem;
                    if (tabitem.Content is RadioButton)
                    {
                        RadioButton rbt = tabitem.Content as RadioButton;
                        rbt.Checked += RadioButton_Checked;
                    }
                    else if (tabitem.Content is StackPanel)
                    {
                        foreach (var c in (tabitem.Content as StackPanel).Children)
                        {
                            if (c is RadioButton)
                            {
                                RadioButton rbt = c as RadioButton;
                                rbt.Checked += RadioButton_Checked;
                            }
                        }
                    }
                }
            }

        }


        private void GeneralSpeakerRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //RadioButton rbt = sender as RadioButton;
            //tbl_Somebody.Text = rbt.Content.ToString();
            //GetSomeBody();
        }
        private void SpeakerStyleRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rbt = sender as RadioButton;
            ChoiceList[PublicFunction.intParse(cmb_GeneralSpeaker.SelectedValue)] = rbt.Name;
            tbl_Somebody.Text = rbt.Content.ToString();


            tbtn_SpeakerStyle.Content = rbt.Content.ToString();
            tbtn_SpeakerStyle.IsChecked = false;
            pop_SpeakerStyle.IsOpen = false;
        }

        private void SpeakerStyleRadioButton_UnChecked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rbt = sender as RadioButton;
            GetCurrentSpeaker(rbt);
            //GetSomeBody();
        }

        private void GetSomeBody()
        {
            if ((tab_Speaker.Items[tab_Speaker.SelectedIndex] as TabItem).Content is StackPanel)
            {
                foreach (var item in ((tab_Speaker.Items[tab_Speaker.SelectedIndex] as TabItem).Content as StackPanel).Children)
                {
                    if (item is RadioButton)
                    {
                        RadioButton rbt = item as RadioButton;
                        if (rbt.IsChecked == true)
                        {
                            tbl_Somebody.Text = rbt.Content.ToString();
                            break;
                        }
                    }
                }
            }
            else if ((tab_Speaker.Items[tab_Speaker.SelectedIndex] as TabItem).Content is RadioButton)
            {
                RadioButton rbt = ((tab_Speaker.Items[tab_Speaker.SelectedIndex] as TabItem).Content as RadioButton);
                if (rbt.IsChecked == true)
                {
                    tbl_Somebody.Text = rbt.Content.ToString();
                }
            }
        }

        private void btn_SynthesisPlay_Click(object sender, RoutedEventArgs e)
        {
            switch (manager.TTSNet)
            {
                case ENet.Cloud:
                    CloudTTS();
                    break;
                case ENet.Native:
                    GetPCMData();
                    string url = AppDomain.CurrentDomain.BaseDirectory + "native_cntts.pcm";
                    PlayPCM(url);
                    break;
            }
            //if (rbt_TTSCloud.IsChecked == true)
            //{
            //    CloudTTS();
            //}
            //else if (rbt_TTSNative.IsChecked == true)
            //{
            //    GetPCMData();
            //    string url = AppDomain.CurrentDomain.BaseDirectory + "native_cntts.pcm";
            //    PlayPCM(url);
            //}
        }

        private List<MVoice> GetVoiceList()
        {
            var json = Encoding.UTF8.GetString(Properties.Resources.TbVoice);
            List<MVoice> list = new List<MVoice>();
            //string json = File.ReadAllText(@"C:\Users\donghua.he\Desktop\TbVoice.json");
            list = JsonHelper.Parse<List<MVoice>>(json);
            return list;
        }

        private void CloudTTS()
        {
            if (!string.IsNullOrEmpty(tb_TTSText.Text))
            {
                var voice = VoiceList.FirstOrDefault(x => Regex.Split(x.Speaker, "；")[0] == tbl_Somebody.Text);
                byte[] bytes = Encoding.UTF8.GetBytes(tb_TTSText.Text);
                double speed = sld_Speed.Value;
                int volumn = PublicFunction.intParse(sld_Volumn.Value);
                DUILiteCloudTTSStart(bytes, voice.ResName, speed, volumn);
                //DUILiteCloudTTSStart(bytes, "zhilingf", speed, volumn);
            }
            else
            {
                MessageBox.Show("请输入文字！");
            }
        }

        private void GetPCMData()
        {
            MTTSConfig config = new MTTSConfig();
            config.speed = sld_Speed.Value;
            config.volume = PublicFunction.intParse(sld_Volumn.Value);
            config.useSSML = 0;
            string ttsResConfig = JsonHelper.ToJson(manager.TTSResConfig);
            int result = DUILiteHelper.DUILiteNativeTTSNew(ttsResConfig);
            //string ttsstartparam = "{\"speed\":1.0,\"volume\":80,\"useSSML\":0}";
            string ttsConfig = JsonHelper.ToJson(config);
            result = DUILiteHelper.DUILiteNativeTTSStart(ttsConfig);
            string text = string.Empty;
            if (string.IsNullOrEmpty(tb_TTSText.Text))
            {
                text = "明天苏州的天气怎么样";
            }
            else
            {
                text = tb_TTSText.Text;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            App.IsNewTTS = true;
            result = DUILiteHelper.DUILiteNativeTTSFeed(bytes);
            result = DUILiteHelper.DUILiteNativeTTSRelease();
        }

        private static void PlayCloudAudio(string url)
        {
            Stop();
            CloudPlayer.URL = url;
            CloudPlayer.controls.play();
        }

        private static void StopCloudAudio()
        {
            if (CloudPlayer != null)
            {
                if (CloudPlayer.playState == WMPPlayState.wmppsPlaying)
                {
                    CloudPlayer.controls.stop();
                }
            }
        }

        private static void PlayPCM(string url)
        {
            //StopPCM();
            Stop();
            byte[] buffer = File.ReadAllBytes(url);
            pcmPlayer.Load(buffer, 0);
            //pcmPlayer.Load(App.PCMBuffer, 0);
            pcmPlayer.Play();
        }

        private static void Stop()
        {
            StopPCM();
            StopCloudAudio();
        }

        private static void StopPCM()
        {
            if (pcmPlayer.PlayStatus == PlayStatus.Playing)
            {
                pcmPlayer.Stop();
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void tab_Speaker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem tabitem = tab_Speaker.SelectedItem as TabItem;
            if (tabitem == null || tbl_Somebody == null)
            {
                return;
            }
            if (tabitem.Content is RadioButton)
            {
                RadioButton rbt = tabitem.Content as RadioButton;
                GetCurrentSpeaker(rbt);
                //tbl_Somebody.Text = rbt.Content.ToString();
            }
            else if (tabitem.Content is StackPanel)
            {
                foreach (var c in (tabitem.Content as StackPanel).Children)
                {
                    if (c is RadioButton)
                    {
                        RadioButton rbt = c as RadioButton;
                        GetCurrentSpeaker(rbt);
                        //tbl_Somebody.Text = rbt.Content.ToString();
                    }
                }
            }
        }

        private void rbt_Wakeup_Checked(object sender, RoutedEventArgs e)
        {
            manager.ASRMode = EASRMode.Wakeup;
        }

        private void rbt_UnWakeup_Checked(object sender, RoutedEventArgs e)
        {
            manager.ASRMode = EASRMode.UnWakeup;
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            manager.OutResult.ARSRealTimeOut = string.Empty;
            OrginalString = string.Empty;
        }

        private void tbtn_ASR_Checked(object sender, RoutedEventArgs e)
        {
            ReleaseDUILite();
            IsAuth = false;
            var callback = new DUILiteCallback(engine_callback);
            bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                manager.DUILiteConfig();
                string authConfig = JsonHelper.ToJson(manager.AuthConfig);
                DUILiteAuth(authConfig, callback, null);
            };
            bw.RunWorkerAsync();

            Thread thread = new Thread(() =>
            {
                while (!IsAuth)
                {
                    Thread.Sleep(30);
                }
                this.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    UpdateDUI(IsAuth);
                    StartASR();
                });
            });
            thread.Start();
        }

        private void StartASR()
        {
            UpdateRelateStartUI(false);
            btn_Clear_Click(null, null);
            int result = -2;
            if (!Directory.Exists(PathHelper.DirAiSpeechDemo))
            {
                Directory.CreateDirectory(PathHelper.DirAiSpeechDemo);
            }
            using (FileStream fs = new FileStream(PathHelper.DirAiSpeechDemo + "local.net.bin", FileMode.OpenOrCreate))
            {
            };
            switch (manager.ASRNet)
            {
                case ENet.Cloud:
                    switch (manager.ASRMode)
                    {
                        case EASRMode.Wakeup:
                            DUILiteHelper.DUILiteCloudASRNew();
                            DUILiteHelper.DUILiteCloudASRStart(null);
                            DUILiteHelper.DUILiteCloudASRStop();
                            string wakeupConfig = JsonHelper.ToJson(manager.WakeupConfig);
                            result = DUILiteHelper.DUILiteWakeupNew(wakeupConfig);
                            string wakeupParam = JsonHelper.ToJson(manager.WakeupParam);
                            result = DUILiteHelper.DUILiteWakeupStart(wakeupParam);
                            break;
                        case EASRMode.UnWakeup:
                            DUILiteHelper.DUILiteCloudASRNew();
                            DUILiteHelper.DUILiteCloudASRStart(null);
                            break;
                    }

                    break;

                case ENet.Native:
                    string gramcfg = JsonHelper.ToJson(manager.NASRGramConfig);
                    string gramparam = JsonHelper.ToJson(manager.NASRParam);
                    string asrcfg = JsonHelper.ToJson(manager.NASRConfig);

                    //string gramcfg = "{\"resBinPath\":\"./res/nativeASR/gram/ebnfc.aicar.1.2.0.bin\"}";
                    //string gramparam = "{\"outputPath\": \"./local.net.bin\", \"ebnfFile\": \"./res/nativeASR/gram/gram.xbnf\"}";
                    //string asrcfg = "{\"resBinPath\":\"./res/nativeASR/gram/ngram.xinhangye.v0.1.bin\", \"netBinPath\": \"./local.net.bin\"}";
                    switch (manager.ASRMode)
                    {
                        case EASRMode.Wakeup:

                            string wakeupConfig = JsonHelper.ToJson(manager.WakeupConfig);
                            result = DUILiteHelper.DUILiteWakeupNew(wakeupConfig);
                            string wakeupParam = JsonHelper.ToJson(manager.WakeupParam);
                            result = DUILiteHelper.DUILiteWakeupStart(wakeupParam);

                            result = DUILiteHelper.DUILiteNativeASRNew(gramcfg, gramparam, asrcfg);
                            if (App.DUIManager.ASRLanguage == EASRLanguage.English)
                            {
                                //string enConfig = JsonHelper.ToJson(manager.EnNASRConfig);
                                //string enConfig = "{\"env\": \"use_xbnf_rec=1;use_frame_split=1;hold_conf=1;\"}";
                                result = DUILiteHelper.DUILiteNativeASRStart(englishConfig);
                            }
                            else
                            {
                                result = DUILiteHelper.DUILiteNativeASRStart(JsonHelper.ToJson(manager.EnNASRConfig));
                            }
                            DUILiteHelper.DUILiteNativeASRStop();
                            tb_ARS.Clear();
                            break;
                        case EASRMode.UnWakeup:
                            result = DUILiteHelper.DUILiteNativeASRNew(gramcfg, gramparam, asrcfg);
                            if (App.DUIManager.ASRLanguage == EASRLanguage.English)
                            {
                                //string enConfig = JsonHelper.ToJson(manager.EnNASRConfig);
                                //string enConfig = "{\"env\": \"use_xbnf_rec=1;use_frame_split=1;hold_conf=1;\"}";
                                result = DUILiteHelper.DUILiteNativeASRStart(englishConfig);
                            }
                            else
                            {
                                result = DUILiteHelper.DUILiteNativeASRStart(JsonHelper.ToJson(manager.EnNASRConfig));
                            }
                            break;
                    }
                    break;
            }
        }

        private void UpdateRelateStartUI(bool isEnable)
        {
            cmb_ASRLanguage.IsEnabled = isEnable;
            rbt_ASRCloud.IsEnabled = isEnable;
            rbt_ASRNative.IsEnabled = isEnable;
            rbt_Wakeup.IsEnabled = isEnable;
            rbt_UnWakeup.IsEnabled = isEnable;
        }

        private void tbtn_ASR_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateRelateStartUI(true);
            switch (manager.ASRNet)
            {
                case ENet.Cloud:
                    switch (manager.ASRMode)
                    {
                        case EASRMode.Wakeup:
                            DUILiteHelper.DUILiteCloudASRStop();
                            DUILiteHelper.DUILiteCloudASRRelease();
                            DUILiteHelper.DUILiteWakeupStop();
                            DUILiteHelper.DUILiteWakeupRelease();
                            break;
                        case EASRMode.UnWakeup:
                            DUILiteHelper.DUILiteCloudASRStop();
                            DUILiteHelper.DUILiteCloudASRRelease();
                            break;
                    }
                    break;
                case ENet.Native:
                    switch (manager.ASRMode)
                    {
                        case EASRMode.Wakeup:
                            DUILiteHelper.DUILiteNativeASRStop();
                            DUILiteHelper.DUILiteNativeASRRelease();
                            DUILiteHelper.DUILiteWakeupStop();
                            DUILiteHelper.DUILiteWakeupRelease();
                            break;
                        case EASRMode.UnWakeup:
                            DUILiteHelper.DUILiteNativeASRStop();
                            DUILiteHelper.DUILiteNativeASRRelease();
                            break;
                    }
                    break;
            }

        }

        private void rbt_ASRCloud_Checked(object sender, RoutedEventArgs e)
        {
            App.DUIManager.ASRNet = ENet.Cloud;
        }

        private void rbt_ASRNative_Checked(object sender, RoutedEventArgs e)
        {
            App.DUIManager.ASRNet = ENet.Native;
        }

        private void rbt_TTSCloud_Checked(object sender, RoutedEventArgs e)
        {
            App.DUIManager.TTSNet = ENet.Cloud;
            if (ChoiceList != null && ChoiceList.Count > 0)
            {
                foreach (var s in ChoiceList)
                {
                    RadioButton rbt = this.FindName(s) as RadioButton;
                    rbt.IsChecked = true;
                }
            }
            ChoiceList.Clear();
            manager.OutResult.IsEnabled = true;
            //InitNativeOrCloud(true);
        }

        private void cmb_NetMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.DUIManager.TTSNet = (ENet)PublicFunction.intParse(cmb_NetMode.SelectedValue);
            switch (App.DUIManager.TTSNet)
            {
                case ENet.Cloud:
                    App.DUIManager.TTSNet = ENet.Cloud;
                    cmb_GeneralSpeaker.SelectedValue = PublicFunction.intParse(LastGeneralSpeaker);
                    //if (ChoiceList != null && ChoiceList.Count > 0)
                    //{
                    //    foreach (var s in ChoiceList)
                    //    {
                    //        RadioButton rbt = this.FindName(s) as RadioButton;
                    //        rbt.IsChecked = true;
                    //    }
                    //}
                    //ChoiceList.Clear();
                    if (ChoiceList != null && ChoiceList.Count > 0)
                    {
                        RadioButton rbt = this.FindName(ChoiceList[PublicFunction.intParse(cmb_GeneralSpeaker.SelectedValue)]) as RadioButton;
                        rbt.IsChecked = true;
                    }
                    manager.OutResult.IsEnabled = true;
                    break;
                case ENet.Native:
                    App.DUIManager.TTSNet = ENet.Native;
                    LastGeneralSpeaker = (EGeneralSpeaker)cmb_GeneralSpeaker.SelectedValue;
                    foreach (var c in ((tab_SpeakerStyle.SelectedItem as TabItem).Content as WrapPanel).Children)
                    {
                        if (c is RadioButton)
                        {
                            RadioButton rbt = c as RadioButton;
                            if (rbt.IsChecked == true)
                            {
                                ChoiceList[PublicFunction.intParse(cmb_GeneralSpeaker.SelectedValue)] = rbt.Name;
                                //ChoiceList.Add(rbt.Name);
                                break;
                            }
                        }
                    }
                    cmb_GeneralSpeaker.SelectedValue = 0;

                    manager.OutResult.IsEnabled = false;
                    //InitNetChangedUI(false);
                    rbt_Women1.IsChecked = true;
                    break;
            }
        }

        private void cmb_GeneralSpeaker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadioButton rbt = this.FindName(ChoiceList[PublicFunction.intParse(cmb_GeneralSpeaker.SelectedValue)]) as RadioButton;
            rbt.IsChecked = true;
        }

        private void InitNetChangedUI(bool IsEnabled)
        {

            cmb_GeneralSpeaker.IsEnabled = IsEnabled;
            tbtn_SpeakerStyle.IsEnabled = IsEnabled;
        }

        private void GetCurrentSpeaker(RadioButton rbt)
        {
            tbl_Somebody.Text = rbt.Content.ToString();
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/DUIDemo;component/Resource/Images/PersonIcon/" + rbt.Content.ToString() + ".png", UriKind.RelativeOrAbsolute));
            BitmapImage imgSource = new BitmapImage(new Uri("pack://application:,,,/DUIDemo;component/Resource/Images/PersonIcon/" + rbt.Content.ToString() + ".png", UriKind.RelativeOrAbsolute));
            img_PersonSpeaker.Source = imgSource;
            bd_PersonSpeaker.Background = imageBrush;
        }

        private void btn_SpeakerStyle_Click(object sender, RoutedEventArgs e)
        {
            pop_SpeakerStyle.IsOpen = true;
            //pop_SpeakerStyle2.IsOpen = true;
        }

        private void tbtn_SpeakerStyle_Checked(object sender, RoutedEventArgs e)
        {
            pop_SpeakerStyle.IsOpen = true;
        }

        private void tbtn_SpeakerStyle_Unchecked(object sender, RoutedEventArgs e)
        {
            pop_SpeakerStyle.IsOpen = false;
        }

        private void bd_Title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (this.Parent as Window).DragMove();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyboardDevice.Modifiers == ModifierKeys.Control
            //    && e.KeyboardDevice.Modifiers == ModifierKeys.Alt
            //    && e.Key == Key.OemOpenBrackets
            //    && e.Key == Key.OemCloseBrackets)
            //{
            //    WinSetting win = new WinSetting();
            //    win.Owner = this.Parent as Window;
            //    win.ShowInTaskbar = false;
            //    win.Icon = (this.Parent as Window).Icon;
            //    win.ShowDialog();
            //    //同时按下了Ctrl + H键（H要最后按，因为判断了此次事件的e.Key）
            //    //修饰键只能按下Ctrl，如果还同时按下了其他修饰键，则不会进入
            //}
        }

        int keyNum = 0;

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F8)
            {
                keyNum += 1;
            }
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer.Tick += (s, e1) => { timer.IsEnabled = false; keyNum = 0; };
            timer.IsEnabled = true;
            if (keyNum >= 5)
            {
                timer.IsEnabled = false;
                keyNum = 0;
                WinSetting win = new WinSetting();
                win.Owner = this.Parent as Window;
                win.ShowInTaskbar = false;
                win.Icon = (this.Parent as Window).Icon;
                win.ShowDialog();
            }
        }
    }
}
