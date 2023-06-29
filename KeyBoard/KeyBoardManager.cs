using OnlyUpCheat.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using static OnlyUpCheat.KeyBoard.KeyBoardBase;

namespace OnlyUpCheat.KeyBoard
{
    internal static class KeyBoardManager
    {
        private static IntPtr _hookHandle = IntPtr.Zero;
        private static LowLevelKeyboardProc _keyboardCallback = KeyboardHookCallback;
        public static event Action FlyHackKeyPressed;
        public static event Action SaveCheckpointKeyPressed;
        public static event Action TeleportToCheckpointKeyPressed;
        public static void Start()
        {
            _hookHandle = SetHook(_keyboardCallback);
        }
        public static void Stop()
        {
            UnhookWindowsHookEx(_hookHandle);
        }
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            IntPtr moduleHandle = GetModuleHandle(null);
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, moduleHandle, 0);
        }
        public static bool IsKeyPressed(int vKey)
        {
            return (GetAsyncKeyState(vKey) & 0x8000) != 0;
        }
        private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode == Config.FlyHackEnableDisableHotkey.VirtualKey)
                {
                    FlyHackKeyPressed?.Invoke();
                }
                else if (vkCode == Config.SaveCheckpointHotkey.VirtualKey)
                {
                    SaveCheckpointKeyPressed?.Invoke();
                }
                else if (vkCode == Config.TeleportToCheckpointHotkey.VirtualKey)
                {
                    TeleportToCheckpointKeyPressed?.Invoke();
                }
            }
            return CallNextHookEx(_hookHandle, nCode, wParam, lParam);
        }
    }
}
