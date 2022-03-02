namespace SpaceShooter.GameUtils
{
	public class CurrentLevel
	{
		public static Level GetLevel { get; private set; }

        public static void SetLevel(Level level)
        {
            GetLevel = level;
        }
	}
}
