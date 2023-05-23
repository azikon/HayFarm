using Core.Managers;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace UI
{
    public class PanelChangeName : UIBasePanel
    {
        [SerializeField]
        private Button buttonCloseSettings;

        [SerializeField]
        private Button buttonChangeName;

        [SerializeField]
        private TMP_InputField _InputField;

        private UserInfoData _userInfoData;

        public override void Initialize()
        {
            buttonCloseSettings.onClick.AddListener( () => { Hide(); } );
            buttonChangeName.onClick.AddListener( () => { ChangeName(); } );

            GameManager.Container.Resolve<SignalBus>().Subscribe<LocalDataSignal>( HandleData );

            base.Initialize();
        }

        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }

        private void ChangeName()
        {
            string userName = _InputField.text;

            if ( _userInfoData != null && string.IsNullOrEmpty( userName ) == false && userName.Length > 3 )
            {
                _userInfoData.UserName = userName;
                ChangeData();
                Hide();
            }
        }

        private void HandleData( LocalDataSignal localDataSignal )
        {
            if ( typeof( UserInfoData ).Name == localDataSignal.TypeName && localDataSignal.State == LocalDataState.UpdateData )
            {
                _userInfoData = localDataSignal.Data as UserInfoData;

                _InputField.text = _userInfoData.UserName;
            }
        }

        private void ChangeData()
        {
            GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( _userInfoData, LocalDataState.SaveData ) );
        }
    }
}