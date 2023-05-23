using UnityEngine;

public class EnvironmentFarmGather : MonoBehaviour
{
    [SerializeField]
    private Transform gatherVFX;

    public void GatherVFX( bool show )
    {
        gatherVFX.gameObject.SetActive( show );
    }
}