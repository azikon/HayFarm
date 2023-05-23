using System.Collections.Generic;

using Zenject;

namespace Core.Managers
{
    public class InventoryManager : BaseManager
    {
        private Dictionary<string, Inventory> _inventoriesCollected;
        private Dictionary<string, Inventory> _inventoriesGathered;

        private int _gatheredCount;
        private int _inventoryGatherMaxCount;

        protected override void OnLoad()
        {
            _inventoriesCollected = new Dictionary<string, Inventory>();
            _inventoriesGathered = new Dictionary<string, Inventory>();

            GameManager.Container.Resolve<SignalBus>().Subscribe<LocalDataSignal>( HandleData );
            GameManager.Container.Resolve<SignalBus>().Subscribe<InventoryCollectSignal>( HandleCollect );
            GameManager.Container.Resolve<SignalBus>().Subscribe<InventoryGatherSignal>( HandleGather );

            base.OnLoad();
        }

        private void HandleData( LocalDataSignal localDataSignal )
        {
            if ( typeof( InventoryData ).Name == localDataSignal.TypeName && localDataSignal.State == LocalDataState.UpdateData )
            {
                InventoryData inventoryData = localDataSignal.Data as InventoryData;
                foreach ( var kvp in inventoryData.Inventories )
                {
                    AddCollectedInventoryCount( kvp.Value );
                }
                if ( inventoryData.InventoriesGathered != null )
                {
                    foreach ( var kvp in inventoryData.InventoriesGathered )
                    {
                        AddGatheredInventoryCount( kvp.Value );
                    }
                }

                if ( inventoryData.InventoryGatherMaxCount > 0 )
                {
                    _inventoryGatherMaxCount = inventoryData.InventoryGatherMaxCount;
                }

                GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( new InventoryData( _inventoriesCollected, _inventoryGatherMaxCount ), LocalDataState.SaveData ) );
            }
            if ( localDataSignal.State == LocalDataState.InitiazlieData )
            {
                Inventory inventory = new Inventory() { ID = "inv_grass_0", Name = "Grass", Count = 1 };
                InventoryData inventoryData = new InventoryData( new Dictionary<string, Inventory>() { { inventory.ID, inventory } }, 200 );
                GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( inventoryData, LocalDataState.UpdateData ) );
            }
        }

        private void HandleGather( InventoryGatherSignal inventoryGatherSignal )
        {
            AddGatheredInventoryCount( new Inventory( inventoryGatherSignal.Inventory ) );
        }

        private void HandleCollect( InventoryCollectSignal inventoryCollectSignal )
        {
            Dictionary<string, Inventory> invGatheredToCollected = new Dictionary<string, Inventory>();
            foreach ( var kvp in _inventoriesGathered )
            {
                invGatheredToCollected.Add( kvp.Key, new Inventory( kvp.Value ) );

                GameManager.Container.Resolve<SignalBus>().TryFire<UserExperienceChangeSignal>( new UserExperienceChangeSignal( 50 ) );
            }
            _inventoriesGathered.Clear();

            InventoryData inventoryData = new InventoryData( invGatheredToCollected, _inventoryGatherMaxCount );
            GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( inventoryData, LocalDataState.UpdateData ) );
        }

        public void AddCollectedInventoryCount( Inventory inventory )
        {
            if ( _inventoriesCollected.ContainsKey( inventory.ID ) == true )
            {
                _inventoriesCollected[ inventory.ID ].Count += inventory.Count;
            }
            else
            {
                Inventory addedInventory = new Inventory( inventory );
                _inventoriesCollected.Add( addedInventory.ID, addedInventory );
            }
        }

        public Inventory GetCollectedInventory( string id )
        {
            if ( _inventoriesCollected.ContainsKey( id ) == true )
            {
                return _inventoriesCollected[ id ];
            }
            return null;
        }

        public void AddGatheredInventoryCount( Inventory inventory )
        {
            if ( _gatheredCount + inventory.Count <= _inventoryGatherMaxCount )
            {
                if ( _inventoriesGathered.ContainsKey( inventory.ID ) == true )
                {
                    _inventoriesGathered[ inventory.ID ].Count += inventory.Count;
                }
                else
                {
                    Inventory addedInventory = new Inventory( inventory );
                    _inventoriesGathered.Add( addedInventory.ID, addedInventory );
                }
                _gatheredCount += inventory.Count;
            }
        }

        public Inventory GetGatheredInventory( string id )
        {
            if ( _inventoriesGathered.ContainsKey( id ) == true )
            {
                return _inventoriesGathered[ id ];
            }
            return null;
        }
    }
}