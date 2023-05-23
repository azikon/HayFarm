namespace Core.Managers
{
    public class CharacterManager : BaseManager
    {
        protected override void OnLoad()
        {
            InitializeCurrentCharacter();
        }

        private void InitializeCurrentCharacter()
        {
            CreateCharacter();
        }

        private CharacterBase CreateCharacter()
        {
            return new CharacterFarmer
            (
                 "prefabs/character/body/character_body_0",
                 GameManager.Container.Resolve<SceneObjectsManager>().RootCharacterMain.transform.Find( "model" ),
                -17,
                15
            );
        }
    }
}