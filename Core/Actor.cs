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
        //these are the implementaions from IDrawable
        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        //these are the implementations from IActor
        private int _attack;
        private int _attackChance;
        private int _awareness;
        private int _defense;
        private int _defenseChance;
        private int _gold;
        private int _health;
        private int _maxHealth;
        private string _name;
        private int _speed;

        //auto properties arent used incase we want to update them on each property , this will add more functionality later on 
        public int Attack
        {
            get
            {
                return _attack;
            }
            set
            {
                _attack = value;
            }
        }
        public int AttackChance
        {
            get
            {
                return _attackChance;
            }
            set
            {
                _attackChance = value;
            }
        }
        public int Awareness
        {
            get
            {
                return _awareness;
            }
            set
            {
                _awareness = value;
            }
        }
        public int Defense
        {
            get
            {
                return _defense;
            }
            set
            {
                _defense = value;
            }
        }
        public int DefenseChance
        {
            get
            {
                return _defenseChance;
            }
            set
            {
                _defenseChance = value;
            }
        }
        public int Gold
        {
            get
            {
                return _gold;
            }
            set
            {
                _gold = value;
            }
        }
        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }
        public int MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                _maxHealth = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }
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
