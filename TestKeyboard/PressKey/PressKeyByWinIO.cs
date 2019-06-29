using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestKeyboard.DriverStageHelper;

namespace TestKeyboard.PressKey
{
    public class PressKeyByWinIO : IPressKey
    {
        private EnumWindowsType mWinType;
        public bool Initialize(EnumWindowsType winType)
        {
            mWinType = winType;
            switch(mWinType)
            {
                case EnumWindowsType.Win64:
                    WinIO64.Initialize();
                    break;
                case EnumWindowsType.Win32:
                    WinIO32.Initialize();
                    break;
                default:
                    return false;
            }

            return true;
        }

        public void KeyDown(char key)
        {
            switch (mWinType)
            {
                case EnumWindowsType.Win64:
                    WinIO64.KeyDown((Keys)key);//按下
                    break;
                case EnumWindowsType.Win32:
                    WinIO32.KeyDown((Keys)key);//按下
                    break;
                default:
                    break;
            }
        }

        public void KeyPress(char key)
        {
            Delay(100);
            KeyDown(key);
            Delay(300);
            KeyUp(key);
            Delay(100);
        }

        public void KeyUp(char key)
        {
            switch (mWinType)
            {
                case EnumWindowsType.Win64:
                    WinIO64.KeyUp((Keys)key);//按下
                    break;
                case EnumWindowsType.Win32:
                    WinIO32.KeyUp((Keys)key);//按下
                    break;
                default:
                    break;
            }
        }


        private static void Delay(int nMilliSeconds)
        {
            DateTime dt = DateTime.Now;
            dt = dt.AddMilliseconds(nMilliSeconds);
            while (DateTime.Now < dt)
            {
                Application.DoEvents();//转让控制权            
            }
            return;
        }
    }
}
