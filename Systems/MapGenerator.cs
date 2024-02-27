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

            for (int r = 1; r < _map.Rooms.Count; r++)
            {   
                //cycle thru each room that was created starting from the second room
                int previousRoomCenterX = _map.Rooms[r - 1].Center.X;
                int previousRoomCenterY = _map.Rooms[r - 1].Center.Y;
                int currentRoomCenterX = _map.Rooms[r].Center.X;
                int currentRoomCenterY = _map.Rooms[r].Center.Y;

                //this creates a 50% chance for L shapped hallway to tunnel out 
                if (Game.Random.Next(1, 2) == 1)
                {
                    CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                }
                else
                {
                    CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                }
            }

            foreach (Rectangle room in _map.Rooms)
            {
                CreateRoom(room);
            }
            PlacePlayer();
            return _map;
        }

        private void PlacePlayer()
        {
            Player player = Game.Player;

            if (player == null)
            {
                player = new Player();
            }

            player.X = _map.Rooms[0].Center.X;  //this adds player to the center of the first room in the list of Rooms of type rectangle
            player.Y = _map.Rooms[0].Center.Y;  //X and Y coordineates

            _map.AddPlayer(player);
        }

        private void CreateRoom(Rectangle room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    _map.SetCellProperties(x , y , true, true, false);   // thisi set the properties of the cell to be true in any given rectangular area on the map
                }                                               //set to false so that the rooms dont show before you can explore them 
            }

        }

        private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition)
        {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                _map.SetCellProperties(x, yPosition, true, true);
            }
        }
                                                                            //code above and below place tunnels horizonally and vertiacally 
        private void CreateVerticalTunnel(int yStart, int yEnd,int xPosition)
        {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                _map.SetCellProperties(xPosition, y, true, true);
            }
        }
    }

}
