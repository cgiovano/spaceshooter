using Microsoft.Xna.Framework;
using SpaceShooter.GameObjects;
using SpaceShooter.GameObjects.Enemies;

namespace SpaceShooter.GameUtils
{
    class Wave
    {
        public bool IsEnded { get; set; }
        public WaveType Type { get { return (_type); } }

        WaveType _type;
        int _spawnTimer = 0;
        Vector2[,] _grid;
        int i = 0;
        int j = 0;
        private GameTime _gameTime;

        public Wave(WaveType type)
        {
            _type = type;
        }

        public Wave(WaveType type, int gridColumns, int gridLines)
        {
            _type = type;

            if (type == WaveType.Grid)
                _grid = Grid.CreateGrid(gridColumns, gridLines);
        }

        public void Update(GameTime gameTime)
        {
            _gameTime = gameTime;

            switch (_type)
            {
                case (WaveType.Grid):
                    GridEnemiesUpdate();
                    break;
                case (WaveType.Rush):
                    RushEnemiesUpdate();
                    break;
                case (WaveType.Bonus):
                    BonusEnemiesUpdate();
                    break;
                case (WaveType.Asteroid):
                    AsteroidsUpdate();
                    break;
                case (WaveType.Comet):
                    CometsUpdate();
                    break;
            }
        }

        public void RushEnemiesUpdate()
        {
            _spawnTimer += _gameTime.ElapsedGameTime.Milliseconds;

            if (Components.Enemies != null)
            {
                if (_spawnTimer > LevelConfiguration.EnemySpawnInterval)
                {
                    Components.Enemies.Add(new Enemy(Textures.Enemy_A, true));
                    _spawnTimer = 0;
                }
            }
        }

        public void GridEnemiesUpdate()
        {
            _spawnTimer += _gameTime.ElapsedGameTime.Milliseconds;

            if (_spawnTimer > LevelConfiguration.Enemy2SpawnInterval)
            {
                if (i < _grid.GetLength(0))
                {
                    if (j < _grid.GetLength(1))
                    {
                        Components.Enemies.Add(new Enemy2(Textures.Enemy_A, true, _grid[i, j]));
                        j++;
                    }
                    else
                    {
                        i++;
                        j = 0;
                    }
                }
                else
                {
                    return;
                }

                _spawnTimer = 0;
            }
        }

        public void BonusEnemiesUpdate()
        {
            _spawnTimer += _gameTime.ElapsedGameTime.Milliseconds;

            if (Components.Enemies != null)
            {
                if (_spawnTimer > LevelConfiguration.Enemy2SpawnInterval)
                {
                    Components.Enemies.Add(new Enemy(Textures.Enemy_A, true));
                    _spawnTimer = 0;
                }
            }
        }

        public void AsteroidsUpdate()
        {
            _spawnTimer += _gameTime.ElapsedGameTime.Milliseconds;

            if (Components.Obstacles != null)
            {
                if (_spawnTimer > LevelConfiguration.ObstacleSpawnInterval)
                {
                    Components.Obstacles.Add(new Obstacle(Textures.Asteroid, true));
                    _spawnTimer = 0;
                }
            }
        }

        public void CometsUpdate()
        {
            _spawnTimer += _gameTime.ElapsedGameTime.Milliseconds;

            if (Components.Obstacles != null)
            {
                if (_spawnTimer > LevelConfiguration.ObstacleSpawnInterval)
                {
                    Components.Obstacles.Add(new Obstacle(Textures.Asteroid, true));
                    _spawnTimer = 0;
                }
            }
        }

        public void BossUpdate(GameTime gameTime)
        {

        }
    }
}
