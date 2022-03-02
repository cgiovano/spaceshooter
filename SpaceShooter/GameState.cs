using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public static class GameState
    {
        public enum State
        {
            Menu,
            Settings,
            Playing,
            Paused,
            GameOver,
            Transition
        }

        public static State currentState;
    }
}
