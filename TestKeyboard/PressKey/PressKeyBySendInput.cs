using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TestKeyboard.DriverStageHelper;
using static TestKeyboard.DriverStageHelper.SendInputHelper;

namespace TestKeyboard.PressKey
{
    public class PressKeyBySendInput : IPressKey
    {
        public bool Initialize(EnumWindowsType winType)
        {
            return true;
        }

        public void KeyDown(char key)
        {
            INPUT[] input = new INPUT[1];
            input[0].type = 1;
            short num = VkKeyScan('A');
            input[0].ki.wVk = 0x14;
            input[0].ki.dwFlags = 0;

            SendInput(1, input, Marshal.SizeOf(default(INPUT)));
        }

        public void KeyPress(char key)
        {
            KeyDown('A');
            Application.DoEvents();
            KeyUp('A');
        }

        public void KeyUp(char key)
        {
            INPUT[] input = new INPUT[1];
            input[0].type = 1;
            short num = VkKeyScan('A');
            input[0].ki.wVk = 0x14;
            input[0].ki.dwFlags = 2;

            SendInput(1, input, Marshal.SizeOf(default(INPUT)));
        }
    }
}