using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameUtils;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Library;
using SpaceShooter.GameObjects;
using SpaceShooter.GameObjects.Enemies;

namespace SpaceShooter
{
    public enum BattleType
    {
        Boss,
        Default
    }

	public class MainGame
	{
        private int _enemiesDestroyed = 0;
        private int _obstaclesDestroyed = 0;
        private int _obstaclesTimer;
        private int _starsTimer;
        private int _EnemiesTimer;

        private Wave wave;

        private bool _primeiraVez = true;

        private BattleType _battleType = BattleType.Default;

        private int _spawnTimer = 0;

        WaveType[] waves = { WaveType.Grid, WaveType.Asteroid, WaveType.Rush};
        int counter = 0;

        public MainGame(Level level)
		{
            CurrentLevel.SetLevel(level);
            LoadComponents();
		}

		private void LoadComponents()
		{

            var random = new Random();
            Components.CreateComponents();

            LevelConfiguration.CreateLevel(CurrentLevel.GetLevel);
		}

        public void UpdateComponents(GameTime gameTime)
        {


            if (_obstaclesDestroyed > LevelConfiguration.ObstaclesToDestroy && _enemiesDestroyed > LevelConfiguration.EnemiesToDestroy)
            {
                BossBattle();

                _battleType = BattleType.Boss;

                _obstaclesDestroyed = 0;
                _enemiesDestroyed = 0;
            }

            if (_battleType == BattleType.Boss && Components.Boss.IsRemoved == false)
            {
                _starsTimer += gameTime.ElapsedGameTime.Milliseconds;

                StarsUpdate();
                BossUpdate(gameTime);
                PlayerUpdate(gameTime);
                RushUpdate(gameTime);
                GridUpdate(gameTime);
                LaserUpdate();
                ObstaclesUpdate(gameTime);
                ExplosionUpdate(gameTime);

                if (Components.Boss.IsRemoved == true)
                {
                    _battleType = BattleType.Default;
                    CurrentLevel.SetLevel(CurrentLevel.GetLevel + 1);
                    LevelConfiguration.CreateLevel(CurrentLevel.GetLevel);
                    Components.Boss = null;
                }
            }
            else if (_battleType == BattleType.Default)
            {
                _obstaclesTimer += gameTime.ElapsedGameTime.Milliseconds;
                _starsTimer += gameTime.ElapsedGameTime.Milliseconds;
                _EnemiesTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (wave == null)
                {
                    CreateWave();
                }
                else
                {
                    switch(wave.Type)
                    {
                        case (WaveType.Grid):
                            GridUpdate(gameTime);
                            break;
                        case (WaveType.Rush):
                            RushUpdate(gameTime);
                            break;
                        case (WaveType.Bonus):
                            //
                            break;
                        case (WaveType.Asteroid):
                            ObstaclesUpdate(gameTime);
                            break;
                        case (WaveType.Comet):
                            //wave = new Wave(WaveType.Comet);
                            break;
                    }
                }

                StarsUpdate();
                //ObstaclesUpdate(gameTime);
                PlayerUpdate(gameTime);
                //EnemyUpdate(gameTime);
                LaserUpdate();
                ExplosionUpdate(gameTime);
            }
        }

        void CreateWave()
        {
            //var waveType = waves[counter];
            var waveType = WaveType.Asteroid;
            counter++;

            switch(waveType)
            {
                case (WaveType.Grid):
                    wave = new Wave(WaveType.Grid, 10, 3);
                    break;
                case (WaveType.Rush):
                    wave = new Wave(WaveType.Rush);
                    break;
                case (WaveType.Bonus):
                    wave = new Wave(WaveType.Bonus);
                    break;
                case (WaveType.Asteroid):
                    wave = new Wave(WaveType.Asteroid);
                    break;
                case (WaveType.Comet):
                    wave = new Wave(WaveType.Comet);
                    break;
            }

        }

        public void DrawComponents(SpriteBatch spriteBatch)
        {
            if (Components.Player != null && Components.Player.IsDestroyed == true)
            {
                spriteBatch.DrawString(Fonts.Joystix, "Game Over\nPress \"space\" to start again", new Vector2(Screen.GetWidth / 2, Screen.GetHeight / 2), Color.White);

                foreach (var star in Components.Stars)
                    star.Draw(spriteBatch);
            }
            
            else if (Components.Stars != null && Components.Explosions != null && Components.Obstacles != null && Components.Lasers != null)
            {
                foreach (var star in Components.Stars)
                    star.Draw(spriteBatch);

                foreach (var obstacle in Components.Obstacles)
                    obstacle.Draw(spriteBatch);

                foreach (var inimigo in Components.Enemies)
                    inimigo.Draw(spriteBatch);

                if (Components.Boss != null)
                    Components.Boss.Draw(spriteBatch);

                Components.Player.Draw(spriteBatch);

                foreach (var laser in Components.Lasers)
                    laser.Draw(spriteBatch);

                Explosion.Draw(spriteBatch);

                if (GameState.currentState == GameState.State.Paused)
                    spriteBatch.DrawString(Fonts.Joystix, "PAUSED", new Vector2(Screen.GetWidth/2 - 30, Screen.GetHeight/2 - 15), Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
                else
                {
                    spriteBatch.DrawString(Fonts.Joystix, "Score: " + Information.Points.ToString(), new Vector2(10, 10), Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                    //spriteBatch.DrawString(Fonts.Joystix, "tst", new Vector2(10, 10), Color.White, 0, Vector2.Zero, 0.3f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(Fonts.Joystix, "Life: " + Information.Life.ToString(), new Vector2(Screen.GetWidth - 150, 10), Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(Fonts.Joystix, "Level: " + CurrentLevel.GetLevel.ToString(), new Vector2(Screen.GetWidth / 2, 30), Color.LightSeaGreen, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(Fonts.Joystix, "Obstacles Destroyed: " + _obstaclesDestroyed, new Vector2(Screen.GetWidth / 2, 50), Color.LightSeaGreen, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                }
            }
        }

        private void ObstaclesUpdate(GameTime gameTime)
        {
            if (Components.Obstacles != null)
            {
                /*if (_obstaclesTimer > LevelConfiguration.ObstacleSpawnInterval)
                {
                    Components.Obstacles.Add(new Obstacle(Textures.Asteroid, true));
                    _obstaclesTimer = 0;
                }*/

                if (!wave.IsEnded)
                    wave.Update(gameTime);

                foreach (var obstacle in Components.Obstacles)
                    obstacle.Update(gameTime);

                for (int i = 0; i < Components.Obstacles.Count; i++)
                {
                    var obstacle = Components.Obstacles[i];

                    if (obstacle.IsRemoved == true)
                        Components.Obstacles.RemoveAt(i);

                    if (obstacle.IsDestroyed == true)
                    {
                        Explosion.Create(new Vector2(obstacle.Position.X + Textures.Asteroid.Width / 2, obstacle.Position.Y + Textures.Asteroid.Height / 2));

                        //int rotationAngle = 18;
                        //Vector2 origin = new Vector2(obstacle.Position.X + Textures.Asteroid.Width / 2, obstacle.Position.Y + Textures.Asteroid.Height / 2);

                        //for (int j = 0; j < 20; j++)
                        //{
                        //    Components.Explosions.Add(new FireParticle(Textures.ExplosionParticle, false, origin, j * rotationAngle));
                        //}

                        Components.Obstacles.RemoveAt(i);
                        _obstaclesDestroyed += 1;
                        Information.Points += 10;    // Preciso mudar isso
                    }

                    if (_obstaclesDestroyed == 5)
                    {
                        wave.IsEnded = true;
                        _obstaclesDestroyed = 0;
                    }
                }

                if (wave.IsEnded && Components.Obstacles.Count < 1)
                {
                    wave = null;
                }
            }
        }

        private void StarsUpdate()
        {
            if (Components.Stars != null)
            {
                if (_primeiraVez == true)
                {
                    for (int i = 0; i < 200; i++)
                        Components.Stars.Add(new Star(Textures.Star, false, _primeiraVez));

                    _primeiraVez = false;
                }
                else if (_starsTimer > 50)
                {
                    Components.Stars.Add(new Star(Textures.Star, false, _primeiraVez));
                    _starsTimer = 0;
                }

                foreach (var star in Components.Stars)
                    star.Update();

                for (int i = 0; i < Components.Stars.Count; i++)
                {
                    var star = Components.Stars[i];

                    if (star.IsRemoved == true)
                        Components.Stars.RemoveAt(i);
                }
            }
        }

        private void RushUpdate(GameTime gameTime)
        {
            if (_EnemiesTimer > LevelConfiguration.EnemySpawnInterval)
            {
                Components.Enemies.Add(new Enemy(Textures.Enemy_A, true));
                _EnemiesTimer = 0;
            }

            foreach (var enemy in Components.Enemies)
                enemy.Update(gameTime);

            for (int i = 0; i < Components.Enemies.Count; i++)
            {
                var enemy = Components.Enemies[i];

                if (enemy.IsRemoved == true)
                    Components.Enemies.RemoveAt(i);

                if (enemy.IsDestroyed == true)
                {
                    Vector2 origin = new Vector2(enemy.Position.X + Textures.Enemy_A.Width / 2, enemy.Position.Y + Textures.Enemy_A.Height / 2);

                    Explosion.Create(origin);

                    Components.Enemies.RemoveAt(i);
                    _enemiesDestroyed += 1;
                    Information.Points += 10;    // Preciso mudar isso
                }
            }
        }

        private void GridUpdate(GameTime gameTime)
        {
            if (Components.Enemies != null)
            {
                wave.Update(gameTime);

                foreach (var enemy in Components.Enemies)
                    enemy.Update(gameTime);

                for (int i = 0; i < Components.Enemies.Count; i++)
                {
                    var enemy = Components.Enemies[i];

                    if (enemy.IsRemoved == true)
                        Components.Enemies.RemoveAt(i);

                    if (enemy.IsDestroyed == true)
                    {
                        Vector2 origin = new Vector2(enemy.Position.X + Textures.Enemy_A.Width / 2, enemy.Position.Y + Textures.Enemy_A.Height / 2);

                        Explosion.Create(origin);

                        Components.Enemies.RemoveAt(i);
                        _enemiesDestroyed += 1;
                        Information.Points += 10;    // Preciso mudar isso
                    }
                }

                if (_enemiesDestroyed == 5)
                {
                    wave.IsEnded = true;
                    _enemiesDestroyed = 0;
                }
            }

            if (wave.IsEnded && Components.Enemies.Count < 1)
                wave = null;
        }

        private void PlayerUpdate(GameTime gameTime)
        {
            if (Components.Player != null)
            {
                if (Components.Player.IsDestroyed == true)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        Game1.NewGame = true;
                    else
                        return;
                }
                else
                {
                    Components.Player.Update(gameTime);
                }

                foreach (var laser in Components.Lasers)
                    laser.Update();
            }
        }

        private void LaserUpdate()
        {
            if (Components.Lasers != null)
            {
                foreach (var laser in Components.Lasers)
                    laser.Update();

                for (int i = 0; i < Components.Lasers.Count; i++)
                {
                    var laser = Components.Lasers[i];

                    if (laser.IsRemoved == true)
                        Components.Lasers.RemoveAt(i);
                }
            }
        }

        private void ExplosionUpdate(GameTime gameTime)
        {
            Explosion.Update(gameTime);
        }

        private void OneUpUpdate(GameTime gameTime)
        {

        }

        private void BossUpdate(GameTime gameTime)
        {
            Components.Boss.Update(gameTime);
        }

        private void BossBattle()
        {
            //Components.Boss = new Boss(false);
            //_battleType = BattleType.Boss;
        }
    }
}
