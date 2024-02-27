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
        public List<Rectangle> Rooms;
        public DungeonMap()                 //this jsut initializes the new list of rooms when the map is created 
        {
            Rooms = new List<Rectangle>();
        }
        // this draw method will be called everytime the map is updated and wil render all the items 
        public void Draw(RLConsole mapConsole)
        {
            mapConsole.Clear();
            foreach (Cell cell in GetAllCells())        // this loop makes the bottom method happen for every cell
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }

        }

        public void AddPlayer(Player player)
        {
            Game.Player = player;
            SetIsWalkable(player.X, player.Y, false);
            UpdatePlayerFieldOfView();
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

        public void SetIsWalkable(int x, int y, bool isWalkable)        // this bool method helps set the isWalkable property on a cell being true or false 
        {
            Cell cell = (Cell)GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }
    }
}
