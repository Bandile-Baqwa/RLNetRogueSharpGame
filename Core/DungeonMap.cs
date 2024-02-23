using RLNET;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Core
{
    internal class DungeonMap : Map    // this map that  was inherited from  using RogueSharp 
    {
        // this draw method will be called everytime the map is updated and wil render all the items 
        public void Draw(RLConsole mapConsole)
        {
            mapConsole.Clear();
            foreach (Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }

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
            if (!IsInFov(cell.X, cell.Y))  // the colors the cells (floor and wall) need to be when not in FOV
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
    }
}
