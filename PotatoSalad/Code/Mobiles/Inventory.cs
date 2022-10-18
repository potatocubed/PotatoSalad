using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad.Code.Mobiles
{
    public class Inventory
    {
        // Every mobile has one.

        public Item head = null;
        public Item body = null;
        public Item mainhand = null;
        public Item offhand = null;
        public Item feet = null;
        public Item ring1 = null;
        public Item ring2 = null;
        public Item neck = null;

        public Item[] bag = new Item[25];

        public Inventory()
        {
            // Initialise a blank inventory.
            for(int i = 0; i < bag.Length; i++)
            {
                bag[i] = null;
            }
        }
    }
}
