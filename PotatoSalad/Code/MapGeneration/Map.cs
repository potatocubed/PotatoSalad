using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    public class Map
    {
        // A map is a collection of tiles.
        public int XDimension;
        public int YDimension;
        public Tile[,] TileArray;
        public List<Mobile> MobileArray = new List<Mobile>();
        public string MapID;    // Includes name, depth, etc.
        public string MapName;
        public string LevelNumber;  // For display.
        public string Depth;    // For internal use. Sort of a difficulty marker, I guess.
        public string mapType;

        public void LoadMap(string mn, string mid, int ln, int d, int xSize, int ySize, string mType)
        {
            // By the time this is called, XMLHandler.saveDir is set to point to the right place.
            ClearMap();

            XDimension = xSize - 1;
            YDimension = ySize - 1;
            mapType = mType;

            TileArray = new Tile[xSize, ySize];

            MapID = mid;
            MapName = mn;
            LevelNumber = ln.ToString();
            Depth = d.ToString();

            // LOAD MAP
            //LevelXML.Load(saveDir + "/data/" + mapID + "/mapdata.xml");
            System.IO.StreamReader mapReader = new System.IO.StreamReader(Game.XMLHandler.saveDir + "/data/" + mid + "/geography.txt");
            string mapLine;
            for (int j = 0; j <= YDimension * 2; j++)
            {
                mapLine = mapReader.ReadLine();
                char[] cArray = mapLine.ToCharArray();
                if (j <= YDimension)
                {
                    for (int i = 0; i < cArray.Length; i++)
                    {
                        TileArray[i, j] = new Tile(i, j);
                        TileArray[i, j].MakeTile(Game.GetTileTypeFromChar(cArray[i]));
                    }
                }
                else
                {
                    for (int i = 0; i < cArray.Length; i++)
                    {
                        if(cArray[i] == Convert.ToChar("."))
                        {
                            TileArray[i, j - (YDimension + 1)].IsExplored = true;
                        }
                    }
                }
            }
            mapReader.Close();
            mapReader.Dispose();
        }

        public void Generate(string mn, string mid, int ln, int d, int xSize = 80, int ySize = 25, string mType = "default")
        {
            ClearMap();

            XDimension = xSize - 1;
            YDimension = ySize - 1;
            mapType = mType;

            TileArray = new Tile[xSize, ySize];

            MapID = mid;
            MapName = mn;
            LevelNumber = ln.ToString();
            Depth = d.ToString();

            // We'll add some more creation parameters and variables later. Like depth, tileset, reference, etc.
            
           switch (mapType)
            {
                case "dungeon":
                    GenerateDungeonMap();
                    PopulateDefault();
                    break;
                case "default":
                default:
                    GenerateDefaultMap();
                    break;
            }
        }

        public void InstantiatePlayer(Tile loc)
        {
            Game.Player = new Player(loc);
            loc.Occupier = Game.Player;
            MobileArray.Add(Game.Player);
        }

        private void PopulateDefault()
        {
            // TODO
            // This function populates the map with three goblins.

            List<string> errorList = new List<string>();

            for (int i = 1; i <= 3; i++)
            {
                // First things first, find an open space.
                int targX;
                int targY;

                // It'll try 100 times to find a spawn point, then give up.
                string mType = "goblin";
                bool exit = false;
                int counter = 0;
                    
                while (!exit || !(counter < 100))
                {
                    targX = Game.Dice.XdY(1, XDimension);
                    targY = Game.Dice.XdY(1, YDimension);
                    if (!TileArray[targX, targY].BlockMovement && TileArray[targX, targY].Occupier == null)
                    {
                        // Tile is clear.
                        // Then select a monster type to put there.
                        errorList.Add($"{mType} spawned at {targX}, {targY}");
                        string uid = Game.MonPop.GenerateUniqueID(mType, MobileArray);
                        Monster mon = new Monster(TileArray[targX, targY], uid);
                        mon.monType = mType;
                        MobileArray.Add(mon);
                        TileArray[targX, targY].Occupier = mon;
                        exit = true;
                    }
                    else
                    {
                        errorList.Add($"Attempt to spawn {mType} at {targX}, {targY} failed. Blocked.");
                        targX = Game.Dice.XdY(1, XDimension);
                        targY = Game.Dice.XdY(1, YDimension);
                        counter++;  // Just a failsafe.
                    }
                }
            }

            // Then feed MobileArray into MonsterPopulater.MonsterSetup [remember to skip the player!]
            Game.MonPop.MonsterSetUp(MobileArray);
        }

        private void ClearMap()
        {
            TileArray = new Tile[0,0];
        }

        private void GenerateDungeonMap()
        {
            // A dungeon map fills the entire level with wall, then cuts ten rooms, then links them with tunnels.
            // Right now it produces quite an open-plan dungeon. Which I'm okay with?
            // But I think I need to refine the overlap parameters.
            for (int i = 0; i <= TileArray.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= TileArray.GetUpperBound(1); j++)
                {
                    TileArray[i, j] = new Tile(i, j);
                    TileArray[i, j].MakeTile("wall");
                }
            }

            // Carve some rooms.
            //List<Code.MapGeneration.Room> RoomList = new List<Code.MapGeneration.Room>();
            Code.MapGeneration.Room currentRoom = null;
            Code.MapGeneration.Room prevRoom = null;
            for (int i = 1; i <= 10; i++)
            {
                int xDim = Game.Dice.XdY(1, 7) + 3;
                int yDim = Game.Dice.XdY(1, 7) + 3;

                currentRoom = CarveRoom(xDim, yDim);
                if (currentRoom != null && prevRoom != null)
                {
                    // carve a corridor
                    int oldX = prevRoom.CentreX;
                    int oldy = prevRoom.CentreY;
                    int newx = currentRoom.CentreX;
                    int newy = currentRoom.CentreY;

                    // Go across
                    CarveLine(oldX, oldy, newx, oldy);

                    // Go down
                    CarveLine(newx, oldy, newx, newy);

                    // then put this room into the queue
                    prevRoom = currentRoom;
                }
                else if(currentRoom != null && prevRoom == null)
                {
                    prevRoom = currentRoom;
                }
            }

            // And then we drop the player in the start of the final room.
            if (currentRoom != null)
            {
                InstantiatePlayer(TileArray[currentRoom.CentreX, currentRoom.CentreY]);
            }
            else if (prevRoom != null)
            {
                InstantiatePlayer(TileArray[prevRoom.CentreX, prevRoom.CentreY]);
            }
            else
            {
                // We've got no valid rooms, somehow.
                GenerateDefaultMap();
            }
        }

        private void GenerateDefaultMap()
        {
            // The default is a 10x10 map, blank in the middle, with walls all around the edge.

            for (int i = 0; i <= TileArray.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= TileArray.GetUpperBound(1); j++)
                {
                    TileArray[i, j] = new Tile(i, j);
                    TileArray[i, j].MakeTile("floor");
                }
            }

            for (int i = 0; i <= TileArray.GetUpperBound(0); i++)
            {
                // Loop through the x-axis, adding walls on top and bottom.
                TileArray[i, 0].MakeTile("wall");
                TileArray[i, TileArray.GetUpperBound(1)].MakeTile("wall");
            }
            for (int i = 0; i <= TileArray.GetUpperBound(1); i++)
            {
                TileArray[0, i].MakeTile("wall");
                TileArray[TileArray.GetUpperBound(0), i].MakeTile("wall");
            }

            TileArray[5, 5].MakeTile("wall");   // For testing.

            // Now we've got some terrain, let's add some beasts.

            // For the default map, lets stick the player just inside the top-left corner.
            InstantiatePlayer(TileArray[2, 2]);
        }

        private Code.MapGeneration.Room CarveRoom(int xSize, int ySize, bool overlap = false)
        {
            // If overlap is set to true, then the carver doesn't care if it draws over other rooms.
            // This method only tries to carve a room 100 times -- if it can't find a good place in that
            // many goes, it gives up.

            Code.MapGeneration.Room r = new Code.MapGeneration.Room();
            r.xDimension = xSize;
            r.yDimension = ySize;

            for (int i = 1; i <= 100; i++)
            {
                // I know row 0, col 0, row yDimension, and col xDimension are forbidden. So.
                r.TopLeftX = Game.Dice.XdY(1, (XDimension - 1) - xSize);
                r.TopLeftY = Game.Dice.XdY(1, (YDimension - 1) - ySize);

                if (!overlap)
                {
                    // Check each corner to make sure it's not covered.
                    if (TileArray[r.TopLeftX, r.TopLeftY].Name != "floor" &&
                        TileArray[r.TopLeftX, r.TopLeftY + ySize - 1].Name != "floor" &&
                        TileArray[r.TopLeftX + xSize - 1, r.TopLeftY].Name != "floor" &&
                        TileArray[r.TopLeftX + xSize - 1, r.TopLeftY + ySize - 1].Name != "floor")
                    {
                        // We're good. Carve and return.
                        r.CalculateCentre();    // While I'm thinking of it.
                        for (int x = 0; x < xSize; x++)
                        {
                            for (int y = 0; y < ySize; y++)
                            {
                                TileArray[r.TopLeftX + x, r.TopLeftY + y].MakeTile("floor");
                            }
                        }
                        return r;
                    }
                }
                else
                {
                    // We're good. Carve and return.
                    r.CalculateCentre();    // While I'm thinking of it.
                    for (int x = 0; x < xSize; x++)
                    {
                        for (int y = 0; y < ySize; y++)
                        {
                            TileArray[r.TopLeftX + x, r.TopLeftY + y].MakeTile("floor");
                        }
                    }
                    return r;
                }
            }

            // If we get here, room generation has failed.
            if (Globals.DEBUG_MODE)
            {
                Game.Globals.DEBUG_ERROR_LIST.Add("Room generation failed!");
            }
            r.CentreX = -1;
            return null;
        }

        private void CarveLine(int startX, int startY, int endX, int endY)
        {
            // Carves a straight line only.
            if ((startX != endX) && (startY != endY))
            {
                // Nope.
                if (Globals.DEBUG_MODE)
                {
                    Game.Globals.DEBUG_ERROR_LIST.Add("Line carving failed! Not a straight line!");
                }
                return;
            }

            // Needs to carve backwards too.
            if (startX > endX)
            {
                int x = startX;
                startX = endX;
                endX = x;
            }
            if (startY > endY)
            {
                int y = startY;
                startY = endY;
                endY = y;
            }

            if (startX == endX)
            {
                // It's a vertical corridor.
                for (int i = startY; i <= endY; i++)
                {
                    TileArray[startX, i].MakeTile("floor");
                }
            }
            else
            {
                for (int i = startX; i <= endX; i++)
                {
                    TileArray[i, startY].MakeTile("floor");
                }
            }
        }

        public string[] MapText()
        {
            // Export a text-based rendition of the map.
            string[] mText = new string[YDimension + 1];
            string[] vText = new string[YDimension + 1];
            var mapRepresentation = new StringBuilder();
            var mapVisibility = new StringBuilder();

            for (int y = 0; y <= YDimension; y++)
            {
                for (int x = 0; x <= XDimension; x++)
                {
                    mapRepresentation.Append(TileArray[x, y].DisplayChar);
                    // And we also need to record the IsExplored property of the various tiles.
                    if (TileArray[x, y].IsExplored)
                    {
                        mapVisibility.Append(".");
                    }
                    else
                    {
                        mapVisibility.Append("#");
                    }
                }
                mText[y] = mapRepresentation.ToString();
                vText[y] = mapVisibility.ToString();
                mapRepresentation.Clear();
                mapVisibility.Clear();
            }

            // FOV will be filled in on game load.

            List<string> tList = new List<string>();
            tList.AddRange(mText);
            tList.AddRange(vText);
            string[] rText = tList.ToArray();
            return rText;
        }
    }
}
