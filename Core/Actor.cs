using RLNET;
using RogueSharp;
using RLNETConsoleGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Core
{
    public class Actor : IActor, IDrawable
    {
        //these are the implementations from IActor
        public string Name { get; set; }
        public int Awareness { get; set; }

        //these are the implementaions from IDrawable
        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public void Draw(RLConsole console, IMap map)      // this must decalare a body because its not marked as a abstract or partial
        {
            if (!map.GetCell(X, Y).IsExplored)          //this return a normal cell without any actors 
            {
                return;
            }

            if (map.IsInFov(X, Y))          //draw the actor , symbol and color in FOV 
            {
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else 
            {
                console.Set(X, Y, Colors.Floor, Colors.FloorBackground, '.');      //draw the normal color and floor symbol of floor outside of FOV
            }

        }
    }
}
