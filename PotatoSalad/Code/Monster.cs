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

        new public XmlDocumentFragment GenerateSaveXML()
        {
            XmlDocumentFragment gsx = new XmlDocument().CreateDocumentFragment();
            gsx.InnerXml = "<monster></monster>";
            XmlElement xElem;

            // Properties inherited from Mobile.
            // &#10; is a line feed, just in case.
            xElem = new XmlDocument().CreateElement("name");
            xElem.InnerText = name;
            gsx.AppendChild(xElem);

            xElem = new XmlDocument().CreateElement("id");
            xElem.InnerText = id;
            gsx.AppendChild(xElem);

            xElem = new XmlDocument().CreateElement("location");
            XmlAttribute attr = new XmlDocument().CreateAttribute("x");
            attr.Value = X().ToString();
            xElem.SetAttributeNode(attr);
            attr = new XmlDocument().CreateAttribute("y");
            attr.Value = Y().ToString();
            xElem.SetAttributeNode(attr);
            gsx.AppendChild(xElem);

            xElem = new XmlDocument().CreateElement("graphic");
            xElem.InnerText = displayGraphic;
            gsx.AppendChild(xElem);

            xElem = new XmlDocument().CreateElement("FOV");
            xElem.InnerText = FOVRange.ToString();
            gsx.AppendChild(xElem);

            // Properties from Monster.
            xElem = new XmlDocument().CreateElement("AI");
            xElem.InnerText = AI_type.ToString();
            gsx.AppendChild(xElem);

            return gsx;
        }
    }
}
