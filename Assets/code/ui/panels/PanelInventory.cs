using Core.Managers;

using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace UI
{
    public class PanelInventory : UIBasePanel
    {
        [SerializeField]
        private Button buttonOpenInventory;

        [SerializeField]
        private Button buttonCloseInventory;

        [SerializeField]
        private Transform popup;

        [SerializeField]
        private Transform itemsParent;

        private Dictionary<string, Inventory> _inventoriesCollected;
        private Dictionary<string, List<TMP_Text>> _items;

        public override void Initialize()
        {
            _inventoriesCollected = new Dictionary<string, Inventory>();
            _items = new Dictionary<string, List<TMP_Text>>();

            buttonOpenInventory.onClick.AddListener( () =>
            {
                ShowPopup( true );
            } );
            buttonCloseInventory.onClick.AddListener( () =>
            {
                ShowPopup( false );
            } );

            GameManager.Container.Resolve<SignalBus>().Subscribe<LocalDataSignal>( HandleData );

            base.Initialize();
        }

        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }

        private void ShowPopup( bool isShow )
        {
            popup.gameObject.SetActive( isShow );
            if ( isShow == true )
            {
                if ( _inventoriesCollected != null )
                {
                    foreach ( var kvp in _inventoriesCollected )
                    {
                        if ( _items.ContainsKey( kvp.Value.ID ) == true )
                        {
                            _items[ kvp.Value.ID ][ 0 ].text = kvp.Value.Name;
                            _items[ kvp.Value.ID ][ 1 ].text = kvp.Value.Count.ToString();
                            continue;
                        }
                        GameObject prefabObject = Resources.Load<GameObject>( "prefabs/ui/ui_panel_inventory/inventory_item_0" );
                        Transform instatiated = UnityEngine.Object.Instantiate( prefabObject ).transform;
                        instatiated.SetParent( itemsParent.transform, false );

                        TMP_Text textName = instatiated.FindInHierarchy( "text_name" ).GetComponent<TMP_Text>();
                        TMP_Text textCount = instatiated.FindInHierarchy( "text_count" ).GetComponent<TMP_Text>();

                        textName.text = kvp.Value.Name;
                        textCount.text = kvp.Value.Count.ToString();

                        instatiated.SetAsFirstSibling();

                        instatiated.name = kvp.Value.ID;

                        _items.Add( kvp.Value.ID, new List<TMP_Text>() { textName, textCount } );
                    }
                }
            }
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
            }
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

        public void RemoveCollectedInventoryCount( Inventory inventory )
        {
            if ( _inventoriesCollected.ContainsKey( inventory.ID ) == true )
            {
                _inventoriesCollected[ inventory.ID ].Count -= inventory.Count;
            }
        }
    }
}