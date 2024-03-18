using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Systems
{
    public class Item
    {
        public string Name { get; }
        public int GoldCost { get; }
        public int Quantity { get; set; }

        public Item(string name, int goldCost, int quantity)    // all items will take this signature 
        {
            Name = name;
            GoldCost = goldCost;
            Quantity = quantity;
        }
    }
}
