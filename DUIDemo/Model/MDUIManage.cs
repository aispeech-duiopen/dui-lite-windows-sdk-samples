using DUIDemo.Enums;
using DUIDemo.Helper;
using DUIDemo.Model.DUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DUIDemo.Helper.DUILiteHelper;

namespace DUIDemo.Model
{
    public class MDUIManage
    {
        private static MDUIManage _uniqueInstance;
        public static MDUIManage uniqueInstance { get { return _uniqueInstance; } }

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        public MLogConfig LogConfig;
        public MAuthConfig AuthConfig;
        public MConfig Config;
        public MTTSRes TTSResConfig;
        public MTTSConfig TTSConfig;
        public MWakeupConfig WakeupConfig;
        public MWakeupParam WakeupParam;
        public MNASRGramConfig NASRGramConfig;
        public MNASRParam NASRParam;
        public MNASRConfig NASRConfig;
        public MEnNASRConfig EnNASRConfig;


        public MOut OutResult = new MOut();

        private EASRLanguage _ASRLanguage;
        public EASRLanguage ASRLanguage
        {
            get
            {
                return _ASRLanguage;
            }
            set
            {
                _ASRLanguage = value;
                ConfigASRLanguage(_ASRLanguage);
            }
        }

        private ENet _ASRNet;
        public ENet ASRNet
        {
            get
            {
                return _ASRNet;
            }
            set
            {
                _ASRNet = value;
            }
        }

        private EASRMode _ASRMode;
        public EASRMode ASRMode
        {
            get
            {
                return _ASRMode;
            }
            set
            {
                _ASRMode = value;
            }
        }

        private ENet _TTSNet;
        public ENet TTSNet
        {
            get
            {
                return _TTSNet;
            }
            set
            {
                _TTSNet = value;
            }
        }

        private EASRModel _ASRModel = EASRModel.comm;
        public EASRModel ASRModel
        {
            get
            {
                return _ASRModel;
            }
            set
            {
                _ASRModel = value;
            }
        }
        // 定义私有构造函数，使外界不能创建该类实例
        private MDUIManage()
        {
            InitLogConfig();
            InitAuthConfig();
            InitWakeupConfig();
            InitWakeupParam();
            InitConfig();
            InitTTSResConfig();
            InitTTSConfig();
            InitNASRGramConfig();
            InitNASRParam();
            InitNASRConfig();
            InitEnNASRConfig();
        }

        #region 初始化配置数据

        private void InitLogConfig()
        {
            LogConfig = new MLogConfig();
            LogConfig.LOG_LEVEL = 2;
            LogConfig.LOG_PATH = "duilite_log.txt";
            LogConfig.LOG_FILE_SWITCH = "true";
            LogConfig.LOG_CONSOLE_SWITCH = "true";
        }

        private void InitAuthConfig()
        {
            AuthConfig = new MAuthConfig();
            AuthConfig.PRODUCT_ID = "278581232";
            AuthConfig.PROFILE_PATH = "duilite_profile";
        }

        private void InitWakeupConfig()
        {
            WakeupConfig = new MWakeupConfig();
            WakeupConfig.resBinPath = "./res/wakeup/wakeup.modify.bin";
            WakeupConfig.vadBinPath = "./res/wakeup/vad_aihome_v0.7.bin";
        }

        private void InitWakeupParam()
        {
            WakeupParam = new MWakeupParam();
            WakeupParam.env = "words=ni hao xiao le,ni hao xiao xing;thresh=0.30,0.24;major=1,1;";
        }


        private void InitConfig()
        {
            Config = new MConfig();
            Config.DUILITE_MODE = "both";
            Config.CLOUD_ASR_RES_MODULE = "aienglish";
            Config.ENABLE_NATIVE_VAD = "true";
            Config.NATIVE_VAD_NEWCFG = new MRes()
            {
                resBinPath = "./res/vad/vad_aihome_v0.7.bin",
                pauseTime = 2 * 1000
            };
        }

        //private void InitTTSResConfig()
        //{
        //    TTSResConfig = new MTTSRes();
        //    TTSResConfig.frontBinPath = "./local_front_2018_12_04.bin";
        //    TTSResConfig.backBinPath = "./lucyf_common_back_ce_local.v2.1.0.bin";
        //    TTSResConfig.dictPath = "./aitts_sent_dict_idx_2.0.4_20180806.db";
        //}

        private void InitTTSResConfig()
        {
            TTSResConfig = new MTTSRes();
            TTSResConfig.frontBinPath = "./res/nativeTTS/local_front.bin";
            TTSResConfig.backBinPath = "./res/nativeTTS/feyinf_common_back_ce_bap_local.v2.1.0.bin";
            TTSResConfig.dictPath = "./res/nativeTTS/aitts_sent_dict_idx_2.0.4_20190215.db";
        }

        private void InitTTSConfig()
        {
            TTSConfig = new MTTSConfig();
            TTSConfig.speed = 1.0;
            TTSConfig.volume = 80;
            TTSConfig.useSSML = 0;
        }

        private void InitNASRGramConfig()
        {
            NASRGramConfig = new MNASRGramConfig();
            NASRGramConfig.resBinPath = "./res/nativeASR/gram/ebnfc.aicar.1.2.0.bin";
        }

        private void InitNASRParam()
        {
            NASRParam = new MNASRParam();
            NASRParam.outputPath = PathHelper.DirAiSpeechDemo + "local.net.bin";
            NASRParam.ebnfFile = "./res/nativeASR/gram/gram.xbnf";
        }

        private void InitNASRConfig()
        {
            NASRConfig = new MNASRConfig();
            NASRConfig.resBinPath = "./res/nativeASR/gram/ngram.xinhangye.v0.1.bin";
            NASRConfig.netBinPath = PathHelper.DirAiSpeechDemo + "local.net.bin";
        }

        private void InitEnNASRConfig()
        {
            EnNASRConfig = new MEnNASRConfig();
            EnNASRConfig.env = "use_xbnf_rec=1;use_pinyin=1;use_frame_split=1;hold_conf=1;";
        }

        #endregion

        public void ConfigASRLanguage(EASRLanguage value)
        {
            Config.CLOUD_ASR_RES_MODULE = "aienglish";
        }




        #region 单例模式

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static MDUIManage GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了
            if (_uniqueInstance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (_uniqueInstance == null)
                    {
                        _uniqueInstance = new MDUIManage();
                    }
                }
            }
            return uniqueInstance;
        }

        #endregion

        #region DUI相关

        public void DUILiteLogInit()
        {
            string logConfig = JsonHelper.ToJson(LogConfig);
            DUILiteHelper.DUILiteLogInit(logConfig);
        }

        public void DUILiteConfig()
        {
            string strconfig = JsonHelper.ToJson(Config);
            DUILiteHelper.DUILiteConfig(strconfig);
        }

        #endregion

        #region 输出相关

        public void SetOutResult()
        {

        }


        #endregion
    }
}
