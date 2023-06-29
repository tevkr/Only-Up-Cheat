using OnlyUpCheat.Core;
using OnlyUpCheat.Game;
using OnlyUpCheat.Memory;
using OnlyUpCheat.KeyBoard;
using System.Threading.Tasks;
using System;

namespace OnlyUpCheat
{
    internal static class Cheat
    {
        public static Action AddrError;
        public static bool FlyHackEnabled { get; private set; } = false;
        private static (double X, double Y, double Z) _checkpoint = (3803.3, 16388.51, -3460.4);
        private static Player _player = null;
        private static bool _prevInGameState = false;
        private static bool _disposed = false;
        public static void Start()
        {
            MemoryManager.Initialize();
            _player = new Player();
            _disposed = false;
            UpdateAddr();
            Task.Run(() => { InGameChecker(); });
            KeyBoardManager.FlyHackKeyPressed += FlyHackKeyPressedHandler;
            KeyBoardManager.SaveCheckpointKeyPressed += SaveCheckpointKeyPressedHandler;
            KeyBoardManager.TeleportToCheckpointKeyPressed += TeleportToCheckpointKeyPressed;
        }
        private static void InGameChecker()
        {
            int magicNumber = 63;
            while (!_disposed)
            {
                if (_player.InGameState != magicNumber)
                {
                    _prevInGameState = false;
                }
            }
        }
        private static bool IsInGame()
        {
            int magicNumber = 63;
            var inGameState = _player.InGameState == magicNumber;
            if (inGameState && !_prevInGameState)
            {
                UpdateAddr();
            }
            _prevInGameState = inGameState;
            return inGameState;
        }
        private static void UpdateAddr()
        {
            var xAddr = MemoryManager.GetAddr(Config.PlayerXCoord);
            var yAddr = MemoryManager.GetAddr(Config.PlayerYCoord);
            var zAddr = MemoryManager.GetAddr(Config.PlayerZCoord);
            var pitchAddr = MemoryManager.GetAddr(Config.CameraPitch);
            var yawAddr = MemoryManager.GetAddr(Config.CameraYaw);
            var groundStateAddr = MemoryManager.GetAddr(Config.GroundState);
            var inGameState = MemoryManager.GetAddr(Config.InGameState);
            _player.Update(xAddr, yAddr, zAddr, pitchAddr, yawAddr, groundStateAddr, inGameState);
        }
        private static void FlyHackKeyPressedHandler()
        {
            FlyHackEnabled = !FlyHackEnabled;
            if (FlyHackEnabled)
            {
                if (IsInGame())
                {
                    Task.Run(() => FlyHack());
                }
                else
                {
                    FlyHackEnabled = false;
                }
            }
        }
        private static void FlyHack()
        {
            var magicNumber = 131073;
            var speed = 0.1;
            while (FlyHackEnabled && IsInGame())
            {
                var direction = _player.GetDirectionVector();
                var rotatedADirection = _player.RotateDirectionVectorAroundZ(-90);
                var rotatedDDirection = _player.RotateDirectionVectorAroundZ(90);
                _player.GroundState = magicNumber;
                if (KeyBoardManager.IsKeyPressed((int)KeyBoardBase.VirtualKey.SPACE))
                {
                    _player.Z = _player.Z + speed;
                }
                if (KeyBoardManager.IsKeyPressed((int)KeyBoardBase.VirtualKey.KEY_W))
                {
                    _player.X = _player.X + direction.X * speed;
                    _player.Y = _player.Y + direction.Y * speed;
                    _player.Z = _player.Z + direction.Z * speed;
                }
                if (KeyBoardManager.IsKeyPressed((int)KeyBoardBase.VirtualKey.KEY_S))
                {
                    _player.X = _player.X - direction.X * speed;
                    _player.Y = _player.Y - direction.Y * speed;
                    _player.Z = _player.Z - direction.Z * speed;
                }
                if (KeyBoardManager.IsKeyPressed((int)KeyBoardBase.VirtualKey.KEY_A))
                {
                    _player.X = _player.X + rotatedADirection.X * speed;
                    _player.Y = _player.Y + rotatedADirection.Y * speed;
                }
                if (KeyBoardManager.IsKeyPressed((int)KeyBoardBase.VirtualKey.KEY_D))
                {
                    _player.X = _player.X + rotatedDDirection.X * speed;
                    _player.Y = _player.Y + rotatedDDirection.Y * speed;
                }
                if (KeyBoardManager.IsKeyPressed((int)KeyBoardBase.VirtualKey.SHIFT))
                {
                    _player.Z = _player.Z - speed;
                }
            }
            if (!IsInGame())
            {
                FlyHackEnabled = false;
                _prevInGameState = false;
                AddrError?.Invoke();
            }
        }
        private static void SaveCheckpointKeyPressedHandler()
        {
            if (IsInGame())
            {
                _checkpoint = (_player.X, _player.Y, _player.Z + 10);
            }
        }
        private static void TeleportToCheckpointKeyPressed()
        {
            if (IsInGame())
            {
                _player.X = _checkpoint.X;
                _player.Y = _checkpoint.Y;
                _player.Z = _checkpoint.Z;
            }
        }
        public static void Stop()
        {
            _checkpoint = (3803.3, 16388.51, -3469.4);
            FlyHackEnabled = false;
            _prevInGameState = false;
            _disposed = true;
            KeyBoardManager.FlyHackKeyPressed -= FlyHackKeyPressedHandler;
            KeyBoardManager.SaveCheckpointKeyPressed -= SaveCheckpointKeyPressedHandler;
            KeyBoardManager.TeleportToCheckpointKeyPressed -= TeleportToCheckpointKeyPressed;
        }
    }
}
