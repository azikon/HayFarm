using Core.Managers;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace UI
{
    public class PanelUserInfo : UIBasePanel
    {
        [SerializeField]
        private Button buttonClose;
        [SerializeField]
        private Button buttonChangeName;

        [SerializeField]
        private TMP_Text _userLevel;
        [SerializeField]
        private TMP_Text _userExperience;
        [SerializeField]
        private Slider _userExperienceSlider;

        [SerializeField]
        private TMP_Text _userName;

        private UserInfoData _userInfoData;

        public override void Initialize()
        {
            buttonClose.onClick.AddListener( () => { Hide(); } );

            buttonChangeName.onClick.AddListener( () =>
            {
                GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_change_name" ).Show();
            } );

            GameManager.Container.Resolve<SignalBus>().Subscribe<LocalDataSignal>( HandleData );

            base.Initialize();
        }

        public override void OnShow()
        {
            UpdateData();
        }

        public override void OnHide()
        {
        }

        private void HandleData( LocalDataSignal localDataSignal )
        {
            if ( typeof( UserInfoData ).Name == localDataSignal.TypeName )
            {
                _userInfoData = localDataSignal.Data as UserInfoData;
                UpdateData();
            }
        }

        private void UpdateData()
        {
            if ( IsShowed == true )
            {
                _userName.text = _userInfoData.UserName;
                _userLevel.text = _userInfoData.UserLevel.ToString();
                _userExperience.text = _userInfoData.UserExperience.ToString() + "/" + _userInfoData.UserMaxExperience.ToString();
                _userExperienceSlider.value = ( float )_userInfoData.UserExperience / ( float )_userInfoData.UserMaxExperience;
            }
        }
    }
}