using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SpaceShooter.GameUtils
{
    class Textures
    {
        public static readonly Texture2D Button;

        public static readonly Texture2D Star;
        public static readonly Texture2D PlayerShip;
        public static readonly Texture2D SpaceShipTextureCollision;
        public static readonly Texture2D Asteroid;
        public static readonly Texture2D AsteroidTextureCollision;
        public static readonly Texture2D Laser;
        public static readonly Texture2D Satelite;
        public static readonly Texture2D ExplosionParticle;

        public static readonly Texture2D Shield;
        public static readonly Texture2D Energy;
        public static readonly Texture2D Life;

        public static readonly Texture2D Enemy_A;
        public static readonly Texture2D Enemy_B;
        public static readonly Texture2D Enemy_C;
        public static readonly Texture2D Enemy_D;
        public static readonly Texture2D Enemy_E;

        public static readonly Texture2D Boss_A;
        public static readonly Texture2D Boss_B;
        public static readonly Texture2D Boss_C;
        public static readonly Texture2D Boss_D;
        public static readonly Texture2D Boss_E;

        public static readonly Texture2D Weakness_A;
        public static readonly Texture2D Weakness_B;
        public static readonly Texture2D Weakness_C;
        public static readonly Texture2D Weakness_D;
        public static readonly Texture2D Weakness_E;

        public Textures(ContentManager content)
        {
            typeof(Textures).GetField("Button").SetValue(Button, content.Load<Texture2D>("Assets\\Images\\Button"));
            typeof(Textures).GetField("Star").SetValue(Star, content.Load<Texture2D>("Assets\\Images\\Star"));
            typeof(Textures).GetField("PlayerShip").SetValue(PlayerShip, content.Load<Texture2D>("Assets\\Images\\SpaceShip"));
            typeof(Textures).GetField("Laser").SetValue(Laser, content.Load<Texture2D>("Assets\\Images\\Laser"));
            typeof(Textures).GetField("Asteroid").SetValue(Asteroid, content.Load<Texture2D>("Assets\\Images\\asteroid"));
            typeof(Textures).GetField("SpaceShipTextureCollision").SetValue(SpaceShipTextureCollision, content.Load<Texture2D>("Assets\\Images\\spaceShipTextureCollision"));
            typeof(Textures).GetField("ExplosionParticle").SetValue(ExplosionParticle, content.Load<Texture2D>("Assets\\Images\\ExplosionParticle"));

            //typeof(Textures).GetField("Shield").SetValue(Shield, content.Load<Texture2D>("Assets\\Images\\Shield"));
            //typeof(Textures).GetField("Energy").SetValue(Energy, content.Load<Texture2D>("Assets\\Images\\Energy"));
            //typeof(Textures).GetField("Life").SetValue(Life, content.Load<Texture2D>("Assets\\Images\\Life"));

            typeof(Textures).GetField("Enemy_A").SetValue(Enemy_A, content.Load<Texture2D>("Assets\\Images\\Enemy1"));
            //typeof(Texturas).GetField("Inimigo_B").SetValue(Inimigo_B, gerenciadorDeConteudo.Load<Texture2D>("Assets\\Images\\Inimigo_B"));
            //typeof(Texturas).GetField("Inimigo_C").SetValue(Inimigo_C, gerenciadorDeConteudo.Load<Texture2D>("Assets\\Images\\Inimigo_C"));
            //typeof(Texturas).GetField("Inimigo_D").SetValue(Inimigo_D, gerenciadorDeConteudo.Load<Texture2D>("Assets\\Images\\Inimigo_D"));
            //typeof(Texturas).GetField("Inimigo_E").SetValue(Inimigo_E, gerenciadorDeConteudo.Load<Texture2D>("Assets\\Images\\Inimigo_E"));

            //typeof(Textures).GetField("Boss_A").SetValue(Enemy_A, content.Load<Texture2D>("Assets\\Images\\Boss_A"));
            //typeof(Textures).GetField("Boss_B").SetValue(Enemy_B, content.Load<Texture2D>("Assets\\Images\\Boss_B"));
            //typeof(Textures).GetField("Boss_C").SetValue(Enemy_C, content.Load<Texture2D>("Assets\\Images\\Boss_C"));
            //typeof(Textures).GetField("Boss_D").SetValue(Enemy_D, content.Load<Texture2D>("Assets\\Images\\Boss_D"));
            //typeof(Textures).GetField("Boss_E").SetValue(Enemy_E, content.Load<Texture2D>("Assets\\Images\\Boss_E"));

            //typeof(Textures).GetField("Weakness_A").SetValue(Weakness_A, content.Load<Texture2D>("Assets\\Images\\Weakness_A"));
            //typeof(Textures).GetField("Weakness_B").SetValue(Weakness_B, content.Load<Texture2D>("Assets\\Images\\Weakness_B"));
            //typeof(Textures).GetField("Weakness_C").SetValue(Weakness_C, content.Load<Texture2D>("Assets\\Images\\Weakness_C"));
            //typeof(Textures).GetField("Weakness_D").SetValue(Weakness_D, content.Load<Texture2D>("Assets\\Images\\Weakness_D"));
            //typeof(Textures).GetField("Weakness_E").SetValue(Weakness_E, content.Load<Texture2D>("Assets\\Images\\Weakness_E"));
        }
	}
}