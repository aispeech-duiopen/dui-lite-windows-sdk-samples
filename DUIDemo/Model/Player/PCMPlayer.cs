using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static DUIDemo.Helper.WinmmDLLHelper;

namespace DUIDemo.Model.Player
{
    public enum PlayStatus : int
    {
        Playing = 1,
        Pause = 2,
        Stop = 3
    }

    public class PCMPlayer
    {
        private WAVEHDR sWaveHdr = new WAVEHDR();
        private IntPtr hWaveOut = IntPtr.Zero;
        private PlayStatus nPlayStatus = PlayStatus.Stop;
        private WAVEFORMATEX sWaveFormat = new WAVEFORMATEX();
        private DeviceNotifyPtr pfnDevNotify = null;
        private object[] mDepArgs = new object[2];

        public PCMPlayer()
        {
            if (waveOutGetNumDevs() <= 0)
            {
                throw new Exception("Cannot find the your computer audio output device."); // 无法找到您电脑的音频输出设备
            }

            //sWaveFormat.nSamplesPerSec = 44100; // 波形采样
            //sWaveFormat.nAvgBytesPerSec = 176400; // 平均传输率
            //sWaveFormat.wFormatTag = 1; // 波形格式
            //sWaveFormat.nChannels = 2; // 声道
            //sWaveFormat.wBitsPerSample = 16; // 采样位深
            //sWaveFormat.nBlockAlign = 2 * 16 / 8; // 块对齐
            //sWaveFormat.cbSize = 16; // 结构尺寸

            sWaveFormat.wFormatTag = 1;//设置波形声音的格式
            sWaveFormat.nChannels = 1;//设置音频文件的通道数量
            sWaveFormat.nSamplesPerSec = 16000;//设置每个声道播放和记录时的样本频率
            sWaveFormat.nBlockAlign = 2;//以字节为单位设置块对齐
            sWaveFormat.nAvgBytesPerSec = sWaveFormat.nBlockAlign * sWaveFormat.nSamplesPerSec;//设置请求的平均数据传输率,单位byte/s。这个值对于创建缓冲大小是很有用的
            sWaveFormat.wBitsPerSample = 16;
            sWaveFormat.cbSize = 16;//额外信息的大小

            pfnDevNotify = DeviceNotify;
            GCHandle.Alloc(pfnDevNotify, GCHandleType.Normal);
        }

        public PlayStatus PlayStatus
        {
            get
            {
                return nPlayStatus;
            }
            set
            {
                nPlayStatus = value;
            }
        }

        public void Load(byte[] buffer, int loop)
        {
            if (hWaveOut != IntPtr.Zero && waveOutReset(hWaveOut) != MMSYSERR_NOERROR
                && waveOutClose(hWaveOut) != MMSYSERR_NOERROR)
            {
                hWaveOut = IntPtr.Zero;
                throw new Exception("Unable to turn off the audio output device."); // 无法关闭音频输出设备
            }
            if (waveOutOpen(ref hWaveOut, -1, ref sWaveFormat, pfnDevNotify,
                NULL, CALLBACK_FUNCTION) != MMSYSERR_NOERROR)
            {
                throw new Exception("Could not open audio output device."); // 无法打开音频输出设备
            }
            sWaveHdr.lpData = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            sWaveHdr.dwBufferLength = buffer.Length;
            var a = waveOutPrepareHeader(hWaveOut, ref sWaveHdr, 32);
            if (waveOutPrepareHeader(hWaveOut, ref sWaveHdr, 32) != MMSYSERR_NOERROR)
            {
                throw new Exception("Unable to prepare the waveform data block used to play."); // 无法准备用于播放的波形数据块
            }
            sWaveHdr.dwLoops = loop;
            sWaveHdr.dwFlags = WHDR_BEGINLOOP | WHDR_ENDLOOP | WHDR_PREPARED;
            mDepArgs[0] = buffer;
            mDepArgs[1] = loop;
        }

        public void Play()
        {
            if (PlayStatus == PlayStatus.Pause)
            {
                if (waveOutRestart(hWaveOut) != MMSYSERR_NOERROR)
                {
                    throw new Exception("Unable to block the output waveform data to the device."); // 无法输出波形数据块到设备
                }
            }
            else if (PlayStatus == PlayStatus.Stop)
            {
                Load((byte[])mDepArgs[0], (int)mDepArgs[1]);
            }
            if (PlayStatus != PlayStatus.Pause && PlayStatus != PlayStatus.Playing
                && waveOutWrite(hWaveOut, ref sWaveHdr, 32) != MMSYSERR_NOERROR)
            {
                throw new Exception("Unable to block the output waveform data to the device."); // 无法输出波形数据块到设备
            }
            PlayStatus = PlayStatus.Playing;
        }

        public void Pause()
        {
            if (waveOutPause(hWaveOut) != MMSYSERR_NOERROR)
            {
                throw new Exception("Unable to pause output waveform data block to device."); // 无法暂停波形数据块输出到设备
            }
            PlayStatus = PlayStatus.Pause;
        }

        public void Stop()
        {
            if (waveOutReset(hWaveOut) != MMSYSERR_NOERROR)
            {
                throw new Exception("Unable to stop output waveform data block to device."); // 无法停止波形数据块输出到设备
            }
            PlayStatus = PlayStatus.Stop;
        }

        public int Volume
        {
            get
            {
                int nWavOutVol = 0;
                if (waveOutGetVolume(hWaveOut, ref nWavOutVol) != MMSYSERR_NOERROR)
                {
                    throw new Exception("Unable to get the volume size of the output device."); // 无法获取输出设备的音量大小
                }
                int nVolLow = ((short)nWavOutVol) / (ushort.MaxValue / 100);
                int nVolHigh = (nWavOutVol >> 16) / (ushort.MaxValue / 100);
                return nVolLow >= nVolHigh ? nVolLow : nVolHigh;
            }
            set
            {
                if (waveOutSetVolume(hWaveOut, value * ushort.MaxValue / 100 << 16
                    | value * ushort.MaxValue / 100) != MMSYSERR_NOERROR)
                {
                    throw new Exception("Unable to modify the volume size of the output device."); // 无法修改输出设备的音量大小
                }
            }
        }

        private void DeviceNotify(int nWaveOutDev, int uMsg, IntPtr dwInstance, IntPtr wParam, IntPtr lParam)
        {
            if (uMsg == WOM_DONE)
            {
                PlayStatus = PlayStatus.Stop;
            }
        }
    }
}
