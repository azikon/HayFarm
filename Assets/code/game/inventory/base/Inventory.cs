using System;

[Serializable]
public class Inventory : IInventory
{
    public string ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public int Count { get; set; }

    public Inventory() { }

    public Inventory( Inventory inventory )
    {
        ID = inventory.ID;
        Name = inventory.Name;
        Description = inventory.Description;
        Type = inventory.Type;
        Count = inventory.Count;
    }
}