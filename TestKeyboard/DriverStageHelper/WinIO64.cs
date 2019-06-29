using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TestKeyboard.DriverStageHelper
{
    /// <summary>
    /// 驱动级帮助类WinIO64
    /// WinIO使用条件：
    /// 1.电脑启用“测试模式”，管理员打开CMD，输入bcdedit /set testsigning on打开测试模式，bcdedit /set testsigning off关闭测试模式
    /// 2.使用PS/2键盘
    /// 3.将WinIo64.dll和WinIo64.sys拷贝到程序运行的根目录下
    /// 4.注册WinIo64的签名，右键WinIo64.sys->属性->数字签名->选择签名列表的项->详细信息->查看证书->安装证书->本地计算机->下一步->将所有的证书都放入下列存储->浏览->受信任的根证书发布机构->确定->下一步->完成->提示导入成功
    /// 5.管理员打开程序
    /// https://blog.csdn.net/no99es/article/details/50537102
    /// </summary>
    public class WinIO64
    {
        private const int KBC_KEY_CMD = 0x64;
        private const int KBC_KEY_DATA = 0x60;

        #region WinIo64.dll
        [DllImport("WinIo64.dll")]
        public static extern bool InitializeWinIo();

        [DllImport("WinIo64.dll")]
        public static extern bool GetPortVal(IntPtr wPortAddr, out int pdwPortVal, byte bSize);

        [DllImport("WinIo64.dll")]
        public static extern bool SetPortVal(uint wPortAddr, IntPtr dwPortVal, byte bSize);

        [DllImport("WinIo64.dll")]
        public static extern byte MapPhysToLin(byte pbPhysAddr, uint dwPhysSize, IntPtr PhysicalMemoryHandle);

        [DllImport("WinIo64.dll")]
        public static extern bool UnmapPhysicalMemory(IntPtr PhysicalMemoryHandle, byte pbLinAddr);

        [DllImport("WinIo64.dll")]
        public static extern bool GetPhysLong(IntPtr pbPhysAddr, byte pdwPhysVal);

        [DllImport("WinIo64.dll")]
        public static extern bool SetPhysLong(IntPtr pbPhysAddr, byte dwPhysVal);

        [DllImport("WinIo64.dll")]
        public static extern void ShutdownWinIo();
        #endregion

        [DllImport("user32.dll")]
        public static extern int MapVirtualKey(uint Ucode, uint uMapType);


        private WinIO64()
        {
            IsInitialize = true;
        }
        public static void Initialize()
        {
            if (InitializeWinIo())
            {
                KBCWait4IBE();
                IsInitialize = true;
            }
            else
                MessageBox.Show("Load WinIO Failed!");
        }
        public static void Shutdown()
        {
            if (IsInitialize)
                ShutdownWinIo();
            IsInitialize = false;
        }

        private static bool IsInitialize { get; set; }

        ///等待键盘缓冲区为空
        private static void KBCWait4IBE()
        {
            int dwVal = 0;
            do
            {
                bool flag = GetPortVal((IntPtr)0x64, out dwVal, 1);
            }
            while ((dwVal & 0x2) > 0);
        }
        /// 模拟键盘标按下
        public static void KeyDown(Keys vKeyCoad)
        {
            if (!IsInitialize) return;

            int btScancode = 0;
            btScancode = MapVirtualKey((uint)vKeyCoad, 0);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD2, 1);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_DATA, (IntPtr)0x60, 1);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD2, 1);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_DATA, (IntPtr)btScancode, 1);
        }
        /// 模拟键盘弹出
        public static void KeyUp(Keys vKeyCoad)
        {
            if (!IsInitialize) return;

            int btScancode = 0;
            btScancode = MapVirtualKey((uint)vKeyCoad, 0);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD2, 1);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_DATA, (IntPtr)0x60, 1);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD2, 1);
            KBCWait4IBE();
            SetPortVal(KBC_KEY_DATA, (IntPtr)(btScancode | 0x80), 1);
        }
    }
}