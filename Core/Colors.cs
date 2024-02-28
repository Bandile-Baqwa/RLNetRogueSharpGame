using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Core
{
    public class Colors
    {
        public static RLColor FloorBackground = RLColor.Black;
        public static RLColor Floor = Palatte.AlternateDarkest;
        public static RLColor FloorBackgroundFov = Palatte.DbDark;
        public static RLColor FloorFov = Palatte.Alternate;

        public static RLColor WallBackground = Palatte.SecondaryDarkest;
        public static RLColor Wall = Palatte.Secondary;
        public static RLColor WallBackgroundFov = Palatte.SecondaryDarker;
        public static RLColor WallFov = Palatte.SecondaryLighter;

        public static RLColor TextHeading = Palatte.DbLight;
        public static RLColor Text = Palatte.DbLightText;
        public static RLColor Gold = Palatte.DbSun;

        public static RLColor Player = Palatte.DbLight;
    }
}
