public struct InventoryGatherSignal
{
    public Inventory Inventory { get; set; }

    public InventoryGatherSignal( Inventory inventory )
    {
        Inventory = inventory;
    }
}