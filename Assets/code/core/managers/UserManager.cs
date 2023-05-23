using Zenject;

namespace Core.Managers
{
    public class UserManager : BaseManager
    {
        protected override void OnLoad()
        {
            GameManager.Container.Resolve<SignalBus>().Subscribe<LocalDataSignal>( HandleData );
        }

        private void HandleData( LocalDataSignal localDataSignal )
        {
            if ( localDataSignal.State == LocalDataState.InitiazlieData )
            {
                UserInfoData userInfoData = new UserInfoData()
                {
                    UserName = "User",
                    UserLevel = 6,
                    UserExperience = 80,
                    UserMaxExperience = 400,
                    UserCurrencyEnenrgy = 1500,
                    UserCurrencyGold = 1600,
                    UserCurrencyRuby = 1700,
                };
                GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( userInfoData, LocalDataState.SaveData ) );
                GameManager.Container.Resolve<SignalBus>().TryFire<LocalDataSignal>( new LocalDataSignal( userInfoData, LocalDataState.UpdateData ) );
            }
        }
    }
}