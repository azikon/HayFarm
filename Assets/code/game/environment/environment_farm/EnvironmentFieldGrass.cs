using System.Collections.Generic;

using UnityEngine;

public class EnvironmentFieldGrass : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> _markersPositions = new List<Vector3>();

    public List<Vector3> GetMarkerPositions() { return _markersPositions; }

    public int GetMarkerPositionsCount() { return _markersPositions.Count; }

    public void AddMarkerPosition( Vector3 position )
    {
        _markersPositions.Add( position );
    }

    public void RemoveMarkerPosition( Vector3 position )
    {
        _markersPositions.Remove( position );
    }

    public void ClearMarkersPositions()
    {
        _markersPositions.Clear();
        _markersPositions.TrimExcess();
    }
}