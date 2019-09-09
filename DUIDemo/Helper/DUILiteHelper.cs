using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DUIDemo.Helper
{
    public static class DUILiteHelper
    {
        #region Log

        /// <summary>
        /// 初始化Log
        /// </summary>
        /// <param name="param"></param>
        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteLogInit", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteLogInit(string param);

        /// <summary>
        /// 释放Log
        /// </summary>
        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteLogRelease", CallingConvention = CallingConvention.Cdecl)]
        public static extern void duiliteLogRelease();

        #endregion

        #region 授权相关

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="param"></param>
        /// <param name="callback"></param>
        /// <param name="userdata"></param>
        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteAuth", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteAuth(string param, DUILiteCallback callback, string userdata);

        [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public delegate int DUILiteCallback(string userdata, int type, IntPtr msg, int len);

        /// <summary>
        /// 释放授权
        /// </summary>
        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteAuthRelease", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteAuthRelease();

        #endregion

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteConfig", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteConfig(string cfg);

        #region 唤醒

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteWakeupNew", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteWakeupNew(string cfg);

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteWakeupStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteWakeupStart(string cfg);

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteWakeupStop", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteWakeupStop();

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteWakeupRelease", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteWakeupRelease();

        [DllImport("DUILite-SDK.dll", EntryPoint = "duilieWakeupFeed", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteWakeupFeed(string data, int len);

        #endregion

        #region TTS

        #region 本地TTS

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteNativeTTSNew", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteNativeTTSNew(string cfg);

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteNativeTTSStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteNativeTTSStart(string param);

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteNativeTTSFeed", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteNativeTTSFeed(byte[] text);

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteNativeTTSRelease", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteNativeTTSRelease();

        #endregion

        #region 云端TTS

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteCloudTTSStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteCloudTTSStart(byte[] text, string voiceId, double speed, int volume);

        #endregion

        #endregion

        #region ASR

        #region 云端ASR

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteCloudASRNew", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteCloudASRNew();

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteCloudASRStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteCloudASRStart(string param);

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteCloudASRStop", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteCloudASRStop();

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteCloudASRRelease", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteCloudASRRelease();

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteCloudASRFeed", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteCloudASRFeed(string data, int len);

        #endregion

        #region 本地ASR

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteNativeASRNew", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteNativeASRNew(string gramcfg, string gramparam, string asrcfg);

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteNativeASRStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteNativeASRStart(string param);

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteNativeASRStop", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteNativeASRStop();

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteNativeASRRelease", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteNativeASRRelease();

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteNativeASRFeed", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteNativeASRFeed(string data, int len);

        #endregion

        #endregion

        #region 线性麦克风阵列

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteFesplNew", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteFesplNew(string cfg);

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteFesplStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteFesplStart(string cfg);

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteFesplStop", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteFesplStop();

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteFesplRelease", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DUILiteFesplRelease();

        [DllImport("DUILite-SDK.dll", EntryPoint = "duiliteFesplFeed", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DUILiteFesplFeed(string data, int len);

        #endregion

        [StructLayout(LayoutKind.Sequential)]
        public struct Cntts
        {
            public double speed;
            public int volume;
            public int lmargin;
            public int rmargin;
            public int useSSML;
        }

        [DllImport("duilite.dll", EntryPoint = "duilite_cntts_feed", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DULLiteCnttsFeed(Cntts data, string para);


    }
}
