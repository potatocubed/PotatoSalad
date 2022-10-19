using PotatoSalad.Code;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PotatoSalad
{
    class InputHandler
    {
        public void KeyIn(char k)
        {
            if (Game.StateMachine.GetState() == Globals.STATE_PLAYER_TURN)
            {
                // Player input should move the player.
                // And then change the state, but we'll get to that. -- TICK
                string direction = "";
                bool attemptedAction = false;
                int deltaX = 0;
                int deltaY = 0;

                switch (k)
                {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        string[] s = GetDirectionAndDeltas(int.Parse(k.ToString()));
                        direction = s[0];
                        deltaX = Convert.ToInt32(s[1]);
                        deltaY = Convert.ToInt32(s[2]);
                        //Game.GAPI.RenderText($"Attempting player movement to the {direction}.");
                        attemptedAction = Game.Player.MoveTo(Game.Player.X() + deltaX, Game.Player.Y() + deltaY);
                        break;
                    case 'x':
                    case 'X':
                        //Game.GAPI.RenderText("Activating CURSOR MODE.");
                        Game.SaladCursor = new PotatoSalad.Code.Cursor(Game.Player.location);
                        Game.StateMachine.SetState(Globals.STATE_CURSOR_MODE);
                        Game.GAPI.CursorDrawMap(Game.DungeonMap, Game.SaladCursor);
                        break;
                    default:
                        break;
                }

                // Letting the monsters have a turn.
                if (attemptedAction)
                {
                    Game.AIHandler.RunAllAIs();
                }
                // If the player's action failed we skip the monster
                // turn and let the player try again.
            }
            else if (Game.StateMachine.GetState() == Globals.STATE_CURSOR_MODE)
            {
                int deltaX = 0;
                int deltaY = 0;
                bool attemptedAction = false;

                switch (k)
                {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        string[] s = GetDirectionAndDeltas(int.Parse(k.ToString()));
                        deltaX = Convert.ToInt32(s[1]);
                        deltaY = Convert.ToInt32(s[2]);
                        //Game.GAPI.RenderText($"Attempting cursor movement to the {s[0]}.");
                        attemptedAction = Game.SaladCursor.MoveTo(Game.SaladCursor.X() + deltaX, Game.SaladCursor.Y() + deltaY);
                        break;
                    case 'x':
                    case 'X':
                    case '\u001b':      // Esc
                        Game.StateMachine.SetState(Globals.STATE_PLAYER_TURN);
                        Game.GAPI.DrawMap(Game.DungeonMap);
                        break;
                    default:
                        break;
                }
            }
        }

        private string[] GetDirectionAndDeltas(int numKey)
        {
            string[] response = new string[3];

            switch (numKey)
            {
                case 1:
                    response[0] = "SW";
                    response[1] = "-1";
                    response[2] = "1";
                    break;
                case 2:
                    response[0] = "S";
                    response[1] = "0";
                    response[2] = "1";
                    break;
                case 3:
                    response[0] = "SE";
                    response[1] = "1";
                    response[2] = "1";
                    break;
                case 4:
                    response[0] = "W";
                    response[1] = "-1";
                    response[2] = "0";
                    break;
                case 6:
                    response[0] = "E";
                    response[1] = "1";
                    response[2] = "0";
                    break;
                case 7:
                    response[0] = "NW";
                    response[1] = "-1";
                    response[2] = "-1";
                    break;
                case 8:
                    response[0] = "N";
                    response[1] = "0";
                    response[2] = "-1";
                    break;
                case 9:
                    response[0] = "NE";
                    response[1] = "1";
                    response[2] = "-1";
                    break;
                default:
                    break;
            }

            return response;
        }
    }
}
