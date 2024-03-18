using RLNET;
using RLNETConsoleGame.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Core
{
    public class PlayerInventory
    {
        private List<Item> playerInventoryList;
        public PlayerInventory()
        {
            playerInventoryList = new List<Item>()      // the list that all the bought items will be sent to 
            {
                new Item("Skeleton Key",0,1)
            };
        }
        public void DisplayPlayerInventory(RLConsole inventoryConsole)
        {
            foreach (var item in playerInventoryList)
            {
                inventoryConsole.Print(1, 1, $" Player Inventory :{playerInventoryList}", Colors.Text);
            }
        }
    }
}
