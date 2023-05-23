using System;
using System.Collections.Generic;

[Serializable]
public class InventoryData
{
    public Dictionary<string, Inventory> Inventories { get; set; }
    public Dictionary<string, Inventory> InventoriesGathered { get; set; }

    public int InventoryGatherMaxCount { get; set; }

    public InventoryData()
    {
    }

    public InventoryData( Dictionary<string, Inventory> inventories, int inventoryGatherMaxCount )
    {
        Inventories = inventories;
        InventoryGatherMaxCount = inventoryGatherMaxCount;
    }

    public InventoryData( Dictionary<string, Inventory> inventories, Dictionary<string, Inventory> inventoriesGathered, int inventoryGatherMaxCount )
    {
        Inventories = inventories;
        InventoriesGathered = inventoriesGathered;
        InventoryGatherMaxCount = inventoryGatherMaxCount;
    }
}