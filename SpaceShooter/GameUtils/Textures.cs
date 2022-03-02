using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SpaceShooter.GameUtils
{
    class Textures
    {
        public static readonly Texture2D StartScreenLogo;
        public static readonly Texture2D StartScreenBackground;
        public static readonly Texture2D EndGameBackground;
        public static readonly Texture2D HelpBackground;
        public static readonly Texture2D ConfigurationsBackground;
        public static readonly Texture2D Credits;
        public static readonly Texture2D HelpScreen;

        public static readonly Texture2D SaveAndBack;
        public static readonly Texture2D EffectsVolume;
        public static readonly Texture2D MusicVolume;
        public static readonly Texture2D FullScreen;
        public static readonly Texture2D OnSlider;
        public static readonly Texture2D OffSlider;

        public static readonly Texture2D Button;
        public static readonly Texture2D Continue;
        public static readonly Texture2D Start;
        public static readonly Texture2D Help;
        public static readonly Texture2D Settings;
        public static readonly Texture2D Quit;
        public static readonly Texture2D Back;
        public static readonly Texture2D Pointer;
        public static readonly Texture2D Ready;
        public static readonly Texture2D GameOver;
        public static readonly Texture2D PlayAgain;
        public static readonly Texture2D SpaceKey;
        public static readonly Texture2D Paused;

        public static readonly Texture2D LifeBar;
        public static readonly Texture2D LifeBarContour;
        public static readonly Texture2D Heart;

        public static readonly Texture2D ShieldOneUp;
        public static readonly Texture2D RepairOneUp;
        public static readonly Texture2D LifeOneUp;
        public static readonly Texture2D PrimaryWeaponSelection;
        public static readonly Texture2D SecondaryWeaponSelection;
        public static readonly Texture2D TerciaryWeaponSelection;

        public static readonly Texture2D TransitionBackground;
        public static readonly Texture2D Stage1;
        public static readonly Texture2D Stage2;
        public static readonly Texture2D Stage3;
        public static readonly Texture2D Stage4;
        public static readonly Texture2D Stage5;

        public static readonly Texture2D Confirmation;
        public static readonly Texture2D MainMenu;
        public static readonly Texture2D Yes;
        public static readonly Texture2D No;

        public static readonly Texture2D StarsSmallTexture;
        public static readonly Texture2D StarsBigTexture;

        public static readonly Texture2D PlayerShipTexture;
        public static readonly Texture2D PlayerShipTextureCombined;
        public static readonly Texture2D PlayerShipFlames;

        public static readonly Texture2D AsteroidSmallSizeTexture;
        public static readonly Texture2D AsteroidMedSizeTexture;
        public static readonly Texture2D AsteroidBigSizeTexture;

        public static readonly Texture2D PlayerLaser_A_Texture;
        public static readonly Texture2D PlayerLaser_B_Texture;
        public static readonly Texture2D PlayerLaser_C_Texture;

        public static readonly Texture2D EnemyLaser_A_Texture;
        public static readonly Texture2D EnemyLaser_B_Texture;
        public static readonly Texture2D EnemyLaser_C_Texture;
        public static readonly Texture2D EnemyLaser_D_Texture;
        public static readonly Texture2D EnemyLaser_E_Texture;
        public static readonly Texture2D EnemyLaser_F_Texture;
        public static readonly Texture2D Boss_Laser_Texture;

        public static readonly Texture2D ExplosionParticleTexture;
        public static readonly Texture2D AsteroidParticleTexture;

        public static readonly Texture2D Shield;
        public static readonly Texture2D Energy;
        public static readonly Texture2D Life;

        public static readonly Texture2D Enemy_A_Texture;
        public static readonly Texture2D Enemy_B_Texture;
        public static readonly Texture2D Enemy_C_Texture;
        public static readonly Texture2D Enemy_D_Texture;
        public static readonly Texture2D Enemy_E_Texture;
        public static readonly Texture2D Enemy_F_Texture;

        public static readonly Texture2D Rush_Enemy_A_Texture;
        public static readonly Texture2D Rush_Enemy_B_Texture;
        public static readonly Texture2D Rush_Enemy_C_Texture;
        public static readonly Texture2D Rush_Enemy_D_Texture;

        public static readonly Texture2D FinalBossStage1_Texture;
        public static readonly Texture2D FinalBossStage1_EyeWeakpoint_Texture;
        public static readonly Texture2D FinalBossStage2_Texture;
        public static readonly Texture2D FinalBossStage3_Texture;
        public static readonly Texture2D FinalBossStage4_Texture;
        public static readonly Texture2D FinalBossStage4_GunWeakpoint_Texture;
        public static readonly Texture2D FinalBossStage4_EyeWeakpoint_Texture;
        public static readonly Texture2D FinalBossStage5_Texture;
        public static readonly Texture2D FinalBossStage5_HandWeakpoint_Texture;
        public static readonly Texture2D FinalBossStage5_BrainWeakpoint_Texture;
        public static readonly Texture2D FinalBossStage5_MouthWeakpoint_Texture;

        public Textures(ContentManager content)
        {
            typeof(Textures).GetField("StartScreenLogo").SetValue(StartScreenLogo, content.Load<Texture2D>("Assets\\Images\\start_screen_logo"));
            typeof(Textures).GetField("StartScreenBackground").SetValue(StartScreenBackground, content.Load<Texture2D>("Assets\\Images\\start_screen_background"));
            typeof(Textures).GetField("EndGameBackground").SetValue(EndGameBackground, content.Load<Texture2D>("Assets\\Images\\end_game_background"));
            typeof(Textures).GetField("HelpBackground").SetValue(HelpBackground, content.Load<Texture2D>("Assets\\Images\\help_background"));
            typeof(Textures).GetField("ConfigurationsBackground").SetValue(ConfigurationsBackground, content.Load<Texture2D>("Assets\\Images\\configurations_background"));
            typeof(Textures).GetField("Credits").SetValue(Credits, content.Load<Texture2D>("Assets\\Images\\credits"));
            typeof(Textures).GetField("HelpScreen").SetValue(HelpScreen, content.Load<Texture2D>("Assets\\Images\\help_screen"));

            typeof(Textures).GetField("SaveAndBack").SetValue(SaveAndBack, content.Load<Texture2D>("Assets\\Images\\save_and_back"));
            typeof(Textures).GetField("EffectsVolume").SetValue(EffectsVolume, content.Load<Texture2D>("Assets\\Images\\effects_volume"));
            typeof(Textures).GetField("MusicVolume").SetValue(MusicVolume, content.Load<Texture2D>("Assets\\Images\\music_volume"));
            typeof(Textures).GetField("FullScreen").SetValue(FullScreen, content.Load<Texture2D>("Assets\\Images\\fullscreen"));
            typeof(Textures).GetField("OnSlider").SetValue(OnSlider, content.Load<Texture2D>("Assets\\Images\\onSlider"));
            typeof(Textures).GetField("OffSlider").SetValue(OffSlider, content.Load<Texture2D>("Assets\\Images\\offSlider"));

            typeof(Textures).GetField("Button").SetValue(Button, content.Load<Texture2D>("Assets\\Images\\Button"));
            typeof(Textures).GetField("Continue").SetValue(Continue, content.Load<Texture2D>("Assets\\Images\\continue"));
            typeof(Textures).GetField("Start").SetValue(Start, content.Load<Texture2D>("Assets\\Images\\start"));
            typeof(Textures).GetField("Help").SetValue(Help, content.Load<Texture2D>("Assets\\Images\\help"));
            typeof(Textures).GetField("Settings").SetValue(Settings, content.Load<Texture2D>("Assets\\Images\\settings"));
            typeof(Textures).GetField("Quit").SetValue(Quit, content.Load<Texture2D>("Assets\\Images\\quit"));
            typeof(Textures).GetField("Back").SetValue(Back, content.Load<Texture2D>("Assets\\Images\\back"));
            typeof(Textures).GetField("Pointer").SetValue(Pointer, content.Load<Texture2D>("Assets\\Images\\pointer"));
            typeof(Textures).GetField("Ready").SetValue(Ready, content.Load<Texture2D>("Assets\\Images\\ready"));
            typeof(Textures).GetField("GameOver").SetValue(GameOver, content.Load<Texture2D>("Assets\\Images\\game_over"));
            typeof(Textures).GetField("PlayAgain").SetValue(PlayAgain, content.Load<Texture2D>("Assets\\Images\\play_again"));
            typeof(Textures).GetField("SpaceKey").SetValue(SpaceKey, content.Load<Texture2D>("Assets\\Images\\space_key"));
            typeof(Textures).GetField("Paused").SetValue(Paused, content.Load<Texture2D>("Assets\\Images\\paused"));
            typeof(Textures).GetField("LifeBar").SetValue(LifeBar, content.Load<Texture2D>("Assets\\Images\\life_bar"));
            typeof(Textures).GetField("LifeBarContour").SetValue(LifeBarContour, content.Load<Texture2D>("Assets\\Images\\life_bar_contour"));
            typeof(Textures).GetField("Heart").SetValue(Heart, content.Load<Texture2D>("Assets\\Images\\heart"));
            typeof(Textures).GetField("TransitionBackground").SetValue(TransitionBackground, content.Load<Texture2D>("Assets\\Images\\transition_background"));
            typeof(Textures).GetField("Stage1").SetValue(Stage1, content.Load<Texture2D>("Assets\\Images\\stage_1"));
            typeof(Textures).GetField("Stage2").SetValue(Stage2, content.Load<Texture2D>("Assets\\Images\\stage_2"));
            typeof(Textures).GetField("Stage3").SetValue(Stage3, content.Load<Texture2D>("Assets\\Images\\stage_3"));
            typeof(Textures).GetField("Stage4").SetValue(Stage4, content.Load<Texture2D>("Assets\\Images\\stage_4"));
            typeof(Textures).GetField("Stage5").SetValue(Stage5, content.Load<Texture2D>("Assets\\Images\\stage_5"));

            typeof(Textures).GetField("ShieldOneUp").SetValue(ShieldOneUp, content.Load<Texture2D>("Assets\\Images\\shieldOneUp"));
            typeof(Textures).GetField("RepairOneUp").SetValue(RepairOneUp, content.Load<Texture2D>("Assets\\Images\\repairOneUp"));
            typeof(Textures).GetField("LifeOneUp").SetValue(LifeOneUp, content.Load<Texture2D>("Assets\\Images\\lifeOneUp"));
            typeof(Textures).GetField("PrimaryWeaponSelection").SetValue(PrimaryWeaponSelection, content.Load<Texture2D>("Assets\\Images\\selectedWeapon1"));
            typeof(Textures).GetField("SecondaryWeaponSelection").SetValue(SecondaryWeaponSelection, content.Load<Texture2D>("Assets\\Images\\selectedWeapon2"));
            typeof(Textures).GetField("TerciaryWeaponSelection").SetValue(TerciaryWeaponSelection, content.Load<Texture2D>("Assets\\Images\\selectedWeapon3"));

            typeof(Textures).GetField("Confirmation").SetValue(Confirmation, content.Load<Texture2D>("Assets\\Images\\confirmation"));
            typeof(Textures).GetField("MainMenu").SetValue(MainMenu, content.Load<Texture2D>("Assets\\Images\\main_menu"));
            typeof(Textures).GetField("Yes").SetValue(Yes, content.Load<Texture2D>("Assets\\Images\\yes"));
            typeof(Textures).GetField("No").SetValue(No, content.Load<Texture2D>("Assets\\Images\\no"));

            typeof(Textures).GetField("StarsSmallTexture").SetValue(StarsSmallTexture, content.Load<Texture2D>("Assets\\Images\\space_background_small_stars"));
            typeof(Textures).GetField("StarsBigTexture").SetValue(StarsBigTexture, content.Load<Texture2D>("Assets\\Images\\space_background_big_stars"));

            typeof(Textures).GetField("PlayerShipTexture").SetValue(PlayerShipTexture, content.Load<Texture2D>("Assets\\Images\\SpaceShip"));
            typeof(Textures).GetField("PlayerShipTextureCombined").SetValue(PlayerShipTexture, content.Load<Texture2D>("Assets\\Images\\SpaceShipCombined"));
            typeof(Textures).GetField("PlayerShipFlames").SetValue(PlayerShipFlames, content.Load<Texture2D>("Assets\\Images\\space_ship_flames"));

            //Laser Textures
            typeof(Textures).GetField("PlayerLaser_A_Texture").SetValue(PlayerLaser_A_Texture, content.Load<Texture2D>("Assets\\Images\\player_laser"));
            typeof(Textures).GetField("PlayerLaser_B_Texture").SetValue(PlayerLaser_B_Texture, content.Load<Texture2D>("Assets\\Images\\player_laser_b"));
            typeof(Textures).GetField("PlayerLaser_C_Texture").SetValue(PlayerLaser_C_Texture, content.Load<Texture2D>("Assets\\Images\\player_laser_c"));

            typeof(Textures).GetField("EnemyLaser_A_Texture").SetValue(EnemyLaser_A_Texture, content.Load<Texture2D>("Assets\\Images\\enemy_laser_a"));
            typeof(Textures).GetField("EnemyLaser_B_Texture").SetValue(EnemyLaser_B_Texture, content.Load<Texture2D>("Assets\\Images\\enemy_laser_b"));
            typeof(Textures).GetField("EnemyLaser_C_Texture").SetValue(EnemyLaser_C_Texture, content.Load<Texture2D>("Assets\\Images\\enemy_laser_c"));
            typeof(Textures).GetField("EnemyLaser_D_Texture").SetValue(EnemyLaser_D_Texture, content.Load<Texture2D>("Assets\\Images\\enemy_laser_d"));
            typeof(Textures).GetField("EnemyLaser_E_Texture").SetValue(EnemyLaser_E_Texture, content.Load<Texture2D>("Assets\\Images\\enemy_laser_e"));
            typeof(Textures).GetField("EnemyLaser_F_Texture").SetValue(EnemyLaser_F_Texture, content.Load<Texture2D>("Assets\\Images\\enemy_laser_f"));
            typeof(Textures).GetField("Boss_Laser_Texture").SetValue(Boss_Laser_Texture, content.Load<Texture2D>("Assets\\Images\\boss_laser"));

            //Asteroid Textures
            typeof(Textures).GetField("AsteroidSmallSizeTexture").SetValue(AsteroidSmallSizeTexture, content.Load<Texture2D>("Assets\\Images\\asteroid_small"));
            typeof(Textures).GetField("AsteroidMedSizeTexture").SetValue(AsteroidMedSizeTexture, content.Load<Texture2D>("Assets\\Images\\asteroid_med"));
            typeof(Textures).GetField("AsteroidBigSizeTexture").SetValue(AsteroidBigSizeTexture, content.Load<Texture2D>("Assets\\Images\\asteroid_big"));

            //Particle Textures
            typeof(Textures).GetField("AsteroidParticleTexture").SetValue(AsteroidParticleTexture, content.Load<Texture2D>("Assets\\Images\\asteroid_destroyed"));
            typeof(Textures).GetField("ExplosionParticleTexture").SetValue(ExplosionParticleTexture, content.Load<Texture2D>("Assets\\Images\\explosion_particle"));

            typeof(Textures).GetField("Shield").SetValue(Shield, content.Load<Texture2D>("Assets\\Images\\shield"));

            // Enemies Texture
            typeof(Textures).GetField("Enemy_A_Texture").SetValue(Enemy_A_Texture, content.Load<Texture2D>("Assets\\Images\\enemy_a"));
            typeof(Textures).GetField("Enemy_B_Texture").SetValue(Enemy_B_Texture, content.Load<Texture2D>("Assets\\Images\\enemy_b"));
            typeof(Textures).GetField("Enemy_C_Texture").SetValue(Enemy_C_Texture, content.Load<Texture2D>("Assets\\Images\\enemy_c"));
            typeof(Textures).GetField("Enemy_D_Texture").SetValue(Enemy_D_Texture, content.Load<Texture2D>("Assets\\Images\\enemy_d"));
            typeof(Textures).GetField("Enemy_E_Texture").SetValue(Enemy_E_Texture, content.Load<Texture2D>("Assets\\Images\\enemy_e"));
            typeof(Textures).GetField("Enemy_F_Texture").SetValue(Enemy_F_Texture, content.Load<Texture2D>("Assets\\Images\\enemy_f"));

            typeof(Textures).GetField("Rush_Enemy_A_Texture").SetValue(Rush_Enemy_A_Texture, content.Load<Texture2D>("Assets\\Images\\rush_enemy_a"));
            typeof(Textures).GetField("Rush_Enemy_B_Texture").SetValue(Rush_Enemy_B_Texture, content.Load<Texture2D>("Assets\\Images\\rush_enemy_b"));
            typeof(Textures).GetField("Rush_Enemy_C_Texture").SetValue(Rush_Enemy_C_Texture, content.Load<Texture2D>("Assets\\Images\\rush_enemy_c"));
            typeof(Textures).GetField("Rush_Enemy_D_Texture").SetValue(Rush_Enemy_D_Texture, content.Load<Texture2D>("Assets\\Images\\rush_enemy_d"));

            typeof(Textures).GetField("FinalBossStage1_Texture").SetValue(FinalBossStage1_Texture, content.Load<Texture2D>("Assets\\Images\\Boss_2_main"));
            typeof(Textures).GetField("FinalBossStage1_EyeWeakpoint_Texture").SetValue(FinalBossStage1_EyeWeakpoint_Texture, content.Load<Texture2D>("Assets\\Images\\Boss_2_eyeWeakpoint"));
            typeof(Textures).GetField("FinalBossStage2_Texture").SetValue(FinalBossStage2_Texture, content.Load<Texture2D>("Assets\\Images\\Boss_1_main"));
            typeof(Textures).GetField("FinalBossStage3_Texture").SetValue(FinalBossStage3_Texture, content.Load<Texture2D>("Assets\\Images\\Boss_3"));
            typeof(Textures).GetField("FinalBossStage4_Texture").SetValue(FinalBossStage4_Texture, content.Load<Texture2D>("Assets\\Images\\Boss_4_main"));
            typeof(Textures).GetField("FinalBossStage4_GunWeakpoint_Texture").SetValue(FinalBossStage4_GunWeakpoint_Texture, content.Load<Texture2D>("Assets\\Images\\Boss_4_gunWeakpoint"));
            typeof(Textures).GetField("FinalBossStage4_EyeWeakpoint_Texture").SetValue(FinalBossStage4_EyeWeakpoint_Texture, content.Load<Texture2D>("Assets\\Images\\Boss_4_eyeWeakpoint"));
            typeof(Textures).GetField("FinalBossStage5_Texture").SetValue(FinalBossStage5_Texture, content.Load<Texture2D>("Assets\\Images\\Boss_5_main"));
            typeof(Textures).GetField("FinalBossStage5_HandWeakpoint_Texture").SetValue(FinalBossStage5_HandWeakpoint_Texture, content.Load<Texture2D>("Assets\\Images\\Boss_5_handWeakpoint"));
            typeof(Textures).GetField("FinalBossStage5_BrainWeakpoint_Texture").SetValue(FinalBossStage5_BrainWeakpoint_Texture, content.Load<Texture2D>("Assets\\Images\\Boss_5_brainWeakPoint"));
            typeof(Textures).GetField("FinalBossStage5_MouthWeakpoint_Texture").SetValue(FinalBossStage5_MouthWeakpoint_Texture, content.Load<Texture2D>("Assets\\Images\\Boss_5_mouthWeakPoint"));

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