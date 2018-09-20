using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    class Monster : Mobile
    {
        public Monster(Tile loc, string uid)
            : base(loc, uid)
        {
            name = "Generic Monster";
            displayGraphic = "../../Graphics/Mobiles/player.png";
        }
    }
}
