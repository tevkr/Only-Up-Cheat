using OnlyUpCheat.Memory;
using System;
using System.Windows;

namespace OnlyUpCheat.Game
{
    internal class Player
    {
        public void Update(IntPtr xAddr, IntPtr yAddr, IntPtr zAddr, IntPtr pitchAddr, IntPtr yawAddr, IntPtr groundStateAddr, IntPtr inGameState)
        {
            _xAddr = xAddr;
            _yAddr = yAddr;
            _zAddr = zAddr;
            _pitchAddr = pitchAddr;
            _yawAddr = yawAddr;
            _groundStateAddr = groundStateAddr;
            _inGameState = inGameState;
        }
        private IntPtr _xAddr;
        public double X 
        {   get
            {
                return MemoryManager.GetValue(_xAddr);
            }
            set
            {
                MemoryManager.SetValue(_xAddr, value);
            }
        }
        private IntPtr _yAddr;
        public double Y
        {
            get
            {
                return MemoryManager.GetValue(_yAddr);
            }
            set
            {
                MemoryManager.SetValue(_yAddr, value);
            }
        }
        private IntPtr _zAddr;
        public double Z
        {
            get
            {
                return MemoryManager.GetValue(_zAddr);
            }
            set
            {
                MemoryManager.SetValue(_zAddr, value);
            }
        }
        private IntPtr _pitchAddr;
        public double Pitch
        {
            get
            {
                return MemoryManager.GetValue(_pitchAddr);
            }
            set
            {
                MemoryManager.SetValue(_pitchAddr, value);
            }
        }
        private IntPtr _yawAddr;
        public double Yaw
        {
            get
            {
                return MemoryManager.GetValue(_yawAddr);
            }
            set
            {
                MemoryManager.SetValue(_yawAddr, value);
            }
        }
        private IntPtr _groundStateAddr;
        public int GroundState
        {
            get
            {
                return MemoryManager.GetValue(_groundStateAddr, 1);
            }
            set
            {
                MemoryManager.SetValue(_groundStateAddr, value);
            }
        }
        private IntPtr _inGameState;
        public int InGameState
        {
            get
            {
                return MemoryManager.GetValue(_inGameState, 1);
            }
            set
            {
                MemoryManager.SetValue(_inGameState, value);
            }
        }
        public (double X, double Y, double Z) GetDirectionVector()
        {
            double pitch = Pitch;
            double yaw = Yaw;
            double pitchRad = ToRadians(pitch);
            double yawRad = ToRadians(yaw);
            double x = Math.Cos(yawRad) * Math.Cos(pitchRad);
            double y = Math.Cos(yawRad) * Math.Sin(pitchRad);
            double z = Math.Sin(yawRad);
            return (x, y, z);
        }
        public (double X, double Y, double Z) RotateDirectionVectorAroundZ(double degrees)
        {
            double radians = ToRadians(degrees);
            double cos = Math.Cos(radians);
            double sin = Math.Cos(radians);
            return (X * cos - Y * sin, X * sin + Y * cos, Z);
        }
        private static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
