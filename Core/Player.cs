using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Core
{
    public class Player : Actor
    {
        public Player()                 // of these properties are aval because it inherited from class Actor
        {
            Awareness = 15;
            Name = "Sterling Archer";
            Color = Colors.Player;
            Symbol = '@';
            X = 10;                     //X and Y are the starting pos of player
            Y = 10;
        }
    }
}
