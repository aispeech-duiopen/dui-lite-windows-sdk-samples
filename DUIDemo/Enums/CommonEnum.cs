using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DUIDemo.Enums
{
    public enum EASRLanguage
    {
        [Description("中文")]
        Simple_Chinese,
        [Description("英文")]
        English,
    }

    public enum EASRMode
    {
        [Description("非唤醒")]
        UnWakeup,
        [Description("唤醒")]
        Wakeup,
    }

    public enum ENet
    {
        [Description("在线")]
        Cloud,
        [Description("本地")]
        Native,
    }

    public enum EGeneralSpeaker
    {
        [Description("女声")]
        Women,
        [Description("男童")]
        Boy,
        [Description("女童")]
        Girl,
        [Description("男声")]
        Men,
    }

    public enum EASRModel
    {
        [Description("智能家居模型")]
        aihome,
        [Description("智能机器人模型")]
        airobot,
        [Description("智能车载模型")]
        aicar,
        [Description("通用模型")]
        comm,
        [Description("智能英语模型")]
        aienglish
    }
}
