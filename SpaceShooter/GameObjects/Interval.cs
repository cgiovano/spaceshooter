using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.GameObjects
{
    class Interval : GameObject
    {
        private SequenceType _intervalType;
        private Texture2D _transitionBackground;
        private Color _transitionBackgroundColor = new Color(255, 255, 255, 255);
        private int fadeTimer;
        private int _intervalTimer;

        public Interval(SequenceType type) : base()
        {
            spriteTexture = Textures.Ready;
            _intervalType = type;
            _transitionBackground = Textures.TransitionBackground;

            if (type == SequenceType.StartStage)
            {    
                _transitionBackgroundColor.A = 255;
            }
            else if (type == SequenceType.EndStage)
            {
                _transitionBackgroundColor.A = 0;
            }
            else
            {
                _transitionBackgroundColor.A = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            fadeTimer += gameTime.ElapsedGameTime.Milliseconds;
            _intervalTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (_intervalType == SequenceType.StartStage)
            {
                FadeIn();
            }
            
            if (_intervalType == SequenceType.EndStage)
            {
                if (fadeTimer > 1000)
                    FadeOut();
            }

            defaultColor.G -= 10;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_intervalType != SequenceType.EndStage)
                spriteBatch.Draw(spriteTexture, new Vector2(Screen.GetWidth / 2 - Textures.Ready.Width / 2, Screen.GetHeight / 2 - Textures.Ready.Height / 2), defaultColor);

            if (_intervalType == SequenceType.StartStage || _intervalType == SequenceType.EndStage)
                spriteBatch.Draw(_transitionBackground, new Vector2(0, 0), _transitionBackgroundColor);
        }

        private void FadeIn()
        {
            if (_transitionBackgroundColor.A > 0)
                _transitionBackgroundColor.A -= 5;
        }

        private void FadeOut()
        {
            if (_transitionBackgroundColor.A < 255)
                _transitionBackgroundColor.A += 5;
            else
                IsEnded = true;
        }
    }
}
