using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;

namespace SpaceShooter.GameObjects
{
    class Explosion
    {
        public static void Create(Vector2 explosionOrigin)
        {
            int rotationAngle = 24;

            for (int i = 0; i < 15; i++)
            {
                Components.Explosions.Add(new FireParticle(Textures.ExplosionParticle, false, explosionOrigin, i * rotationAngle));
            }

            Sounds.Explosion.Play();
        }

        public static void Update(GameTime gameTime)
        {
            foreach (var particle in Components.Explosions)
            {
                particle.Update(gameTime);
            }

            for (int i = 0; i < Components.Explosions.Count; i++)
            {
                var particle = Components.Explosions[i];

                if (particle.IsRemoved == true)
                {
                    Components.Explosions.RemoveAt(i);
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var particle in Components.Explosions)
            {
                particle.Draw(spriteBatch);
            }
        }
    }
}
