using Core.Managers;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Zenject;

namespace UI
{
    public class PanelSettings : UIBasePanel
    {
        [SerializeField]
        private Button buttonOpenSettings;

        [SerializeField]
        private Button buttonCloseSettings;

        [SerializeField]
        private Transform popupSettings;

        [SerializeField]
        private Button buttonOpenLanguage;

        [SerializeField]
        private Slider sliderSoundVolume;
        [SerializeField]
        private Slider sliderMusicVolume;

        private SettingsData _settingsData;

        public override void Initialize()
        {
            buttonOpenSettings.onClick.AddListener( () => { ShowPopup( true ); } );
            buttonCloseSettings.onClick.AddListener( () =>
            {
                ShowPopup( false );
                ChangeData();
            } );

            buttonOpenLanguage.onClick.AddListener( () =>
            {
                GameManager.Container.Resolve<UIManager>().Get<UIBasePanel>( "ui_panel_language" ).Show();
            } );

            EventTrigger.Entry entrySoundSlider = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };
            entrySoundSlider.callback.AddListener( ( data ) => { OnChangedSoundSliderValue(); } );
            sliderSoundVolume.GetComponent<EventTrigger>().triggers.Add( entrySoundSlider );

            EventTrigger.Entry entryMusicSlider = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };
            entryMusicSlider.callback.AddListener( ( data ) => { OnChangedMusicSliderValue(); } );
            sliderMusicVolume.GetComponent<EventTrigger>().triggers.Add( entryMusicSlider );

            GameManager.Container.Resolve<SignalBus>().Subscribe<LocalDataSignal>( HandleData );

            base.Initialize();
        }

        public override void OnShow()
        {
        }

        public override void OnHide()
        {
        }

        private void ShowPopup( bool show )
        {
            popupSettings.gameObject.SetActive( show );
        }

        private void HandleData( LocalDataSignal localDataSignal )
        {
            if ( typeof( SettingsData ).Name == localDataSignal.TypeName && localDataSignal.State == LocalDataState.UpdateData )
            {
                _settingsData = localDataSignal.Data as SettingsData;

                sliderSoundVolume.value = _settingsData.SoundLevel;
                sliderMusicVolume.value = _settingsData.MusicLevel;
            }
        }

        private void OnChangedSoundSliderValue()
        {
            if ( _settingsData != null )
            {
                _settingsData.SoundLevel = sliderSoundVolume.value;
            }
        }

        private void OnChangedMusicSliderValue()
        {
            if ( _settingsData != null )
            {
                _settingsData.MusicLevel = sliderMusicVolume.value;
            }
        }

        private void ChangeData()
        {
            GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( _settingsData, LocalDataState.SaveData ) );
        }
    }
}