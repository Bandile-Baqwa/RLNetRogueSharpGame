using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using System.Drawing;
using RLNETConsoleGame.Core;
using RLNETConsoleGame.Systems;
using RogueSharp.Random;

namespace RLNETConsoleGame
{
    public class Game
    {
        

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

        private static int _mapLevel = 1;
        private static bool _renderRequired = true;

        //private static int _steps = 0;                      //for testing MessageLog purposes *NOT FOR PRODUCTION*
        public static Player Player { get; set; }
        public static DungeonMap DungeonMap { get; private set; }
        public static CommandSystem CommandSystem { get; private set; }
        public static MessageLog MessageLog { get; private set; }
        public static IRandom Random { get; set; }      //this will be used thru out the game to generate random numbers (rogueSharp - Singleton )
        public static SchedulingSystem SchedulingSystem { get; private set; }

        public static void Main()
        {
            int seed = (int)DateTime.UtcNow.Ticks;     //this establishes the seed for the random number gnerator from the current time
            Random = new DotNetRandom(seed);        // this will produce a unique seed everytime a new game is stared 

            //the title name will includde the seed used to generate the level
            string consoleTitle = $"Bandiles RLNet Console - Level {_mapLevel} - Seed{seed}";

            //create a new MessageLog and print the random seeed to generate the level
            MessageLog = new MessageLog();
            MessageLog.Add("Agent Bands arrives on level 1");
            MessageLog.Add($"Level created with seed '{seed}'");

            //telling the RLNet to use the bitmap and setting the dimentions for the tile which are 8x8
            _rootConsole = new RLRootConsole("C:\\Users\\bsbaq\\source\\repos\\RLNETConsoleGame\\terminal8x8.png", _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);

            //the next few lines of code is me instantiating the sub consoles 
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messagesConsole = new RLConsole(_messagesWidth, _messagesHeight);
            _statsConsole = new RLConsole(_statsWidth, _statsHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            SchedulingSystem = new SchedulingSystem();


            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight,20,13,7, _mapLevel);    //the numbers are the paramerters for the Max Rooms and Size and Min size
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();
            CommandSystem = new CommandSystem();

            //this set ups the handler to update
            _rootConsole.Update += OnRootConsoleUpdate;

            //this does the same as above but renders it 
            _rootConsole.Render += OnRootConsoleRender;
            _rootConsole.Run();

        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            bool didPlayerAct = false;
            var storeItems = new Store();
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();          // this links the keyboard presses to the players movemenmts and COmmandSystem.MovePlayer() handels all the movement
            if (CommandSystem.IsPlayerTurn)
            {
                if (keyPress != null)
                {
                    if (keyPress.Key == RLKey.Up)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                    }
                    else if (keyPress.Key == RLKey.Down)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                    }
                    else if (keyPress.Key == RLKey.Left)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                    }
                    else if (keyPress.Key == RLKey.Right)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                    }
                    else if (keyPress.Key == RLKey.Escape)
                    {
                        _rootConsole.Close();
                    }
                    else if(keyPress.Key == RLKey.S)
                    {
                        storeItems.DisplayStoreItems();
                    }
                    else if (keyPress.Key == RLKey.Period)
                    {
                        if (DungeonMap.CanMoveDownToNextLevel())
                        {
                            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 13, 7, ++_mapLevel);
                            DungeonMap = mapGenerator.CreateMap();
                            MessageLog = new MessageLog();
                            CommandSystem = new CommandSystem();
                            _rootConsole.Title = $"Bandile's RLNet Console - Level {_mapLevel}";
                            didPlayerAct = true;
                        }
                    }
                }
                if (didPlayerAct)
                {
                    //MessageLog.Add($"Step # {++_steps}");     this is MessageLog testing *NOT FOR PRODUCTION*
                    _renderRequired = true;
                    CommandSystem.EndPlayerTurn();
                }
            }
            else
            {
                CommandSystem.ActivateMonsters();
                _renderRequired = true;
            }


            // this block of code sets the color and names/ labels in the sub COnsoles within the RootConsole
            //_mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Colors.FloorBackground);
            //_mapConsole.Print(1, 1, "Map", Colors.TextHeading);         //Colors here are taken from the colors.cs instead of using RLColor.Black

            //_messagesConsole.SetBackColor(0, 0, _messagesWidth, _messagesHeight, Palatte.DbDeepWater);
            //_messagesConsole.Print(1, 1, "Messages", Colors.TextHeading);        // color here is taken from the palatte.cs
            

            //_statsConsole.SetBackColor(0, 0, _statsWidth, _statsHeight, Palatte.DbOldStone);
            //_statsConsole.Print(1, 1, "Stats", Colors.TextHeading);

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Palatte.DbWood);
            _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);

        }
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            if (_renderRequired)
            {
                _mapConsole.Clear();
                _statsConsole.Clear();
                _messagesConsole.Clear();

             DungeonMap.Draw(_mapConsole,_statsConsole);
             
             Player.Draw(_mapConsole, DungeonMap);
             MessageLog.Draw(_messagesConsole);
             Player.DrawStats(_statsConsole);
                

            // this block of code transfers the information from the sub consoles to RootConsole (BLIT)
            RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
            RLConsole.Blit(_statsConsole, 0, 0, _statsWidth, _statsHeight, _rootConsole, _mapWidth, 0);
            RLConsole.Blit(_messagesConsole, 0, 0, _messagesWidth, _messagesHeight, _rootConsole, 0, _screenHeight - _messagesHeight);
            RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

                _rootConsole.Draw();


                _renderRequired = false;
            }
        }
    }
}
