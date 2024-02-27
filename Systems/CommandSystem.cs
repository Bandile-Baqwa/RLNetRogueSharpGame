using RLNETConsoleGame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Systems
{
    public class CommandSystem
    {
        public bool MovePlayer(Direction direction)     // return true if the player was abel to moveand reutrn false if not ie hit a wall or seomthing 
        {
            int x = Game.Player.X;
            int y = Game.Player.Y;

            switch (direction)
            {
                case Direction.Up:
                    {
                        y = Game.Player.Y - 1;
                        break;
                    }
                    case Direction.Down:
                    {
                        y = Game.Player.Y + 1;
                        break;
                    }
                case Direction.Left:
                    {
                        x = Game.Player.X - 1;
                        break;
                    }
                case Direction.Right:
                    {
                        x = Game.Player.X + 1;
                            break;
                    }
                default:
                    {
                        return false;
                    }
            }
            if (Game.DungeonMap.SetActorPosition(Game.Player, x, y))    //this moves the plaer to a new position and return the result ,true if moved false if didnt 
            {
                return true;
            }
            return false;
        }
    }
}
