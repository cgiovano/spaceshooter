using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.GameObjects;
using SpaceShooter.GameObjects.Enemies;
using SpaceShooter.Library;
using SpaceShooter.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Principal;

namespace SpaceShooter.GameUtils
{
    enum ArrivalType
    {
        Single,
        Double
    }

    enum SpawnFrom
    {
        Left, 
        Right
    }

    /*
     * No lugar de wave type deve ser um objeto "sequencia ou sequence", sendo que nele, deverá conter o tipo de onda
     * e as configurações da mesma, sobretudo no caso das ondas de grid e bonus. Ou seja, no caso de uma onda grid
     * se a chegada dos inimidos se dara de uma só vez ou se será por partes, de qual lado os inimigos vão surgir.
     * 
     * No caso das ondas bonus deverá ser especificado qual o número de inimigos na onda, de quais tipos serão os
     * inimigos. por exemplo se numa onda os inimigos serão apenas de um tipo, de dois tipos ou de qualquer tipo.
     * Além disso também ver os inimigos kamikaze que no final da lista de inimigos ele deverá sair e atacar o player.
     * além disso na sequencia bonus devera ser especificada se ela será unica ou dupla. caso seja unica especificar
     * de qual lado os inimigos vão spawnar.
     * */

    class Sequence
    {
        public bool IsEnded { get; set; }
        public SequenceType Type { get { return (_type); } }
        public int GridEnemiesToSpawn { get { return (listOfArrivals.Length); } }

        public int OneUpTrigger { get; private set; }

        SequenceType _type;
        int _spawnTimer = 0;
        List<Vector2> _grid;
        List<GridEnemyHelper> gridEnemyDisposition;
        List<GridEnemyHelper> ArrivalList;

        int[][] bonusEnemiesArray = new int[6][]; 

        int counter = 0;
        int i = 0;
        int j = 0;
        private int[] _kamikazeDisposition;
        private GameTime _gameTime;
        int bonusCounter = 0;
        GridEnemyHelper[,] listOfArrivals;
        List<int> listTypeOfArrival = new List<int>();

        private List<bool> _canAttackPattern;
        private Texture2D[] AsteroidTextureList = { Textures.AsteroidSmallSizeTexture, Textures.AsteroidMedSizeTexture, Textures.AsteroidBigSizeTexture };
        private Texture2D[] _bonusEnemiesTextureList = { Textures.Enemy_A_Texture, Textures.Enemy_B_Texture, Textures.Enemy_D_Texture, Textures.Enemy_E_Texture, Textures.Enemy_F_Texture };

        private Point2D[][] _listOfBonusPatterns = LevelConfiguration.GetPattern(); // = new Point2D[6][] {Patterns.BonusStage1_A, Patterns.BonusStage1_A, Patterns.BonusStage1_B, Patterns.BonusStage1_B, Patterns.BonusStage1_A, Patterns.BonusStage1_A };

        private int[,] _arrivePattern = new int[,] { { } };

        private Point2D[] pointstwo = new Point2D[]{new Point2D(-40, 300), new Point2D(-40, 300), new Point2D(250, 300), new Point2D(370, 200), new Point2D(400, 100),
                      new Point2D(480, 250), new Point2D(350, 400), new Point2D(280, 500), new Point2D(500, 550), new Point2D(700, 100) };
        private int _intervalTimer;

        private Interval _interval;

        public int RushEnemiesDestroyed { get; set; }
        public int GridEnemiesDestroyed { get; set; }
        public int BonusEnemiesDestroyed { get; set; }
        public int ObstaclesDestroyed { get; set; }

        public Sequence(SequenceType type)
        {
            _type = type;
            _interval = new Interval(type);

            LevelConfiguration.CreateLevel(GameInformation.CurrentLevel);

            if (type == SequenceType.Bonus)
            {
                BonusDisposition();

                OneUpTrigger = RandomMod.GetRandom(bonusEnemiesArray.Length);
            }

            if (type == SequenceType.Grid)
            {
                _grid = Grid.CreateGrid();
                gridEnemyDisposition = Grid.GridEnemyDisposition();
                gridEnemyDisposition.ShuffleCrypto();
                _canAttackPattern = LevelConfiguration.GetCanAttackPattern(GameInformation.CurrentLevel);
                GridDisposition();

                OneUpTrigger = RandomMod.GetRandom(gridEnemyDisposition.Count - 1);
            }

            if (type == SequenceType.Rush)
            {
                OneUpTrigger = RandomMod.GetRandom(1, LevelConfiguration.RushEnemiesToDestroy);
            }
            if (type == SequenceType.Asteroid)
            {
                OneUpTrigger = RandomMod.GetRandom(1, LevelConfiguration.ObstaclesToDestroy);
            }

            if (type == SequenceType.Boss)
            {
                switch (GameInformation.CurrentLevel)
                {
                    case (Level.Level_1):
                        Components.Boss.Add(new FinalBoss_Stage1());
                        break;
                    case (Level.Level_2):
                        Components.Boss.Add(new FinalBoss_Stage2());
                        break;
                    case (Level.Level_3):
                        Components.Boss.Add(new FinalBoss_Stage3());
                        break;
                    case (Level.Level_4):
                        Components.Boss.Add(new FinalBoss_Stage4());
                        break;
                    case (Level.Level_5):
                        Components.Boss.Add(new FinalBoss_Stage5());
                        break;
                }
            }

            if (type == SequenceType.Interval || type == SequenceType.StartStage || type == SequenceType.EndStage)
            {
                Components.UserInterfaceElements.Add(new Interval(type));
            }
        }

        public Sequence(SequenceType type, int gridColumns, int gridLines)
        {
            _type = type;

            if (type == SequenceType.Grid)
            {
                _grid = Grid.CreateGrid();
                gridEnemyDisposition = Grid.GridEnemyDisposition();
                gridEnemyDisposition.ShuffleCrypto();
                _canAttackPattern = LevelConfiguration.GetCanAttackPattern(GameInformation.CurrentLevel);
                GridDisposition();
            }
        }

        public List<Point2D> GetArrivePattern(int ngen)
        {
            if (ngen % 2 == 0)
                return (Patterns.ArrivePattern_A);
            else
                return (Patterns.ArrivePattern_B);
        }

        public void BonusDisposition()
        {
            for (int i = 0; i < 6;)
            {
                if (i == 0)
                {
                    bonusEnemiesArray[i] = BonusDispositionHelper();
                    bonusEnemiesArray[i + 1] = bonusEnemiesArray[i];

                    i += 2;
                }
                else
                {
                    bonusEnemiesArray[i] = BonusDispositionHelper();

                    i++;
                }
            }
        }

        private int[] BonusDispositionHelper()
        {
            int typeOfArrival = RandomMod.GetRandom(0, 3);

            List<int> bonusArrival = new List<int>();
            int texA;
            int texB;

            // Alternate Enemy
            if (typeOfArrival == 0)
            {
                if ((int)GameInformation.CurrentLevel < 3)
                {
                    texA = RandomMod.GetRandom(0, 3);
                    texB = RandomMod.GetRandom(0, 3);
                }
                else
                {
                    texA = RandomMod.GetRandom(0, 5);
                    texB = RandomMod.GetRandom(0, 5);
                }

                for (int j = 0; j < 8; j++)
                {
                    if (j % 2 == 0)
                        bonusArrival.Add(texA);
                    else
                        bonusArrival.Add(texB);
                }
            }

            // Single Enemy
            if (typeOfArrival == 1)
            {
                if ((int)GameInformation.CurrentLevel < 3)
                    texA = RandomMod.GetRandom(0, 3);
                else
                    texA = RandomMod.GetRandom(0, 5);

                for (int j = 0; j < 8; j++)
                {
                    bonusArrival.Add(texA);
                }
            }

            // Step (of 4 in this case) 
            if (typeOfArrival == 2)
            {
                if ((int)GameInformation.CurrentLevel < 3)
                {
                    texA = RandomMod.GetRandom(0, 3);
                    texB = RandomMod.GetRandom(0, 3);
                }
                else
                {
                    texA = RandomMod.GetRandom(0, 5);
                    texB = RandomMod.GetRandom(0, 5);
                }

                for (int j = 0; j < 8; j++)
                {
                    if (j < 4)
                        bonusArrival.Add(texA);
                    else
                        bonusArrival.Add(texB);
                }
            }

            return (bonusArrival.ToArray());
        }

        public void GridDisposition()
        {
            ArrivalList = new List<GridEnemyHelper>();
            int step = 8;
            int totalNumberOfArrivals = gridEnemyDisposition.Count / step + 1;

            _kamikazeDisposition = new int[totalNumberOfArrivals];

            //int numberOfKamikazeEnemiesByLevel = (int)GameInformation.CurrentLevel + 1;

            // List for wich arrival the kamikaze enemies will be appear. 
            if ((int)GameInformation.CurrentLevel > 1)
            {
                for (int l = 0; l < totalNumberOfArrivals; l++)
                {
                    _kamikazeDisposition[l] = RandomMod.GetRandom(0, (int)GameInformation.CurrentLevel + 1);
                }
            }

            listOfArrivals = new GridEnemyHelper[totalNumberOfArrivals, step + (int)GameInformation.CurrentLevel];

            //GridEnemyHelper[] geh = gridEnemyDisposition.ToArray();
            int index = 0;

            for (int i = 0; i < totalNumberOfArrivals; i++)
            {
                if (i < totalNumberOfArrivals - 1)
                {
                    for (int j = 0; j < step; j++)
                    {
                        listOfArrivals[i, j] = gridEnemyDisposition[index];
                        index++;
                    }
                }
                else
                {
                    for (int j = 0; j < gridEnemyDisposition.Count % step; j++)
                    {
                        listOfArrivals[i, j] = gridEnemyDisposition[index];
                        index++;
                    }
                }
            }


            for (int l = 0; l < _kamikazeDisposition.Length; l++)
            {
                if (_kamikazeDisposition[l] != 0)
                {
                    for (int k = 0; k < _kamikazeDisposition[i]; k++)
                    {
                        listOfArrivals[l, step + k] = new GridEnemyHelper(EnemyType.Kamikaze);
                    }
                }
            }

            // Determines if the current arrival will be spawn from right, left or both
            for (int i = 0; i < listOfArrivals.GetLength(0); i++)
            {
                int rng;

                if (i < listOfArrivals.GetLength(0) - 3)
                    rng = RandomMod.GetRandom(1, 4);
                else
                    rng = RandomMod.GetRandom(1, 3);

                listTypeOfArrival.Add(rng);
            }
        }

        public void Update(GameTime gameTime)
        {
            _gameTime = gameTime;

            switch (_type)
            {
                case (SequenceType.Grid):
                    GridEnemiesUpdate();
                    break;
                case (SequenceType.Rush):
                    RushEnemiesUpdate();
                    break;
                case (SequenceType.Bonus):
                    BonusEnemiesUpdate();
                    break;
                case (SequenceType.Asteroid):
                    AsteroidsUpdate();
                    break;
                case (SequenceType.Interval):
                    IntervalUpdate();
                    break;
                case (SequenceType.StartStage):
                    IntervalUpdate();
                    break;
                case (SequenceType.EndStage):
                    IntervalUpdate();
                    break;
                case (SequenceType.Boss):
                    BossUpdate();
                    break;
            }
        }

        private void BossUpdate()
        {
            foreach (var boss in Components.Boss)
            {
                boss.Update(_gameTime);
            }

            if (Components.Boss.Count <= 0)
                IsEnded = true;
        }

        public void RushEnemiesUpdate()
        {
            _spawnTimer += _gameTime.ElapsedGameTime.Milliseconds;

            if (Components.Enemies != null)
            {
                if (_spawnTimer > LevelConfiguration.RushEnemySpawnInterval)
                {
                    int rndEnemy = RandomMod.GetRandom(0, 4);

                    switch (rndEnemy)
                    {
                        case (0):
                            Components.Enemies.Add(new RushDragonflyEnemy());
                            break;
                        case (1):
                            Components.Enemies.Add(new RushSpyShipEnemy());
                            break;
                        case (2):
                            Components.Enemies.Add(new RushFlagShipEnemy());
                            break;
                        case (3):
                            Components.Enemies.Add(new RushSatelliteEnemy());
                            break;
                    }

                    
                    _spawnTimer = 0;
                }
            }

            if (RushEnemiesDestroyed > LevelConfiguration.RushEnemiesToDestroy)
            {
                IsEnded = true;
                RushEnemiesDestroyed = 0;
            }
        }

        public void GridEnemiesUpdate()
        {
            _spawnTimer += _gameTime.ElapsedGameTime.Milliseconds;

            if (i >= listOfArrivals.GetLength(0))
                IsEnded = true;

            if (!IsEnded)
            {
                if (listTypeOfArrival[counter] == 1)
                {
                    if (_spawnTimer > 200)
                    {
                        if (j < listOfArrivals.GetLength(1) && listOfArrivals[i, j] != null)
                        {
                            switch (listOfArrivals[i, j].EnType)
                            {
                                case (EnemyType.Bee):
                                    Components.GridEnemies.Add(new GridBeeEnemy(listOfArrivals[i, j].EnType, GetArrivePattern(i), listOfArrivals[i, j].Index, _canAttackPattern[j]));
                                    break;
                                case (EnemyType.Butterfly):
                                    Components.GridEnemies.Add(new GridButterflyEnemy(listOfArrivals[i, j].EnType, GetArrivePattern(i), listOfArrivals[i, j].Index, _canAttackPattern[j]));
                                    break;
                                case (EnemyType.Boss):
                                    Components.GridEnemies.Add(new GridBossEnemy(listOfArrivals[i, j].EnType, GetArrivePattern(i), listOfArrivals[i, j].Index, _canAttackPattern[j]));
                                    break;
                                case (EnemyType.Kamikaze):
                                    Components.GridEnemies.Add(new GridKamikazeEnemy(EnemyType.Kamikaze, GetArrivePattern(i), Components.Player.Position, false));
                                    break;
                            }

                            j++;
                        }
                        else
                        {
                            if (Components.GridEnemies.All(enemy => enemy.IsPositioned))
                            {
                                j = 0;
                                i++;
                                counter++;
                            }

                            _canAttackPattern.ShuffleCrypto();
                        }

                        _spawnTimer = 0;
                    }
                }
                else if (listTypeOfArrival[counter] == 2)
                {
                    if (_spawnTimer > 200)
                    {
                        if (j < listOfArrivals.GetLength(1) && listOfArrivals[i, j] != null)
                        {
                            switch (listOfArrivals[i, j].EnType)
                            {
                                case (EnemyType.Bee):
                                    Components.GridEnemies.Add(new GridBeeEnemy(listOfArrivals[i, j].EnType, GetArrivePattern(i).Mirror(), listOfArrivals[i, j].Index, _canAttackPattern[j]));
                                    break;
                                case (EnemyType.Butterfly):
                                    Components.GridEnemies.Add(new GridButterflyEnemy(listOfArrivals[i, j].EnType, GetArrivePattern(i).Mirror(), listOfArrivals[i, j].Index, _canAttackPattern[j]));
                                    break;
                                case (EnemyType.Boss):
                                    Components.GridEnemies.Add(new GridBossEnemy(listOfArrivals[i, j].EnType, GetArrivePattern(i).Mirror(), listOfArrivals[i, j].Index, _canAttackPattern[j]));
                                    break;
                                case (EnemyType.Kamikaze):
                                    Components.GridEnemies.Add(new GridKamikazeEnemy(EnemyType.Kamikaze, GetArrivePattern(i).Mirror(), Components.Player.Position, false));
                                    break;
                            }

                            j++;
                        }
                        else
                        {
                            if (Components.GridEnemies.All(enemy => enemy.IsPositioned))
                            {
                                j = 0;
                                i++;
                                counter++;
                            }

                            _canAttackPattern.ShuffleCrypto();
                        }

                        _spawnTimer = 0;
                    }
                }
                else if (listTypeOfArrival[counter] == 3)
                {
                    if (_spawnTimer > 200)
                    {
                        if (j < listOfArrivals.GetLength(1))
                        {
                            if (listOfArrivals[i, j] != null)
                            {
                                switch (listOfArrivals[i, j].EnType)
                                {
                                    case (EnemyType.Bee):
                                        Components.GridEnemies.Add(new GridBeeEnemy(listOfArrivals[i, j].EnType, GetArrivePattern(i), listOfArrivals[i, j].Index, _canAttackPattern[j]));
                                        break;
                                    case (EnemyType.Butterfly):
                                        Components.GridEnemies.Add(new GridButterflyEnemy(listOfArrivals[i, j].EnType, GetArrivePattern(i), listOfArrivals[i, j].Index, _canAttackPattern[j]));
                                        break;
                                    case (EnemyType.Boss):
                                        Components.GridEnemies.Add(new GridBossEnemy(listOfArrivals[i, j].EnType, GetArrivePattern(i), listOfArrivals[i, j].Index, _canAttackPattern[j]));
                                        break;
                                    case (EnemyType.Kamikaze):
                                        Components.GridEnemies.Add(new GridKamikazeEnemy(EnemyType.Kamikaze, GetArrivePattern(i), Components.Player.Position, false));
                                        break;
                                }
                            }
                            
                            if (listOfArrivals[i + 1, j] != null)
                            {
                                switch (listOfArrivals[i + 1, j].EnType)
                                {
                                    case (EnemyType.Bee):
                                        Components.GridEnemies.Add(new GridBeeEnemy(listOfArrivals[i + 1, j].EnType, GetArrivePattern(i).Mirror(), listOfArrivals[i + 1, j].Index, _canAttackPattern[j]));
                                        break;
                                    case (EnemyType.Butterfly):
                                        Components.GridEnemies.Add(new GridButterflyEnemy(listOfArrivals[i + 1, j].EnType, GetArrivePattern(i).Mirror(), listOfArrivals[i + 1, j].Index, _canAttackPattern[j]));
                                        break;
                                    case (EnemyType.Boss):
                                        Components.GridEnemies.Add(new GridBossEnemy(listOfArrivals[i + 1, j].EnType, GetArrivePattern(i).Mirror(), listOfArrivals[i + 1, j].Index, _canAttackPattern[j]));
                                        break;
                                    case (EnemyType.Kamikaze):
                                        Components.GridEnemies.Add(new GridKamikazeEnemy(EnemyType.Kamikaze, GetArrivePattern(i).Mirror(), Components.Player.Position, false));
                                        break;
                                }
                            }

                            j++;
                        }
                        else
                        {
                            if (Components.GridEnemies.All(enemy => enemy.IsPositioned))
                            {
                                j = 0;
                                i += 2;
                                counter += 2;
                            }

                            _canAttackPattern.ShuffleCrypto();
                        }

                        _spawnTimer = 0;
                    }
                }
            }

            #region
            /*
            if (_spawnTimer > 200)
            {
                if (counter < gridEnemyDisposition.Count)
                {
                    switch (gridEnemyDisposition[counter].EnType)
                    {
                        case (EnemyType.a):
                            Components.GridEnemies.Add(new BeeEnemy(gridEnemyDisposition[counter].EnType, _splinePoints, gridEnemyDisposition[counter].Index));
                            break;
                        case (EnemyType.b):
                            Components.GridEnemies.Add(new ButterflyEnemy(gridEnemyDisposition[counter].EnType, _splinePoints, gridEnemyDisposition[counter].Index));
                            break;
                        case (EnemyType.c):
                            Components.GridEnemies.Add(new BossEnemy(gridEnemyDisposition[counter].EnType, _splinePoints, gridEnemyDisposition[counter].Index));
                            break;
                    }

                    counter++;
                }
                else
                {
                    IsEnded = true;
                }

                _spawnTimer = 0;
            }*/
            #endregion
        }

        public void BonusEnemiesUpdate()
        {
            _spawnTimer += _gameTime.ElapsedGameTime.Milliseconds;
            int spawnTime = 170;
            int textureIndex;

            if (i >= bonusEnemiesArray.GetLength(0))
            {
                IsEnded = true;
            }
                
            if (!IsEnded)
            {
                if (i == 0)
                {
                    if (_spawnTimer > spawnTime)
                    {
                        if (j < 8)
                        {
                            textureIndex = bonusEnemiesArray[i][j];
                            Components.BonusEnemies.Add(new BonusEnemy(_bonusEnemiesTextureList[textureIndex], _listOfBonusPatterns[i].ToList(), false));

                            textureIndex = bonusEnemiesArray[i + 1][j];
                            Components.BonusEnemies.Add(new BonusEnemy(_bonusEnemiesTextureList[textureIndex], _listOfBonusPatterns[i + 1].ToList().Mirror(), false));

                            j++;
                        }
                        else
                        {
                            if (Components.BonusEnemies.All(enemy => enemy.IsPositioned))
                            {
                                j = 0;
                                i += 2;
                                counter++;
                            }
                        }

                        _spawnTimer = 0;
                    }
                }
                else if (i % 2 == 0)
                {
                    if (_spawnTimer > spawnTime)
                    {
                        if (j < 8)
                        {
                            textureIndex = bonusEnemiesArray[i][j];
                            Components.BonusEnemies.Add(new BonusEnemy(_bonusEnemiesTextureList[textureIndex], _listOfBonusPatterns[i].ToList(), false));

                            j++;
                        }
                        else
                        {
                            if (Components.BonusEnemies.All(enemy => enemy.IsPositioned))
                            {
                                j = 0;
                                i++;
                                counter++;
                            }
                        }

                        _spawnTimer = 0;
                    }
                }
                else
                {
                    if (_spawnTimer > spawnTime)
                    {
                        if (j < 8)
                        {
                            textureIndex = bonusEnemiesArray[i][j];
                            Components.BonusEnemies.Add(new BonusEnemy(_bonusEnemiesTextureList[textureIndex], _listOfBonusPatterns[i].ToList().Mirror(), false));

                            j++;
                        }
                        else
                        {
                            if (Components.BonusEnemies.All(enemy => enemy.IsPositioned))
                            {
                                j = 0;
                                i++;
                                counter++;
                            }
                        }

                        _spawnTimer = 0;
                    }
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
                    Components.Obstacles.Add(new Obstacle(AsteroidTextureList[RandomMod.GetRandom(0, AsteroidTextureList.Length)], true));
                    _spawnTimer = 0;
                }
            }

            if (ObstaclesDestroyed > LevelConfiguration.ObstaclesToDestroy)
            {
                IsEnded = true;
                ObstaclesDestroyed = 0;
            }
        }

        public void IntervalUpdate()
        {
            _spawnTimer += _gameTime.ElapsedGameTime.Milliseconds;
            _intervalTimer += _gameTime.ElapsedGameTime.Milliseconds;

            //_interval.Update(_gameTime);

            for (int i = 0; i < Components.UserInterfaceElements.Count; i++)
            {
                Components.UserInterfaceElements[i].Update(_gameTime);
            }

            if (_intervalTimer > 3000)
            {
                Components.Lasers.Clear();
                Components.GridEnemies.Clear();
                Components.Enemies.Clear();
                Components.BonusEnemies.Clear();
                IsEnded = true;
            }
        }
    }
}
