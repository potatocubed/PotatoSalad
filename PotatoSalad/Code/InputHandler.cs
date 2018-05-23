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
            //if (Globals.DEBUG_MODE)
            //{
            //    Game.ConsoleForm.RenderText(k);
            //}
            if (Game.StateMachine.GetState() == Globals.STATE_PLAYER_TURN)
            {
                // Player input should move the player.
                // And then change the state, but we'll get to that.
                string direction = "";
                int deltaX = 0;
                int deltaY = 0;
                switch (k)
                {
                    
                    case "1":
                        direction = "SW";
                        deltaX = -1;
                        deltaY = 1;
                        break;
                    case "2":
                        direction = "S";
                        deltaX = 0;
                        deltaY = 1;
                        break;
                    case "3":
                        direction = "SE";
                        deltaX = 1;
                        deltaY = 1;
                        break;
                    case "4":
                        direction = "W";
                        deltaX = -1;
                        deltaY = 0;
                        break;
                    case "6":
                        direction = "E";
                        deltaX = 1;
                        deltaY = 0;
                        break;
                    case "7":
                        direction = "NW";
                        deltaX = -1;
                        deltaY = -1;
                        break;
                    case "8":
                        direction = "N";
                        deltaX = 0;
                        deltaY = -1;
                        break;
                    case "9":
                        direction = "NE";
                        deltaX = 1;
                        deltaY = -1;
                        break;
                    default:
                        break;
                }
                switch (k)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        Game.ConsoleForm.RenderText($"Attempting player movement to the {direction}.");
                        Game.Player.MoveTo(Game.Player.X() + deltaX, Game.Player.Y() + deltaY);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
