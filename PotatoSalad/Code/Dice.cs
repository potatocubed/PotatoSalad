using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    class Dice
    {
        Random rng = new Random();

        public int XdY(int numOfDice, int sidesOfDice)
        {
            int result = 0;
            for(int i = numOfDice; i <= numOfDice; i++)
            {
                result += rng.Next(1, sidesOfDice + 1);
            }

            return result;
        }
    }
}
