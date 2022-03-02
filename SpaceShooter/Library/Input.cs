using Microsoft.Xna.Framework.Input;

namespace SpaceShooter.Library
{
    public static class Input
    {
        public static Keys Up { get; set; } = Keys.W; // Default is Up Arrow key
        public static Keys Down { get; set; } = Keys.S; // Default is Down Arrow key
        public static Keys Left { get; set; } = Keys.A; // Default is Left Arrow key
        public static Keys Right{ get; set; } = Keys.D; // Default is Right Arrow key
        public static Keys Shoot { get; set; } = Keys.N; // Default is Space key
        public static Keys Pause { get; set; } = Keys.P;
    }
}
