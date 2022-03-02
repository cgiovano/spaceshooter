using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using SpaceShooter.Library;
using SpaceShooter.Library.Utils;
using System;

namespace SpaceShooter.GameObjects
{
    public enum LaserType
    {
        Player,
        Enemy
    }

    public enum LaserMode
    {
        Standard,
        Disruptor
    }

    public abstract class Laser : GameObject
    {
        public LaserType LaserType { get; protected set; }
        public LaserMode Mode { get; protected set; }
        //public bool IsEnded { get; private set; } = false;

        protected Color _laserColor;
        protected Animation _animation;
        protected double _laserTimer;
        protected int _laserFrameTimer;
        protected int _laserCounter = 1;

        public Laser(Vector2 startPosition, Color laserColor, int energy)
        {
            position = startPosition;
            defaultColor = laserColor;
            Energy = energy;
            Speed = 4f;
            origin = new Vector2(frameWidth / 2, frameHeight / 2);
        }

        public Laser(Vector2 startPosition, Color laserColor, int energy, float speed)
        {
            position = startPosition;
            defaultColor = laserColor;
            Energy = energy;
            Speed = speed;
            origin = new Vector2(frameWidth / 2, frameHeight / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, position, defaultColor);
        }

        public void OutOfScreenHandler()
        {
            if (rectangle.Bottom < 0 || rectangle.Top > Screen.GetHeight ||
                rectangle.Right < 0 || rectangle.Left > Screen.GetWidth)
            {
                IsRemoved = true;
            }
        }
    }

    public class PlayerLaser_A : Laser
    {
        public PlayerLaser_A (Vector2 startPosition, Color laserColor, int energy, float speed) : base (startPosition, laserColor, energy)
        {
            spriteTexture = Textures.PlayerLaser_A_Texture;
            LaserType = LaserType.Player;
            Speed = 4f;

            frameWidth = spriteTexture.Width;
            frameHeight = spriteTexture.Height;
        }

        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth , frameHeight);

            position.Y -= Speed;

            OutOfScreenHandler();
        }
    }

    public class PlayerLaser_B : Laser
    {
        private Direction _direction;
        private float _timer;

        public PlayerLaser_B(Vector2 startPosition, Color laserColor, int energy, Direction direction) : base(startPosition, laserColor, energy)
        {
            spriteTexture = Textures.PlayerLaser_B_Texture;
            LaserType = LaserType.Player;
            defaultColor = laserColor;
            _direction = direction;

            frameWidth = spriteTexture.Width;
            frameHeight = spriteTexture.Height;
        }

        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_direction == Direction.Center)
            {
                position.Y -= Speed;
            }
            else if (_direction == Direction.Right)
            {
                position.Y -= Speed;
                position.X += (Speed / 1) * _timer;
            }
            else if (_direction == Direction.Left)
            {
                position.Y -= Speed;
                position.X -= (Speed / 1) * _timer;
            }

            OutOfScreenHandler();
        }
    }

    public class PlayerLaser_C : Laser
    {
        private float _timer;

        public PlayerLaser_C(Vector2 startPosition, Color laserColor, int energy) : base(startPosition, laserColor, energy)
        {
            spriteTexture = Textures.PlayerLaser_C_Texture;
            LaserType = LaserType.Player;
            defaultColor = laserColor;
            Mode = LaserMode.Disruptor;

            frameWidth = spriteTexture.Width;
            frameHeight = spriteTexture.Height;
        }

        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            position.Y -= Speed * (_timer + 2);

            OutOfScreenHandler();
        }
    }

    public class EnemyLaser_A : Laser
    {
        public EnemyLaser_A(Vector2 startPosition, Color laserColor, int energy) : base(startPosition, laserColor, energy)
        {
            spriteTexture = Textures.EnemyLaser_A_Texture;
            LaserType = LaserType.Enemy;
            defaultColor = laserColor;

            frameWidth = spriteTexture.Width;
            frameHeight = spriteTexture.Height;
        }

        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

            position.Y += Speed;

            OutOfScreenHandler();
        }
    }

    public class EnemyLaser_B : Laser
    {
        private Direction _direction;
        private float _timer;
        private bool _invertDirection = false;
        private float _horizontalSpeed;

        public EnemyLaser_B(Vector2 startPosition, Color laserColor, int energy, Direction direction) : base(startPosition, laserColor, energy)
        {
            spriteTexture = Textures.EnemyLaser_B_Texture;
            LaserType = LaserType.Enemy;
            defaultColor = laserColor;
            _direction = direction;
            _horizontalSpeed = Speed;

            frameWidth = spriteTexture.Width;
            frameHeight = spriteTexture.Height;
        }

        public EnemyLaser_B(Vector2 startPosition, Color laserColor, int energy, Direction direction, Texture2D texture, bool invertDirection) : base(startPosition, laserColor, energy)
        {
            spriteTexture = texture;
            LaserType = LaserType.Enemy;
            defaultColor = laserColor;
            _direction = direction;
            _invertDirection = invertDirection;
            _horizontalSpeed = Speed;
            Speed = -Speed;

            frameWidth = spriteTexture.Width;
            frameHeight = spriteTexture.Height;
        }

        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_direction == Direction.Center)
            {
                position.Y += Speed;
            }
            else if (_direction == Direction.Right)
            {
                position.Y += Speed;
                position.X += (_horizontalSpeed / 3) * _timer;
            }
            else if (_direction == Direction.Left)
            {
                position.Y += Speed;
                position.X -= (_horizontalSpeed / 3) * _timer;
            }

            OutOfScreenHandler();
        }
    }

    public class EnemyLaser_C : Laser
    {
        public EnemyLaser_C (Vector2 startPosition, Color laserColor, int energy) : base (startPosition, laserColor, energy)
        {
            spriteTexture = Textures.Boss_Laser_Texture;
            LaserType = LaserType.Enemy;
            frameWidth = spriteTexture.Width / 3;
            frameHeight = 0;
            _animation = new Animation(2, spriteTexture.Width / 3, 0);
        }

        public override void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);

            _laserTimer += gameTime.ElapsedGameTime.TotalSeconds;
            _laserFrameTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (frameHeight > Textures.Boss_Laser_Texture.Height)
                _laserCounter = -_laserCounter;

            if (_laserFrameTimer > 200)
            {
                frameHeight += _laserCounter;
            }

            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            _animation.SetRectangleHeight(frameHeight);

            if (_animation.GetSourceRectangle.Height < 0)
            {
                IsEnded = true;
            }

            OutOfScreenHandler();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _animation.Draw(spriteBatch, spriteTexture, position, defaultColor, scale, 0, origin);
        }
    }

    public class EnemyLaser_D : Laser
    {
        private float _rotationAngle;
        private float _rotation;
        private Vector2 _direction;
        private int _timer;
        private bool _selfRotation = false;
        private Vector2 _localOrigin;

        public EnemyLaser_D(Vector2 startPosition, Color laserColor, int energy, float rotationAngle) : base(startPosition, laserColor, energy)
        {
            spriteTexture = Textures.EnemyLaser_B_Texture;
            LaserType = LaserType.Enemy;
            _rotationAngle = rotationAngle;
            frameWidth = spriteTexture.Width / 3;
            frameHeight = 0;
            _animation = new Animation(2, spriteTexture.Width / 3, 0);
            _localOrigin = new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2);
        }

        public EnemyLaser_D(Vector2 startPosition, Color laserColor, int energy, float rotationAngle, Texture2D texture, bool selfRotation) : base(startPosition, laserColor, energy)
        {
            spriteTexture = texture;
            LaserType = LaserType.Enemy;
            _rotationAngle = rotationAngle;
            frameWidth = spriteTexture.Width / 3;
            frameHeight = 0;
            _animation = new Animation(2, spriteTexture.Width / 3, 0);

            _selfRotation = selfRotation;
        }

        public EnemyLaser_D(Vector2 startPosition, Color laserColor, int energy, float rotationAngle, int speed) : base(startPosition, laserColor, energy)
        {
            spriteTexture = Textures.EnemyLaser_B_Texture;
            LaserType = LaserType.Enemy;
            _rotationAngle = rotationAngle;
            frameWidth = spriteTexture.Width / 3;
            frameHeight = 0;
            Speed = speed;
            _animation = new Animation(2, spriteTexture.Width / 3, 0);
        }

        public override void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            _timer += gameTime.ElapsedGameTime.Milliseconds;
            _rotation = MathHelper.ToRadians(_rotationAngle);
            _direction = new Vector2((float)Math.Cos(_rotation), -(float)Math.Sin(_rotation));

            position += _direction * Speed;

            OutOfScreenHandler();

            if (_selfRotation)
            {
                rotation += 0.1f;
                _localOrigin = new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(spriteTexture, Position, defaultColor);
            spriteBatch.Draw(spriteTexture, Position, null, Color.White, rotation, _localOrigin, scale, SpriteEffects.None, 0f);
        }
    }
}
