using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Library;

namespace SpaceShooter.GameObjects
{
    public enum LaserType
    {
        Player,
        Enemy
    }

    public class Laser : GameObject
    {
        public LaserType LaserType { get; private set; }

        private Color _laserColor;

        public Laser(Texture2D texture, bool createColorMap, int energy, LaserType laserType, Color laserColor, Vector2 startPosition) : base(texture, createColorMap)
        {
            Energy = energy;
            position = startPosition;
            Speed = 2f;
            LaserType = laserType;
            _laserColor = laserColor;
        }

        public override void Update()
        {
            Rectangle = new Rectangle((int)position.X, (int)position.Y, 1, 3);

            // Controle do laser do inimigo e do jogador.
            switch (LaserType)
            {
                case (LaserType.Player):
                    position.Y -= Speed;
                    break;
                case (LaserType.Enemy):
                    position.Y += Speed;
                    break;
            }

            // Se o laser for alem da visão de camera ele será removido.
            if (rectangle.Bottom < 0)
                IsRemoved = true;
                
            if (rectangle.Top > Screen.GetHeight)
                IsRemoved = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, Position, _laserColor);
        }
    }
}
