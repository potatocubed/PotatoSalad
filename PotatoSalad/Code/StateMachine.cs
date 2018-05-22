using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace PotatoSalad
{
    public class StateMachine
    {
        private const int PLAYER_TURN = 1;
        private const int ENEMY_TURN = 2;
        private int _state = PLAYER_TURN;

        public int GetState()
        {
            return _state;
        }

        public void SetState(int i)
        {
            _state = i;
        }
    }
}