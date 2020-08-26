using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TestKeyboard.DriverStageHelper
{
    public class SendInputHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(uint nInput, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        internal static extern int MapVirtualKey(uint Ucode, uint uMapType);

        [DllImport("user32.dll")]
        internal static extern short VkKeyScan(char ch);

        [DllImport("user32.dll")]
        internal static extern short GetKeyState(int nVirtKey);

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public int type;

            [FieldOffset(4)]
            public KEYBDINPUT ki;

            [FieldOffset(4)]
            public MOUSEINPUT mi;

            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }

        public struct MOUSEINPUT
        {
            public int dx;

            public int dy;

            public int mouseData;

            public int dwFlags;

            public int time;

            public IntPtr dwExtraInfo;
        }

        public struct KEYBDINPUT
        {
            public short wVk;

            public short wScan;

            public int dwFlags;

            public int time;

            public IntPtr dwExtraInfo;
        }

        public struct HARDWAREINPUT
        {
            public int uMsg;

            public short wParamL;

            public short wParamH;
        }


        public static void SimulateInputString(string sText)
        {
            char[] cText = sText.ToCharArray();
            foreach (char c in cText)
            {
                INPUT[] input = new INPUT[2];
                if (c >= 0 && c < 256)//a-z A-Z
                {
                    short num = VkKeyScan(c);//获取虚拟键码值
                    if (num != -1)
                    {
                        bool shift = (num >> 8 & 1) != 0;//num >>8表示 高位字节上当状态，如果为1则按下Shift，否则没有按下Shift，即大写键CapsLk没有开启时，是否需要按下Shift。
                        if ((GetKeyState(20) & 1) != 0 && ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')))//Win32API.GetKeyState(20)获取CapsLk大写键状态
                        {
                            shift = !shift;
                        }

                        if (shift)
                        {
                            input[0].type = 1;//模拟键盘
                            input[0].ki.wVk = 16;//Shift键
                            input[0].ki.dwFlags = 0;//按下
                            SendInput(1u, input, Marshal.SizeOf(default(INPUT)));
                        }

                        input[0].type = 1;
                        input[0].ki.wVk = (short)(num & 0xFF);
                        input[0].ki.dwFlags = 0;

                        input[1].type = 1;
                        input[1].ki.wVk = (short)(num & 0xFF);
                        input[1].ki.dwFlags = 2;
                        SendInput(2u, input, Marshal.SizeOf(default(INPUT)));

                        if (shift)
                        {
                            input[0].type = 1;
                            input[0].ki.wVk = 16;
                            input[0].ki.dwFlags = 2;//抬起
                            SendInput(1u, input, Marshal.SizeOf(default(INPUT)));
                        }
                        continue;
                    }
                }

                input[0].type = 1;
                input[0].ki.wVk = 0;//dwFlags 为KEYEVENTF_UNICODE 即4时，wVk必须为0
                input[0].ki.wScan = (short)c;
                input[0].ki.dwFlags = 4;//输入UNICODE字符
                input[0].ki.time = 0;
                input[0].ki.dwExtraInfo = IntPtr.Zero;
                input[1].type = 1;
                input[1].ki.wVk = 0;
                input[1].ki.wScan = (short)c;
                input[1].ki.dwFlags = 6;
                input[1].ki.time = 0;
                input[1].ki.dwExtraInfo = IntPtr.Zero;
                SendInput(2u, input, Marshal.SizeOf(default(INPUT)));
            }
        }
    }
}
