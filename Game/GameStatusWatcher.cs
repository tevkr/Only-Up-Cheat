using OnlyUpCheat.Core;
using System;
using System.Diagnostics;
using System.Threading;

namespace OnlyUpCheat.Game
{
    internal static class GameStatusWatcher
    {
        public static event Action<bool> StatusChanged;
        private static bool _isRunning = false;
        private static bool _disposed = false;
        public static void Start()
        {
            while (!_disposed)
            {
                Process[] processes = Process.GetProcessesByName(Config.ExeName);
                bool found = processes.Length > 0;
                if (found != _isRunning)
                {
                    _isRunning = found;
                    StatusChanged?.Invoke(_isRunning);
                }
                Thread.Sleep(3000);
            }
        }
        public static void Stop()
        {
            _disposed = true;
        }
    }
}
