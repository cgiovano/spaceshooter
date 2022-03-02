using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter.GameUtils
{
    class Fonts
    {
        public static SpriteFont Joystix;

        public Fonts(ContentManager content)
        {
            typeof(Fonts).GetField("Joystix").SetValue(Joystix, content.Load<SpriteFont>("Assets\\Fonts\\Font"));
        }
    }
}
