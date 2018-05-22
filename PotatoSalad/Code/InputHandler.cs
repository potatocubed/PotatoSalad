using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    class InputHandler
    {
        public void KeyIn(string k)
        {
            if (Globals.DEBUG_MODE)
            {
                Game.ConsoleForm.RenderText(k);
            }
            if (Game.StateMachine.GetState() == Globals.STATE_PLAYER_TURN)
            {
                // Player input should move the player.
            }
        }
    }
}
