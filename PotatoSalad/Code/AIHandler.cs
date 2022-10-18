using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad.Code
{
    internal class AIHandler
    {
        public void RunAllAIs()
        {
            // Sets game state to monster state.
            Game.StateMachine.SetState(Globals.STATE_ENEMY_TURN);

            // Loops through map.mobilearray and grabs anything
            // with an AI_state.
            foreach(Mobile m in Game.DungeonMap.MobileArray)
            {
                try
                {
                    if(m.GetType().Name == "Monster")
                    {
                        // Run the AI
                        try
                        {
                            switch (m.AI_type)
                            {
                                case 0:
                                    // Unactivated, do nothing.
                                    break;
                                case 1:
                                    AIMoveRandom(m);
                                    break;
                                default:
                                    AIMoveRandom(m);
                                    break;
                            }
                        }
                        catch
                        {
                            // If there's something wrong with it,
                            // do nothing. For now.
                        }
                    }
                }
                catch
                {
                    // If this fails we're okay not doing anything.
                }
            }

            // Activates those AIs.

            // Return to player_state.
            Game.StateMachine.SetState(Globals.STATE_PLAYER_TURN);
        }

        private void AIMoveRandom(Mobile m)
        {
            // 1 is north, then circle clockwise.
            string direction = "";
            int deltaX = 0;
            int deltaY = 0;
            int direc = Game.Dice.XdY(1, 8);
            switch (direc)
            {
                case 1:
                    direction = "N";
                    deltaX = 0;
                    deltaY = -1;
                    break;
                case 2:
                    direction = "NE";
                    deltaX = 1;
                    deltaY = -1;
                    break;
                case 3:
                    direction = "E";
                    deltaX = 1;
                    deltaY = 0;
                    break;
                case 4:
                    direction = "SE";
                    deltaX = 1;
                    deltaY = 1;
                    break;
                case 5:
                    direction = "S";
                    deltaX = 0;
                    deltaY = 1;
                    break;
                case 6:
                    direction = "SW";
                    deltaX = -1;
                    deltaY = 1;
                    break;
                case 7:
                    direction = "W";
                    deltaX = -1;
                    deltaY = 0;
                    break;
                case 8:
                    direction = "NW";
                    deltaX = -1;
                    deltaY = -1;
                    break;
            }

            //Game.GAPI.RenderText($"Attempting {m.id} movement to the {direction}.");
            bool moveAttempt = m.MoveTo(m.X() + deltaX, m.Y() + deltaY);
        }
    }
}
