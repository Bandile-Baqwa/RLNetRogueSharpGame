using RLNET;
using RLNETConsoleGame.Interfaces;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Core
{
    public class Door : IDrawable
    {
        public Door()
        {
            Symbol = '+';       //door is closed by default 
            Color = Colors.Door;
            BackgroundColor = Colors.DoorBackground;
        }

        public bool IsOpen { get; set; }
        public RLColor Color { get; set; }
        public RLColor BackgroundColor { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void Draw(RLConsole console, IMap map)
        {
            if (!map.GetCell(X, Y).IsExplored)
            {
                return;
            }
            Symbol = IsOpen ? '-' : '+';       //this is a consise way of writing a if-else statement (?) with type boolean (IsOpen), true'-' false '+'
            if (map.IsInFov(X, Y))
            {
                Color = Colors.DoorFov;
                BackgroundColor = Colors.DoorBackgroundFov;
            }
            else
            {
                Color = Colors.Door;
                BackgroundColor = Colors.DoorBackground;
            }
            console.Set(X, Y, Color, BackgroundColor, Symbol);
        }
    }
}
