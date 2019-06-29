using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TestKeyboard.PressKey;

namespace TestKeyboard
{
    public partial class Form1 : Form
    {
        private IPressKey mPressKey;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            mPressKey = new PressKeyByWinIO();
            bool bInitResult = mPressKey.Initialize(EnumWindowsType.Win64);
            if (bInitResult == false)
            {
                MessageBox.Show("组件初始化失败");
            }

            this.richTextBox1.Focus();
            Thread.Sleep(1000);
            mPressKey.KeyPress('A');
            mPressKey.KeyPress('B');
            mPressKey.KeyPress('C');
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            mPressKey = new PressKeyByWinRing0();
            bool bInitResult = mPressKey.Initialize(EnumWindowsType.Win32);
            if (bInitResult == false)
            {
                MessageBox.Show("组件初始化失败");
            }

            this.richTextBox1.Focus();
            Thread.Sleep(1000);
            mPressKey.KeyPress('A');
            mPressKey.KeyPress('B');
            mPressKey.KeyPress('C');

        }
    }
}
