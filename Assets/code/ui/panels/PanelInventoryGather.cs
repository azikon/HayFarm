using Core.Managers;

using DG.Tweening;

using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace UI
{
    public class PanelInventoryGather : UIBasePanel
    {
        [SerializeField]
        private Slider sliderGather;

        [SerializeField]
        private TMP_Text textGatherCount;

        [SerializeField]
        private int _gatheredCount;

        [SerializeField]
        private int _inventoryGatherMaxCount;

        [SerializeField]
        private Transform _parentAnimatedItems;

        [SerializeField]
        private Transform _targetAnimatedItems;

        private Dictionary<string, Inventory> _inventoriesGathered;

        public override void Initialize()
        {
            _inventoriesGathered = new Dictionary<string, Inventory>();

            GameManager.Container.Resolve<SignalBus>().Subscribe<LocalDataSignal>( HandleData );
            GameManager.Container.Resolve<SignalBus>().Subscribe<InventoryCollectSignal>( HandleCollect );
            GameManager.Container.Resolve<SignalBus>().Subscribe<InventoryGatherSignal>( HandleGather );

            base.Initialize();
        }

        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }

        private void UpdateData()
        {
            sliderGather.value = ( float )_gatheredCount / ( float )_inventoryGatherMaxCount;
            textGatherCount.text = _gatheredCount.ToString() + " / " + _inventoryGatherMaxCount.ToString();
        }

        private void HandleData( LocalDataSignal localDataSignal )
        {
            if ( typeof( InventoryData ).Name == localDataSignal.TypeName && localDataSignal.State == LocalDataState.UpdateData )
            {
                InventoryData inventoryData = localDataSignal.Data as InventoryData;

                if ( inventoryData.InventoryGatherMaxCount > 0 )
                {
                    _inventoryGatherMaxCount = inventoryData.InventoryGatherMaxCount;
                }
            }
        }

        private void HandleGather( InventoryGatherSignal inventoryGatherSignal )
        {
            AddGatheredInventoryCount( new Inventory( inventoryGatherSignal.Inventory ) );
            UpdateData();
        }

        private void HandleCollect( InventoryCollectSignal inventoryCollectSignal )
        {
            foreach ( var kvp in _inventoriesGathered )
            {
                AddItemAnimation( "prefabs/ui/ui_panel_inventory/inventory_item_0_animated", kvp.Value.Count, () => { } );
            }

            _inventoriesGathered.Clear();
            _gatheredCount = 0;
            UpdateData();
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

        private void AddItemAnimation( string prefabPath, int addedExperience, System.Action returnMethod )
        {
            Transform item = Instantiate( Resources.Load<GameObject>( prefabPath ) ).transform;
            item.SetParent( _parentAnimatedItems, false );

            item.Find( "text_experience" ).GetComponent<TMP_Text>().text = "+" + addedExperience.ToString();

            Camera envCamera = GameManager.Container.Resolve<SceneObjectsManager>().EnvironmentCamera;
            Transform character = GameManager.Container.Resolve<SceneObjectsManager>().RootCharacterMain.transform;

            Vector3 startWorldPosition = envCamera.WorldToScreenPoint( character.position );
            Vector3 targetScreenPosition = _targetAnimatedItems.position;

            item.position = startWorldPosition;
            Vector3[] path = GetAnimationPath( startWorldPosition, targetScreenPosition );
            ( item as RectTransform ).DOPath( path, 1f ).OnComplete( () => { returnMethod(); Destroy( item.gameObject ); } );
        }

        Vector3[] GetAnimationPath( Vector3 startWorldPosition, Vector3 targetScreenPosition )
        {
            Vector3[] path = new Vector3[ 3 ];
            path[ 0 ] = startWorldPosition;
            path[ 0 ] = new Vector3( path[ 0 ].x, path[ 0 ].y, 0.0f );
            path[ 1 ] = new Vector3( path[ 0 ].x + 280f, path[ 0 ].y - 120f, 0.0f );
            path[ 2 ] = targetScreenPosition;
            path[ 2 ] = new Vector3( path[ 2 ].x, path[ 2 ].y, 0.0f );
            return path;
        }
    }
}