using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad.Code.MapGeneration
{
    class Room
    {
        public int TopLeftX;
        public int TopLeftY;
        public int xDimension;
        public int yDimension;
        public int CentreX;
        public int CentreY;

        public void CalculateCentre()
        {
            // Errs to the top-left.
            //int x = xDimension - TopLeftX;
            //int y = yDimension - TopLeftY;
            CentreX = TopLeftX + (xDimension / 2);
            CentreY = TopLeftY + (yDimension / 2);
        }
    }
}
