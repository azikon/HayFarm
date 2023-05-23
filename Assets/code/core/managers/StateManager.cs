using Zenject;

namespace Core.Managers
{
    public class StateManager : BaseManager
    {
        [Inject] private readonly SignalBus _signalBus;

        public Enums.GameStates States;

        protected override void OnLoad()
        {
            ChangeState( Enums.GameStates.Loading );
        }

        public void ChangeState( Enums.GameStates state )
        {
            States = state;
            _signalBus.TryFire( new GameStateChangeSignal( state ) );
        }
    }
}