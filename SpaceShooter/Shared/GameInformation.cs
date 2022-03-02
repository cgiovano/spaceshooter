using SpaceShooter.GameUtils;
using System;
using System.Collections.Generic;

namespace SpaceShooter.Shared
{
    public static class GameInformation
    {
        public static int EffectsVolume { get; set; }
        public static int MusicVolume { get; set; }
        public static bool FullScreen { get; set; }

        public static int Life { get; set; }
        public static int Lives { get; set; }
        public static int Points { get; set; }
        public static Level CurrentLevel { get; set; }
    }
}
