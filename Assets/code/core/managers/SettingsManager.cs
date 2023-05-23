using Zenject;

namespace Core.Managers
{
    public class SettingsManager : BaseManager
    {
        protected override void OnLoad()
        {
            GameManager.Container.Resolve<SignalBus>().Subscribe<LocalDataSignal>( HandleData );
        }

        private void HandleData( LocalDataSignal localDataSignal )
        {
            if ( localDataSignal.State == LocalDataState.InitiazlieData )
            {
                SettingsData settingsData = new SettingsData()
                {
                    Language = "en-EN",
                    PushIsEnabled = false,
                    SoundLevel = 1,
                    MusicLevel = 1,
                    VibrationIsEnabled = false,
                };
                GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( settingsData, LocalDataState.SaveData ) );
                GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( settingsData, LocalDataState.UpdateData ) );
            }
        }
    }
}