using UnityEngine;

[ExecuteInEditMode]
public class EnvironmentFieldGrassCameraEffects : MonoBehaviour
{
    [SerializeField]
    private Transform _targetToFollow;

    [SerializeField]
    private RenderTexture _renderTexture;
    [SerializeField]
    private string _globalTexName = "_GlobalEffectRT";
    [SerializeField]
    private string _globalOrthoName = "_OrthographicCamSize";
    [SerializeField]
    private float _orthographicSize = 0;

    private void Awake()
    {
        _orthographicSize = GetComponent<Camera>().orthographicSize;
        Shader.SetGlobalFloat( _globalOrthoName, _orthographicSize );
        Shader.SetGlobalTexture( _globalTexName, _renderTexture );
        Shader.SetGlobalFloat( "_HasRT", 1 );
    }
    private void Update()
    {
        if ( _targetToFollow != null )
        {
            transform.position = new Vector3( _targetToFollow.position.x, _targetToFollow.position.y + 20, _targetToFollow.position.z );
        }
        Shader.SetGlobalVector( "_Position", transform.position );
        transform.rotation = Quaternion.Euler( new Vector3( 90, 0, 0 ) );
    }
}