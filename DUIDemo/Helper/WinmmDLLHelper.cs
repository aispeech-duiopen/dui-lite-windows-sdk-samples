using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DUIDemo.Helper
{
    public static class WinmmDLLHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct WAVEFORMATEX
        {
            public short wFormatTag;
            public short nChannels;
            public int nSamplesPerSec;
            public int nAvgBytesPerSec;
            public short nBlockAlign;
            public short wBitsPerSample;
            public short cbSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WAVEHDR
        {
            public IntPtr lpData;
            public int dwBufferLength;
            public int dwBytesRecorded;
            public int dwUser;
            public int dwFlags;
            public int dwLoops;
            public int lpNext;
            public int nReserved;
        }

        public const int NULL = 0;
        public const int MMSYSERR_NOERROR = NULL;
        public const int WHDR_PREPARED = 2;
        public const int WHDR_ENDLOOP = 8;
        public const int WHDR_BEGINLOOP = 4;
        public const int WOM_DONE = 957;
        public const int CALLBACK_FUNCTION = 196608;

        [DllImport("WinMm.dll", SetLastError = true)]
        public static extern int waveOutOpen(ref IntPtr hWaveOut,
            int uDeviceID,
            ref WAVEFORMATEX lpFormat,
            [MarshalAs(UnmanagedType.FunctionPtr)]DeviceNotifyPtr dwCallback,
            int dwInstance,
            int dwFlags);

        [DllImport("WinMm.dll", SetLastError = true)]
        public static extern int waveOutPrepareHeader(IntPtr hWaveOut, ref WAVEHDR lpWaveOutHdr, int uSize);

        [DllImport("WinMm.dll", SetLastError = true)]
        public static extern int waveOutWrite(IntPtr hWaveOut, ref WAVEHDR lpWaveOutHdr, int uSize);

        [DllImport("WinMm.dll", SetLastError = true)]
        public static extern int waveOutPause(IntPtr hWaveOut);

        [DllImport("WinMm.dll", SetLastError = true)]
        public static extern int waveOutRestart(IntPtr hWaveOut);

        [DllImport("WinMm.dll", SetLastError = true)]
        public static extern int waveOutReset(IntPtr hWaveOut);

        [DllImport("WinMm.dll", SetLastError = true)]
        public static extern uint waveOutClose(IntPtr hWaveOut);

        [DllImport("WinMm.dll", SetLastError = true)]
        public static extern int waveOutSetVolume(IntPtr hWaveOut, int dwVolume);

        [DllImport("WinMm.dll", SetLastError = true)]
        public static extern int waveOutGetVolume(IntPtr hWaveOut, ref int dwVolume);

        [DllImport("WinMm.dll", SetLastError = true)]
        public static extern int waveOutGetNumDevs();

        public delegate void DeviceNotifyPtr(int nWaveOutDev, int uMsg, IntPtr dwInstance, IntPtr wParam, IntPtr lParam);
    }
}
