using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DUIDemo.Enums
{
    public enum EEventReturn
    {
        //DUILite Event
        DUILITE_EV_OUT_WAKEUP_RESULT = 1,	//返回唤醒结果事件
        DUILITE_EV_OUT_NATIVE_VAD_AUDIO = 2,	//返回本地VAD音频事件
        DUILITE_EV_OUT_NATIVE_VAD_BEGIN = 3,	//返回本地VAD开始事件
        DUILITE_EV_OUT_NATIVE_VAD_END = 4,	//返回本地VAD结束事件
        DUILITE_EV_OUT_NATIVE_TTS_BEGIN = 5,	//返回本地TTS开始事件
        DUILITE_EV_OUT_NATIVE_TTS_END = 6,//返回本地TTS结束事件
        DUILITE_EV_OUT_NATIVE_TTS_AUDIO = 7,	//返回本地TTS音频事件
        DUILITE_EV_OUT_CLOUD_TTS_RESULT = 8,	//返回云端TTS结果(URL)事件
        DUILITE_EV_OUT_CLOUD_ASR_FINAL_RESULT = 9,	//返回云端识别结果事件
        DUILITE_EV_OUT_CLOUD_ASR_REALBACK_RESULT = 10,//返回云端识别实时反馈结果事件
        DUILITE_EV_OUT_CLOUD_ASR_PINYIN_RESULT = 11,//返回云端识别拼音事件
        DUILITE_EV_OUT_AUTH_COMPLETE = 12,//返回授权成功事件
        DUILITE_EV_OUT_ERROR = 13,//返回错误消息事件
        DUILITE_EV_OUT_NATIVE_ASR_FINAL_RESULT = 14,                //返回本地识别结果事件
        DUILITE_EV_OUT_NATIVE_ASR_REALBACK_RESULT = 15,      //返回本地ASR实时反馈结果事件

    }
}
