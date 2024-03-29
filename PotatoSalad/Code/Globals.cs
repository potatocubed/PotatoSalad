﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    class Globals
    {
        public static bool DEBUG_MODE = true;

        public static int STATE_WORKING = 0;
        public static int STATE_PLAYER_TURN = 1;
        public static int STATE_ENEMY_TURN = 2;
        public static int STATE_MAIN_MENU = 3;
        public static int STATE_CURSOR_MODE = 4;

        public static int AI_TYPE_DO_NOTHING = 0;
        public static int AI_TYPE_MOVE_RANDOM = 1;
        public static int AI_TYPE_NORMAL = 2;   // Go to where they last saw the PC.
        public static int AI_TYPE_HUNTER = 3;   // Always knows where you are.

        public List<string> DEBUG_ERROR_LIST = new List<string>();
    }
}
