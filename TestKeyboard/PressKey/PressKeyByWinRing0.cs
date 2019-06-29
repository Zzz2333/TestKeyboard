using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TestKeyboard.DriverStageHelper;

namespace TestKeyboard.PressKey
{
    public class PressKeyByWinRing0 : IPressKey
    {
        public bool Initialize(EnumWindowsType winType)
        {
            return WinRing0.init();
        }

        public void KeyDown(char key)
        {
            WinRing0.KeyDown(key);//按下
        }

        public void KeyPress(char key)
        {
            KeyDown(key);//按下
            Thread.Sleep(100);
            KeyUp(key);//松开
        }

        public void KeyUp(char key)
        {
            WinRing0.KeyUp(key);//松开
        }
    }
}
