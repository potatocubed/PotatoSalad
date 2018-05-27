using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    class Player : Mobile
    {
        public string SaveDataFile;

        public Player(Tile loc)
            : base(loc, "UniqueIDPlayer")
        {
            name = "Player";
            displayGraphic = "../../Graphics/Mobiles/player.png";
            //displayGraphic = "C:/Users/Chris/source/repos/PotatoSalad/PotatoSalad/Graphics/Mobiles/player.png";
        }
    }
}
