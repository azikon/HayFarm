namespace Core.Managers
{
    public class EnvironmentManager : BaseManager
    {
        protected override void OnLoad()
        {
            InitializeCurrentEnvironment();
        }

        private void InitializeCurrentEnvironment()
        {
            CreateEnvironment();
        }

        private EnvironmentBase CreateEnvironment()
        {
            return new EnvironmentFarm
            (
                "prefabs/environment/maps/map_1", GameManager.Container.Resolve<SceneObjectsManager>().RootEnvironment.transform
            );
        }
    }
}