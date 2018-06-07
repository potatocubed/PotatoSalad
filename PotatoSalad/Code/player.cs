using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    class Player : Mobile
    {
        //public string SaveDataFile;

        public Player(Tile loc)
            : base(loc, "UniqueIDPlayer")
        {
            name = "Player";
            displayGraphic = "../../Graphics/Mobiles/player.png";
        }

        public void LoadPlayer(string n, string id, string dg, int fovr)
        {
            name = n;
            this.id = id;
            displayGraphic = dg;
            FOVRange = fovr;
        }
    }
}
