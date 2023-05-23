using Core.Managers;

using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace UI
{
    public class PanelUserCurrency : UIBasePanel
    {
        [SerializeField]
        private int _currencyEnergyValue;
        [SerializeField]
        private int _currencyGoldValue;
        [SerializeField]
        private int _currencyRubyValue;

        [SerializeField]
        private TMP_Text _currencyEnergy;
        [SerializeField]
        private TMP_Text _currencyGold;
        [SerializeField]
        private TMP_Text _currencyRuby;

        [SerializeField]
        private Button _currencyEnergyButton;
        [SerializeField]
        private Button _currencyGoldButton;
        [SerializeField]
        private Button _currencyRubyButton;

        public override void Initialize()
        {
            _currencyEnergyButton.onClick.AddListener( () => {
                GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_shop_currency" ).Show();
            } );
            _currencyGoldButton.onClick.AddListener( () => {
                GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_shop_currency" ).Show();
            } );
            _currencyRubyButton.onClick.AddListener( () => {
                GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_shop_currency" ).Show();
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

        private void HandleData( LocalDataSignal localDataSignal )
        {
            if ( typeof( UserInfoData ).Name == localDataSignal.TypeName && localDataSignal.State == LocalDataState.UpdateData )
            {
                UserInfoData userInfoData = localDataSignal.Data as UserInfoData;

                StaticCoroutine.StartCoroutine( UpdateViewedCurrency( _currencyEnergy, _currencyEnergyValue, userInfoData.UserCurrencyEnenrgy, () => { _currencyEnergyValue = userInfoData.UserCurrencyEnenrgy; } ) );
                StaticCoroutine.StartCoroutine( UpdateViewedCurrency( _currencyGold, _currencyGoldValue, userInfoData.UserCurrencyGold, () => { _currencyGoldValue = userInfoData.UserCurrencyGold; } ) );
                StaticCoroutine.StartCoroutine( UpdateViewedCurrency( _currencyRuby, _currencyRubyValue, userInfoData.UserCurrencyRuby, () => { _currencyRubyValue = userInfoData.UserCurrencyRuby; } ) );
            }
        }

        private IEnumerator UpdateViewedCurrency( TMP_Text currencyLabel, int viewedCurrency, int currentCurrency, System.Action returnAction )
        {
            while ( viewedCurrency != currentCurrency )
            {
                int difference = viewedCurrency - ( currentCurrency );
                if ( difference > 2000000 )
                {
                    viewedCurrency -= 1000000;
                }
                else if ( difference > 200000 )
                {
                    viewedCurrency -= 100000;
                }
                else if ( difference > 20000 )
                {
                    viewedCurrency -= 10000;
                }
                else if ( difference > 2000 )
                {
                    viewedCurrency -= 1000;
                }
                else if ( difference > 200 )
                {
                    viewedCurrency -= 100;
                }
                else if ( difference > 20 )
                {
                    viewedCurrency -= 10;
                }
                else if ( difference > 0 )
                {
                    viewedCurrency -= 1;
                }
                else if ( difference < -2000000 )
                {
                    viewedCurrency += 1000000;
                }
                else if ( difference < -200000 )
                {
                    viewedCurrency += 100000;
                }
                else if ( difference < -20000 )
                {
                    viewedCurrency += 10000;
                }
                else if ( difference < -2000 )
                {
                    viewedCurrency += 1000;
                }
                else if ( difference < -200 )
                {
                    viewedCurrency += 100;
                }
                else if ( difference < -20 )
                {
                    viewedCurrency += 10;
                }
                else if ( difference < 0 )
                {
                    viewedCurrency += 1;
                }
                currencyLabel.text = viewedCurrency.ToString();
                yield return true;
            }
            returnAction();
        }
    }
}