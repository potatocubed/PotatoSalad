using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    class FOVCalculator
    {
        // Basically, we're going to pass the TileArray, the location of the player,
        // and the FOVRadius... then return a list of tiles from the TileArray.

        public List<Tile> ReturnFOVList(Tile[,] tileList, int pX, int pY, int radius)
        {
            List<Tile> results = new List<Tile>();
            results.Add(tileList[pX, pY]);  // We can always see our own space. (Unless blind, but we'll handle that later.)

            // So we get a square.
            int[,] box = GetBox(pX, pY, radius);

            // Then we test LOS from the origin to every boundary, in octants.
            for (int i = 0; i <= box.GetUpperBound(1); i++)
            {
                foreach (Tile t in TilesInALine(pX, pY, box[0, i], box[1, i], Game.DungeonMap))
                {
                    results.Add(t);
                }
            }

            // MUST NOT return tiles that are outside the map.
            return results;
        }

        private int[,] GetBox(int originX, int originY, int radius)
        {
            // Just returns a list of co-ordinates of boxes around the edge.
            int boxSideLength = (2 * radius) + 1;
            int arraySize = (2 * boxSideLength) + (2 * (boxSideLength - 2));
            int targRow = 0;

            int[,] results = new int[2, arraySize];
            // Dimension 1:
            // 0: x     1: y

            // Draw two lines across the top and bottom.
            for (int i = -radius; i <= radius; i++)
            {
                results[0, targRow] = originX + i;
                results[1, targRow] = originY - radius;
                targRow++;
                results[0, targRow] = originX + i;
                results[1, targRow] = originY + radius;
                targRow++;
            }

            // And then fill them in down the sides.
            for (int i = -(radius-1); i <=(radius-1); i++)
            {
                results[0, targRow] = originX - radius;
                results[1, targRow] = originY + i;
                targRow++;
                results[0, targRow] = originX + radius;
                results[1, targRow] = originY + i;
                targRow++;
            }

            return results;
        }

        private List<Tile> TilesInALine(int xo, int yo, int xd, int yd, Map m)
        {
            // The 'o' and 'd' are 'origin' and 'destination'.
            // Credit to Eric Woshorow who did most of the work here.
            // http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html

            List<Tile> results = new List<Tile>();
            // This is where we use Bresenham's algorithm to get all the tiles between origin and destination.

            // Step 1. Which octant are we looking at?
            // Get a transform to make it into the lower-right, dx > dy octant.
            // Then transform it back again to get your actual tile.

            // Starting at 12 o'clock, the octants go clockwise 1-8.

            bool rightSide = false;
            bool topSide = false;
            bool longX = false;

            int deltaX = Math.Abs(xo - xd);
            int deltaY = Math.Abs(yo - yd);

            if (xo < xd || ((xo == xd) && (yo < yd)))
            {
                // It's on the right OR straight up.
                rightSide = true;
            }
            else
            {
                // It's on the left OR straight down.
                rightSide = false;
            }

            if (yo < yd || ((yo == yd) && (xo > xd)))
            {
                // It's on top OR straight left.
                rightSide = false;
            }
            else
            {
                // It's underneath OR straight right.
                rightSide = true;
            }

            if (deltaX > deltaY || ((xd > xo) && (yd < yo)) || ((xd < xo) && (yd > yo)))
            {
                // It's a 'flat' octant, OR you're going northeast, OR you're going southwest.
                longX = true;
            }
            else
            {
                // It's a 'standing' octant, OR you're going southeast, OR you're going northwest.
                longX = false;
            }

            int x0 = 0;
            int x1 = 0;
            int y0 = 0;
            int y1 = 0;

            int octant = 0;

            if (!topSide && rightSide && longX)
            {
                // 3. The default octant.
                octant = 3;
            }
            else if(topSide && rightSide && longX)
            {
                octant = 2;
            }
            else if(topSide && rightSide && !longX)
            {
                octant = 1;
            }
            else if(topSide && !rightSide && !longX)
            {
                octant = 8;
            }
            else if (topSide && !rightSide && longX)
            {
                octant = 7;
            }
            else if (!topSide && !rightSide && longX)
            {
                octant = 6;
            }
            else if (!topSide && !rightSide && !longX)
            {
                octant = 5;
            }
            else if (!topSide && rightSide && !longX)
            {
                octant = 4;
            }

            switch (octant)
            {
                case 1:
                    // Origin remains the same.
                    // Destination is transformed using deltas.
                    deltaX = xd - xo;
                    deltaY = yo - yd;
                    x0 = xo;
                    y0 = yo;
                    x1 = xo + deltaY;
                    y1 = yo + deltaX;
                    break;
                case 2:
                    // Invert the y of origin and destination.
                    x0 = xo;
                    y0 = yd;
                    x1 = xd;
                    y1 = yo;
                    break;
                case 3:
                    x0 = xo;
                    y0 = yo;
                    x1 = xd;
                    y1 = yd;
                    break;
                case 4:
                    // Origin remains the same.
                    // Destination is transformed using deltas.
                    deltaX = xd - xo;
                    deltaY = yd - yo;
                    x0 = xo;
                    y0 = yo;
                    x1 = xo + deltaY;
                    y1 = yo + deltaX;
                    break;
                case 7:
                    // Invert origin and destination.
                    x0 = yo;
                    x1 = xo;
                    y0 = yd;
                    y1 = xd;
                    break;
            }


            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            bool invert = x0 > x1;
            if (steep)
            {
                int t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }
            if (invert)    // Inverted
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int xstep = (invert) ? -1 : 1;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            if (xstep == 1)
            {
                // We're reading left to right.
                for (int x = x0; x <= x1; x++)
                {
                    if (x >= 0 && y >= 0 && x < m.XDimension && y < m.YDimension)
                    {
                        if (steep)
                        {
                            results.Add(m.TileArray[y, x]);
                        }
                        else
                        {
                            results.Add(m.TileArray[x, y]);
                        }
                    }
                    error = error - dy;
                    if (error < 0)
                    {
                        y += ystep;
                        error += dx;
                    }
                }
            }
            else
            {
                // We're reading right to left.
                for (int x = x1; x >= x0; x--)
                {
                    if (x >= 0 && y >= 0 && x < m.XDimension && y < m.YDimension)
                    {
                        if (steep)
                        {
                            results.Add(m.TileArray[y, x]);
                        }
                        else
                        {
                            results.Add(m.TileArray[x, y]);
                        }
                    }
                    error = error - dy;
                    if (error < 0)
                    {
                        y += ystep;
                        error += dx;
                    }
                }
            }
            results = results.Distinct().ToList();
            return results;
        }

        //public static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
        //{
        //    bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        //    if (steep)
        //    {
        //        int t;
        //        t = x0; // swap x0 and y0
        //        x0 = y0;
        //        y0 = t;
        //        t = x1; // swap x1 and y1
        //        x1 = y1;
        //        y1 = t;
        //    }
        //    if (x0 > x1)
        //    {
        //        int t;
        //        t = x0; // swap x0 and x1
        //        x0 = x1;
        //        x1 = t;
        //        t = y0; // swap y0 and y1
        //        y0 = y1;
        //        y1 = t;
        //    }
        //    int dx = x1 - x0;
        //    int dy = Math.Abs(y1 - y0);
        //    int error = dx / 2;
        //    int ystep = (y0 < y1) ? 1 : -1;
        //    int y = y0;
        //    for (int x = x0; x <= x1; x++)
        //    {
        //        yield return new Point((steep ? y : x), (steep ? x : y));
        //        error = error - dy;
        //        if (error < 0)
        //        {
        //            y += ystep;
        //            error += dx;
        //        }
        //    }
        //    yield break;
        //}
    }
}
