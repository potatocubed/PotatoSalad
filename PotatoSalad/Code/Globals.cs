using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    class Globals
    {
        public static bool DEBUG_MODE = true;

        public static int STATE_PLAYER_TURN = 1;
        public static int STATE_ENEMY_TURN = 2;
        public static int STATE_MAIN_MENU = 3;

        public List<string> DEBUG_ERROR_LIST = new List<string>();
    }
}
