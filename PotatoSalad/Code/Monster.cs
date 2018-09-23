using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    class Monster : Mobile
    {
        public int AI_type;
        public string monType = "generic-monster";

        public Monster(Tile loc, string uid)
            : base(loc, uid)
        {
            name = "Generic Monster";
            displayGraphic = "../../Graphics/Mobiles/generic-monster.png";
            AI_type = 1;    // There's room here for monsters who do nothing until disturbed.
                            // set AI_type to 0, then set it to something else when the player is spotted.
        }
    }
}
