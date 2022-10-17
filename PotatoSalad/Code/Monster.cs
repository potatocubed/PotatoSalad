using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PotatoSalad
{
    class Monster : Mobile
    {
        public string monType = "generic-monster";

        public Monster(Tile loc, string uid)
            : base(loc, uid)
        {
            name = "Generic Monster";
            displayGraphic = "../../Graphics/Mobiles/generic-monster.png";
            AI_type = 1;    // Move at random.
            
            // There's room here for monsters who do nothing until disturbed.
            // set AI_type to 0, then set it to something else when the player is spotted.
        }

        public override string GenerateSaveXML()
        {
            string s = "<monster>";

            // Properties inherited from Mobile.
            // &#10; is a line feed, just in case.
            s = $"{s}<name>{name}</name>";
            s = $"{s}<id>{id}</id>";
            s = $"{s}<montype>{monType}</montype>";
            s = $"{s}<location x=\"{X()}\" y = \"{Y()}\"/>";
            s = $"{s}<graphic>{displayGraphic}</graphic>";
            s = $"{s}<fov>{FOVRange}</fov>";

            // Properties from Monster.
            s = $"{s}<ai>{AI_type}</ai>";

            s = $"{s}</monster>";
            return s;
        }
    }
}
