using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestKeyboard.PressKey
{
    interface IPressKey
    {
        bool Initialize(EnumWindowsType winType);

        void KeyPress(char key);

        void KeyDown(char key);

        void KeyUp(char key);
    }
}
