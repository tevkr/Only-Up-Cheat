using OnlyUpCheat.Core;
using System;
using System.Diagnostics;
using System.Linq;

namespace OnlyUpCheat.Memory
{
    internal static class MemoryManager
    {
        private static Process _process;
        private static IntPtr _hProcess;
        private static IntPtr _modBase;
        public static void Initialize()
        {
            _process = Process.GetProcessesByName(Config.ExeName).FirstOrDefault();
            _hProcess = MemoryBase.OpenProcess(MemoryBase.ProcessAccessFlags.All, false, _process.Id);
            _modBase = MemoryBase.GetModuleBaseAddress(_process.Id, Config.ModuleName);
        }
        public static IntPtr GetAddr((int Pointer, int[] Offsets) values)
        {
            return MemoryBase.FindDMAAddy(_hProcess, (IntPtr)(_modBase + values.Pointer), values.Offsets);
        }
        public static double GetValue(IntPtr addr)
        {
            double value = 0;
            byte[] buffer = new byte[sizeof(double)];
            if (MemoryBase.ReadProcessMemory(_hProcess, addr, buffer, sizeof(double), out _))
            {
                value = BitConverter.ToDouble(buffer, 0);
            }
            return value;
        }
        public static int GetValue(IntPtr addr, int wheelchair)
        {
            int value = 0;
            byte[] buffer = new byte[sizeof(int)];
            if (MemoryBase.ReadProcessMemory(_hProcess, addr, buffer, sizeof(int), out _))
            {
                value = BitConverter.ToInt32(buffer, 0);
            }
            return value;
        }
        public static void SetValue(IntPtr addr, double value)
        {
            MemoryBase.WriteProcessMemory(_hProcess, addr, value, sizeof(double), out _);
        }
        public static void SetValue(IntPtr addr, int value)
        {
            MemoryBase.WriteProcessMemory(_hProcess, addr, value, sizeof(int), out _);
        }
    }
}
