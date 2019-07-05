using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GeneralSetting
{
    public static class BinaryHelper
    {
        public static string ReadBinary(string path)
        {
            int i = 0;
            double d = 0;
            bool b = false;
            string s = string.Empty;
            BinaryReader br;
            try
            {
                br = new BinaryReader(new FileStream(path, FileMode.OpenOrCreate));
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message + "\n Cannot open file.");
                return null;
            }
            try
            {
                i = br.ReadInt32();
                d = br.ReadDouble();
                b = br.ReadBoolean();
                s = br.ReadString();
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message + "\n Cannot read from file.");
                return null;
            }
            br.Close();
            return s;
        }

        public static bool WriteBinary(string data)
        {
            BinaryWriter bw;
            int i = 25;
            double d = 3.14157;
            bool b = true;
            // 创建文件
            try
            {
                bw = new BinaryWriter(new FileStream("setting", FileMode.OpenOrCreate));
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message + "\n Cannot create file.");
                return false;
            }
            // 写入文件
            try
            {
                bw.Write(i);
                bw.Write(d);
                bw.Write(b);
                bw.Write(data);
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message + "\n Cannot write to file.");
                return false;
            }
            bw.Close();
            return true;
        }

    }
}
