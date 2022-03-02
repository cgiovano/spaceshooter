using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Shared;

namespace SpaceShooter.GameObjects
{
    internal enum OneUpType
    {
        Life, 
        Repair, 
        Shield
    }

    public class OneUp : GameObject
    {
        private int _timer;
        private int _spawnTime = 30000;
        private OneUpType _oneUpType;

        public OneUp (Vector2 pos) : base()
        {
            if (Components.Player.Lives == 1)
                _oneUpType = OneUpType.Life;
            else
            {
                if (Components.Player.Energy <= 30)
                    _oneUpType = OneUpType.Repair;
                else
                    _oneUpType = RandomMod.RandomBool() ? OneUpType.Life : OneUpType.Shield;
            }

            switch(_oneUpType)
            {
                case (OneUpType.Life):
                    spriteTexture = Textures.LifeOneUp;
                    break;
                case (OneUpType.Repair):
                    spriteTexture = Textures.RepairOneUp;
                    break;
                case (OneUpType.Shield):
                    spriteTexture = Textures.ShieldOneUp;
                    break;
            }

            position = pos; //new Vector2(RandomMod.GetRandom(Screen.GetWidth), -spriteTexture.Height);
        }

        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, spriteTexture.Width, spriteTexture.Height);
            position.Y += Speed;

            if (rectangle.Intersects(Components.Player.Rectangle))
            {
                switch (_oneUpType)
                {
                    case (OneUpType.Repair):
                        Components.Player.Energy = 100;
                        break;
                    case (OneUpType.Life):
                        MainGame.InGameLives++;
                        Components.Player.Lives = MainGame.InGameLives;
                        break;
                    case (OneUpType.Shield):
                        Components.Player.CreateShield();
                        break;
                }

                IsRemoved = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, position, Color.White);
        }
    }
}
