using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using System.Drawing;
using RLNETConsoleGame.Core;
using RLNETConsoleGame.Systems;

namespace RLNETConsoleGame
{
    public class Game
    {
        public static DungeonMap DungeonMap { get; private set; }
        public static Player Player { get; private set; }

        // these fields are static and readonly cause the screen dimentions shouldnt be allow to change 
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;

        //where the game its self will take place 
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;           //RLRootConsole is not used cause thats for the mainm console 
                                                        //and the RLConsole are for the other sub consoles

        //thsi will show the player and monster stats 
        private static readonly int _statsWidth = 20;
        private static readonly int _statsHeight = 70;
        private static RLConsole _statsConsole;

        // this is for the message console for all relevent information 
        private static readonly int _messagesWidth = 80;
        private static readonly int _messagesHeight = 11;
        private static RLConsole _messagesConsole;

        //this is for the players equipment, abilities and items
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;


        public static void Main()
        {
            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight);
            DungeonMap = mapGenerator.CreateMap();
            Player = new Player();

            //juat linkiing up the bitmap file
            //string fontFileName = "C:\\Users\\bsbaq\\source\\repos\\RLNETConsoleGame\\Bitmap\\terminal8x8.bmp";

            string consoleTitle = "Bandiles RLNet Console - Level 1";

            //telling the RLNet to use the bitmap and setting the dimentions for the tile which are 8x8
            _rootConsole = new RLRootConsole("C:\\Users\\bsbaq\\source\\repos\\RLNETConsoleGame\\terminal8x8.png", _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);

            //the next few lines of code is me instantiating the sub consoles 
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messagesConsole = new RLConsole(_messagesWidth, _messagesHeight);
            _statsConsole = new RLConsole(_statsWidth, _statsHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            //this set ups the handler to update
            _rootConsole.Update += OnRootConsoleUpdate;

            //this does the same as above but renders it 
            _rootConsole.Render += OnRootConsoleRender;
            _rootConsole.Run();
            DungeonMap.UpdatePlayerFieldOfView();

        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            // this block of code sets the color and names/ labels in the sub COnsoles within the RootConsole
            _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Colors.FloorBackground);
            _mapConsole.Print(1, 1, "Map", Colors.TextHeading);         //Colors here are taken from the colors.cs instead of using RLColor.Black

            _messagesConsole.SetBackColor(0, 0, _messagesWidth, _messagesHeight, Palatte.DbDeepWater);
            _messagesConsole.Print(1, 1, "Messages", Colors.TextHeading);        // color here is taken from the palatte.cs

            _statsConsole.SetBackColor(0, 0, _statsWidth, _statsHeight, Palatte.DbOldStone);
            _statsConsole.Print(1, 1, "Stats", Colors.TextHeading);

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Palatte.DbWood);
            _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);

        }
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            // this block of code transfers the information from the sub consoles to RootConsole (BLIT)
            RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
            RLConsole.Blit(_statsConsole, 0, 0, _statsWidth, _statsHeight, _rootConsole, _mapWidth, 0);
            RLConsole.Blit(_messagesConsole, 0, 0, _messagesWidth, _messagesHeight, _rootConsole, 0, _screenHeight - _messagesHeight);
            RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

            _rootConsole.Draw();
            DungeonMap.Draw(_mapConsole);
            Player.Draw(_mapConsole, DungeonMap);
        }
    }
}
