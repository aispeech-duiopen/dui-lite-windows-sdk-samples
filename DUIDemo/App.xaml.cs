﻿using DUIDemo.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DUIDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static bool IsNewTTS = false;
        public static byte[] PCMBuffer;
        public static MDUIManage DUIManager = MDUIManage.GetInstance();
    }
}
