using RLNETConsoleGame.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Core
{
    public class Store
    {
        private List<Item> storeItems; // List of items available in the store

        public Store()
        {
            // Initialize the store with some items
            storeItems = new List<Item>
        {
            new Item("First Aid Kit", 35, 1), // Item name, gold cost, quantity
            new Item("Drugs", 75, 50),
            new Item("Armour Lv.1", 20, 1),
            new Item("Armour Lv.2", 35, 1),
            new Item("Armour Lv.3", 50, 1),
            new Item("Gun", 45, 1),
            new Item("Knife", 25, 1)
            
        };
        }

        // Display the available items in the store
        public void DisplayStoreItems()
        {
            Game.MessageLog.Add("Welcome to the store! Available items:");
            foreach (var item in storeItems)
            {
               Game.MessageLog.Add($"{item.Name} - {item.GoldCost} gold");
            }
        }

        // Buy an item from the store
        public bool BuyItem(PlayerInventory playerInventory, string itemName)
        {
            var itemToBuy = storeItems.Find(item => item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (itemToBuy != null)
            {
                if (playerInventory.HasEnoughGold(itemToBuy.GoldCost))
                {
                    playerInventory.AddToInventory(itemToBuy);
                    playerInventory.DeductGold(itemToBuy.GoldCost);
                    Game.MessageLog.Add($"You bought {itemToBuy.Name}!");
                    return true;
                }
                else
                {
                    Console.WriteLine("Not enough gold to buy this item.");
                }
            }
            else
            {
                Game.MessageLog.Add("Item not found in the store.");
            }
            return false;
        }
    }
}
