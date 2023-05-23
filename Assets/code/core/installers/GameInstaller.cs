using Core.Managers;
using Core.Signals;

using Zenject;

namespace Core.Installers
{
    public sealed class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InitializeSignal();

            // Common Managers
            Container.BindInterfacesAndSelfTo<LocalDataManager>().AsSingle().NonLazy();

            // Classes from Hierarchy
            Container.BindInterfacesAndSelfTo<SceneObjectsManager>().FromComponentInHierarchy().AsSingle().NonLazy();


            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();


            Container.BindInterfacesAndSelfTo<SettingsManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UserManager>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<CurrencyManager>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<UIManager>().AsSingle().NonLazy();


            Container.BindInterfacesAndSelfTo<EnvironmentManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CharacterManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InventoryManager>().AsSingle().NonLazy();


            Container.BindInterfacesAndSelfTo<StateManager>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<CameraManager>().AsSingle().NonLazy();
        }

        private void InitializeSignal()
        {
            SignalBusInstaller.Install( Container );
            Container.DeclareSignal<CharacterMovementDirectionSignal>();
            Container.DeclareSignal<GameStateChangeSignal>();
            Container.DeclareSignal<LocalDataSignal>();
            Container.DeclareSignal<CharacterCollisionTriggerSignal>();
            Container.DeclareSignal<InventoryCollectSignal>();
            Container.DeclareSignal<InventoryGatherSignal>();
            Container.DeclareSignal<UserExperienceChangeSignal>();
        }
    }
}