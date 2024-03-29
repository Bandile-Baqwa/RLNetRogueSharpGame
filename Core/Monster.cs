﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RLNETConsoleGame.Behaviors;
using RLNETConsoleGame.Core;
using RLNETConsoleGame.Systems;


namespace RLNETConsoleGame.Core
{
    public class Monster : Actor
    {
        public int? TurnsAlerted { get; set; }      //able to be nullable so that the monster can be "unalerted"

       
        public void DrawStats(RLConsole statConsole, int position)
        {
            //starting at Y = 13 because if you look at Player.Drawstats() youll see the last player stat is Y = 9
            //this will place the monstet stats below the pplayers last stat
            int yPosition = 13 + (position * 2);

            //this prints the symbol of the monsters inthe room and their repective colors 
            statConsole.Print(1, yPosition, Symbol.ToString(), Color);

            int width = Convert.ToInt32(((double) Health / (double) MaxHealth ) * 16.0);
            int remainingWidth = 16 - width;

            //these colors will show how damaaged the monsters are 
            statConsole.SetBackColor(3, yPosition, width, 1, Palatte.Primary);
            statConsole.SetBackColor(3 + width, yPosition, remainingWidth, 1, Palatte.PrimaryDarkest);

            //this will print the monster name over the top of the health bar 
            statConsole.Print(2, yPosition, $": {Name}", Palatte.DbLight);
        }

        //simce this is virtual and in monster all monsters will have this by default, Override switch can be used to add in tailor made monster actions 
        public virtual void PerformAction(CommandSystem commandSystem)
        {
            var behavior = new StandardMoveAndAttack();
            behavior.Act(this, commandSystem);
        }

    }
}
