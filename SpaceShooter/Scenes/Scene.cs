using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.Scenes
{
    public interface IScene
    {
        void Update(GameTime gameTime);

        void Update(BaseGame game);

        void Update(BaseGame game, GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);
    }
}
