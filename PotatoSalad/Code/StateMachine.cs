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
        //private const int PLAYER_TURN = 1;
        //private const int ENEMY_TURN = 2;
        //private int _state = PLAYER_TURN;
        private int _state;

        public StateMachine(int initial_state)
        {
            _state = initial_state;
        }

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