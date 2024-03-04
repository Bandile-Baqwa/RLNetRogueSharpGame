using RLNET;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Core
{
    public class DungeonMap : Map    // this map that  was inherited from  using RogueSharp 
    {
        public readonly List<Monster> _monsters;
        public List<Rectangle> Rooms;
        public DungeonMap()                 //this jsut initializes the new list of rooms & monsters when the map is created 
        {
            _monsters = new List<Monster>();
            Rooms = new List<Rectangle>();
        }

        // this draw method will be called everytime the map is updated and wil render all the items 
        public void Draw(RLConsole mapConsole, RLConsole statConsole)
        {
            //mapConsole.Clear();
            foreach (Cell cell in GetAllCells())        // this loop makes the bottom method happen for every cell
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }

            int i = 0;

            foreach (Monster monster in _monsters)      //this iterates thru all the monsters on the map after the cells have been  drawn above
            {
                monster.Draw(mapConsole, this);

                //this will show the stats of the monsters in FOV
                if (IsInFov(monster.X, monster.Y))
                {
                    monster.DrawStats(statConsole, i);
                    i++;
                }
            }

        }

        public void AddPlayer(Player player)
        {
            Game.Player = player;
            SetIsWalkable(player.X, player.Y, false);
            UpdatePlayerFieldOfView();
        }

        public void AddMonsters(Monster monster)
        {
            _monsters.Add(monster);
            //this sets that the monster is not walkable 
            SetIsWalkable(monster.X, monster.Y, false);
            
        }

        public void RemoveMonsters(Monster monster)
        {
            _monsters.Remove(monster);
            SetIsWalkable(monster.X, monster.Y, true);  // this is set to true because the monster would of been killed and removed for the map so the cell will be walkable again 
        }


        public void SetIsWalkable(int x, int y, bool isWalkable)        // this bool method helps set the isWalkable property on a cell being true or false 
        {
            Cell cell = (Cell)GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }


        //this looks for a random place in the room thats walkable so the monster can spawn there 
        public Point? GetRandomWalkableLocationInRoom(Rectangle room)
        {
            if (DoesRoomHaveWalkableSpace(room))
            {
                for (int i = 0; i < 100; i++)
                {
                    int x = Game.Random.Next(1, room.Width - 2) + room.X;
                    int y = Game.Random.Next(1, room.Height - 2) + room.Y;
                    if (IsWalkable(x, y))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return null;
        }

        public Monster GetMonsterAt(int x, int y)
        {
            //the type of this lambda expression is a Func<Monster, bool> of type Monster
            return _monsters.FirstOrDefault(m => m.X == x && m.Y == y); //basically Monster => monster.X == int x && monster.Y == int y 
        }

        public bool DoesRoomHaveWalkableSpace(Rectangle room)
        {
            for (int x = 1; x <= room.Width - 2; x++)
            {
                for (int y = 1; y <= room.Height - 2; y++)
                {
                    if (IsWalkable(x + room.X, y + room.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
        {
            if (!cell.IsExplored)
            {
                return; // this will make sure if we havent explored the cell nothing must be drawn
            }

            if (IsInFov(cell.X, cell.Y))    //code to  make a cell lighter if its in Field of view (IsInFov)
            {
                //now we have to code in everything we want the cells to do when inFOV 
                //we will use a symbol to draw if the cell is walkable or not (floor . or wall #)
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallFov,Colors.WallBackgroundFov,'#');
                }
            }
            else   // the colors the cells (floor and wall) need to be when not in FOV
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
                }
            }
        }

        public void UpdatePlayerFieldOfView()       // this is created so every time you move the player the FOV is updated 
        {
            Player player = Game.Player;            //this is taken from the Player property (getter setter) in the Game.cs

            ComputeFov(player.X, player.Y, player.Awareness, true);     //ComputeFOV is bulit in RLNet,, it slots in the properties form the player.cs and updated them according to the pos of Player

            foreach (Cell cell in GetAllCells())    //marking all the cells in FOV as being explored to have the colors ,symbols and etc active
            {
                if (IsInFov(cell.X, cell.Y))
                {
                SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        public bool SetActorPosition(Actor actor, int x, int y)         // returns true if actor can be placed on call and false if not 
        {
            if (GetCell(x, y).IsWalkable)           //only allows the actor placement if thwe actotr is walkable
            {
                SetIsWalkable(actor.X, actor.Y, true);  // makes the previous cell that the actor was on now walkble 

                actor.X = x;                            //this updates the actors position 
                actor.Y = y;

                SetIsWalkable(actor.X, actor.Y, false); //the actors current cell position is not walkable 

                if (actor is Player)                //if the player is repositioned then we have to update the Players FOV
                {
                    UpdatePlayerFieldOfView();
                }
                return true;
            }
            return false;
        }

       


        
    }
}
