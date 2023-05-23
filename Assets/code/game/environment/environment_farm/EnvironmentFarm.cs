using Core.Managers;
using Core.Signals;

using UnityEngine;

using Zenject;

public class EnvironmentFarm : EnvironmentBase
{
    public EnvironmentFarm( string prefabPath, Transform prefabParent )
    {
        PrefabPath = prefabPath;
        PrefabParent = prefabParent;

        new EnvironmentFieldGrassCutter( GameManager.Container.Resolve<SceneObjectsManager>().RootCharacterMain.transform );

        GameManager.Container.Resolve<SignalBus>().Subscribe<CharacterCollisionTriggerSignal>( HandleCollision );

        LoadModel();
    }

    public override void OnModelLoaded()
    {
    }

    private void HandleCollision( CharacterCollisionTriggerSignal triggerSignal )
    {
        if ( triggerSignal.TriggeredCollider.TryGetComponent<EnvironmentFarmGather>( out var farmGather ) )
        {
            if ( triggerSignal.IsEnter == true )
            {
                farmGather.GatherVFX( true );

                GameManager.Container.Resolve<SignalBus>().TryFire<InventoryCollectSignal>( new InventoryCollectSignal() );
            }
            else
            {
                farmGather.GatherVFX( false );
            }
        }
    }
}