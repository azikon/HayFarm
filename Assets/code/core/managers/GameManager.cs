using Zenject;

namespace Core.Managers
{
    public sealed class GameManager : BaseManager
    {
        public static DiContainer Container { get; private set; }

        [Inject] private readonly LocalDataManager _localDataManager;
        [Inject] private readonly SceneObjectsManager _sceneObjectsManager;

        [Inject] private readonly CurrencyManager _currencyManager;

        [Inject] private readonly UIManager _uiManager;

        [Inject] private readonly EnvironmentManager _environmentManager;
        [Inject] private readonly CharacterManager _characterManager;

        [Inject] private readonly InventoryManager _inventoryManager;

        [Inject] private readonly StateManager _stateManager;

        [Inject] private readonly UserManager _userManager;
        [Inject] private readonly SettingsManager _settingManager;
        [Inject] private readonly CameraManager _cameraManager;

        protected override void OnInitialize()
        {
            Container = _container;

            _localDataManager.Load();
            _sceneObjectsManager.Load();

            _settingManager.Load();

            _uiManager.Load();

            _currencyManager.Load();
            _userManager.Load();

            _environmentManager.Load();
            _characterManager.Load();
            _inventoryManager.Load();

            _stateManager.Load();

            _cameraManager.Load();
        }
    }
}