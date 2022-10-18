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
            AI_type = 0;    // Do nothing; when disturbed, set to something else.
            
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
            s = $"{s}<health>{health}</health>";
            s = $"{s}<mana>{mana}</mana>";
            s = $"{s}<faction>{faction}</faction>";

            s = $"{s}<skills>";
            for (int i = 0; i < skillArray.GetLength(0); i++)
            {
                s = $"{s}<skill name=\"{skillArray[i, 0]}\" rating=\"{skillArray[i, 1]}\" checks=\"{skillArray[i, 2]}\"/>";
            }
            s = $"{s}</skills>";

            // Properties from Monster.
            s = $"{s}<ai>{AI_type}</ai>";

            s = $"{s}</monster>";
            return s;
        }
    }
}
