using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad.Code
{
    public class Cursor
    {
        public string graphic;
        public Tile location;

        public Cursor()
        {
            graphic = "../../Graphics/cursor.png";
        }

        public int X()
        {
            return location.X;
        }

        public int Y()
        {
            return location.Y;
        }

        public bool MoveTo(int destX, int destY)
        {
            int origX = X();
            int origY = Y();

            // don't move off the map
            if (destX < 0 || destY < 0 || destX > Game.DungeonMap.XDimension || destY > Game.DungeonMap.YDimension)
            {
                // You're trying to move off the edge of the map. Nope.
                //Game.GAPI.RenderText($"No movement! {this.id} is trying to walk off the map at {destX}, {destY}.");
                return false;
            }

            return false;
        }

        public string GenerateDescription(int x, int y)
        {
            // bear in mind 'you can see' vs 'you last saw'
            return "";
        }
    }
}
