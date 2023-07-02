namespace OnlyUpCheat.Core
{
    internal static class Config
    {
        public static string ExeName = "OnlyUP-Win64-Shipping";
        public static string ModuleName = $"{ExeName}.exe";

        public static (string Letter, int VirtualKey) FlyHackEnableDisableHotkey = ("F", 0x46);
        public static (string Letter, int VirtualKey) SaveCheckpointHotkey = ("H", 0x48);
        public static (string Letter, int VirtualKey) TeleportToCheckpointHotkey = ("T", 0x54);

        public static (int Pointer, int[] Offsets) InGameState = (0x07862560, new int[] { });
        public static (int Pointer, int[] Offsets) PlayerXCoord = (0x074685C0, new int[] { 0x48, 0x6B0, 0xA0, 0x260 });
        public static (int Pointer, int[] Offsets) PlayerYCoord = (0x074685C0, new int[] { 0x48, 0x6B0, 0xA0, 0x268 });
        public static (int Pointer, int[] Offsets) PlayerZCoord = (0x074685C0, new int[] { 0x48, 0x6B0, 0xA0, 0x270 });
        public static (int Pointer, int[] Offsets) CameraPitch = (0x07872B00, new int[] { 0x30, 0x310 });
        public static (int Pointer, int[] Offsets) CameraYaw = (0x07872B00, new int[] { 0x30, 0x308 });
        public static (int Pointer, int[] Offsets) GroundState = (0x074685C0, new int[] { 0x48, 0x6B0, 0x1A4 });
    }
}
