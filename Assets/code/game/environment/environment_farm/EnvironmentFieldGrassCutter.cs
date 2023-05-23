using Core.Managers;

using System;
using System.Collections.Generic;

using UnityEngine;

using Zenject;

[Serializable]
public class EnvironmentFieldGrassCutter : IFixedTickable
{
    [SerializeField]
    private float _distanceToFillMarkers = 0.9f;

    [SerializeField]
    private EnvironmentFieldGrass _mowingManager;

    [SerializeField]
    private float _closestMarker = 1000f;

    [SerializeField]
    private Transform _target;

    public EnvironmentFieldGrassCutter( Transform target )
    {
        _target = target;

        GameManager.Container.Resolve<TickableManager>().AddFixed( this, -10 );
    }

    public void FixedTick()
    {
        int layerMask = 1 << 0;

        if ( Physics.Raycast( _target.position + Vector3.up, Vector3.down, out RaycastHit hit, 2000, layerMask ) )
        {
            if ( hit.transform.TryGetComponent<EnvironmentFieldGrass>( out var foundedMowingManager ) )
            {
                if ( foundedMowingManager != _mowingManager )
                {
                    _mowingManager = foundedMowingManager;
                }
            }
            else
            {
                _mowingManager = null;
            }
        }

        if ( _mowingManager == null )
        {
            return;
        }

        List<Vector3> markersPosForRemove = GetMarkersForRemove();

        RemoveMarkersPositions( markersPosForRemove );
    }

    private List<Vector3> GetMarkersForRemove()
    {
        List<Vector3> markersPosForRemove = new List<Vector3>();
        foreach ( Vector3 pos in _mowingManager.GetMarkerPositions() )
        {
            float distanceToMarker = Vector3.Distance( new Vector3( pos.x, 0, pos.z ), new Vector3( _target.position.x, 0, _target.position.z ) );
            if ( distanceToMarker < _closestMarker )
            {
                _closestMarker = distanceToMarker;
            }
            if ( distanceToMarker < _distanceToFillMarkers )
            {
                markersPosForRemove.Add( pos );
            }
        }
        if ( _closestMarker > _distanceToFillMarkers * 2f )
        {
        }
        return markersPosForRemove;
    }

    private void RemoveMarkersPositions( List<Vector3> markersPosForRemove )
    {
        foreach ( Vector3 posToRemove in markersPosForRemove )
        {
            _mowingManager.RemoveMarkerPosition( posToRemove );

            GameManager.Container.Resolve<SignalBus>().TryFire<InventoryGatherSignal>( new InventoryGatherSignal( new Inventory() { ID = "inv_grass_0", Count = 1 } ) );
        }
    }
}