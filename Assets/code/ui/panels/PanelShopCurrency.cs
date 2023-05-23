using Core.Managers;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace UI
{
    public class PanelShopCurrency : UIBasePanel
    {
        [SerializeField]
        private Button buttonCloseShop;

        [SerializeField]
        private Button[] buttonsBuyGold;
        [SerializeField]
        private Button[] buttonsBuyRuby;


        private UserInfoData _userInfoData;

        public override void Initialize()
        {
            buttonCloseShop.onClick.AddListener( () => { Hide(); } );

            for ( int i = 0; i < buttonsBuyGold.Length; i++ )
            {
                int index = i;
                buttonsBuyGold[ index ].onClick.AddListener( () => { PressedBuyGold( index ); } );
            }

            for ( int i = 0; i < buttonsBuyRuby.Length; i++ )
            {
                int index = i;
                buttonsBuyRuby[ index ].onClick.AddListener( () => { PressedBuyRuby( index ); } );
            }

            GameManager.Container.Resolve<SignalBus>().Subscribe<LocalDataSignal>( HandleData );

            base.Initialize();
        }

        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }

        private void HandleData( LocalDataSignal localDataSignal )
        {
            if ( typeof( UserInfoData ).Name == localDataSignal.TypeName && localDataSignal.State == LocalDataState.UpdateData )
            {
                _userInfoData = localDataSignal.Data as UserInfoData;
            }
        }

        private void PressedBuyGold( int index )
        {
            int[] goldAdded = new int[ 5 ] { 1500, 4000, 12000, 25000, 60000 };
            int[] goldCoast = new int[ 5 ] { 12, 48, 120, 240, 490 };

            _userInfoData.UserCurrencyGold += goldAdded[ index ];
            _userInfoData.UserCurrencyRuby -= goldCoast[ index ];

            GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( _userInfoData, LocalDataState.UpdateData ) );
            GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( _userInfoData, LocalDataState.SaveData ) );
        }

        private void PressedBuyRuby( int index )
        {
            int[] rubyAdded = new int[ 5 ] { 40, 220, 480, 1200, 2100 };

            _userInfoData.UserCurrencyRuby += rubyAdded[ index ];

            GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( _userInfoData, LocalDataState.UpdateData ) );
            GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( _userInfoData, LocalDataState.SaveData ) );
        }
    }
}