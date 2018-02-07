/*
	PlayerInventory.cs
	Created 12/7/2017 3:33:11 PM
	Project Boardgame by Base Games
*/
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [System.Serializable]
    public class PlayerInventory
	{
        public List<string> InventoryItems = new List<string>();

        public bool GetItem(string item)
        {
            for (int i = 0; i < InventoryItems.Count; i++)
            {
                if(item == InventoryItems[i])
                {
                    return true;
                }
            }
            return false;
        }

        public void AddInventoryItem(string itemName)
        {
            if(InventoryItems.Count < 10)
            {
                InventoryItems.Add(itemName);
            }
            else if(InventoryItems.Count >= 10)
            {
                if(itemName == "Teleport Key")
                {
                    for (int i = 0; i < InventoryItems.Count; i++)
                    {
                        if(InventoryItems[i] != "Teleport Key")
                        {
                            RemoveInventoryItem(InventoryItems[i]);
                            InventoryItems.Add(itemName);
                            return;
                        }
                    }
                }
            }
        }

        public bool IsItemInInventory(string itemName)
        {
            for (int i = 0; i < InventoryItems.Count; i++)
            {
                if (InventoryItems[i] == itemName)
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveInventoryItem(string itemName)
        {
            for (int i = 0; i < InventoryItems.Count; i++)
            {
                if (InventoryItems[i] == itemName)
                {
                    InventoryItems.RemoveAt(i);
                    break;
                }
            }
        }

        public bool HasEnoughTeleportKeys()
        {
            int numberOfKeys = 0;
            for (int i = 0; i < InventoryItems.Count; i++)
            {
                if (InventoryItems[i] == "Teleport Key")
                    numberOfKeys += 1;
            }

            if (numberOfKeys >= 3)
                return true;
            else
                return false;
        }
	}
}