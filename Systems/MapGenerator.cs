using RLNETConsoleGame.Core;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Systems
{
    internal class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _maxRooms;
        private readonly int _roomMaxSize;
        private readonly int _roomMinSize;

        private readonly DungeonMap _map;

        // constructing a new MapGenerator requires the demensions of the maps it will create
        public MapGenerator(int width, int height, int maxRooms, int roomMaxSize, int roomMinSize)
        {
            _width = width;
            _height = height;
            _maxRooms = maxRooms;
            _roomMaxSize = roomMaxSize;
            _roomMinSize = roomMinSize;
            _map = new DungeonMap();
        }
        public DungeonMap CreateMap()
        {
            //inistailize the cells in the map by setting walkable, transparency and explored to true
            _map.Initialize(_width, _height);

            for (int r = _maxRooms; r > 0; r--)
            {
                //this determines the size and position of the rooms randomly 
                int roomWidth = Game.Random.Next(_roomMinSize, _roomMaxSize);
                int roomHeight = Game.Random.Next(_roomMinSize, _roomMaxSize);
                int roomXPosition = Game.Random.Next(0, _width - roomWidth - 1);
                int roomYPosition = Game.Random.Next(0, _height - roomHeight - 1);

                //this will make all the rooms rectangles with in the above size and Position parameters 
                var newRoom = new Rectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);

                //this uses a bool to check if any rooms intersect (bool because it can only  return true or false)
                bool newRoomIntersects = _map.Rooms.Any(room => newRoom.Intersects(room));

                if (!newRoomIntersects)
                {
                    _map.Rooms.Add(newRoom);   // as long as it doesnt intersect it will be added to the list(the list in DungeonMap of type rectangle) of rooms
                }
            }

            foreach (Rectangle room in _map.Rooms)
            {
                CreateMap(room);
            }
            return _map;
        }

        private void CreateMap(Rectangle room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    _map.SetCellProperties(x , y , true, true, true);   // thisi set the properties of the cell to be true in any given rectangular area on the map
                }
            }
        }
    }

}
